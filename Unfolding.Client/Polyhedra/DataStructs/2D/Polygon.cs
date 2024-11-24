namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Polygon
    {
        public Point2D[] Vertices { get; set; }
        public Edge[] Edges { get; set; }

        public Point2D Centroid { get
            {
                double X = Vertices.Sum(p => p.X) / Vertices.Length;
                double Y = Vertices.Sum(p => p.Y) / Vertices.Length;
                return new(X, Y);
            }
        }

        public void Rotate(double theta)
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].Rotate(theta);
            }
        }

        public double FindConnectionAngle(Polygon poly)
        {

            return null;//Math.Acos(a.Dot(b) / );
        }
    }
}
