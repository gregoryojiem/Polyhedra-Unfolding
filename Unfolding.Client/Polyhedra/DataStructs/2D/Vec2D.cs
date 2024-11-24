namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Vec2D
    {
        private double[] _position;
        public double X
        {
            get { return _position[0]; }
            set { _position[0] = value; }
        }

        public double Y
        {
            get { return _position[1]; }
            set { _position[1] = value; }
        }

        public double Magnitude
        {
            get { return Math.Sqrt(X * X + Y * Y); }
        }

        public Vec2D(double x, double y)
        {
            _position = [x, y];
        }

        public double Dot(Vec2D vec)
        {
            return (X * vec.X) + (Y * vec.Y);
        }
    }
}
