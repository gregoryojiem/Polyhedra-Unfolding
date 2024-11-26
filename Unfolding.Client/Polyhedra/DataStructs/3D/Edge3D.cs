namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Edge3D
    {
        public Point3D[] Vertices;
        public PolyhedronFace ConnectedPoly;

        public Edge3D(PolyhedronFace firstFace, PolyhedronFace secondFace) 
        {
            var sharedVertices = firstFace.Vertices.Intersect(secondFace.Vertices).ToArray();
            Vertices = sharedVertices.Select(v => firstFace.Vertices.First(p => p.Equals(v))).ToArray();
            ConnectedPoly = secondFace;
        }
    }
}
