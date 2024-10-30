using MIConvexHull;
using Unfolding.Client.Polyhedra.DataStructs;

namespace Unfolding.Client.Polyhedra
{
    public class PolyMain
    {
        public static Polyhedron GetPolyhedron()
        {
            var vertices = Point3D.GenerateRandPoints(100, 0.5);
            var convexHull = MIConvexHull.ConvexHull.Create<Point3D, ConvexHullFace>(vertices);
            var polyhedron = new Polyhedron(convexHull);
            return polyhedron;
        }
    }
}
