using Unfolder.Polyhedra;
using Unfolder.Polyhedra.Solvers;
using Polyhedra.DataStructs3D;

namespace PerformanceTesting
{
    public class TestSuite
    {
        public static void RunPerformanceTests()
        {
            Console.WriteLine("Beginning performance testing");
            Solver.UseSteps = false;
            SphereTest();
            Solver.UseSteps = true;
            Console.WriteLine("Performance testing has finished");
        }

        public static void SphereTest()
        {
            var sphere = new Polyhedron(PolyhedronLibrary.GetSpherePoints(100, 100, 1));
            Console.WriteLine("Face count for sphere test is: " + sphere.Faces.Length);
            var solver = new DFS(sphere);
            var net = solver.Solve();
            Visualization.SaveNetToImage(net);
        }
    }
}
