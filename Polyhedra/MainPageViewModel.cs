using Polyhedra.DataStructs3D;
using Polyhedra.Solvers;

namespace Polyhedra
{
    public class MainPageViewModel
    {
        public static string currentView = "3D";

        private static Polyhedron currentPolyhedron = PolyhedronLibrary.GetPolyhedron("Cube");

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
        
        public static string GetDisplayPolyhedronJSON()
        {
            var polyhedron = currentPolyhedron.Copy();
            if (Flatten)
            {
                polyhedron.FlattenFaces();
            }
            return polyhedron.ToJSON();
        }

        public static string GetDisplayNetJSON()
        {
            var solver = new DFS(currentPolyhedron);
            var outputNet = solver.Solve();
            return outputNet.ToJSON(HideUnplacedPolygons);
        }

        public static void PerformStep()
        {
            Solver.StepsToDo++;
        }

        public static void UndoStep()
        {
            if (Solver.StepsToDo > 0)
            {
                Solver.StepsToDo--;
            }

        }
        public static void SelectPolyhedra(string polyhedron)
        {
            currentPolyhedron = PolyhedronLibrary.GetPolyhedron(polyhedron);
        }
    }
}
