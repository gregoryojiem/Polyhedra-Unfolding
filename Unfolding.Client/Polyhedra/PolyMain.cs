using MIConvexHull;
using System.Diagnostics;
using Unfolding.Client.Polyhedra.DataStructs;

namespace Unfolding.Client.Polyhedra
{
    public class PolyMain
    {
        public static Polyhedron GetPolyhedron()
        {
            var pyramidVertices = new Point3D[] { new Point3D(-0.5, -0.5, -0.5), new Point3D(0.5, -0.5, -0.5), new Point3D(0.5, -0.5, 0.5), new Point3D(-0.5, -0.5, 0.5), new Point3D(0, 0.5, 0) };
            var vertices = Point3D.GenerateRandPoints(20, 0.5);
            var convexHull = MIConvexHull.ConvexHull.Create<Point3D, ConvexHullFace>(pyramidVertices);
            var polyhedron = new Polyhedron(convexHull);

            for (int i = 0; i < polyhedron.Faces.Length; i++)
            {
                //PolyhedraFace face = polyhedron.Faces[i];
                //polyhedron.Faces[i] = polyhedron.Faces[i].Rotate3DToAlign();
            }

            return polyhedron;
        }
    }
}
