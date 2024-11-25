using MIConvexHull;
using System.Linq;
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
            var polyToConvMapping = new Dictionary<PolyhedraFace, ConvexHullFace>();
            var convToPolyMapping = new Dictionary<ConvexHullFace, PolyhedraFace>();

            for (int i = 0; i < convexHullFaces.Length; i++)
            {
                Faces[i] = new PolyhedraFace(convexHullFaces[i]);
                polyToConvMapping[Faces[i]] = convexHullFaces[i];
                convToPolyMapping[convexHullFaces[i]] = Faces[i];
            }

            var mergedMapping = MergeCoplanarTriangles();

            for (int i = 0; i < Faces.Length; i++)
            {
                ConvexHullFace[] adjacencyList;
                var adjacencySet = new HashSet<PolyhedraFace>();
                if (polyToConvMapping.ContainsKey(Faces[i]))
                {
                    adjacencyList = polyToConvMapping[Faces[i]].Adjacency;
                } else
                {
                    var originalFaces = mergedMapping[Faces[i]];
                    var mergedFaces = new HashSet<ConvexHullFace>();
                    foreach (var originalFace in originalFaces)
                    {
                        foreach (var adjFace in polyToConvMapping[originalFace].Adjacency)
                        {
                            mergedFaces.Add(adjFace);
                        }
                    }
                    adjacencyList = mergedFaces.ToArray();
                }

                for (int j = 0; j < adjacencyList.Length; j++)
                {
                    var adjacentFace = adjacencyList[j];
                    var foundMatch = false;
                    foreach (var kvp in mergedMapping)
                    {
                        if (kvp.Value.Contains(convToPolyMapping[adjacentFace]))
                        {
                            adjacencySet.Add(kvp.Key);
                            foundMatch = true;
                            break;
                        }
                    }
                    if (!foundMatch)
                    {
                        adjacencySet.Add(convToPolyMapping[adjacentFace]);
                    }
                }

                adjacencySet.Remove(Faces[i]);
                Faces[i].Adjacency = adjacencySet.ToList();
            }
        }

        public Dictionary<PolyhedraFace, List<PolyhedraFace>> MergeCoplanarTriangles()
        {
            var mergedMapping = new Dictionary<PolyhedraFace, List<PolyhedraFace>>();
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
                        
                        List<PolyhedraFace> originalFaces;
                        if (!mergedMapping.ContainsKey(mergedFace)) {
                            originalFaces = new List<PolyhedraFace>();
                            mergedMapping[mergedFace] = originalFaces;
                        } else
                        {
                            originalFaces = mergedMapping[mergedFace];
                        }
                        
                        if (!originalFaces.Contains(Faces[i]))
                        {
                            originalFaces.Add(Faces[i]);
                        }

                        if (!originalFaces.Contains(Faces[j]))
                        {
                            originalFaces.Add(Faces[j]);
                        }
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
            return mergedMapping;
        }

        public void FlattenFaces()
        {
            for (int i = 0; i < Faces.Length; i++)
            {
                Faces[i].Rotate3DToAlign();
            }

        }

        public string GetPolyhedraJSON()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            return JsonSerializer.Serialize(this, options);
        }
    }
}
