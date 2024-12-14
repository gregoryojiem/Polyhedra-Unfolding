﻿using Polyhedra.DataStructs3D;

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
        private static Point3D[] icosahedron =
            [
                new(0, 0, -0.951057),
                new(0, 0, 0.951057),
                new(-0.850651, 0, -0.425325),
                new(0.850651, 0, 0.425325),
                new(0.688191, -0.5, -0.425325),
                new(0.688191, 0.5, -0.425325),
                new(-0.688191, -0.5, 0.425325),
                new(-0.688191, 0.5, 0.425325),
                new(-0.262866, -0.809017, -0.425325),
                new(-0.262866, 0.809017, -0.425325),
                new(0.262866, -0.809017, 0.425325),
                new(0.262866, 0.809017, 0.42532)
            ];
        private static Point3D[] truncatedTetrahedron =
            [
                new(0, -1, -0.612372),
                new(0, 1, -0.612372),
                new(-0.57735, -1, 0.204124),
                new(-0.57735, 1, 0.204124),
                new(-0.288675, -0.5, 1.02062),
                new(-0.288675, 0.5, 1.02062),
                new(0.57735, 0, 1.02062),
                new(1.1547, 0, 0.204124),
                new(-0.866025, -0.5, -0.612372),
                new(-0.866025, 0.5, -0.612372),
                new(0.866025, -0.5, -0.612372),
                new(0.866025, 0.5, -0.612372)
            ];
        private static Point3D[] cuboctahedron =
            [
                new(-1, 0, 0),
                new(-0.5, -0.5, -0.707107),
                new(-0.5, -0.5, 0.707107),
                new(-0.5, 0.5, -0.707107),
                new(-0.5, 0.5, 0.707107),
                new(0, -1, 0),
                new(0, 1, 0),
                new(0.5, -0.5, -0.707107),
                new(0.5, -0.5, 0.707107),
                new(0.5, 0.5, -0.707107),
                new(0.5, 0.5, 0.707107),
                new(1, 0, 0)
            ];
        private static Point3D[] truncatedCube =
            [
                new(-0.5, 1.20711, 1.20711),
                new(-0.5, 1.20711, -1.20711),
                new(-0.5, -1.20711, 1.20711),
                new(-0.5, -1.20711, -1.20711),
                new(0.5, 1.20711, 1.20711),
                new(0.5, 1.20711, -1.20711),
                new(0.5, -1.20711, 1.20711),
                new(0.5, -1.20711, -1.20711),
                new(1.20711, -0.5, 1.20711),
                new(1.20711, -0.5, -1.20711),
                new(1.20711, 0.5, 1.20711),
                new(1.20711, 0.5, -1.20711),
                new(1.20711, 1.20711, -0.5),
                new(1.20711, 1.20711, 0.5),
                new(1.20711, -1.20711, -0.5),
                new(1.20711, -1.20711, 0.5),
                new(-1.20711, -0.5, 1.20711),
                new(-1.20711, -0.5, -1.20711),
                new(-1.20711, 0.5, 1.20711),
                new(-1.20711, 0.5, -1.20711),
                new(-1.20711, 1.20711, -0.5),
                new(-1.20711, 1.20711, 0.5),
                new(-1.20711, -1.20711, -0.5),
                new(-1.20711, -1.20711, 0.5),
            ];
        private static Point3D[] truncatedOctahedron =
            [
                new(-1.5, -0.5, 0),
                new(-1.5, 0.5, 0),
                new(-1, -1, -0.707107),
                new(-1, -1, 0.707107),
                new(-1, 1, -0.707107),
                new(-1, 1, 0.707107),
                new(-0.5, -1.5, 0),
                new(-0.5, -0.5, -1.41421),
                new(-0.5, -0.5, 1.41421),
                new(-0.5, 0.5, -1.41421),
                new(-0.5, 0.5, 1.41421),
                new(-0.5, 1.5, 0),
                new(0.5, -1.5, 0),
                new(0.5, -0.5, -1.41421),
                new(0.5, -0.5, 1.41421),
                new(0.5, 0.5, -1.41421),
                new(0.5, 0.5, 1.41421),
                new(0.5, 1.5, 0),
                new(1, -1, -0.707107),
                new(1, -1, 0.707107),
                new(1, 1, -0.707107),
                new(1, 1, 0.707107),
                new(1.5, -0.5, 0),
                new(1.5, 0.5, 0),
            ];
        private static Point3D[] Rhombicuboctahedron =
            [

            ];
        private static Point3D[] truncatedCuboctahedron =
            [
                new(-0.5, 1.20711, -1.91421),
                new(-0.5, 1.20711, 1.91421),
                new(-0.5, -1.20711, -1.91421),
                new(-0.5, -1.20711, 1.91421),
                new(-0.5, -1.91421, 1.20711),
                new(-0.5, -1.91421, -1.20711),
                new(-0.5, 1.91421, 1.20711),
                new(-0.5, 1.91421, -1.20711),
                new(0.5, 1.20711, -1.91421),
                new(0.5, 1.20711, 1.91421),
                new(0.5, -1.20711, -1.91421),
                new(0.5, -1.20711, 1.91421),
                new(0.5, -1.91421, 1.20711),
                new(0.5, -1.91421, -1.20711),
                new(0.5, 1.91421, 1.20711),
                new(0.5, 1.91421, -1.20711),
                new(1.20711, -0.5, -1.91421),
                new(1.20711, -0.5, 1.91421),
                new(1.20711, 0.5, -1.91421),
                new(1.20711, 0.5, 1.91421),
                new(1.20711, -1.91421, -0.5),
                new(1.20711, -1.91421, 0.5),
                new(1.20711, 1.91421, -0.5),
                new(1.20711, 1.91421, 0.5),
                new(-1.20711, -0.5, -1.91421),
                new(-1.20711, -0.5, 1.91421),
                new(-1.20711, 0.5, -1.91421),
                new(-1.20711, 0.5, 1.91421),
                new(-1.20711, -1.91421, -0.5),
                new(-1.20711, -1.91421, 0.5),
                new(-1.20711, 1.91421, -0.5),
                new(-1.20711, 1.91421, 0.5),
                new(-1.91421, -0.5, 1.20711),
                new(-1.91421, -0.5, -1.20711),
                new(-1.91421, 0.5, 1.20711),
                new(-1.91421, 0.5, -1.20711),
                new(-1.91421, 1.20711, -0.5),
                new(-1.91421, 1.20711, 0.5),
                new(-1.91421, -1.20711, -0.5),
                new(-1.91421, -1.20711, 0.5),
                new(1.91421, -0.5, 1.20711),
                new(1.91421, -0.5, -1.20711),
                new(1.91421, 0.5, 1.20711),
                new(1.91421, 0.5, -1.20711),
                new(1.91421, 1.20711, -0.5),
                new(1.91421, 1.20711, 0.5),
                new(1.91421, -1.20711, -0.5),
                new(1.91421, -1.20711, 0.5)
            ];
        private static Point3D[] snubCube =
            [
                new(-1.14261, -0.337754, -0.621226),
                new(-1.14261, 0.337754, 0.621226),
                new(-1.14261, -0.621226, 0.337754),
                new(-1.14261, 0.621226, -0.337754),
                new(1.14261, -0.337754, 0.621226),
                new(1.14261, 0.337754, -0.621226),
                new(1.14261, -0.621226, -0.337754),
                new(1.14261, 0.621226, 0.337754),
                new(-0.337754, -1.14261, 0.621226),
                new(-0.337754, 1.14261, -0.621226),
                new(-0.337754, -0.621226, -1.14261),
                new(-0.337754, 0.621226, 1.14261),
                new(0.337754, -1.14261, -0.621226),
                new(0.337754, 1.14261, 0.621226),
                new(0.337754, -0.621226, 1.14261),
                new(0.337754, 0.621226, -1.14261),
                new(-0.621226, -1.14261, -0.337754),
                new(-0.621226, 1.14261, 0.337754),
                new(-0.621226, -0.337754, 1.14261),
                new(-0.621226, 0.337754, -1.14261),
                new(0.621226, -1.14261, 0.337754),
                new(0.621226, 1.14261, -0.337754),
                new(0.621226, -0.337754, -1.14261),
                new(0.621226, 0.337754, 1.14261),
            ];
        private static Point3D[] icosidodecahedron =
            [
                new(0, -1.61803, 0),
                new(0, 1.61803, 0),
                new(0.262866, -0.809017, -1.37638),
                new(0.262866, 0.809017, -1.37638),
                new(0.425325, -1.30902, 0.850651),
                new(0.425325, 1.30902, 0.850651),
                new(0.688191, -0.5, 1.37638),
                new(0.688191, 0.5, 1.37638),
                new(1.11352, -0.809017, -0.850651),
                new(1.11352, 0.809017, -0.850651),
                new(-1.37638, 0, -0.850651),
                new(-0.688191, -0.5, -1.37638),
                new(-0.688191, 0.5, -1.37638),
                new(1.37638, 0, 0.850651),
                new(0.951057, -1.30902, 0),
                new(0.951057, 1.30902, 0),
                new(0.850651, 0, -1.37638),
                new(-0.951057, -1.30902, 0),
                new(-0.951057, 1.30902, 0),
                new(-1.53884, -0.5, 0),
                new(-1.53884, 0.5, 0),
                new(1.53884, -0.5, 0),
                new(1.53884, 0.5, 0),
                new(-0.850651, 0, 1.37638),
                new(-1.11352, -0.809017, 0.850651),
                new(-1.11352, 0.809017, 0.850651),
                new(-0.425325, -1.30902, -0.850651),
                new(-0.425325, 1.30902, -0.850651),
                new(-0.262866, -0.809017, 1.37638),
                new(-0.262866, 0.809017, 1.37638)
            ];
        private static Point3D[] truncatedDodecahedron =
            [
                new(0, -1.61803, 2.4899),
                new(0, -1.61803, -2.4899),
                new(0, 1.61803, 2.4899),
                new(0, 1.61803, -2.4899),
                new(0.425325, -2.92705, 0.262866),
                new(0.425325, 2.92705, 0.262866),
                new(0.688191, -2.11803, 1.96417),
                new(0.688191, 2.11803, 1.96417),
                new(-2.75276, 0, -1.11352),
                new(-2.06457, -2.11803, 0.262866),
                new(-2.06457, 2.11803, 0.262866),
                new(-1.37638, -2.61803, -0.262866),
                new(-1.37638, 2.61803, -0.262866),
                new(-0.688191, -2.11803, -1.96417),
                new(-0.688191, 2.11803, -1.96417),
                new(1.37638, -2.61803, 0.262866),
                new(1.37638, 2.61803, 0.262866),
                new(2.75276, 0, 1.11352),
                new(1.80171, -1.30902, -1.96417),
                new(1.80171, 1.30902, -1.96417),
                new(2.06457, -2.11803, -0.262866),
                new(2.06457, 2.11803, -0.262866),
                new(2.22703, 0, 1.96417),
                new(2.22703, -1.61803, -1.11352),
                new(2.22703, 1.61803, -1.11352),
                new(-2.65236, -1.30902, 0.262866),
                new(-2.65236, 1.30902, 0.262866),
                new(2.65236, -1.30902, -0.262866),
                new(2.65236, 1.30902, -0.262866),
                new(2.91522, -0.5, 0.262866),
                new(2.91522, 0.5, 0.262866),
                new(-2.91522, -0.5, -0.262866),
                new(-2.91522, 0.5, -0.262866),
                new(0.951057, -1.30902, 2.4899),
                new(0.951057, -1.30902, -2.4899),
                new(0.951057, 1.30902, 2.4899),
                new(0.951057, 1.30902, -2.4899),
                new(0.850651, -2.61803, 1.11352),
                new(0.850651, 2.61803, 1.11352),
                new(-0.951057, -1.30902, 2.4899),
                new(-0.951057, -1.30902, -2.4899),
                new(-0.951057, 1.30902, 2.4899),
                new(-0.951057, 1.30902, -2.4899),
                new(-1.53884, -0.5, 2.4899),
                new(-1.53884, -0.5, -2.4899),
                new(-1.53884, 0.5, 2.4899),
                new(-1.53884, 0.5, -2.4899),
                new(1.53884, -0.5, 2.4899),
                new(1.53884, -0.5, -2.4899),
                new(1.53884, 0.5, 2.4899),
                new(1.53884, 0.5, -2.4899),
                new(-2.22703, 0, -1.96417),
                new(-2.22703, -1.61803, 1.11352),
                new(-2.22703, 1.61803, 1.11352),
                new(-0.850651, -2.61803, -1.11352),
                new(-0.850651, 2.61803, -1.11352),
                new(-1.80171, -1.30902, 1.96417),
                new(-1.80171, 1.30902, 1.96417),
                new(-0.425325, -2.92705, -0.262866),
                new(-0.425325, 2.92705, -0.262866)
            ];
        private static Point3D[] truncatedIcosahedron =
            [
                new(-0.16246, -2.11803, 1.27598),
                new(-0.16246, 2.11803, 1.27598),
                new( 0.16246, -2.11803, -1.27598),
                new( 0.16246, 2.11803, -1.27598),
                new( -0.262866, -0.809017, -2.32744),
                new( -0.262866, -2.42705, -0.425325),
                new( -0.262866, 0.809017, -2.32744),
                new( -0.262866, 2.42705, -0.425325),
                new( 0.262866, -0.809017, 2.32744),
                new( 0.262866, -2.42705, 0.425325),
                new( 0.262866, 0.809017, 2.32744),
                new( 0.262866, 2.42705, 0.425325),
                new( 0.688191, -0.5, -2.32744),
                new( 0.688191, 0.5, -2.32744),
                new( 1.21392, -2.11803, 0.425325),
                new( 1.21392, 2.11803, 0.425325),
                new( -2.06457, -0.5, 1.27598),
                new( -2.06457, 0.5, 1.27598),
                new( -1.37638, -1, 1.80171),
                new( -1.37638, 1, 1.80171),
                new( -1.37638, -1.61803, -1.27598),
                new( -1.37638, 1.61803, -1.27598),
                new( -0.688191, -0.5, 2.32744),
                new( -0.688191, 0.5, 2.32744),
                new( 1.37638, -1, -1.80171),
                new( 1.37638, 1, -1.80171),
                new( 1.37638, -1.61803, 1.27598),
                new( 1.37638, 1.61803, 1.27598),
                new( -1.7013, 0, -1.80171),
                new( 1.7013, 0, 1.80171),
                new( -1.21392, -2.11803, -0.425325),
                new( -1.21392, 2.11803, -0.425325),
                new( -1.96417, -0.809017, -1.27598),
                new( -1.96417, 0.809017, -1.27598),
                new( 2.06457, -0.5, -1.27598),
                new( 2.06457, 0.5, -1.27598),
                new( 2.22703, -1, -0.425325),
                new( 2.22703, 1, -0.425325),
                new( 2.38949, -0.5, 0.425325),
                new( 2.38949, 0.5, 0.425325),
                new( -1.11352, -1.80902, 1.27598),
                new( -1.11352, 1.80902, 1.27598),
                new( 1.11352, -1.80902, -1.27598),
                new( 1.11352, 1.80902, -1.27598),
                new( -2.38949, -0.5, -0.425325),
                new( -2.38949, 0.5, -0.425325),
                new( -1.63925, -1.80902, 0.425325),
                new( -1.63925, 1.80902, 0.425325),
                new( 1.63925, -1.80902, -0.425325),
                new( 1.63925, 1.80902, -0.425325),
                new( 1.96417, -0.809017, 1.27598),
                new( 1.96417, 0.809017, 1.27598),
                new( 0.850651, 0, 2.32744),
                new( -2.22703, -1, 0.425325),
                new( -2.22703, 1, 0.425325),
                new( -0.850651, 0, -2.32744),
                new( -0.525731, -1.61803, -1.80171),
                new( -0.525731, 1.61803, -1.80171),
                new( 0.525731, -1.61803, 1.80171),
                new( 0.525731, 1.61803, 1.80171)
            ];
        private static Point3D[] stuff =
            [

            ];


        private static double phi = (1 + Math.Sqrt(5)) / 2; // Golden Ratio
        private static double sqrt2 = Math.Sqrt(2);
        private static double sqrt3 = Math.Sqrt(3);
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
                case "Icosahedron":
                    points = icosahedron;
                    break;
                case "Truncated Tetrahedron":
                    points = truncatedTetrahedron;
                    break;
                case "Cuboctahedron":
                    points = cuboctahedron;
                    break;
                case "Truncated Cube":
                    points = truncatedCube;
                    break;
                case "Truncated Octahedron":
                    points = truncatedOctahedron;
                    break;
                case "Rhombicuboctahedron":
                    points = Rhombicuboctahedron;
                    break;
                case "Truncated Cuboctahedron":
                    points = truncatedCuboctahedron;
                    break;
                case "Snub Cube":
                    points = snubCube;
                    break;
                case "Icosidodecahedron":
                    points = icosidodecahedron;
                    break;
                case "Truncated Dodecahedron":
                    points = truncatedDodecahedron;
                    break;
                case "Truncated Icosahedron":
                    points = truncatedIcosahedron;
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
                //"Tetrahedron", "Cube", "Octahedron", "Dodecahedron", "Icosahedron",
                "Truncated Tetrahedron", "Cuboctahedron", "Truncated Cube", "Truncated Octahedron", "Rhombicuboctahedron", "Snub Cube", "Icosidodecahedron", "Truncated Dodecahedron",
                "Elongated Square Dipyramid", "New random shape"
            ];
        }
    }
}
