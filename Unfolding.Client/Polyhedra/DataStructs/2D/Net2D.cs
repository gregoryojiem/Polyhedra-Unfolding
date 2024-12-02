namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Net2D
    {
        private readonly Polygon[] Polygons;
        private readonly List<int> Placements = [];
        private int placementIndex = 0;

        public Net2D(Polygon[] polygons)
        {
            Polygons = polygons;
            Placements = [];
        }

        // Assumes current polygon is placed, and adjacent hasn't been placed
        public void PlacePolygon(Polygon currentPolygon, Polygon? adjacentPolygon)
        {
            if (adjacentPolygon == null)
            {
                currentPolygon.Status = PolygonStatus.Starting;
                Placements.Add(Array.IndexOf(Polygons, currentPolygon));
                placementIndex++;
                return;
            }

            var currentEdge = currentPolygon.GetConnectingEdge(adjacentPolygon);
            var adjacentEdge = adjacentPolygon.GetConnectingEdge(currentPolygon);

            var vecToCurrEdge = currentPolygon.GetVecToEdge(currentEdge);
            var vecToAdjEdge = adjacentPolygon.GetVecToEdge(adjacentEdge);
            var angle = vecToCurrEdge.FindAngleBetween(vecToAdjEdge * -1);
            var crossProduct = vecToCurrEdge.Cross(vecToAdjEdge);
            if (crossProduct < 0)
            {
                angle = 2 * Math.PI - angle;
            }

            adjacentPolygon.Rotate(angle);
            vecToAdjEdge = adjacentPolygon.GetVecToEdge(adjacentEdge);
            var adjacentPolygonCentroid = vecToCurrEdge - vecToAdjEdge + currentPolygon.Centroid;
            adjacentPolygon.TranslateToPoint(adjacentPolygonCentroid.ToPoint());

            adjacentPolygon.Status = PolygonStatus.Current;
            var lastPolygonPlaced = Polygons[Placements[Placements.Count - 1]];
            if (lastPolygonPlaced.Status != PolygonStatus.Starting)
            {
                lastPolygonPlaced.Status = PolygonStatus.Placed;
            }
            Placements.Add(Array.IndexOf(Polygons, adjacentPolygon));
            placementIndex++;

            currentEdge.Connector = true;
            adjacentEdge.Connector = true;
        }

        public void Undo()
        {
            Console.WriteLine("Net2D is calling undo.");
            if (placementIndex == 0)
            {
                throw new Exception("Can't call undo on an empty Net2D");
            }

            placementIndex--;
            int lastPlacedIndex = Placements[placementIndex];
            var polygon = Polygons[lastPlacedIndex];
            polygon.Status = PolygonStatus.Unplaced;
            Placements.RemoveAt(placementIndex);

            if (placementIndex > 0) // Ignore starting polygon's edges
            {
                return;
            }

            foreach (var edge in polygon.Edges)
            {
                if (!edge.Connector)
                {
                    continue;
                }

                edge.Connector = false;
                var adjacentEdge = polygon.GetConnectingEdge(edge.AdjacentPolygon);
                adjacentEdge.Connector = false;
            }
        }

        // Should always return moves until net is complete
        public List<(Polygon, Polygon?)> GetMoves()
        {
            var moves = new List<(Polygon, Polygon?)>();

            if (placementIndex == 0)
            {
                foreach (var polygon in Polygons)
                {
                    moves.Add((polygon, null));
                }

            }


            foreach (var placement in Placements) {
                var polygon = Polygons[placement];

                foreach (var edge in polygon.Edges)
                {
                    if (edge.AdjacentPolygon.Status == PolygonStatus.Unplaced)
                    {
                        moves.Add((polygon, edge.AdjacentPolygon));
                    }
                }
            }

            return moves;
        }

        // TODO optimize with bounding boxes
        private NetStatus Validate()
        {
            for (int i = 0; i < Polygons.Length; i++)
            {
                var currentPolygon = Polygons[i];
                if (currentPolygon.Status == PolygonStatus.Unplaced)
                {
                    continue;
                }
                for (int j = 0; j < Polygons.Length; j++)
                {
                    var adjacentPolygon = Polygons[j];
                    if (i == j || adjacentPolygon.Status == PolygonStatus.Unplaced)
                    {
                        continue;
                    }

                    if (currentPolygon.Intersecting(adjacentPolygon))
                    {
                        return NetStatus.Invalid;

                    }
                }
            }

            return NetStatus.Valid;
        }

        public NetStatus GetStatus()
        {
            if (Validate() == NetStatus.Invalid)
            {
                return NetStatus.Invalid;
            }

            foreach (var polygon in Polygons)
            {
                if (polygon.Status == PolygonStatus.Unplaced)
                {
                    return NetStatus.Valid;
                }
            }

            return NetStatus.Complete;
        }
    }
}
