using System.Text.Json.Serialization;
using Polyhedra.DataStructs3D;
using RBush;

namespace Polyhedra.DataStructs2D
{
    public class Polygon2D : ISpatialData
    {
        public Point2D[] Vertices { get; set; }

        public Edge2D[] Edges { get; set; }

        public PolygonStatus Status { get; set; }

        public int Id { get; set; }

        [JsonIgnore]
        public Envelope Bounds;

        [JsonIgnore]
        public bool BoundsChanged = true;

        [JsonIgnore]
        public Point2D Centroid {
            get
            {
                double X = Vertices.Average(p => p.X);
                double Y = Vertices.Average(p => p.Y);
                return new(X, Y);
            }
        }

        [JsonIgnore]
        public ref readonly Envelope Envelope
        {
            get
            {
                return ref Bounds;
            }
        }

        public Polygon2D(Point2D[] vertices, Edge2D[] edges, int id)
        {
            Vertices = vertices;
            Edges = edges;
            Status = PolygonStatus.Unplaced;
            Id = id;
        }

        public static Polygon2D[] PolyhedraToPolygons(Polyhedron polyhedron)
        {
            Polygon2D[] polygons = new Polygon2D[polyhedron.Faces.Length];
            var polyhedraToPolygonMap = new Dictionary<Polygon3D, Polygon2D>();

            for (int i = 0; i < polyhedron.Faces.Length; i++)
            {
                Polygon3D face = polyhedron.Faces[i];
                Point2D[] vertices2D = new Point2D[face.Vertices.Length];

                for (int j = 0; j < face.Vertices.Length; j++)
                {
                    vertices2D[j] = new Point2D(face.Vertices[j].X, face.Vertices[j].Z);
                }

                var edges = new Edge2D[face.Adjacency.Count];
                polygons[i] = new Polygon2D(vertices2D, edges, i);
                polyhedraToPolygonMap[face] = polygons[i];
            }

            for (int i = 0; i < polyhedron.Faces.Length; i++)
            {
                var face = polyhedron.Faces[i];
                var polygon = polygons[i];

                var counter = 0;
                foreach (var edge in face.Adjacency)
                {
                    var convertedStart = new Point2D(edge.Start.X, edge.Start.Z);
                    var convertedEnd = new Point2D(edge.End.X, edge.End.Z);
                    var start = polygon.Vertices.First(v => v == convertedStart);
                    var end = polygon.Vertices.First(v => v == convertedEnd);
                    var adjacentPolygon = polyhedraToPolygonMap[edge.ConnectedFace];
                    polygon.Edges[counter++] = new Edge2D(start, end, i, adjacentPolygon.Id);
                }
            }

            return polygons;
        }

        public double FindAngleBetween(Polygon2D adjacentPolgon, Edge2D currentEdge, Edge2D adjacentEdge)
        {
            var vecToCurrEdge = GetVecToEdge(currentEdge);
            var vecToAdjEdge = adjacentPolgon.GetVecToEdge(adjacentEdge);

            var perpendicularCurr = new Vec2D(
                -(currentEdge.End.Y - currentEdge.Start.Y),
                currentEdge.End.X - currentEdge.Start.X);

            var perpendicularAdj = new Vec2D(
                -(adjacentEdge.End.Y - adjacentEdge.Start.Y),
                adjacentEdge.End.X - adjacentEdge.Start.X);

            var invertPerpCurr = vecToCurrEdge.Dot(perpendicularCurr) > 0;
            var invertPerpAdj = vecToAdjEdge.Dot(perpendicularAdj) > 0;
            if (invertPerpCurr)
            {
                perpendicularCurr = perpendicularCurr * -1;
            }
            if (invertPerpAdj)
            {
                perpendicularAdj = perpendicularAdj * -1;
            }

            return perpendicularCurr.FindAngleBetween(perpendicularAdj * -1, true);
        }

        public void Rotate(double theta)
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i] = Vertices[i].Rotate(theta);
            }

            for (int i = 0; i < Edges.Length; i++)
            {
                Edges[i] = Edges[i].Rotate(theta);
            }

            BoundsChanged = true;
        }

        public void Translate(Point2D pointToTranslateTo)
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i] = Vertices[i] + pointToTranslateTo;
            }

            for (int i = 0; i < Edges.Length; i++)
            {
                Edges[i] = Edges[i].Translate(pointToTranslateTo.X, pointToTranslateTo.Y);
            }

            BoundsChanged = true;
        }

        public void TranslateToOrigin()
        {
            var centroid = Centroid;
            Translate(new Point2D(-centroid.X, -centroid.Y));
        }

        public void TranslateToEdge(Edge2D adjacentEdge, Edge2D currentEdge, Polygon2D currentPolygon)
        {
            var vecToCurrEdge = currentPolygon.GetVecToEdge(currentEdge);
            var vecToAdjEdge = GetVecToEdge(adjacentEdge);
            var translation = vecToCurrEdge - vecToAdjEdge + currentPolygon.Centroid;
            TranslateToOrigin();
            Translate(new Point2D(translation.X, translation.Y));
        }

        public Vec2D GetVecToEdge(Edge2D edge)
        {
            return new Vec2D(edge.GetMidpoint() - Centroid);
        }

        public Edge2D GetConnectingEdge(Polygon2D adjacentPolygon)
        {
            return Edges.First(e => e.AdjacentPolygonIndex == adjacentPolygon.Id);
        }

        public bool Intersecting(Polygon2D otherPolygon)
        {
            foreach (Edge2D edge in Edges)
            {
                if (edge.AdjacentPolygonIndex == otherPolygon.Id)
                {
                    continue;
                }
                foreach (Edge2D otherEdge in otherPolygon.Edges)
                {
                    if (otherEdge.AdjacentPolygonIndex == Id)
                    {
                        continue;
                    }
                    if (edge.Intersection(otherEdge))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public Envelope GetBounds()
        {
            if (!BoundsChanged)
            {
                return Bounds;
            }

            double minX = Vertices[0].X;
            double maxX = Vertices[0].X;
            double minY = Vertices[0].Y;
            double maxY = Vertices[0].Y;

            foreach (var vertex in Vertices)
            {
                minX = Math.Min(minX, vertex.X);
                maxX = Math.Max(maxX, vertex.X);
                minY = Math.Min(minY, vertex.Y);
                maxY = Math.Max(maxY, vertex.Y);
            }

            BoundsChanged = false;
            Bounds = new Envelope(minX, minY, maxX, maxY);
            return Bounds;
        }

        public bool DoBoundsIntersect(Polygon2D otherpolygon)
        {
            var bounds = GetBounds();
            var otherBounds = otherpolygon.GetBounds();
            return bounds.MaxX >= otherBounds.MinX && bounds.MinX <= otherBounds.MaxX && 
                bounds.MaxY >= otherBounds.MinY && bounds.MinY <= otherBounds.MaxY;
        }

        public void Scale(double scalar)
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i] = new Point2D(Vertices[i].X * scalar, Vertices[i].Y * scalar);
            }
        }

        public void SortVerticesClockwise()
        {
            double centerX = Vertices.Average(v => v.X);
            double centerY = Vertices.Average(v => v.Y);
            Vertices = Vertices.OrderBy(v => Math.Atan2(v.Y - centerY, v.X - centerX)).ToArray();
        }

        public override string ToString()
        {
            var status = Status == PolygonStatus.Unplaced ? "Unplaced" : "Placed";
            return Id + ", " + Vertices.Length + " vertices. " + status;
        }
    }
}
