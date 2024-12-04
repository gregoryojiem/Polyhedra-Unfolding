using MIConvexHull;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Polyhedron
    {
        public PolyhedronFace[] Faces { get; set; }

        public Polyhedron(ConvexHullCreationResult<Point3D, ConvexHullFace> convexHull)
        {
            var convexHullFaces = convexHull.Result.Faces.ToArray();
            Faces = new PolyhedronFace[convexHullFaces.Length];
            var polyToConvMapping = new Dictionary<PolyhedronFace, ConvexHullFace>();
            var convToPolyMapping = new Dictionary<ConvexHullFace, PolyhedronFace>();

            for (int i = 0; i < convexHullFaces.Length; i++)
            {
                Faces[i] = new PolyhedronFace(convexHullFaces[i]);
                polyToConvMapping[Faces[i]] = convexHullFaces[i];
                convToPolyMapping[convexHullFaces[i]] = Faces[i];
            }

            // TODO clean up this section... truly atrocious
            var mergingResults = MergeCoplanarTriangles();
            var mergedMapping = new Dictionary<PolyhedronFace, List<PolyhedronFace>>();
            var merges = mergingResults.Values.ToList();
            foreach (var merge in merges)
            {
                var associatedFaces = mergingResults.Where(kvp => kvp.Value.Equals(merge))
                    .Select(kvp => kvp.Key)
                    .ToList();
                mergedMapping[merge] = associatedFaces;
            }

            for (int i = 0; i < Faces.Length; i++)
            {
                ConvexHullFace[] adjacencyList;
                var adjacencySet = new HashSet<PolyhedronFace>();
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
                var finalAdjacencyList = adjacencySet.ToList();

                for (int j = 0; j < finalAdjacencyList.Count; j++)
                {
                    var adjacentFace = finalAdjacencyList[j];
                    Faces[i].Adjacency[adjacentFace] = new Edge3D(Faces[i], adjacentFace);
                }
            }

            var idCounter = 1;
            foreach (var face in Faces)
            {
                face.Id = idCounter++;
            }
        }

        public Dictionary<PolyhedronFace, PolyhedronFace> MergeCoplanarTriangles()
        {
            var mergedMapping = new Dictionary<PolyhedronFace, PolyhedronFace>();
            var faceHasMerged = new bool[Faces.Length];
            var mappingsToAssign = new List<(int, int)>();
            
            for (int i = 0; i < Faces.Length; i++)
            {
                if (faceHasMerged[i])
                {
                    continue;
                }

                PolyhedronFace currentFace = Faces[i];
                for (int j = i + 1; j < Faces.Length; j++)
                {
                    PolyhedronFace faceToMerge = Faces[j];
                    if (faceHasMerged[j] || !currentFace.Mergeable(faceToMerge))
                    {
                        continue;
                    }

                    var mergedFace = currentFace.Merge(Faces[j]);
                    faceHasMerged[j] = true;
                    faceHasMerged[i] = true;
                    mergedMapping[Faces[i]] = mergedFace;
                    mappingsToAssign.Add((j, i));
                    currentFace = mergedFace;
                }
            }

            var newFaces = new List<PolyhedronFace>();
            for (int i = 0; i < Faces.Length; i++)
            {
                if (!faceHasMerged[i])
                {
                    newFaces.Add(Faces[i]);
                }
            }
            newFaces.AddRange(mergedMapping.Values.ToList());

            foreach (var kvp in mappingsToAssign) 
            {
                mergedMapping[Faces[kvp.Item1]] = mergedMapping[Faces[kvp.Item2]]; 
            }

            Faces = newFaces.ToArray();
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
