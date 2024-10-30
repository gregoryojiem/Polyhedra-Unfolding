namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class PolyhedraFace
    {
        public Point3D[] Vertices { get; set; }
        public double[] Normal { get; set; }

        public PolyhedraFace(ConvexHullFace convexHullFace)
        {
            Vertices = convexHullFace.Vertices;
            Normal = convexHullFace.Normal;
        }
    }
}
