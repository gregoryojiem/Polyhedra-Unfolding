using MIConvexHull;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Point3D : IVertex
    {
        private double[] _position;

        public Point3D(double x, double y, double z)
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

        public void Add(Point3D point)
        {
            X = X + point.X;
            Y = Y + point.Y;
            Z = Z + point.Z;
        }

        public void Subtract(Point3D point)
        {
            X = X - point.X;
            Y = Y - point.Y;
            Z = Z - point.Z;
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
    }
}
