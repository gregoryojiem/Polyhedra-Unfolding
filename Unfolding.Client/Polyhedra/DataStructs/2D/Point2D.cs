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

        public void Add(Point2D point)
        {
            X += point.X;
            Y += point.Y;
        }

        public static Point2D[] SortPoints(Point2D[] points)
        {
            double avgX = points.Average(p => p.X);
            double avgY = points.Average(p => p.Y);

            Point2D centerPoint = new Point2D(avgX, avgY);

            var anglesAndIndices = points.Select((point, index) =>
            {
                double dx = point.X - centerPoint.X;
                double dy = point.Y - centerPoint.Y;
                double angle = Math.Atan2(dy, dx); 
                return (angle, index, point);
            }).ToList();

            anglesAndIndices.Sort((a, b) => a.angle.CompareTo(b.angle));

            return anglesAndIndices.Select(item => item.point).ToArray();


        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }
    }
}
