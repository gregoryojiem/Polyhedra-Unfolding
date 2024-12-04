using MIConvexHull;
using System.Text.Json;
using Unfolding.Client.Polyhedra.DataStructs;
using Unfolding.Client.Polyhedra.Solvers;

namespace Unfolding.Client.Polyhedra
{
    public class PolyMain
    {
        public static string currentView = "3D";

        private static Point3D[] randomVertices = Point3D.GenerateRandPoints(50, 0.5);

        private static Point3D[] currentShape = CustomPolyhedra.dodecahedron;

        private static bool Flatten = false;

        private static bool HideUnplacedPolygons = true;

        public static int StepsToDo = 1;

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
            var net = new Net2D(polygons);
            var solver = new DFS();
            try
            {
                solver.Solve(net);
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }
            if (HideUnplacedPolygons)
            {
                polygons = polygons.Where(p => p.Status != PolygonStatus.Unplaced).ToArray();
            }
            return JsonSerializer.Serialize(polygons, new JsonSerializerOptions { WriteIndented = true });
        }

        public static void PerformStep()
        {
            StepsToDo++;
        }

        public static void UndoStep()
        {
            if (StepsToDo > 0)
            {
                StepsToDo--;
            }

        }
    }
}
