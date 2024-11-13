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

        public PolyhedraFace(Point3D[] points)
        {
            Vertices = points;
            Normal = [1, 1, 1]; //TODO calculate normal
        }

        public void TranslateToOrigin()
        {
            // TODO update vertices and the normal vector to origin
        }
    }
}
