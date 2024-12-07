using MIConvexHull;
using System.Text.Json;
using Polyhedra.DataStructs2D;

namespace Polyhedra.DataStructs3D
{
    public class Polyhedron
    {
        public PolyhedronFace[] Faces { get; set; }

        public Polyhedron(int faceLength)
        {
            Faces = new PolyhedronFace[faceLength];
        }

        public Polyhedron(Point3D[] convexHullPoints)
        {
            var convexHull = ConvexHull.Create<Point3D, ConvexHullFace>(convexHullPoints);
            var convexHullFaces = convexHull.Result.Faces.ToArray();
            Faces = new PolyhedronFace[convexHullFaces.Length];

            BuildAdjacency(convexHullFaces);
            var mergeMapping = MergeCoplanarTriangles();
            RemapMergedAdjacencies(mergeMapping);

            var idCounter = 1;
            foreach (var face in Faces)
            {
                face.Id = idCounter++;
            }
        }

        public void RemapMergedAdjacencies(Dictionary<PolyhedronFace, PolyhedronFace> mergeMapping)
        {
            foreach (var face in Faces)
            {
                for (int i = face.Adjacency.Count - 1; i >= 0; i--)
                {
                    var edge = face.Adjacency[i];
                    var adjacentFace = edge.ConnectedFace;
                    if (!mergeMapping.ContainsKey(adjacentFace))
                    {
                        continue;
                    }

                    face.Adjacency.Remove(edge);
                    var mergedFace = mergeMapping[adjacentFace];
                    if (face == mergedFace)
                    {
                        continue;
                    }

                    var newEdge = new Edge3D(face, mergedFace);
                    if (!face.Adjacency.Contains(newEdge))
                    {
                        face.Adjacency.Add(newEdge);
                    }
                }
            }
        }

        public void BuildAdjacency(ConvexHullFace[] convexHullFaces)
        {
            var convToPolyMapping = new Dictionary<ConvexHullFace, PolyhedronFace>();
            var polyToConvMapping = new Dictionary<PolyhedronFace, ConvexHullFace>();
            for (int i = 0; i < convexHullFaces.Length; i++)
            {
                Faces[i] = new PolyhedronFace(convexHullFaces[i]);
                convToPolyMapping[convexHullFaces[i]] = Faces[i];
                polyToConvMapping[Faces[i]] = convexHullFaces[i];
            }

            for (int i = 0; i < Faces.Length; i++)
            {
                var adjacencyList = polyToConvMapping[Faces[i]].Adjacency;

                for (int j = 0; j < adjacencyList.Length; j++)
                {
                    var adjacentFace = convToPolyMapping[adjacencyList[j]];
                    if (adjacentFace != Faces[i])
                    {
                        Faces[i].Adjacency.Add(new Edge3D(Faces[i], adjacentFace));
                    }
                }
            }
        }

        /// <summary>
        /// A function to combine triangles that lie on the same 2D plane in 3D space
        /// This is required because our convex hull algorithm only returns triangular
        /// faces
        /// </summary>
        /// <returns>A Dictionary of Original face -> New merged face object </returns>
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

        public Polyhedron Copy()
        {
            var polyhedron = new Polyhedron(Faces.Length);
            var faces = polyhedron.Faces;

            for (int i = 0; i < faces.Length; i++)
            {
                faces[i] = new PolyhedronFace(Faces[i]);
            }

            for (int i = 0; i < faces.Length; i++)
            {
                faces[i].CopyAdjacency(polyhedron, this);
            }

            return polyhedron;
        }

        public Net2D ToNet2D()
        {
            var copyPolyhedron = Copy();
            var polygons = Polygon.PolyhedraToPolygons(copyPolyhedron);
            var net = new Net2D(polygons);
            return net;
        }

        public string ToJSON()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
