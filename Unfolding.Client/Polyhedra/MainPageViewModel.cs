using System.Text.Json;
using Unfolding.Client.Polyhedra.DataStructs;
using Unfolding.Client.Polyhedra.Solvers;

namespace Unfolding.Client.Polyhedra
{
    public class MainPageViewModel
    {
        public static string currentView = "3D";

        private static Polyhedron currentPolyhedra = PolyhedronLibrary.GetPolyhedron("Cube");

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
            var polyhedron = currentPolyhedra.Copy();
            if (Flatten)
            {
                polyhedron.FlattenFaces();
            }
            return polyhedron.ToJSON();
        }

        public static string GetDisplayNetJSON()
        {
            var inputNet = currentPolyhedra.Copy().ToNet2D();
            var solver = new DFS(inputNet);
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
            currentPolyhedra = PolyhedronLibrary.GetPolyhedron(polyhedron);
        }
    }
}
