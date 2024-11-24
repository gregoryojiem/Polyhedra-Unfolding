namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Edge
    {
        public Point2D Start { get; set; }
        public Point2D End { get; set; }

        public Point2D Mid
        {
            get
            {
                double X = (Start.X + End.X) / 2;
                double Y = (Start.Y + End.Y) / 2;
                return new(X, Y);
            }
        }

        public Edge(Point2D start, Point2D end)
        {
            Start = start;
            End = end;
        }
    }
}
