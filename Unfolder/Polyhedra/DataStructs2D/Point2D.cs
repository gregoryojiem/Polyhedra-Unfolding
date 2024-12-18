﻿namespace Polyhedra.DataStructs2D
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

        public static Point2D operator -(Point2D point1, Point2D point2)
        {
            return new(point1.X - point2.X, point1.Y - point2.Y);
        }

        public void Rotate(double theta)
        {
            var tempX = X * Math.Cos(theta) - Y * Math.Sin(theta);
            Y = X * Math.Sin(theta) + Y * Math.Cos(theta);
            X = tempX;
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

        public Vec2D ToVector()
        {
            return new Vec2D(X, Y);
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }

        //TODO come up with a more efficient way of handling minor rotation differences?
        public static bool operator ==(Point2D p1, Point2D p2)
        {
            if (ReferenceEquals(p1, p2))
            {
                return true;
            }
            if (p1 is null || p2 is null)
            {
                return false;
            }
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
    }
}
