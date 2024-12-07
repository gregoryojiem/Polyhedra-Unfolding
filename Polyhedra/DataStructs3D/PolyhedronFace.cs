using System.Text;
using System.Text.Json.Serialization;

namespace Polyhedra.DataStructs3D
{
    public class PolyhedronFace
    {
        public Point3D[] Vertices { get; set; }
        public double[] Normal { get; set; }

        [JsonIgnore]
        public List<Edge3D> Adjacency { get; set; }

        public int Id = 0;

        public PolyhedronFace(ConvexHullFace convexHullFace)
        {
            var convHullVertices = convexHullFace.Vertices;
            Vertices = new Point3D[convHullVertices.Length];
            for (int i = 0; i < convHullVertices.Length; i++)
            {
                Vertices[i] = new Point3D(convHullVertices[i].X, convHullVertices[i].Y, convHullVertices[i].Z);
            }
            Normal = convexHullFace.Normal;
            Adjacency = [];
        }

        public PolyhedronFace(Point3D[] points, double[] normal, List<Edge3D> adjacency)
        {
            Vertices = points;
            Normal = normal;
            Adjacency = adjacency;
        }

        public PolyhedronFace(PolyhedronFace face)
        {
            var vertices = face.Vertices;
            Vertices = new Point3D[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                Vertices[i] = new Point3D(vertices[i].X, vertices[i].Y, vertices[i].Z);
            }
            Normal = (double[])face.Normal.Clone();
            Adjacency = [];
            Id = face.Id;
        }
        
        public void CopyAdjacency(Polyhedron copyPolyhedron, Polyhedron oldPolyhedron)
        {
            var thisFaceIndex = Array.IndexOf(copyPolyhedron.Faces, this);
            var oldFace = oldPolyhedron.Faces[thisFaceIndex];
            
            foreach (var edge in oldFace.Adjacency)
            {
                var keyIndex = Array.IndexOf(oldPolyhedron.Faces, edge.ConnectedFace);
                var matchingAdjacentFace = copyPolyhedron.Faces[keyIndex];
                Adjacency.Add(new Edge3D(this, matchingAdjacentFace));
            }
        }

        public bool Mergeable(PolyhedronFace otherFace)
        {
            var sharedVertices = Vertices.Intersect(otherFace.Vertices).ToArray();
            if (sharedVertices.Length != 2)
            {
                return false;
            }

            return Math.Abs(Normal[0] - otherFace.Normal[0]) < 0.0001 && 
                Math.Abs(Normal[1] - otherFace.Normal[1]) < 0.0001 && 
                Math.Abs(Normal[2] - otherFace.Normal[2]) < 0.0001;
        }

        public PolyhedronFace Merge(PolyhedronFace otherFace)
        {
            var mergedVertices = new HashSet<Point3D>(Vertices);
            mergedVertices.UnionWith(otherFace.Vertices);

            var mergedAdjacency = new List<Edge3D>();
            mergedAdjacency.AddRange(Adjacency);
            mergedAdjacency.AddRange(otherFace.Adjacency);
            mergedAdjacency = mergedAdjacency.Distinct().ToList();
            mergedAdjacency.RemoveAll(edge => edge.ConnectedFace == this || edge.ConnectedFace == otherFace);

            return new PolyhedronFace(mergedVertices.ToArray(), (double[])Normal.Clone(), mergedAdjacency);
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
            TranslateToOrigin();

            if (Normal.SequenceEqual([0, -1, 0]))
            {
                return;
            }
            if (Normal.SequenceEqual([0, 1, 0]))
            {
                Normal = [0, -1, 0];
                return;
            }

            Vec3D planeNormalVec = new(Normal[0], Normal[1], Normal[2]);
            Vec3D targetVector = new(0, -1, 0);

            double angle = Math.Acos(planeNormalVec.Dot(targetVector));
            var rotationAxis = planeNormalVec.Cross(targetVector);
            rotationAxis.Normalize();
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

            return Id + " with " + Vertices.Length + " vertices. " + sb.ToString();
        }

        public static bool operator ==(PolyhedronFace face1, PolyhedronFace face2)
        {
            return face1.Equals(face2);
        }

        public static bool operator !=(PolyhedronFace face1, PolyhedronFace face2)
        {
            return !(face1.Equals(face2));
        }

        public override bool Equals(object? obj)
        {
            if (obj is not PolyhedronFace other)
            {
                return false;
            }

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

            // TODO re-examine possible edge cases. Should just be able to use one check?
            return Id == other.Id;
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
