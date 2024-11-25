using MIConvexHull;
using System.Runtime.CompilerServices;

namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Polygon
    {
        public Point2D[] Vertices { get; set; }
        public Edge[] Edges { get; set; }

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
        }

        public static Polygon[] PolyhedraToPolygons(Polyhedron polyhedron)
        {
            Polygon[] polygons = new Polygon[polyhedron.Faces.Length];

            for (int i = 0; i < polyhedron.Faces.Length; i++)
            {
                PolyhedraFace face = polyhedron.Faces[i];
                PolyhedraFace alignedFace = face.Rotate3DToAlign();
                Point2D[] vertices2D = new Point2D[alignedFace.Vertices.Length];

                for (int j = 0; j < alignedFace.Vertices.Length; j++)
                {
                    vertices2D[j] = new Point2D(alignedFace.Vertices[j].X, alignedFace.Vertices[j].Z);
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
    }
}
