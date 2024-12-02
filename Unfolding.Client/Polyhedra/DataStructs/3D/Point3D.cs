using MIConvexHull;

namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Point3D : IVertex
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public double[] Position
        {
            get { return [X, Y, Z]; }
        }

        public Point3D(double x, double y, double z) { 
            X = x; 
            Y = y; 
            Z = z; 
        }

        public static Point3D[] GenerateRandPoints(int amount, double extent)
        {
            var points = new Point3D[amount];
            var rand = new Random(5);

            for (int i = 0; i < amount; i++)
            {
                var x = rand.NextDouble() * extent - 0.5 * extent;
                var y = rand.NextDouble() * extent - 0.5 * extent;
                var z = rand.NextDouble() * extent - 0.5 * extent;
                points[i] = new Point3D(x, y, z);
            }

            return points;
        }

        public void Rotate(Matrix3D matrix)
        {
            double newX = matrix[0, 0] * X + matrix[0, 1] * Y + matrix[0, 2] * Z;
            double newY = matrix[1, 0] * X + matrix[1, 1] * Y + matrix[1, 2] * Z;
            double newZ = matrix[2, 0] * X + matrix[2, 1] * Y + matrix[2, 2] * Z;

            X = newX;
            Y = newY;
            Z = newZ;
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ", " + Z + ")";
        }

        //TODO come up with a more efficient way of handling minor rotation differences?
        public static bool operator ==(Point3D p1, Point3D p2)
        {
            if (ReferenceEquals(p1, p2))
            {
                return true;
            }
            if (p1 is null || p2 is null)
            {
                return false;
            }
            return Math.Abs(p1.X - p2.X) < 0.0001 && Math.Abs(p1.Y - p2.Y) < 0.0001 && Math.Abs(p1.Z - p2.Z) < 0.0001;
        }

        public static bool operator !=(Point3D p1, Point3D p2)
        {
            return !(p1 == p2);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Point3D other)
            {
                return this == other;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }
    }
}
