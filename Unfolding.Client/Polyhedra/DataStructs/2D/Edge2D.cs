namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Edge2D
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

        public Polygon AdjacentPolygon;

        public double? Slope
        {
            get
            {
                try
                {
                    return (End.Y - Start.Y) / (End.X - Start.X);
                }
                catch (DivideByZeroException)
                {
                    // Vertical line
                    return null;
                }
            }
        }

        public Edge2D(Point2D start, Point2D end, Polygon adjacentPolygon)
        {
            Start = start;
            End = end;
            AdjacentPolygon = adjacentPolygon;
        }

        public bool Intersection(Edge2D otherEdge) //TODO: Is there a way to avoid all the floating point related checks?
        {
            var sharedLeftEndpoint = Start == otherEdge.Start || Start == otherEdge.End;
            var sharedRightEndpoint = End == otherEdge.Start || End == otherEdge.End;
            if (sharedLeftEndpoint && sharedRightEndpoint)
            {
                return true;
            }

            var thisEdgeVec = new Vec2D(End - Start);
            var otherEdgeVec = new Vec2D(otherEdge.End - otherEdge.Start); 

            double cp1 = thisEdgeVec.Cross(new Vec2D(otherEdge.Start - Start));
            double cp2 = thisEdgeVec.Cross(new Vec2D(otherEdge.End - Start));
            double cp3 = otherEdgeVec.Cross(new Vec2D(Start - otherEdge.Start));
            double cp4 = otherEdgeVec.Cross(new Vec2D(End - otherEdge.Start));
         
            double epsilon = 1e-8;
            var collinearOrEndpointTouchCheck = (cp1 * cp2) > -epsilon || (cp3 * cp4) > -epsilon;
            if ((sharedLeftEndpoint || sharedRightEndpoint) && collinearOrEndpointTouchCheck)
            {
                return false;
            }

            return (cp1 * cp2 < 0 && cp3 * cp4 < 0);
        }

        public override string ToString()
        {
            return Start.ToString() + " - " + End.ToString();
        }
    }
}
