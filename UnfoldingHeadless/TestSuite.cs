using PolyhedraUnfolding;
using PolyhedraUnfolding.Solvers;
using PolyhedraUnfolding.DataStructs3D;

namespace UnfoldingHeadless
{
    public class TestSuite
    {
        public static void RunPerformanceTests()
        {
            Console.WriteLine("Beginning performance testing");
            Solver.UseNetSteps = false;
            SphereTest();
            Solver.UseNetSteps = true;
            Console.WriteLine("Performance testing has finished");
        }

        public static void SphereTest()
        {
            var sphere = new Polyhedron(PolyhedronExamples.GetSpherePoints(100, 100, 1));
            Console.WriteLine("Face count for sphere test is: " + sphere.Faces.Length);
            var solver = new DFS(sphere);
            var net = solver.Solve();
            Visualization.SaveNetToImage(net);
        }
    }
}
