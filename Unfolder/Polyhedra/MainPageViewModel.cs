using Polyhedra.DataStructs3D;
using Unfolder.Polyhedra.Solvers;

namespace Unfolder.Polyhedra
{
    public class MainPageViewModel
    {
        public static string currentView = "3D";

        private static Polyhedron currentPolyhedron = PolyhedronLibrary.GetPolyhedron("Cube");

        // 3D toggles
        private static bool Flatten = false;

        public static bool DoUnfoldAnimation = false;

        // 2D toggles
        private static bool HideUnplacedPolygons = true;

        public static void SwapView()
        {
            currentView = (currentView == "3D") ? "2D" : "3D";
        }

        public static void FlattenToggle()
        {
            Flatten = !Flatten;
        }

        public static void UnfoldAnimation()
        {
            DoUnfoldAnimation = true;
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
            var solver = new DFS(currentPolyhedron.Copy());
            var outputNet = solver.Solve();
            if (outputNet.IsComplete())
            {
                Solver.StepsToDo = solver.StepsTaken;
            }
            return outputNet.ToJSON(HideUnplacedPolygons);
        }

        public static void PerformStep()
        {
            Solver.StepsToDo++;
        }

        public static void UndoStep()
        {
            if (Solver.StepsToDo > 1)
            {
                Solver.StepsToDo--;
            }
        }

        public static void CompleteStep()
        {
            Solver.StepsToDo = int.MaxValue;
        }

        public static void ResetStep()
        {
            Solver.StepsToDo = 1;
        }

        public static void SelectPolyhedra(string polyhedron)
        {
            currentPolyhedron = PolyhedronLibrary.GetPolyhedron(polyhedron);
        }
    }
}
