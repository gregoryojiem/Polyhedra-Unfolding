namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Net2D
    {
        Polygon[] Polygons;
        public static int StepsToDo = 0;

        public Net2D(Polygon[] polygons, Polygon largestPolygon)
        {
            Polygons = polygons;
            largestPolygon.HasBeenPlaced = true;
        }

        public static Net2D GenerateNet(Polygon[] polygons)
        {
            int largestIndex = 0;
            Polygon largestPolygon = polygons[largestIndex];
            for (int i = 1; i < polygons.Length; i++)
            {
                if (polygons[i].Vertices.Length > largestPolygon.Vertices.Length)
                {
                    largestPolygon = polygons[i];
                    largestIndex = i;
                }
            }
            Net2D net = new(polygons, largestPolygon);
            net.Test(largestPolygon);
            largestPolygon.Color = [0, 0, 1, 1];
            return GenerateNetBacktrack(net);
        }

        private void Test(Polygon currentPolygon)
        {
            currentPolygon.HasBeenPlaced = true;
            var steps = Math.Min(StepsToDo, currentPolygon.Edges.Length);

            for (int i = 0; i < steps; i++)
            {
                var currentEdge = currentPolygon.Edges[i];
                var adjacentPolygon = currentEdge.AdjacentPolygon;

                if (adjacentPolygon.HasBeenPlaced)
                {
                    continue;
                }

                // Find rotation angle, then rotate and translate the polygon we're attaching
                var adjacentEdge = adjacentPolygon.GetConnectingEdge(currentPolygon);
                var vecToCurrEdge = currentPolygon.GetVecToEdge(currentEdge);
                var vecToAdjEdge = adjacentPolygon.GetVecToEdge(adjacentEdge);
                var angle = vecToCurrEdge.FindAngleBetween(vecToAdjEdge * -1);
                
                adjacentPolygon.Rotate(angle);
                adjacentPolygon.TranslateToPoint(new(vecToCurrEdge + vecToAdjEdge * -1));

                // Check for intersections
                if (!CheckForIntersections(adjacentPolygon))
                {
                    // The polygon fits in this location
                    //currentPolygon = adjacentPolygon;
                    adjacentPolygon.HasBeenPlaced = true;
                }

                // TODO Visualization code, can be removed later when mass testing
                adjacentPolygon.HasBeenPlaced = true; //TODO remove once intersections work
                adjacentPolygon.Color = [0, 0, 1, 1];
                if (i == steps - 1)
                {
                    adjacentPolygon.Color = [0, 1, 0, 1];
                }
            }
        }

        private bool CheckForIntersections(Polygon adjacentPolygon)
        {
            // TODO optimize by checking bounding boxes
            for (int j = 0; j < Polygons.Length; j++)
            {
                var polygon = Polygons[j];
                if (polygon.HasBeenPlaced)
                {
                    foreach (Edge2D setEdge in Polygons[j].Edges)
                    {
                        foreach (Edge2D edge in adjacentPolygon.Edges)
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
