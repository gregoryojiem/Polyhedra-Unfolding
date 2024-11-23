﻿namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Matrix3D
    {
        private double[] _data;

        public Matrix3D() {
            _data = new double[9];
        }

        public Matrix3D(double[] data)
        {
            if (data.Length != 9)
            {
                throw new ArgumentException("Data array must have length 9.");
            }
            _data = data;
        }

        public double this[int row, int col]
        {
            get => _data[row * 3 + col];
            set => _data[row * 3 + col] = value;
        }

        public static Matrix3D GetIdentityMatrix()
        {
            return new Matrix3D(new double[] { 1, 0, 0, 0, 1, 0, 0, 0, 1 });
        }

        public static Matrix3D operator +(Matrix3D m1, Matrix3D m2)
        {
            double[] resultData = new double[9];
            for (int i = 0; i < 9; i++)
            {
                resultData[i] = m1._data[i] + m2._data[i];
            }
            return new Matrix3D(resultData);
        }

        public static Matrix3D operator *(Matrix3D m1, Matrix3D m2)
        {
            Matrix3D result = new Matrix3D();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        result[i, j] += m1[i, k] * m2[k, j];
                    }
                }
            }
            return result;
        }

        public static Matrix3D GetRodriguezRotation(Vec3D rotationAxis, double angleRadians)
        {
            double cosTheta = Math.Cos(angleRadians);
            double sinTheta = Math.Sin(angleRadians);
            double oneMinusCosTheta = 1 - cosTheta;

            double x = rotationAxis.X;
            double y = rotationAxis.Y;
            double z = rotationAxis.Z;

            var rotationMatrix = new Matrix3D();

            rotationMatrix[0, 0] = cosTheta + oneMinusCosTheta * x * x;
            rotationMatrix[0, 1] = oneMinusCosTheta * x * y - sinTheta * z;
            rotationMatrix[0, 2] = oneMinusCosTheta * x * z + sinTheta * y;

            rotationMatrix[1, 0] = oneMinusCosTheta * y * x + sinTheta * z;
            rotationMatrix[1, 1] = cosTheta + oneMinusCosTheta * y * y;
            rotationMatrix[1, 2] = oneMinusCosTheta * y * z - sinTheta * x;

            rotationMatrix[2, 0] = oneMinusCosTheta * z * x - sinTheta * y;
            rotationMatrix[2, 1] = oneMinusCosTheta * z * y + sinTheta * x;
            rotationMatrix[2, 2] = cosTheta + oneMinusCosTheta * z * z;


            return rotationMatrix;
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    s += this[i, j] + " ";
                }
                s += "\n";
            }
            return s;
        }
    }
}
