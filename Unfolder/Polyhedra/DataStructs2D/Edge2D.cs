namespace Polyhedra.DataStructs2D
{
    public struct Edge2D
    {
        public readonly Point2D Start { get; }

        public readonly Point2D End { get; }

        public readonly int PolygonIndex;

        public readonly int AdjacentPolygonIndex;

        public Edge2D(Point2D start, Point2D end, int polygonIndex, int adjacentPolygonIndex)
        {
            Start = new Point2D(start.X, start.Y);
            End = new Point2D(end.X, end.Y);
            PolygonIndex = polygonIndex;
            AdjacentPolygonIndex = adjacentPolygonIndex;
        }

        public Point2D GetMidpoint()
        {
            double X = (Start.X + End.X) / 2;
            double Y = (Start.Y + End.Y) / 2;
            return new Point2D(X, Y);
        }

        public bool Intersection(Edge2D otherEdge)
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
         
            double epsilon = 1e-4;
            var collinearOrEndpointTouchCheck = (cp1 * cp2) > -epsilon || (cp3 * cp4) > -epsilon;
            if ((sharedLeftEndpoint || sharedRightEndpoint) && collinearOrEndpointTouchCheck)
            {
                return false;
            }

            return (cp1 * cp2 < 0 && cp3 * cp4 < 0);
        }

        public Edge2D Rotate(double angle)
        {
            var rotatedStart = Start.Rotate(angle);
            var rotatedEnd = End.Rotate(angle);
            return new Edge2D(rotatedStart, rotatedEnd, PolygonIndex, AdjacentPolygonIndex);
        }

        public Edge2D Translate(double x, double y)
        {
            var translatedStart = new Point2D(Start.X + x, Start.Y + y);
            var translatedEnd = new Point2D(End.X + x, End.Y + y);
            return new Edge2D(translatedStart, translatedEnd, PolygonIndex, AdjacentPolygonIndex);
        }

        public override string ToString()
        {
            return Start + "\n" + End;
        }
    }
}
