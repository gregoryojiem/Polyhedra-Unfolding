using Polyhedra.DataStructs3D;

namespace Polyhedra
{
    public class PolyhedronLibrary
    {
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
                case "Random Polyhedra":
                    points = Point3D.GenerateRandPoints(20, 1);
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

        public static List<string> GetShapeNames()
        {
            return [
                "Triangular Pyramid", "Cube", "Tetrahedron", "Octahedron",
                "Hexagonal Pyramid", "Dodecahedron", "Elongated Square Dipyramid",
                "Random Polyhedra"
            ];
        }
    }
}
