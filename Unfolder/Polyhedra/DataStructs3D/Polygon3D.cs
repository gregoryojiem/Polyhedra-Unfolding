using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;

namespace Polyhedra.DataStructs3D
{
    public class Polygon3D
    {
        public Point3D[] Vertices { get; set; }
        public double[] Normal { get; set; }

        [JsonIgnore]
        public List<Edge3D> Adjacency { get; set; }

        public int Id = 0;

        public Polygon3D(ConvexHullFace convexHullFace)
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

        public Polygon3D(Point3D[] points, double[] normal, List<Edge3D> adjacency)
        {
            Vertices = points;
            Normal = normal;
            Adjacency = adjacency;
        }

        public Polygon3D(Polygon3D face)
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

        public bool Mergeable(Polygon3D otherFace)
        {
            if (!(Math.Abs(Normal[0] - otherFace.Normal[0]) < 0.0001 &&
                  Math.Abs(Normal[1] - otherFace.Normal[1]) < 0.0001 &&
                  Math.Abs(Normal[2] - otherFace.Normal[2]) < 0.0001))
            {
                return false;
            }

            var sharedVertices = 0;

            foreach (var vertex in Vertices)
            {
                foreach (var otherVertex in otherFace.Vertices)
                {
                    if (vertex == otherVertex)
                    {
                        sharedVertices++;
                        break;
                    }
                }
            }

            return sharedVertices == 2;
        }

        public Polygon3D Merge(Polygon3D otherFace)
        {
            var mergedVertices = new HashSet<Point3D>(Vertices);
            mergedVertices.UnionWith(otherFace.Vertices);

            var mergedAdjacency = new List<Edge3D>();
            mergedAdjacency.AddRange(Adjacency);
            mergedAdjacency.AddRange(otherFace.Adjacency);
            mergedAdjacency = mergedAdjacency.Distinct().ToList();
            mergedAdjacency.RemoveAll(edge => edge.ConnectedFace == this || edge.ConnectedFace == otherFace);

            return new Polygon3D(mergedVertices.ToArray(), (double[])Normal.Clone(), mergedAdjacency);
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

        public void Translate(Point3D pointToTranslateTo)
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i] = Vertices[i].Translate(pointToTranslateTo);
            }

            for (int i = 0; i < Adjacency.Count; i++)
            {
                var edge = Adjacency[i];
                edge.Start = edge.Start.Translate(pointToTranslateTo);
                edge.End = edge.End.Translate(pointToTranslateTo);
            }
        }

        public void Rotate(Quaternion rotationQuaternion)
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i] = Vertices[i].Rotate(rotationQuaternion);
            }

            for (int i = 0; i < Adjacency.Count; i++)
            {
                var edge = Adjacency[i];
                edge.Start = edge.Start.Rotate(rotationQuaternion);
                edge.End = edge.End.Rotate(rotationQuaternion);
            }
        }

        public void TranslateToOrigin()
        {
            var centroid = GetCentroid();
            Translate(new Point3D(-centroid.X, -centroid.Y, -centroid.Z));
        }

        public void AlignFaceWithXZPlane()
        {
            TranslateToOrigin();
            
            if (Normal.SequenceEqual([0, -1, 0]))
            {
                return;
            }
            if (Normal.SequenceEqual([0, 1, 0]))
            {
                Normal = [0, -1, 0];
                for (int i = 0; i < Vertices.Length; i++)
                {
                    var point = Vertices[i]; 
                    Vertices[i] = new Point3D(-point.X, point.Y, -point.Z);
                }
                return;
            }

            Vector3 planeNormalVec = new((float)Normal[0], (float)Normal[1], (float)Normal[2]);
            Vector3 targetVector = new(0, -1, 0);
            Vector3 axis = Vector3.Normalize(Vector3.Cross(planeNormalVec, targetVector));
            var angle = (float)Math.Acos(Vector3.Dot(planeNormalVec, targetVector));
            Quaternion rotationQuaternion = Quaternion.CreateFromAxisAngle(axis, angle);
            Rotate(rotationQuaternion);

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

        public static bool operator ==(Polygon3D face1, Polygon3D face2)
        {
            return face1.Equals(face2);
        }

        public static bool operator !=(Polygon3D face1, Polygon3D face2)
        {
            return !(face1.Equals(face2));
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Polygon3D other)
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
