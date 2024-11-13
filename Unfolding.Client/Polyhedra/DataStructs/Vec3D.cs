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
    }
}
