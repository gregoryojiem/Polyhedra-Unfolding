using System.Text.Json.Serialization;

namespace Polyhedra.DataStructs2D
{
    public class Edge2D
    {
        public Point2D Start { get; set; }

        public Point2D End { get; set; }

        public bool Connector { get; set; }

        [JsonIgnore]
        public Polygon Polygon;

        [JsonIgnore]
        public Polygon AdjacentPolygon;

        [JsonIgnore]
        public Point2D Mid
        {
            get
            {
                double X = (Start.X + End.X) / 2;
                double Y = (Start.Y + End.Y) / 2;
                return new(X, Y);
            }
        }

        public Edge2D(Point2D start, Point2D end, Polygon polygon, Polygon adjacentPolygon)
        {
            Start = start;
            End = end;
            Polygon = polygon;
            AdjacentPolygon = adjacentPolygon;
        }

        public double FindAngleBetween(Edge2D adjacentEdge)
        {
            var vecToCurrEdge = Polygon.GetVecToEdge(this);
            var vecToAdjEdge = AdjacentPolygon.GetVecToEdge(adjacentEdge);

            var perpendicularCurr = new Vec2D(
                -(End.Y - Start.Y),
                End.X - Start.X);

            var perpendicularAdj = new Vec2D(
                -(adjacentEdge.End.Y - adjacentEdge.Start.Y),
                adjacentEdge.End.X - adjacentEdge.Start.X);

            var invertPerpCurr = vecToCurrEdge.Dot(perpendicularCurr) > 0;
            var invertPerpAdj = vecToAdjEdge.Dot(perpendicularAdj) > 0;
            if (invertPerpCurr)
            {
                perpendicularCurr = perpendicularCurr * -1;
            }
            if (invertPerpAdj)
            {
                perpendicularAdj = perpendicularAdj * -1;
            }

            return perpendicularCurr.FindAngleBetween(perpendicularAdj * -1, true);
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
         
            double epsilon = 1e-4;
            var collinearOrEndpointTouchCheck = (cp1 * cp2) > -epsilon || (cp3 * cp4) > -epsilon;
            if ((sharedLeftEndpoint || sharedRightEndpoint) && collinearOrEndpointTouchCheck)
            {
                return false;
            }

            return (cp1 * cp2 < 0 && cp3 * cp4 < 0);
        }

        public Vec2D ToVector()
        {
            return new Vec2D(End.X - Start.X, End.Y - Start.Y);
        }

        public override string ToString()
        {
            return Start + "\n" + End;
        }
    }
}
