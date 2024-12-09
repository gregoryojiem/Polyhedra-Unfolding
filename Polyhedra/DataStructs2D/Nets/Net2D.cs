using System.Text.Json;

namespace Polyhedra.DataStructs2D.Nets
{
    public class Net2D
    {
        public Polygon[] Polygons;
        public readonly List<int> Placements = [];
        private int placementIndex = 0;

        private Polygon LastPolygonPlaced
        {
            get
            {
                return Polygons[Placements[Placements.Count - 1]];
            }
        }

        public Net2D(Polygon[] polygons)
        {
            Polygons = polygons;
            Placements = [];
        }

        private void PlaceStartingPolygon(Polygon polygon)
        {
            polygon.Status = PolygonStatus.Starting;
            Placements.Add(Array.IndexOf(Polygons, polygon));
            placementIndex++;
        }

        private void ConnectPolygons(Polygon currentPolygon, Polygon adjacentPolygon)
        {
            var currentEdge = currentPolygon.GetConnectingEdge(adjacentPolygon);
            var adjacentEdge = adjacentPolygon.GetConnectingEdge(currentPolygon);
            adjacentPolygon.Rotate(currentEdge.FindAngleBetween(adjacentEdge));
            adjacentPolygon.TranslateToEdge(adjacentEdge, currentEdge);

            adjacentPolygon.Status = PolygonStatus.Current;
            if (LastPolygonPlaced.Status != PolygonStatus.Starting)
            {
                LastPolygonPlaced.Status = PolygonStatus.Placed;
            }
            Placements.Add(Array.IndexOf(Polygons, adjacentPolygon));
            placementIndex++;

            currentEdge.Connector = true;
            adjacentEdge.Connector = true;
        }
        public void MakeMove(NetMove move)
        {
            if (move is StartingMove startingMove)
            {
                PlaceStartingPolygon(startingMove.StartingPolygon);
            }

            if (move is PlacementMove placementMove)
            {
                ConnectPolygons(placementMove.CurrentPolygon, placementMove.AdjacentPolygon);
            }
        }

        public void Undo()
        {
            if (placementIndex == 0)
            {
                throw new Exception("Can't call undo on an empty Net2D");
            }

            placementIndex--;
            int lastPlacedIndex = Placements[placementIndex];
            var polygon = Polygons[lastPlacedIndex];
            polygon.Status = PolygonStatus.Unplaced;
            Placements.RemoveAt(placementIndex);

            if (placementIndex == 0) // We can ignore the starting polygon's edges
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
                var adjacentEdge = edge.AdjacentPolygon.GetConnectingEdge(polygon);
                adjacentEdge.Connector = false;
            }
        }

        // Should always return moves until net is complete
        public List<NetMove> GetMoves()
        {
            var moves = new List<NetMove>();

            if (placementIndex == 0)
            {
                foreach (var polygon in Polygons)
                {
                    moves.Add(new StartingMove(polygon));
                }

            }


            foreach (var placement in Placements)
            {
                var polygon = Polygons[placement];

                foreach (var edge in polygon.Edges)
                {
                    if (edge.AdjacentPolygon.Status == PolygonStatus.Unplaced)
                    {
                        moves.Add(new PlacementMove(polygon, edge.AdjacentPolygon));
                    }
                }
            }

            return moves;
        }

        private NetStatus ValidateLastMove()
        {
            for (int i = 0; i < Polygons.Length; i++)
            {
                var polygon = Polygons[i];

                if (polygon.Status == PolygonStatus.Unplaced || i == Placements[Placements.Count - 1])
                {
                    continue;
                }

                if (LastPolygonPlaced.DoBoundsIntersect(polygon))
                {
                    if (LastPolygonPlaced.Intersecting(polygon))
                    {
                        return NetStatus.Invalid;
                    }
                }
            }

            return NetStatus.Valid;
        }

        public NetStatus GetStatus()
        {
            if (ValidateLastMove() == NetStatus.Invalid)
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

        public string ToJSON(bool hideUnplaced)
        {
            var polygons = Polygons.Where(p => p.Status != PolygonStatus.Unplaced || !hideUnplaced).ToArray();
            return JsonSerializer.Serialize(polygons, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
