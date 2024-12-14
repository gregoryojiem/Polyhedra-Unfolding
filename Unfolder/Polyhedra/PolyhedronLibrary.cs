using Polyhedra.DataStructs3D;

namespace Unfolder.Polyhedra
{
    public class PolyhedronLibrary
    {
        public static int sphereRefinement = 10;

        // Set test shapes
        private static Point3D[] triangularPyramid =
            [
                new(-0.5, -0.5, -0.5),
                new(0.5, -0.5, -0.5),
                new(0.5, -0.5, 0.5),
                new(-0.5, -0.5, 0.5),
                new(0, 0.5, 0)
            ];
        public static Point3D[] cube =
            [
                new(-0.5, -0.5, -0.5),
                new(-0.5, -0.5, 0.5),
                new(-0.5, 0.5, -0.5),
                new(-0.5, 0.5, 0.5),
                new(0.5, -0.5, -0.5),
                new(0.5, -0.5, 0.5),
                new(0.5, 0.5, -0.5),
                new(0.5, 0.5, 0.5)
            ];
        private static Point3D[] tetrahedron =
            [
                new(0, 0.5, 0),
                new(0.5, -0.5, 0),
                new(Math.Sin(60), -0.5, 0.5),
                new(Math.Sin(60), -0.5, -0.5)
            ];
        private static Point3D[] elongatedSquareDipyramid =
            [
                new(0, -1.5, 0),
                new(-0.5, -0.5, -0.5),
                new(-0.5, -0.5, 0.5),
                new(-0.5, 0.5, -0.5),
                new(-0.5, 0.5, 0.5),
                new(0.5, -0.5, -0.5),
                new(0.5, -0.5, 0.5),
                new(0.5, 0.5, -0.5),
                new(0.5, 0.5, 0.5),
                new(0, 1.5, 0)
            ];
        private static Point3D[] octahedron =
            [
                new(0, 0.5, 0),
                new(-1/(2*Math.Sqrt(2)), 0, -1/(2*Math.Sqrt(2))),
                new(-1/(2*Math.Sqrt(2)), 0, 1/(2*Math.Sqrt(2))),
                new(1/(2*Math.Sqrt(2)), 0, -1/(2*Math.Sqrt(2))),
                new(1/(2*Math.Sqrt(2)), 0, 1/(2*Math.Sqrt(2))),
                new(0, -0.5, 0)
            ];
        private static Point3D[] hexagonalPyramid =
            [
                new(0, 0.5*Math.Sqrt(3)/2, 0),
                new(-0.5, -0.5*Math.Sqrt(3)/2, 0),
                new(0.5, -0.5*Math.Sqrt(3)/2, 0),
                new(-0.25, -0.5*Math.Sqrt(3)/2, -0.25*Math.Sqrt(3)),
                new(-0.25, -0.5*Math.Sqrt(3)/2, 0.25*Math.Sqrt(3)),
                new(0.25, -0.5*Math.Sqrt(3)/2, -0.25*Math.Sqrt(3)),
                new(0.25, -0.5*Math.Sqrt(3)/2, 0.25*Math.Sqrt(3))
            ];

        private static double phi = (1 + Math.Sqrt(5)) / 2;
        private static Point3D[] dodecahedron =
        [
            new(1, 1, 1),
            new(1, 1, -1),
            new(1, -1, 1),
            new(1, -1, -1),
            new(-1, 1, 1),
            new(-1, 1, -1),
            new(-1, -1, 1),
            new(-1, -1, -1),
            new(0, 1/phi, phi),
            new(0, 1/phi, -phi),
            new(0, -1/phi, phi),
            new(0, -1/phi, -phi),
            new(1/phi, phi, 0),
            new(1/phi, -phi, 0),
            new(-1/phi, phi, 0),
            new(-1/phi, -phi, 0),
            new(phi, 0, 1/phi),
            new(-phi, 0, 1/phi),
            new(phi, 0, -1/phi),
            new(-phi, 0, -1/phi)
        ];

        public static Polyhedron GetPolyhedron(string polyhedra)
        {
            Point3D[] points;
            switch (polyhedra)
            {
                case "Triangular Pyramid":
                    points = triangularPyramid;
                    break;
                case "Cube":
                    points = cube;
                    break;
                case "Tetrahedron":
                    points = tetrahedron;
                    break;
                case "Octahedron":
                    points = octahedron;
                    break;
                case "Hexagonal Pyramid":
                    points = hexagonalPyramid;
                    break;
                case "Dodecahedron":
                    points = ScalePoints(dodecahedron, 0.5);
                    break;
                case "Elongated Square Dipyramid":
                    points = ScalePoints(elongatedSquareDipyramid, 0.75);
                    break;
                case "Sphere":
                    points = GetSpherePoints(sphereRefinement, sphereRefinement, 0.75);
                    break;
                case "New random shape":
                    points = Point3D.GenerateRandPoints(10000, 1);
                    break;
                default: throw new InvalidDataException();
            }
            return new Polyhedron(points);
        }

        public static Point3D[] ScalePoints(Point3D[] points, double scale)
        {
            var scaledPoints = new Point3D[points.Length];

            for (int i = 0; i < points.Length; i++)
            {
                scaledPoints[i] = new Point3D(
                    points[i].X * scale,
                    points[i].Y * scale,
                    points[i].Z * scale);
            }

            return scaledPoints;
        }

        public static Point3D[] GetSpherePoints(int slices, int stacks, double radius)
        {
            var spherePoints = new List<Point3D>();

            for (var stack = 1; stack < stacks; stack++)
            {
                var phi = Math.PI * stack / stacks;

                for (var slice = 0; slice < slices; slice++)
                {
                    var theta = 2 * Math.PI * slice / slices;
                    var x = radius * Math.Sin(phi) * Math.Cos(theta);
                    var y = radius * Math.Sin(phi) * Math.Sin(theta);
                    var z = radius * Math.Cos(phi);
                    spherePoints.Add(new Point3D(x, y, z));
                }
            }

            spherePoints.Add(new Point3D(0, 0, radius));
            spherePoints.Add(new Point3D(0, 0, -radius));
            return spherePoints.ToArray();
        }

        public static Polyhedron GetSphere(int slices, int stacks, double radius)
        {
            var points = GetSpherePoints(slices, stacks, radius);
            return new Polyhedron(points);
        }

        public static Polyhedron GetRandomPolyhedron(int numOfPoints, double radius)
        {
            var points = Point3D.GenerateRandPoints(numOfPoints, radius);
            return new Polyhedron(points);
        }

        public static List<string> GetShapeNames()
        {
            return [
                "Triangular Pyramid", "Cube", "Tetrahedron", "Octahedron",
                "Hexagonal Pyramid", "Dodecahedron", "Elongated Square Dipyramid",
                "New random shape"
            ];
        }
    }
}
