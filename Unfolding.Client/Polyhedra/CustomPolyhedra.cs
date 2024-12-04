using Unfolding.Client.Polyhedra.DataStructs;

namespace Unfolding.Client.Polyhedra
{
    public class CustomPolyhedra
    {
        // Set test shapes
        public static Point3D[] triangularPyramid =
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
        public static Point3D[] tetrahedron =
            [
                new(0, 0.5, 0),
                new(0.5, -0.5, 0),
                new(Math.Sin(60), -0.5, 0.5),
                new(Math.Sin(60), -0.5, -0.5)
            ];
        public static Point3D[] elongatedSquareDipyramid =
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
        public static Point3D[] octahedron =
            [
                new(0, 0.5, 0),
                new(-1/(2*Math.Sqrt(2)), 0, -1/(2*Math.Sqrt(2))),
                new(-1/(2*Math.Sqrt(2)), 0, 1/(2*Math.Sqrt(2))),
                new(1/(2*Math.Sqrt(2)), 0, -1/(2*Math.Sqrt(2))),
                new(1/(2*Math.Sqrt(2)), 0, 1/(2*Math.Sqrt(2))),
                new(0, -0.5, 0)
            ];
        public static Point3D[] hexagonalPyramid =
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

        public static Point3D[] dodecahedron =
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
    }
}
