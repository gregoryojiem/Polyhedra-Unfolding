﻿using System.Text;
using System.Text.Json.Serialization;
using Polyhedra.DataStructs3D;

namespace Polyhedra.DataStructs2D
{
    public class Polygon
    {
        public Point2D[] Vertices { get; set; }

        public Edge2D[] Edges { get; set; }

        public PolygonStatus Status { get; set; }

        public int Id { get; set; }

        [JsonIgnore]
        public Point2D Centroid {
            get
            {
                double X = Vertices.Average(p => p.X);
                double Y = Vertices.Average(p => p.Y);
                return new(X, Y);
            }
        }

        public Point2D[] Bounds {
            get
            {
                double maxX = Vertices.Max(v => v.X);
                double minX = Vertices.Min(v => v.X);
                double maxY = Vertices.Max(v => v.Y);
                double minY = Vertices.Min(v => v.Y);
                return [new(minX, minY), new(minX, maxY), new(maxX, minY), new(maxX, maxY)];
            }
        }

        public Polygon(Point2D[] vertices, Edge2D[] edges, int id)
        {
            Vertices = vertices;
            Edges = edges;
            Status = PolygonStatus.Unplaced;
            Id = id;
        }

        public static Polygon[] PolyhedraToPolygons(Polyhedron polyhedron)
        {
            Polygon[] polygons = new Polygon[polyhedron.Faces.Length];
            polyhedron.FlattenFaces();
            var polyhedraToPolygonMap = new Dictionary<Polygon3D, Polygon>();

            for (int i = 0; i < polyhedron.Faces.Length; i++)
            {
                Polygon3D face = polyhedron.Faces[i];
                Point2D[] vertices2D = new Point2D[face.Vertices.Length];

                for (int j = 0; j < face.Vertices.Length; j++)
                {
                    vertices2D[j] = new Point2D(face.Vertices[j].X, face.Vertices[j].Z);
                }

                var edges = new Edge2D[face.Adjacency.Count];
                polygons[i] = new Polygon(vertices2D, edges, face.Id);
                polyhedraToPolygonMap[face] = polygons[i];
            }

            for (int i = 0; i < polyhedron.Faces.Length; i++)
            {
                var face = polyhedron.Faces[i];
                var polygon = polygons[i];

                var counter = 0;
                foreach (var edge in face.Adjacency)
                {
                    var start = polygon.Vertices.First(v => v.X == edge.Vertices[0].X && v.Y == edge.Vertices[0].Z);
                    var end = polygon.Vertices.First(v => v.X == edge.Vertices[1].X && v.Y == edge.Vertices[1].Z);
                    var adjacentPoly = polyhedraToPolygonMap[edge.ConnectedFace];
                    polygon.Edges[counter++] = new Edge2D(start, end, polygon, adjacentPoly);
                }
            }

            return polygons;
        }

        public void Rotate(double theta)
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].Rotate(theta);
            }
        }
         
        public void TranslateToOrigin()
        {
            var centroid = Centroid;
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].X -= centroid.X;
                Vertices[i].Y -= centroid.Y;
            }
        }

        public void TranslateToPoint(Point2D pointToTranslateTo)
        {
            TranslateToOrigin();
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].X += pointToTranslateTo.X;
                Vertices[i].Y += pointToTranslateTo.Y;
            }
        }

        public void TranslateToEdge(Edge2D edge, Edge2D matchingEdge)
        {
            var vecToCurrEdge = matchingEdge.Polygon.GetVecToEdge(matchingEdge);
            var vecToAdjEdge = GetVecToEdge(edge);
            var adjacentPolygonCentroid = vecToCurrEdge - vecToAdjEdge + matchingEdge.Polygon.Centroid;
            TranslateToPoint(adjacentPolygonCentroid.ToPoint());
        }

        public Vec2D GetVecToEdge(Edge2D edge)
        {
            return (edge.Mid - Centroid).ToVector();
        }

        public Edge2D GetConnectingEdge(Polygon polygon)
        {
            return Edges.First(e => e.AdjacentPolygon == polygon);
        }

        public bool Intersecting(Polygon otherPolygon)
        {
            foreach (Edge2D edge in Edges)
            {
                if (edge.AdjacentPolygon == otherPolygon)
                {
                    continue;
                }
                foreach (Edge2D otherEdge in otherPolygon.Edges)
                {
                    if (otherEdge.AdjacentPolygon == this)
                    {
                        continue;
                    }
                    if (edge.Intersection(otherEdge))
                    {
                        if (edge.Intersection(otherEdge))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool DoBoundsIntersect(Polygon otherpolygon)
        {
            //this.Bounds;
            //otherpolygon.Bounds;
            //TODO check for bound intersection
            return true;
        }

        public override string ToString()
        {
            return Id + ", " + Vertices.Length + " vertices.";
        }
    }
}
