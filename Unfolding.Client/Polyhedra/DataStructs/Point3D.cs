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
            _position = new double[] { x, y, z };
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

        public static Point3D[] GenerateRandPoints(int amount, int extent)
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
    }
}
