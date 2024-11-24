using System.Security.Cryptography.X509Certificates;

namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Point2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point2D(double X, double Y) { this.X = X; this.Y = Y; }

        public void Rotate(double theta)
        {
            X = X * Math.Cos(theta) - Y * Math.Sin(theta);
            Y = X * Math.Sin(theta) + Y * Math.Cos(theta);
        }
    }
}
