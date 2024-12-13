using Polyhedra.DataStructs2D.Nets;
using Polyhedra.DataStructs3D;
using System.Reflection;
using Unfolder.Polyhedra.Solvers;

namespace Unfolder.Polyhedra
{
    public class MainPageViewModel
    {
        // Shared 2D/3D objects
        public static string currentView = "3D";

        private static Polyhedron currentPolyhedron = PolyhedronLibrary.GetPolyhedron("Cube");

        private static Net2D currentNet = currentPolyhedron.Copy().ToNet2D();

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
            var polyhedron = currentPolyhedron;
            if (Flatten)
            {
                polyhedron = currentPolyhedron.Copy();
                polyhedron.FlattenFaces();
            }
            return polyhedron.ToJSON();
        }

        public static string GetDisplayNetJSON()
        {
            var solver = new DFS(currentNet);
            var outputNet = solver.Solve();
            if (outputNet.IsComplete())
            {
                outputNet.StepsToDo = outputNet.StepsTaken;
            }
            return outputNet.ToJSON(HideUnplacedPolygons);
        }

        public static void PerformStep()
        {
            currentNet.StepsToDo++;
        }

        public static void UndoStep()
        {
            if (currentNet.StepsToDo > 1)
            {
                currentNet.StepsTaken--;
                currentNet.StepsToDo--;
            }
            currentNet.Undo();
        }

        public static void CompleteStep()
        {
            currentNet.StepsToDo = int.MaxValue;
        }

        public static void ResetStep()
        {
            currentNet.Reset();
        }

        public static void SelectPolyhedra(string polyhedron)
        {
            currentPolyhedron = PolyhedronLibrary.GetPolyhedron(polyhedron);
            currentNet = currentPolyhedron.Copy().ToNet2D();
        }
    }
}
