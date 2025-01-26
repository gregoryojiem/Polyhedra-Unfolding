using MIConvexHull;
using System.Text.Json;
using PolyhedraUnfolding.DataStructs2D;
using PolyhedraUnfolding.DataStructs2D.Nets;

namespace PolyhedraUnfolding.DataStructs3D
{
    public class Polyhedron
    {
        public Polygon3D[] Faces { get; set; }

        public Polyhedron(int faceLength)
        {
            Faces = new Polygon3D[faceLength];
        }

        public Polyhedron(Point3D[] convexHullPoints)
        {
            var convexHull = ConvexHull.Create<Point3D, ConvexHullFace>(convexHullPoints);
            var convexHullFaces = convexHull.Result.Faces.ToArray();
            Faces = new Polygon3D[convexHullFaces.Length];

            BuildAdjacency(convexHullFaces);
            var mergeMapping = MergeCoplanarTriangles();
            RemapMergedAdjacencies(mergeMapping);

            var idCounter = 0;
            foreach (var face in Faces)
            {
                face.Id = idCounter++;
            }
        }

        public void RemapMergedAdjacencies(Dictionary<Polygon3D, Polygon3D> mergeMapping)
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
            var convToPolyMapping = new Dictionary<ConvexHullFace, Polygon3D>();
            var polyToConvMapping = new Dictionary<Polygon3D, ConvexHullFace>();
            for (int i = 0; i < convexHullFaces.Length; i++)
            {
                Faces[i] = new Polygon3D(convexHullFaces[i]);
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

        public int CalculateNormalHash(double[] normal)
        {
            const double tolerance = 0.0001;
            const double quantizationFactor = 1.0 / tolerance;

            var nx = (int)(normal[0] * quantizationFactor);
            var ny = (int)(normal[1] * quantizationFactor);
            var nz = (int)(normal[2] * quantizationFactor);
            return HashCode.Combine(nx, ny, nz);
        }

        /// <summary>
        /// A function to combine triangles that lie on the same 2D plane. This is
        /// required because our convex hull algorithm only returns triangular faces.
        /// We can assume all polyhedra are convex, and so it's safe to merge based on 
        /// normals.
        /// </summary>
        /// <returns>A Dictionary of Original face -> New merged face object </returns>
        public Dictionary<Polygon3D, Polygon3D> MergeCoplanarTriangles()
        {
            var mergedMapping = new Dictionary<Polygon3D, Polygon3D>();
            var sharedNormalFaces= new Dictionary<int, List<Polygon3D>>();

            for (int i = 0; i < Faces.Length; i++)
            {
                var face = Faces[i];
                var faceHash = CalculateNormalHash(face.Normal);
                if (sharedNormalFaces.ContainsKey(faceHash))
                {
                    sharedNormalFaces[faceHash].Add(face);
                } else
                {
                    sharedNormalFaces[faceHash] = [face];
                }
            }

            var facesListsToMerge = sharedNormalFaces.Values.ToList();
            Faces = new Polygon3D[facesListsToMerge.Count];

            for (int i = 0; i < facesListsToMerge.Count; i++)
            {
                List<Polygon3D> facesToMerge = facesListsToMerge[i];
                var originalFace = facesToMerge[0];
                for (int j = 1; j < facesToMerge.Count; j++)
                {
                    facesToMerge[0] = facesToMerge[0].Merge(facesToMerge[j]);
                }
                var mergedFace = facesToMerge[0];
                for (int j = 1; j < facesToMerge.Count; j++)
                {
                    mergedMapping[facesToMerge[j]] = mergedFace;
                }
                mergedMapping[originalFace] = mergedFace;
                Faces[i] = mergedFace;
            }

            return mergedMapping;
        }

        public void FlattenFaces()
        {
            for (int i = 0; i < Faces.Length; i++)
            {
                Faces[i].AlignFaceWithXZPlane();
            }

        }

        public Polyhedron Copy()
        {
            var polyhedron = new Polyhedron(Faces.Length);
            var faces = polyhedron.Faces;

            for (int i = 0; i < faces.Length; i++)
            {
                faces[i] = new Polygon3D(Faces[i]);
            }

            for (int i = 0; i < faces.Length; i++)
            {
                faces[i].CopyAdjacency(polyhedron, this);
            }

            return polyhedron;
        }

        public Net2D ToNet2D()
        {
            FlattenFaces();
            var polygons = Polygon2D.PolyhedraToPolygons(this);
            var net = new Net2D(polygons);
            return net;
        }

        public string ToJSON()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        }

        public int CountVertices()
        {
            HashSet<Point3D> uniqueVertices = [];
            foreach (var face in Faces)
            {
                foreach (var vertex in face.Vertices)
                {
                    uniqueVertices.Add(vertex);
                }
            }
            return uniqueVertices.Count;
        }
    }
}
