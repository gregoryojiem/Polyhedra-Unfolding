namespace Polyhedra.DataStructs2D.Nets
{
    public class PlacementMove : NetMove
    {
        public readonly Polygon CurrentPolygon;
        public readonly Polygon AdjacentPolygon;

        public PlacementMove(Polygon currentPolygon, Polygon adjacentPolygon)
        {
            CurrentPolygon = currentPolygon;
            AdjacentPolygon = adjacentPolygon;
        }
    }
}
