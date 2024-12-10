namespace Polyhedra.DataStructs2D.Nets
{
    public class PlacementMove : NetMove, IComparable<PlacementMove>
    {
        public readonly Polygon CurrentPolygon;
        public readonly Polygon AdjacentPolygon;
        public int lowerId;
        public int higherId;

        public PlacementMove(Polygon currentPolygon, Polygon adjacentPolygon)
        {
            CurrentPolygon = currentPolygon;
            AdjacentPolygon = adjacentPolygon;
            lowerId = Math.Min(currentPolygon.Id, adjacentPolygon.Id);
            higherId = Math.Max(currentPolygon.Id, adjacentPolygon.Id);
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
