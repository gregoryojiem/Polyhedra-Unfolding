using System.Text.Json;

namespace Polyhedra.DataStructs2D.Nets
{
    public class Net2D
    {
        public Polygon2D[] Polygons;
        public readonly List<int> Placements = [];
        private int placementIndex = 0;

        private Polygon2D LastPolygonPlaced
        {
            get
            {
                return Polygons[Placements[Placements.Count - 1]];
            }
        }

        public Net2D(Polygon2D[] polygons)
        {
            Polygons = polygons;
            Placements = [];
        }

        private void PlaceStartingPolygon(int polygonIndex)
        {
            var polygon = Polygons[polygonIndex];
            polygon.Status = PolygonStatus.Starting;
            Placements.Add(polygon.Id);
            placementIndex++;
        }

        private void ConnectPolygons(int currentPolygonIndex, int adjacentPolygonIndex)
        {
            var currentPolygon = Polygons[currentPolygonIndex];
            var adjacentPolygon = Polygons[adjacentPolygonIndex];
            var currentEdge = currentPolygon.GetConnectingEdge(adjacentPolygon);
            var adjacentEdge = adjacentPolygon.GetConnectingEdge(currentPolygon);

            var angle = currentPolygon.FindAngleBetween(adjacentPolygon, currentEdge, adjacentEdge);
            adjacentPolygon.Rotate(angle);
            adjacentPolygon.TranslateToEdge(adjacentEdge, currentEdge, currentPolygon);

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
                PlaceStartingPolygon(startingMove.StartingPolygonId);
            }

            if (move is PlacementMove placementMove)
            {
                ConnectPolygons(placementMove.CurrentPolygonId, placementMove.AdjacentPolygonId);
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
                var adjacentEdge = Polygons[edge.AdjacentPolygonIndex].GetConnectingEdge(polygon);
                adjacentEdge.Connector = false;
            }
        }

        public NetMove? GetMove(int moveIndex)
        {
            if (placementIndex == 0 && moveIndex < Polygons.Length)
            {
                //var optimalStartingPolygon = Polygons.OrderByDescending(p => p.Vertices.Length).ElementAt(moveIndex);
                var startingPolygon = Polygons[moveIndex];
                return new StartingMove(startingPolygon.Id);
            }

            int currentMoveIndex = 0;
            for (int i = 0; i < Placements.Count; i++)
            {
                int placement = Placements[i];
                var polygon = Polygons[placement];

                for (int j = 0; j < polygon.Edges.Length; j++)
                {
                    var foundMove = Polygons[polygon.Edges[j].AdjacentPolygonIndex].Status == PolygonStatus.Unplaced;
                    if (foundMove && currentMoveIndex == moveIndex)
                    {
                        return new PlacementMove(polygon.Id, polygon.Edges[j].AdjacentPolygonIndex);
                    }
                    if (foundMove)
                    {
                        currentMoveIndex++;
                    }
                }
            }

            return null;
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

            
            return IsComplete() ? NetStatus.Complete : NetStatus.Valid;
        }

        public bool IsComplete()
        {
            foreach (var polygon in Polygons)
            {
                if (polygon.Status == PolygonStatus.Unplaced)
                {
                    return false;
                }
            }

            return true;
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
            return ((int)netSize.Item1 + padding, (int)netSize.Item2 + padding);
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
            return (
                Polygons.SelectMany(p => p.Vertices).Max(v => v.X) - Polygons.SelectMany(p => p.Vertices).Min(v => v.X),
                Polygons.SelectMany(p => p.Vertices).Max(v => v.Y) - Polygons.SelectMany(p => p.Vertices).Min(v => v.Y)
            );
        }

        private Point2D GetNetCenter()
        {
            var middleX = (Polygons.SelectMany(p => p.Vertices).Max(v => v.X) + Polygons.SelectMany(p => p.Vertices).Min(v => v.X)) / 2;
            var middleY = (Polygons.SelectMany(p => p.Vertices).Max(v => v.Y) + Polygons.SelectMany(p => p.Vertices).Min(v => v.Y)) / 2;
            return new Point2D(middleX, middleY);
        }

        private void ScaleAndCenter(double goalImageSize, double padding)
        {
            // Center net at origin
            var center = GetNetCenter();
            Translate(new Point2D(-center.X, -center.Y));

            // Scaling
            var (width, height) = GetNetSize();
            double requiredScaling = goalImageSize / Math.Min(width, height);
            Scale(requiredScaling);

            // Translation
            var (scaledWidth, scaledHeight) = GetNetSize();
            double translationX = scaledWidth / 2 + padding / 2;
            double translationY = scaledHeight / 2 + padding / 2;
            var translation = new Point2D(translationX, translationY);
            Translate(translation);
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
