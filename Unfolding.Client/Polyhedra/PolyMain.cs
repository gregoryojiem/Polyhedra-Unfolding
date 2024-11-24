using MIConvexHull;
using System.Diagnostics;
using System.Text.Json;
using Unfolding.Client.Polyhedra.DataStructs;

namespace Unfolding.Client.Polyhedra
{
    public class PolyMain
    {
        public static string currentView = "3D";

        public static void SwapView()
        {
            currentView = (currentView == "3D") ? "2D" : "3D";
        }

        public static Polyhedron GetPolyhedron()
        {
            var pyramidVertices = new Point3D[] { new Point3D(-0.5, -0.5, -0.5), new Point3D(0.5, -0.5, -0.5), new Point3D(0.5, -0.5, 0.5), new Point3D(-0.5, -0.5, 0.5), new Point3D(0, 0.5, 0) };
            var vertices = Point3D.GenerateRandPoints(20, 0.5);
            var convexHull = MIConvexHull.ConvexHull.Create<Point3D, ConvexHullFace>(pyramidVertices);
            var polyhedron = new Polyhedron(convexHull);

            for (int i = 0; i < polyhedron.Faces.Length; i++)
            {
                //PolyhedraFace face = polyhedron.Faces[i];
                //polyhedron.Faces[i] = polyhedron.Faces[i].Rotate3DToAlign();
            }

            return polyhedron;
        }

        public static string GetPolygonsJSON()
        {
            var polygons = new List<Polygon>();

            polygons.Add(new Polygon
            {
                Vertices =
                [
                    new Point2D { X = 0, Y = 0 },
                    new Point2D { X = 1, Y = 0 },
                    new Point2D { X = 1, Y = 1 },
                    new Point2D { X = 0, Y = 1 }
                ]
            });

            polygons.Add(new Polygon
            {
                Vertices = new Point2D[]
                {
                    new Point2D { X = 2, Y = 2 },
                    new Point2D { X = 3, Y = 2 },
                    new Point2D { X = 2.5, Y = 3 }
                }
            });

            return JsonSerializer.Serialize(polygons, new JsonSerializerOptions { WriteIndented = true });
        }

        public static void PerformStep()
        {

        }
    }
}
