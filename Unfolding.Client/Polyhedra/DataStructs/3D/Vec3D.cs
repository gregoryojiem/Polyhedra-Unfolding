namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Vec3D 
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public double Magnitude
        {
            get { return Math.Sqrt(X * X + Y * Y + Z * Z); }
        }

        public Vec3D(double x, double y, double z) { X = x; Y = y; Z = z; }

        public void Normalize()
        {
            X /= Magnitude;
            Y /= Magnitude;
            Z /= Magnitude;
        }

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
