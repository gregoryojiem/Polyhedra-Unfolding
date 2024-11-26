using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Polygon
    {
        public Point2D[] Vertices { get; set; }

        [JsonIgnore]
        public Edge2D[] Edges { get; set; } 


        public bool HasBeenPlaced { get; set; }

        public Point2D Centroid { get
            {
                double X = Vertices.Average(p => p.X);
                double Y = Vertices.Average(p => p.Y);
                return new(X, Y);
            }
        }

        public Polygon(Point2D[] vertices, Edge2D[] edges)
        {
            Vertices = vertices;
            Edges = edges;
            HasBeenPlaced = false;
        }

        public static Polygon[] PolyhedraToPolygons(Polyhedron polyhedron)
        {
            Polygon[] polygons = new Polygon[polyhedron.Faces.Length];
            polyhedron.FlattenFaces();
            var polyhedraToPolygonMap = new Dictionary<PolyhedronFace, Polygon>();

            for (int i = 0; i < polyhedron.Faces.Length; i++)
            {
                PolyhedronFace face = polyhedron.Faces[i];
                Point2D[] vertices2D = new Point2D[face.Vertices.Length];

                for (int j = 0; j < face.Vertices.Length; j++)
                {
                    vertices2D[j] = new Point2D(face.Vertices[j].X, face.Vertices[j].Z);
                }

                var edges = new Edge2D[face.Adjacency.Count];
                polygons[i] = new Polygon(vertices2D, edges);
                polyhedraToPolygonMap[face] = polygons[i];
            }

            for (int i = 0; i < polyhedron.Faces.Length; i++)
            {
                var face = polyhedron.Faces[i];
                var polygon = polygons[i];

                var counter = 0;
                foreach (var kvp in face.Adjacency)
                {
                    var edge = kvp.Value;
                    var start = polygon.Vertices.First(v => v.X == edge.Vertices[0].X && v.Y == edge.Vertices[0].Z);
                    var end = polygon.Vertices.First(v => v.X == edge.Vertices[1].X && v.Y == edge.Vertices[1].Z);
                    var adjacentPoly = polyhedraToPolygonMap[edge.ConnectedPoly];
                    polygon.Edges[counter++] = new Edge2D(start, end, adjacentPoly);
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

        public void TranslateToPoint(Point2D pointToTranslateTo)
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i] += pointToTranslateTo;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Vertices: {");

            for (int i = 0; i < Vertices.Length; i++)
            {
                sb.Append($"({Vertices[i].X}, {Vertices[i].Y})");
                if (i < Vertices.Length - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append("}\n");

            return sb.ToString();
        }
    }
}
