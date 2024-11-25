using System.Drawing;

namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Net2D
    {
        Polygon[] Polygons;
        bool[] SetPolygons;
        public Net2D(Polygon[] polys, int index)
        {
            Polygons = polys;
            SetPolygons = new bool[Polygons.Length];
            SetPolygons[index] = true;
        }

        private static Net2D GenerateNet(Polygon[] polys)
        {
            int largestIndex = 0;
            Polygon largestPoly = polys[largestIndex];
            for (int i = 1;  i < polys.Length; i++)
            {
                if (polys[i].Vertices.Length > largestPoly.Vertices.Length)
                {
                    largestPoly = polys[i];
                    largestIndex = i;
                }
            }
            Net2D net = new(polys, largestIndex);
            net.Test(largestIndex);
            return GenerateNetBacktrack(net);
        }

        private void Test(int currPolyIndex)
        {
            Polygon currPoly = Polygons[currPolyIndex];
            SetPolygons[currPolyIndex] = true;

            for (int i = 0; i < Polygons.Length; i++)
            {
                if (!SetPolygons[i])
                {
                    Polygon nextPoly = Polygons[i];
                    bool adjacent = true; //todo figure out if the two polygons are adjacent
                    if (adjacent)
                    {
                        Edge nextEdge = nextPoly.Edges[0];
                        Edge currEdge = currPoly.Edges[0];

                        // Rotate
                        nextPoly.Rotate(nextEdge.FindAngleBetween(currEdge));

                        // Translate
                        Vec2D nextVec = new(nextEdge.Mid); //TODO replace with the adjacent edge
                        Vec2D currVec = new(currEdge.Mid); //TODO replace with the adjacent edge
                        nextPoly.TranslateToPoint(new(currVec - nextVec));

                        // Check for intersections
                        if (!CheckForIntersections(nextPoly))
                        {
                            // The polygon fits in this location
                            currPoly = nextPoly;
                            SetPolygons[i] = true;
                        }
                    }
                }
            }
        }

        private bool CheckForIntersections(Polygon nextPoly)
        {
            // TODO optimize by checking bounding boxes
            for (int j = 0; j < Polygons.Length; j++)
            {
                if (SetPolygons[j])
                {
                    foreach (Edge setEdge in Polygons[j].Edges)
                    {
                        foreach (Edge edge in nextPoly.Edges)
                        {
                            if (edge.Intersection(setEdge))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private static Net2D GenerateNetBacktrack(Net2D net)
        {
            return null;
        }
    }
}
