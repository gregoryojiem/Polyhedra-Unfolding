using System.Text;

namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class PolyhedraFace
    {
        public Point3D[] Vertices { get; set; }
        public double[] Normal { get; set; }

        public PolyhedraFace(ConvexHullFace convexHullFace)
        {
            Vertices = convexHullFace.Vertices;
            Normal = convexHullFace.Normal;
        }

        public PolyhedraFace(Point3D[] points)
        {
            Vertices = points;
            Normal = [1, 1, 1]; //TODO calculate normal
        }

        public PolyhedraFace(PolyhedraFace other)
        {
            Vertices = new Point3D[other.Vertices.Length];
            for (int i = 0; i < other.Vertices.Length; i++)
            {
                Vertices[i] = new Point3D(other.Vertices[i].X, other.Vertices[i].Y, other.Vertices[i].Z);
            }

            Normal = new double[other.Normal.Length];
            Array.Copy(other.Normal, Normal, other.Normal.Length);
        }

        public bool Mergeable(PolyhedraFace otherFace)
        {
            var sharedVertices = this.Vertices.Intersect(otherFace.Vertices).ToArray();
            if (sharedVertices.Length != 2)
            {
                return false;
            }

            return this.Normal.SequenceEqual(otherFace.Normal);
        }

        public PolyhedraFace Merge(PolyhedraFace otherFace)
        {
            var mergedVertices = new HashSet<Point3D>(this.Vertices);
            mergedVertices.UnionWith(otherFace.Vertices);
            return new PolyhedraFace(mergedVertices.ToArray());
        }

        private Point3D GetCentroid()
        {
            int numVertices = Vertices.Length;
            double sumX = 0, sumY = 0, sumZ = 0;
            for (int i = 0; i < numVertices; i++)
            {
                Point3D currVertex = Vertices[i];
                sumX += currVertex.X; sumY += currVertex.Y; sumZ += currVertex.Z;
            }
            return new Point3D(sumX / numVertices, sumY / numVertices, sumZ / numVertices);
        }

        public void TranslateToOrigin()
        {
            var centroid = GetCentroid();
            foreach (Point3D point in Vertices)
            {
                point.Subtract(centroid);
            }
        }

        public void TranslateToPoint(Point3D pointToTranslateTo)
        {
            foreach (Point3D point in Vertices)
            {
                point.Add(pointToTranslateTo);
            }
        }

        public PolyhedraFace Rotate3DToAlign()
        {
            var alignedFace = new PolyhedraFace(this);

            alignedFace.TranslateToOrigin();

            Vec3D planeNormalVec = new(alignedFace.Normal[0], alignedFace.Normal[1], alignedFace.Normal[2]);
            Vec3D targetVector = new(0, 1, 0);

            double angle = Math.Acos(planeNormalVec.Dot(targetVector));
            var rotationAxis = planeNormalVec.Cross(targetVector);
            rotationAxis.Normalize();
            var rotationMatrix = Matrix3D.GetRodriguezRotation(rotationAxis, angle);

            foreach (Point3D point in alignedFace.Vertices)
            {
                point.Rotate(rotationMatrix);
            }

            alignedFace.Normal[0] = 0;
            alignedFace.Normal[1] = 1;
            alignedFace.Normal[2] = 0;
            return alignedFace;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Vertices: {"); 

            for (int i = 0; i < Vertices.Length; i++)
            {
                sb.Append($"({Vertices[i].X}, {Vertices[i].Y}, {Vertices[i].Z})");
                if (i < Vertices.Length - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append("}\n");


            sb.Append("Normal: (");
            for (int i = 0; i < Normal.Length; i++)
            {
                sb.Append(Normal[i]);
                if (i < Normal.Length - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append(")"); 

            return sb.ToString();
        }
    }
}
