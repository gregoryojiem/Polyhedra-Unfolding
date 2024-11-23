namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Vec3D
    {
        private double[] _position;
        public Vec3D(double x, double y, double z)
        {
            _position = [x, y, z];
        }

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

        public double Z
        {
            get { return _position[2]; }
            set { _position[2] = value; }
        }

        public double[] Position => _position;

        public double Dot(Vec3D vec)
        {
            return (X * vec.X) + (Y * vec.Y) + (Z * vec.Z);
        }

        public Vec3D Cross(Vec3D vec)
        {
            return new Vec3D(Y * vec.Z - Z * vec.Y, -(X * vec.Z - Z * vec.X), X * vec.Y - Y * vec.X);
        }

        public void Rotate(double theta, String dimension)
        {
            Vec3D[] rotMatrix;
            if (dimension == "x")
            {
                rotMatrix = [new Vec3D(1, 0, 0), new Vec3D(0, Math.Cos(theta), -Math.Sin(theta)), new Vec3D(0, Math.Sin(theta), Math.Cos(theta))];
            }
            else if (dimension == "y")
            {
                rotMatrix = [new Vec3D(Math.Cos(theta), 0, Math.Sin(theta)), new Vec3D(0, 1, 0), new Vec3D(-Math.Sin(theta), 0, Math.Cos(theta))];
            }
            else
            {
                rotMatrix = [new Vec3D(Math.Cos(theta), -Math.Sin(theta), 0), new Vec3D(Math.Sin(theta), Math.Cos(theta), 0), new Vec3D(0, 0, 1)];
            }

        }
    }
}
