using System.Text.Json;

namespace Polyhedra.DataStructs2D
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

            // Improve this section
            var perpendicularCurr = new Vec2D(
                -(currentEdge.End.Y - currentEdge.Start.Y),
                currentEdge.End.X - currentEdge.Start.X);

            var perpendicularAdj = new Vec2D(
                -(adjacentEdge.End.Y - adjacentEdge.Start.Y),
                adjacentEdge.End.X - adjacentEdge.Start.X);

            if (!perpendicularCurr.ToEdge(currentPolygon.Centroid).Intersection(currentEdge))
            {
                perpendicularCurr.X = -perpendicularCurr.X;
                perpendicularCurr.Y = -perpendicularCurr.Y;
            }

            if (!perpendicularAdj.ToEdge(adjacentPolygon.Centroid).Intersection(adjacentEdge))
            {
                perpendicularAdj.X = -perpendicularAdj.X;
                perpendicularAdj.Y = -perpendicularAdj.Y;
            }

            var angle = perpendicularCurr.FindAngleBetween(perpendicularAdj * -1, true);
            adjacentPolygon.Rotate(angle);
            vecToAdjEdge = adjacentPolygon.GetVecToEdge(adjacentEdge);
            var adjacentPolygonCentroid = vecToCurrEdge - vecToAdjEdge + currentPolygon.Centroid;
            adjacentPolygon.TranslateToPoint((adjacentPolygonCentroid).ToPoint());

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


            foreach (var placement in Placements)
            {
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
