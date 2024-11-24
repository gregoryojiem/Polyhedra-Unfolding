using MIConvexHull;
using System.Text.Json;

namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Polyhedron
    {
        public PolyhedraFace[] Faces { get; set; }

        public Polyhedron(ConvexHullCreationResult<Point3D, ConvexHullFace> convexHull)
        {
            var convexHullFaces = convexHull.Result.Faces.ToArray();
            Faces = new PolyhedraFace[convexHullFaces.Length];

            for (int i = 0; i < convexHullFaces.Length; i++)
            {
                Faces[i] = new PolyhedraFace(convexHullFaces[i]);
            }

            MergeCoplanarTriangles();
        }

        public void MergeCoplanarTriangles()
        {
            var mergedFaces = new List<PolyhedraFace>();
            var faceHasMerged = new bool[Faces.Length];

            for (int i = 0; i < Faces.Length; i++)
            {
                for (int j = i + 1; j < Faces.Length; j++)
                {
                    if (!faceHasMerged[j] && Faces[i].Mergeable(Faces[j]))
                    {
                        var mergedFace = Faces[i].Merge(Faces[j]);
                        mergedFaces.Add(mergedFace);
                        faceHasMerged[i] = true;
                        faceHasMerged[j] = true;
                    }
                }
            }

            for (int i = 0; i < Faces.Length; i++)
            {
                if (!faceHasMerged[i])
                {
                    mergedFaces.Add(Faces[i]);
                }
            }

            Faces = mergedFaces.ToArray();
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
