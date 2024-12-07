namespace Polyhedra.DataStructs3D
{
    public class Edge3D
    {
        public Point3D[] Vertices;
        public PolyhedronFace ConnectedFace;

        public Edge3D(PolyhedronFace firstFace, PolyhedronFace secondFace) 
        {
            var sharedVertices = firstFace.Vertices.Intersect(secondFace.Vertices).ToArray();
            Vertices = sharedVertices.Select(v => firstFace.Vertices.First(p => p.Equals(v))).ToArray();
            ConnectedFace = secondFace;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Edge3D other)
            {
                return false;
            }

            if (!Vertices.SequenceEqual(other.Vertices))
            {
                return false;
            }

            return ConnectedFace == other.ConnectedFace;
        }

        public override int GetHashCode()
        {
            return Vertices[0].GetHashCode() ^ Vertices[1].GetHashCode() ^ ConnectedFace.GetHashCode();
        }
    }
}
