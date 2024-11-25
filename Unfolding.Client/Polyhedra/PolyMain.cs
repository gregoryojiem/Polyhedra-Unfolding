using MIConvexHull;
using System.Diagnostics;
using System.Text.Json;
using Unfolding.Client.Polyhedra.DataStructs;

namespace Unfolding.Client.Polyhedra
{
    public class PolyMain
    {
        public static string currentView = "3D";

        private static Point3D[] randomVertices = Point3D.GenerateRandPoints(20, 0.5);

        private static Point3D[] pyramidVertices = new Point3D[] { new Point3D(-0.5, -0.5, -0.5), new Point3D(0.5, -0.5, -0.5), new Point3D(0.5, -0.5, 0.5), new Point3D(-0.5, -0.5, 0.5), new Point3D(0, 0.5, 0) };

        public static void SwapView()
        {
            currentView = (currentView == "3D") ? "2D" : "3D";
        }

        public static Polyhedron GetPolyhedron()
        {
            var convexHull = ConvexHull.Create<Point3D, ConvexHullFace>(pyramidVertices);
            var polyhedron = new Polyhedron(convexHull);
            polyhedron.FlattenFaces();
            return polyhedron;
        }

        public static string GetPolygonsJSON()
        {
            var convexHull = ConvexHull.Create<Point3D, ConvexHullFace>(pyramidVertices);
            var polyhedron = new Polyhedron(convexHull);
            var polygons = Polygon.PolyhedraToPolygons(polyhedron);
            return JsonSerializer.Serialize(polygons, new JsonSerializerOptions { WriteIndented = true });
        }

        public static void PerformStep()
        {

        }
    }
}
