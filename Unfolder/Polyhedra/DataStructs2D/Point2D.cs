namespace Polyhedra.DataStructs2D
{
    public struct Point2D
    {
        public readonly double X { get; }
        public readonly double Y { get; }

        public Point2D(double x, double y) { X = x; Y = y; }

        public static Point2D operator +(Point2D point1, Point2D point2)
        {
            return new(point1.X + point2.X, point1.Y + point2.Y);
        }

        public static Point2D operator -(Point2D point1, Point2D point2)
        {
            return new(point1.X - point2.X, point1.Y - point2.Y);
        }

        public Point2D Rotate(double theta)
        {
            var tempX = X * Math.Cos(theta) - Y * Math.Sin(theta);
            var newY = X * Math.Sin(theta) + Y * Math.Cos(theta);
            var newX = tempX;
            return new Point2D(newX, newY);
        }

        public static Point2D[] SortPoints(Point2D[] points)
        {
            double avgX = points.Average(p => p.X);
            double avgY = points.Average(p => p.Y);

            var anglesAndIndices = points.Select((point, index) =>
            {
                double dx = point.X - avgX;
                double dy = point.Y - avgY;
                double angle = Math.Atan2(dy, dx); 
                return (angle, index, point);
            }).ToList();

            anglesAndIndices.Sort((a, b) => a.angle.CompareTo(b.angle));

            return anglesAndIndices.Select(item => item.point).ToArray();
        }

        //TODO move this to a separate class for globals
        public static bool operator ==(Point2D p1, Point2D p2)
        {
            return Math.Abs(p1.X - p2.X) < 0.0001 && Math.Abs(p1.Y - p2.Y) < 0.0001;
        }

        public static bool operator !=(Point2D p1, Point2D p2)
        {
            return !(p1 == p2);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Point2D other)
            {
                return this == other;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }
    }
}
