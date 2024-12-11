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

        //
        // Functionality related to converting nets to images
        //

        public (int, int) PrepForImageConversion(int desiredSize, int padding)
        {
            ArrangeVerticesClockwise();
            ScaleAndCenter(desiredSize, padding);
            var netSize = GetNetSize();
            return ((int)netSize.Item1 + padding * 2, (int)netSize.Item2 + padding * 2);
        }

        private void ArrangeVerticesClockwise()
        {
            foreach (var polygon in Polygons)
            {
                polygon.SortVerticesClockwise();
            }
        }

        private (double, double) GetNetSize()
        {
            double minX = double.PositiveInfinity;
            double maxX = double.NegativeInfinity;
            double minY = double.PositiveInfinity;
            double maxY = double.NegativeInfinity;

            foreach (var polygon in Polygons)
            {
                foreach (var vertex in polygon.Vertices)
                {
                    minX = Math.Min(minX, vertex.X);
                    maxX = Math.Max(maxX, vertex.X);
                    minY = Math.Min(minY, vertex.Y);
                    maxY = Math.Max(maxY, vertex.Y);
                }
            }

            return (maxX - minX, maxY - minY);
        }

        private void ScaleAndCenter(double goalImageSize, double padding)
        {
            // Center net at origin
            var center = GetNetCenter();
            Translate(new Point2D(-center.X, -center.Y));

            // Scaling
            var (width, height) = GetNetSize();
            double requiredScaling = goalImageSize / Math.Max(width, height);
            Scale(requiredScaling);

            // Translation
            var scaledCenter = GetNetCenter();
            double translationX = goalImageSize / 2 + padding - scaledCenter.X;
            double translationY = goalImageSize / 2 + padding - scaledCenter.Y;
            var translation = new Point2D(translationX, translationY);
            Translate(translation);
        }


        private Point2D GetNetCenter()
        {
            var centerX = Polygons.Average(p => p.Centroid.X);
            var centerY = Polygons.Average(p => p.Centroid.Y);
            return new Point2D(centerX, centerY);
        }

        private void Scale(double scalar)
        {
            foreach (var polygon in Polygons)
            {
                polygon.Scale(scalar);
            }
        }

        private void Translate(Point2D point)
        {
            foreach (var polygon in Polygons)
            {
                polygon.Translate(point);
            }
        }
    }
}
