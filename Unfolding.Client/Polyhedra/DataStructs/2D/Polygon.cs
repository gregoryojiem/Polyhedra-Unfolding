using System.Text;
using System.Text.Json.Serialization;

namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Polygon
    {
        public Point2D[] Vertices { get; set; }

        [JsonIgnore]
        public Edge[] Edges { get; set; }

        [JsonIgnore]
        public Polygon[] Adjacency { get; set; }

        public Point2D Centroid { get
            {
                double X = Vertices.Average(p => p.X);
                double Y = Vertices.Average(p => p.Y);
                return new(X, Y);
            }
        }

        public Polygon(Point2D[] vertices, Edge[] edges)
        {
            Vertices = vertices;
            Edges = edges;
            Adjacency = [];
        }

        public static Polygon[] PolyhedraToPolygons(Polyhedron polyhedron)
        {
            Polygon[] polygons = new Polygon[polyhedron.Faces.Length];
            polyhedron.FlattenFaces();
            var polyhedraToPolygonMap = new Dictionary<PolyhedraFace, Polygon>();

            for (int i = 0; i < polyhedron.Faces.Length; i++)
            {
                PolyhedraFace face = polyhedron.Faces[i];
                Point2D[] vertices2D = new Point2D[face.Vertices.Length];

                for (int j = 0; j < face.Vertices.Length; j++)
                {
                    vertices2D[j] = new Point2D(face.Vertices[j].X, face.Vertices[j].Z);
                }

                var sortedVertices = Point2D.SortPoints(vertices2D);
                int numVertices = sortedVertices.Length;
                var edges = new Edge[numVertices];
                
                for (int j = 0; j < numVertices; j++)
                {
                    Point2D start = sortedVertices[j];
                    Point2D end = sortedVertices[(j + 1) % numVertices];
                    edges[j] = new Edge(start, end);
                }


                polygons[i] = new Polygon(sortedVertices, edges);
                polyhedraToPolygonMap[face] = polygons[i];
            }

            for (int i = 0; i < polygons.Length; i++)
            {
                var face = polyhedron.Faces[i];
                var adjacencyList = new Polygon[face.Adjacency.Count];

                for (int j = 0; j < face.Adjacency.Count; j++)
                {
                    adjacencyList[j] = polyhedraToPolygonMap[face.Adjacency[j]];
                }

                polygons[i].Adjacency = adjacencyList;
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
