namespace Polyhedra.DataStructs2D.Nets
{
    public class StartingMove : NetMove
    {
        public readonly Polygon StartingPolygon;

        public StartingMove(Polygon polygon) {
            StartingPolygon = polygon;
        }

        public override string ToString()
        {
            return StartingPolygon.Id.ToString();
        }
    }
}
