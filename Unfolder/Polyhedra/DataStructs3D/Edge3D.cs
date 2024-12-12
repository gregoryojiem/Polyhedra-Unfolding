namespace Polyhedra.DataStructs3D
{
    public class Edge3D
    {
        public Point3D Start;
        public Point3D End;
        public Polygon3D ConnectedFace;

        public Edge3D(Polygon3D firstFace, Polygon3D secondFace) 
        {
            var sharedVertices = firstFace.Vertices.Intersect(secondFace.Vertices).ToArray();
            var vertices = sharedVertices.Select(v => firstFace.Vertices.First(p => p.Equals(v))).ToArray();
            if (vertices.Length != 2)
            {
                throw new Exception("Could not construct an edge with faces: " + firstFace + " and " + secondFace);
            }
            Start = vertices[0];
            End = vertices[1];
            ConnectedFace = secondFace;
        }

        public override string ToString()
        {
            return Start + "\n" + End;
        }
    }
}
