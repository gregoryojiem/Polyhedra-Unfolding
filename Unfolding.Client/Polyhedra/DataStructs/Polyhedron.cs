using MIConvexHull;
using System.Text.Json;

namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Polyhedron
    {
        public PolyhedraFace[] Faces { get; }

        public Polyhedron(ConvexHullCreationResult<Point3D, ConvexHullFace> convexHull)
        {
            var convexHullFaces = convexHull.Result.Faces.ToArray();
            Faces = new PolyhedraFace[convexHullFaces.Length];

            for (int i = 0; i < convexHullFaces.Length; i++)
            {
                Faces[i] = new PolyhedraFace(convexHullFaces[i]);
            }
        }

        public string GetPolyhedraJSON()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            return System.Text.Json.JsonSerializer.Serialize(this, options);
        }
    }
}
