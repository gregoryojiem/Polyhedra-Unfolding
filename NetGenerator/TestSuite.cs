using PolyhedraUnfolding;
using PolyhedraUnfolding.Solvers;
using PolyhedraUnfolding.DataStructs3D;

namespace NetGenerator
{
    public class TestSuite
    {
        public static string RunPerformanceTests()
        {
            Console.WriteLine("Beginning performance testing");
            var imagePath = SphereTest();
            Console.WriteLine("Performance testing has finished");
            return imagePath;
        }

        public static string SphereTest()
        {
            var sphere = new Polyhedron(PolyhedronExamples.GetSpherePoints(100, 100, 1));
            Console.WriteLine("Face count for sphere test is: " + sphere.Faces.Length);
            var solver = new DFS(sphere);
            var net = solver.Solve();
            var imagePath = Visualization.SaveNetToImage(net, "sphereTest.png", 8000, 8000);
            return imagePath;
        }
    }
}
