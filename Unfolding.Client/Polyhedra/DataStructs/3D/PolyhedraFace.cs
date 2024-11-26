using System.Drawing;
using System.Text;
using System.Text.Json.Serialization;

namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class PolyhedraFace
    {
        public Point3D[] Vertices { get; set; }
        public double[] Normal { get; set; }

        [JsonIgnore]
        public List<PolyhedraFace> Adjacency { get; set; }

        public PolyhedraFace(ConvexHullFace convexHullFace)
        {
            Vertices = convexHullFace.Vertices;
            Normal = convexHullFace.Normal;
            Adjacency = new List<PolyhedraFace>();
        }

        public PolyhedraFace(Point3D[] points, double[] normal)
        {
            Vertices = points;
            Normal = normal;
            Adjacency = new List<PolyhedraFace>();
        }

        public bool Mergeable(PolyhedraFace otherFace)
        {
            var sharedVertices = this.Vertices.Intersect(otherFace.Vertices).ToArray();
            if (sharedVertices.Length != 2)
            {
                return false;
            }

            return Normal.SequenceEqual(otherFace.Normal);
        }

        public PolyhedraFace Merge(PolyhedraFace otherFace)
        {
            var mergedVertices = new HashSet<Point3D>(Vertices);
            mergedVertices.UnionWith(otherFace.Vertices);
            return new PolyhedraFace(mergedVertices.ToArray(), Normal);
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
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].X -= centroid.X;
                Vertices[i].Y -= centroid.Y;
                Vertices[i].Z -= centroid.Z;
            }
        }

        public void Rotate3DToAlign()
        {
            // TODO this should be done elsewhere
            Point3D[] newVertices = new Point3D[Vertices.Length];
            for (int i = 0; i < Vertices.Length; i++)
            {
                newVertices[i] = new(Vertices[i].X, Vertices[i].Y, Vertices[i].Z);
            }
            Vertices = newVertices;

            TranslateToOrigin();
            double angle;
            Vec3D rotationAxis;
            if (Normal.SequenceEqual([0, -1, 0]))
            {
                return;
            }
            else if (Normal.SequenceEqual([0, 1, 0]))
            {
                angle = Math.PI;
                rotationAxis = new(1, 0, 0);
            }
            else
            {
                Vec3D planeNormalVec = new(Normal[0], Normal[1], Normal[2]);
                Vec3D targetVector = new(0, -1, 0);

                angle = Math.Acos(planeNormalVec.Dot(targetVector));
                rotationAxis = planeNormalVec.Cross(targetVector);
                rotationAxis.Normalize();
            }
            var rotationMatrix = Matrix3D.GetRodriguezRotation(rotationAxis, angle);

            foreach (Point3D point in Vertices)
            {
                point.Rotate(rotationMatrix);
            }

            Normal[0] = 0;
            Normal[1] = -1;
            Normal[2] = 0;
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

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            PolyhedraFace other = (PolyhedraFace)obj;

            if (Vertices.Length != other.Vertices.Length || Normal.Length != other.Normal.Length)
            {
                return false;
            }

            if (!Vertices.SequenceEqual(other.Vertices))
            {
                return false;
            }

            if (!Normal.SequenceEqual(other.Normal))
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 17;

            foreach (var vertex in Vertices)
            {
                hash = hash * 31 + vertex.GetHashCode();
            }

            return hash;
        }
    }
}
