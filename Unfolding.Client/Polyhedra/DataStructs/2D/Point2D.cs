namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Point2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point2D(double x, double y) { X = x; Y = y; }

        public Point2D(Vec2D vec) { X = vec.X; Y = vec.Y; }

        public static Point2D operator +(Point2D point1, Point2D point2)
        {
            return new(point1.X + point2.X, point1.Y + point2.Y);
        }

        public void Rotate(double theta)
        {
            X = X * Math.Cos(theta) - Y * Math.Sin(theta);
            Y = X * Math.Sin(theta) + Y * Math.Cos(theta);
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
