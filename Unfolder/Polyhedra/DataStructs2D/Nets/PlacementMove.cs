namespace Polyhedra.DataStructs2D.Nets
{
    public class PlacementMove : NetMove, IComparable<PlacementMove>
    {
        public readonly int CurrentPolygonId;
        public readonly int AdjacentPolygonId;
        public int lowerId;
        public int higherId;

        public PlacementMove(int currentPolygonId, int adjacentPolygonId)
        {
            CurrentPolygonId = currentPolygonId;
            AdjacentPolygonId = adjacentPolygonId;
            lowerId = Math.Min(currentPolygonId, adjacentPolygonId);
            higherId = Math.Max(currentPolygonId, adjacentPolygonId);
        }

        public int CompareTo(PlacementMove? other)
        {
            if (other == null)
            {
                return 1;
            }

            int comparison = lowerId.CompareTo(other.lowerId);
            if (comparison != 0)
            {
                return comparison;
            }
            return higherId.CompareTo(other.higherId);
        }

        public override bool Equals(object? obj)
        {
            if (obj is PlacementMove other)
            {
                return lowerId == other.lowerId && higherId == other.higherId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return lowerId.GetHashCode() ^ higherId.GetHashCode();
        }

        public override string ToString()
        {
            return lowerId + ":" + higherId;
        }
    }
}
