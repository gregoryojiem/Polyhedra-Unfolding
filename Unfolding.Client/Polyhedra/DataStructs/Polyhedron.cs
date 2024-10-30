using MIConvexHull;

namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Polyhedron
    {
        public Face[] Faces { get; }

        public Polyhedron(ConvexHullCreationResult<Point3D, Face> convexHull)
        {
            Faces = convexHull.Result.Faces.ToArray();
            Console.WriteLine();
        }
    }
}
