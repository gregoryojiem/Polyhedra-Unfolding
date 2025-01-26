using PolyhedraUnfolding.DataStructs3D;
using RBush;
using System.Text.Json;

namespace PolyhedraUnfolding.DataStructs2D.Nets
{
    public class Net2D
    {
        public RBush<Polygon2D> constructedNet;
        public Polygon2D[] Polygons;
        public readonly List<int> Placements = [];
        private int placementIndex = 0;
        public int lastPlacementIndexWithMoves = 0;

        // Used for stepping through DFS on a net
        public int StepsToDo = 1;
        public int StepsTaken = 0;

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
            constructedNet = new(maxEntries: Polygons.Length + 1);
        }

        public Net2D(Net2D existingNet)
        {
            Polygons = new Polygon2D[existingNet.Polygons.Length];
            for (int i = 0; i < existingNet.Polygons.Length; i++)
            {
                Polygons[i] = new Polygon2D(existingNet.Polygons[i]);
            }

            Placements = [];
            Placements.AddRange(existingNet.Placements);
            placementIndex = existingNet.placementIndex;
            lastPlacementIndexWithMoves = existingNet.lastPlacementIndexWithMoves;
            StepsToDo = existingNet.StepsToDo;
            StepsTaken = existingNet.StepsTaken;

            var placedCopyPolygons = existingNet.Polygons.Where(p => p.Status != PolygonStatus.Unplaced).ToList();
            constructedNet = new(maxEntries: Polygons.Length + 1);
            constructedNet.BulkLoad(placedCopyPolygons);
        }

        private void PlaceStartingPolygon(int polygonIndex)
        {
            var polygon = Polygons[polygonIndex];
            polygon.Status = PolygonStatus.Starting;
            Placements.Add(polygon.Id);
            placementIndex++;

            polygon.GetBounds();
            constructedNet.Insert(polygon);
        }

        private void ConnectPolygons(int currentPolygonIndex, int adjacentPolygonIndex)
        {
            var currentPolygon = Polygons[currentPolygonIndex];
            var adjacentPolygon = Polygons[adjacentPolygonIndex];
            var currentEdge = currentPolygon.GetConnectingEdge(adjacentPolygon);
            var adjacentEdge = adjacentPolygon.GetConnectingEdge(currentPolygon);

            var angle = currentPolygon.FindAngleBetween(adjacentPolygon, currentEdge, adjacentEdge);
            adjacentPolygon.Rotate(angle);
            adjacentEdge = adjacentPolygon.GetConnectingEdge(currentPolygon);
            adjacentPolygon.TranslateToEdge(adjacentEdge, currentEdge, currentPolygon);

            adjacentPolygon.Status = PolygonStatus.Current;
            if (LastPolygonPlaced.Status != PolygonStatus.Starting)
            {
                LastPolygonPlaced.Status = PolygonStatus.Placed;
            }
            Placements.Add(Array.IndexOf(Polygons, adjacentPolygon));
            placementIndex++;

            adjacentPolygon.GetBounds();
            constructedNet.Insert(adjacentPolygon);
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
                return;
            }

            placementIndex--;
            if (lastPlacementIndexWithMoves > 0)
            {
                lastPlacementIndexWithMoves--;
            }
            int lastPlacedIndex = Placements[placementIndex];
            var polygon = Polygons[lastPlacedIndex];
            polygon.Status = PolygonStatus.Unplaced;
            Placements.RemoveAt(placementIndex);
            constructedNet.Delete(polygon);
        }

        public void Reset()
        {
            StepsToDo = 1;
            while (Placements.Count > 0)
            {
                Undo();
            }
            StepsTaken = 0;
        }

        public NetMove? GetMove(int moveIndex)
        {
            if (placementIndex == 0 && moveIndex < Polygons.Length)
            {
                //var optimalStartingPolygon = Polygons.OrderByDescending(p => p.Vertices.Length).ElementAt(moveIndex);
                var startingPolygon = Polygons[moveIndex];
                return new StartingMove(startingPolygon.Id);
            }

            bool foundFirstMove = false;
            int currentMoveIndex = 0;
            for (int i = lastPlacementIndexWithMoves; i < Placements.Count; i++)
            {
                int placement = Placements[i];
                var polygon = Polygons[placement];

                for (int j = 0; j < polygon.Edges.Length; j++)
                {
                    var foundMove = Polygons[polygon.Edges[j].AdjacentPolygonIndex].Status == PolygonStatus.Unplaced;
                    if (foundMove && !foundFirstMove)
                    {
                        lastPlacementIndexWithMoves = i;
                        foundFirstMove = true;
                    }
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
            var polygonsToCheck = constructedNet.Search(LastPolygonPlaced.Bounds);

            for (int i = 0; i < polygonsToCheck.Count; i++)
            {
                var polygon = polygonsToCheck[i];

                if (polygon.Id == LastPolygonPlaced.Id)
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
            if (Placements.Count == 0)
            {
                return NetStatus.Valid;
            }

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
        private const double PaddingFactor = 0.05;

        public void PrepForImageConversion(int width, int height)
        {
            ArrangeVerticesClockwise();
            ScaleAndCenter(width, height);
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

        private void ScaleAndCenter(int width, int height)
        {
            // Center net at origin
            var center = GetNetCenter();
            Translate(new Point2D(-center.X, -center.Y));

            // Scaling
            var padding = Math.Max(width, height) * PaddingFactor;
            var (currentWidth, currentHeight) = GetNetSize();
            double requiredScaling = (Math.Min(width, height) - padding) / Math.Max(currentWidth, currentHeight);
            Scale(requiredScaling);

            // Translation
            var (scaledWidth, scaledHeight) = GetNetSize();
            var translation = new Point2D(width / 2, height / 2);
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
