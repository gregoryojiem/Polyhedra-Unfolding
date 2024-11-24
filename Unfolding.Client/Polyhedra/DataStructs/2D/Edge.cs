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

        public bool Connector { get; set; }

        public Edge(Point2D start, Point2D end)
        {
            Start = start;
            End = end;
            Connector = false;
        }

        public double FindAngleBetween(Edge otherEdge)
        {
            Vec2D vec = new(End.X - Start.X, End.Y - Start.Y);
            Vec2D otherVec = new(otherEdge.End.X - otherEdge.Start.X, otherEdge.End.Y - otherEdge.Start.Y);
            return Math.Acos(vec.Dot(otherVec) / (vec.Magnitude * otherVec.Magnitude));
        }
    }
}
