﻿namespace PolyhedraUnfolding.DataStructs2D
{
    public struct Vec2D
    {
        public readonly double X;
        public readonly double Y;

        public double Magnitude
        {
            get { return Math.Sqrt(X * X + Y * Y); }
        }

        public Vec2D(double x, double y) { 
            X = x; 
            Y = y; 
        }

        public Vec2D(Point2D point) { 
            X = point.X; 
            Y = point.Y; 
        }

        public static Vec2D operator +(Vec2D vec1, Vec2D vec2)
        {
            return new(vec1.X + vec2.X, vec1.Y + vec2.Y);
        }

        public static Vec2D operator +(Vec2D vec1, Point2D p2)
        {
            return new(vec1.X + p2.X, vec1.Y + p2.Y);
        }

        public static Vec2D operator -(Vec2D vec1, Vec2D vec2)
        {
            return new(vec1.X - vec2.X, vec1.Y - vec2.Y);
        }

        public static Vec2D operator -(Vec2D vec1, Point2D p2)
        {
            return new(vec1.X - p2.X, vec1.Y - p2.Y);
        }

        public static Vec2D operator *(Vec2D vec1, double number)
        {
            return new(vec1.X * number, vec1.Y * number);
        }

        public double Dot(Vec2D vec)
        {
            return (X * vec.X) + (Y * vec.Y);
        }

        public double Cross(Vec2D vec)
        {
            return (X * vec.Y) - (Y * vec.X);
        }

        public double FindAngleBetween(Vec2D otherVector, bool invertForCross)
        {
            var cosOfTheta = Dot(otherVector) / (Magnitude * otherVector.Magnitude);
            var angle = Math.Acos(Math.Clamp(cosOfTheta, -1, 1));
            var crossProduct = Cross(otherVector * (invertForCross ? -1 : 1));
            if (crossProduct < 0)
            {
                angle = 2 * Math.PI - angle;
            }
            return angle;
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }
    }
}
