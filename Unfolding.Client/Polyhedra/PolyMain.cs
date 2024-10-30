using MIConvexHull;
using Unfolding.Client.Polyhedra.DataStructs;

namespace Unfolding.Client.Polyhedra
{
    public class PolyMain
    {
        public static int[] GetFaces()
        {
            var vertices = Point3D.GenerateRandPoints(10, 2);
            var convexHull = MIConvexHull.ConvexHull.Create<Point3D, Face>(vertices);
            var polyhedra = new Polyhedron(convexHull);
            return new int[] { 2, 3, 4 };
        }
    }
}
