using MIConvexHull;
using System.Diagnostics;
using System.Text.Json;
using Unfolding.Client.Polyhedra.DataStructs;

namespace Unfolding.Client.Polyhedra
{
    public class PolyMain
    {
        public static string currentView = "3D";

        private static Point3D[] randomVertices = Point3D.GenerateRandPoints(100, 0.5);

        // Set test shapes
        private static Point3D[] triangularPyramid =
            [
                new(-0.5, -0.5, -0.5),
                new(0.5, -0.5, -0.5),
                new(0.5, -0.5, 0.5),
                new(-0.5, -0.5, 0.5),
                new(0, 0.5, 0)
            ];
        private static Point3D[] cube =
            [
                new(-0.5, -0.5, -0.5),
                new(-0.5, -0.5, 0.5),
                new(-0.5, 0.5, -0.5),
                new(-0.5, 0.5, 0.5),
                new(0.5, -0.5, -0.5),
                new(0.5, -0.5, 0.5),
                new(0.5, 0.5, -0.5),
                new(0.5, 0.5, 0.5)
            ];
        private static Point3D[] tetrahedron =
            [
                new(0, 0.5, 0),
                new(0.5, -0.5, 0),
                new(Math.Sin(60), -0.5, 0.5),
                new(Math.Sin(60), -0.5, -0.5)
            ];
        private static Point3D[] elongatedSquareDipyramid =
            [
                new(0, -1.5, 0),
                new(-0.5, -0.5, -0.5),
                new(-0.5, -0.5, 0.5),
                new(-0.5, 0.5, -0.5),
                new(-0.5, 0.5, 0.5),
                new(0.5, -0.5, -0.5),
                new(0.5, -0.5, 0.5),
                new(0.5, 0.5, -0.5),
                new(0.5, 0.5, 0.5),
                new(0, 1.5, 0)
            ];
        private static Point3D[] octahedron =
            [
                new(0, 0.5, 0),
                new(-1/(2*Math.Sqrt(2)), 0, -1/(2*Math.Sqrt(2))),
                new(-1/(2*Math.Sqrt(2)), 0, 1/(2*Math.Sqrt(2))),
                new(1/(2*Math.Sqrt(2)), 0, -1/(2*Math.Sqrt(2))),
                new(1/(2*Math.Sqrt(2)), 0, 1/(2*Math.Sqrt(2))),
                new(0, -0.5, 0)
            ];

        private static double phi = (1 + Math.Sqrt(5)) / 2;

        private static Point3D[] dodecahedronVertices =
        [
            new(1, 1, 1),
            new(1, 1, -1),
            new(1, -1, 1),
            new(1, -1, -1),
            new(-1, 1, 1),
            new(-1, 1, -1),
            new(-1, -1, 1),
            new(-1, -1, -1),
            new(0, 1/phi, phi),
            new(0, 1/phi, -phi),
            new(0, -1/phi, phi),
            new(0, -1/phi, -phi),
            new(1/phi, phi, 0),
            new(1/phi, -phi, 0),
            new(-1/phi, phi, 0),
            new(-1/phi, -phi, 0),
            new(phi, 0, 1/phi),
            new(-phi, 0, 1/phi),
            new(phi, 0, -1/phi),
            new(-phi, 0, -1/phi)
        ];

        private static Point3D[] currentShape = octahedronVertices;

        private static bool Flatten = false;

        private static bool HideUnplacedPolygons = true;

        public static void SwapView()
        {
            currentView = (currentView == "3D") ? "2D" : "3D";
        }

        public static void FlattenToggle()
        {
            Flatten = !Flatten;
        }

        public static void UnplacedVisibilityToggle()
        {
            HideUnplacedPolygons = !HideUnplacedPolygons;
        }
        
        public static Polyhedron GetPolyhedron()
        {
            var convexHull = ConvexHull.Create<Point3D, ConvexHullFace>(currentShape);
            var polyhedron = new Polyhedron(convexHull);
            if (Flatten)
            {
                polyhedron.FlattenFaces();
            }
            return polyhedron;
        }

        public static string GetPolygonsJSON()
        {
            var convexHull = ConvexHull.Create<Point3D, ConvexHullFace>(currentShape);
            var polyhedron = new Polyhedron(convexHull);
            var polygons = Polygon.PolyhedraToPolygons(polyhedron);
            var net = Net2D.GenerateNet(polygons);
            if (HideUnplacedPolygons)
            {
                polygons = polygons.Where(p => p.HasBeenPlaced).ToArray();
            }
            return JsonSerializer.Serialize(polygons, new JsonSerializerOptions { WriteIndented = true });
        }

        public static void PerformStep()
        {
            Net2D.StepsToDo++;
        }

        public static void UndoStep()
        {
            if (Net2D.StepsToDo > 0)
            {
                Net2D.StepsToDo--;
            }
        }
    }
}
