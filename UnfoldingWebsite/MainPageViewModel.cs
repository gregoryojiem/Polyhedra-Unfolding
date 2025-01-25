using PolyhedraUnfolding;
using PolyhedraUnfolding.DataStructs2D.Nets;
using PolyhedraUnfolding.DataStructs3D;
using PolyhedraUnfolding.Solvers;

namespace UnfoldingWebsite
{
    public class MainPageViewModel
    {
        // Shared 2D/3D objects
        public static string currentView = "3D";

        private static Polyhedron currentPolyhedron = PolyhedronExamples.GetPolyhedron("Cube");

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
            var attemptCount = 0;
            while (attemptCount < 4)
            {
                try
                {
                    var solver = new DFS(currentNet);
                    var outputNet = solver.Solve();
                    if (outputNet.IsComplete())
                    {
                        outputNet.StepsToDo = outputNet.StepsTaken;
                    }
                    return outputNet.ToJSON(HideUnplacedPolygons);
                }
                catch (Exception e)
                {
                    var newNet = currentPolyhedron.Copy().ToNet2D();
                    newNet.StepsToDo = currentNet.StepsToDo;
                    currentNet = newNet;
                    attemptCount++;
                    Console.WriteLine(e);
                }
            }
            return "{}";
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
            currentPolyhedron = PolyhedronExamples.GetPolyhedron(polyhedron);
            currentNet = currentPolyhedron.Copy().ToNet2D();
        }

        public static void SelectSphere(int slices, int stacks, double radius)
        {
            currentPolyhedron = PolyhedronExamples.GetSphere(slices, stacks, radius);
            currentNet = currentPolyhedron.Copy().ToNet2D();
        }

        public static void SelectRandom(int numOfPoints, double radius)
        {
            var attemptCount = 0;
            while (attemptCount < 10) {
                try {
                    currentPolyhedron = PolyhedronExamples.GetRandomPolyhedron(numOfPoints, radius);
                    currentNet = currentPolyhedron.Copy().ToNet2D();
                    return;
                } catch (Exception e)
                {
                    numOfPoints /= 2;
                    attemptCount++;
                    Console.WriteLine(e);
                }
            }

            currentPolyhedron = PolyhedronExamples.GetPolyhedron("Cube");
            currentNet = currentPolyhedron.Copy().ToNet2D();
        }
    }
}
