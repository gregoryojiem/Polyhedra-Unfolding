using MIConvexHull;
using Unfolding.Client.Polyhedra.DataStructs;

namespace Unfolding.Client.Polyhedra
{
    public class PolyMain
    {
        public static int[] GetFaces()
        {
            var vertices = Point3D.GenerateRandPoints(10, 2);
            var MI_convexHull = MIConvexHull.ConvexHull.Create<Point3D, Face>(vertices);
            return new int[] { 2, 3, 4 };
        }
    }
}
