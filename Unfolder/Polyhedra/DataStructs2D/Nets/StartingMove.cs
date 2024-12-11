namespace Polyhedra.DataStructs2D.Nets
{
    public class StartingMove : NetMove
    {
        public readonly int StartingPolygonId;

        public StartingMove(int startingPolygonId) {
            StartingPolygonId = startingPolygonId;
        }

        public override string ToString()
        {
            return StartingPolygonId.ToString();
        }
    }
}
