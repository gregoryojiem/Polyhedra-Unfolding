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

        public bool Intersection(Edge2D otherEdge)
        {
            double xIntersection = 0;

            Point2D start = Start;
            Point2D end = End;
            double slope;
            bool swap = false;
            Point2D otherStart = otherEdge.Start;
            Point2D otherEnd = otherEdge.End;
            double otherSlope;
            bool otherSwap = false;
            if (start.X > end.X) { (end, start) = (start, end); swap = true; }
            if (otherStart.X > otherEnd.X) { (otherEnd, otherStart) = (otherStart, otherEnd); swap = false; }

            if (Slope == null && otherEdge.Slope == null)
            {
                // Both lines are vertical
                return false;
            }
            else if (Slope == null)
            {
                // This line is vertical
                if (Start.Y > End.Y)
                {
                    (end, start) = (start, end);
                }
                otherSlope = (double)otherEdge.Slope;
                if (otherSwap)
                {
                    otherSlope = otherSlope * -1;
                }

                double x = Start.X;
                double y = otherSlope * (x - otherStart.X) + otherStart.Y;

                if ((Start.Y < y && y < End.Y) && (otherStart.X < x && x < otherEnd.X) )
                {
                    return true;
                }
            }
            else if (otherEdge.Slope == null)
            {
                // Other line is vertical
                if (otherEdge.Start.Y > otherEdge.End.Y)
                {
                    (otherEnd, otherStart) = (otherStart, otherEnd);
                }
                slope = (double)Slope;
                if (swap)
                {
                    slope = slope * -1;
                }

                double x = otherStart.X;
                double y = slope * (x - start.X) + start.Y;

                if ((otherStart.Y < y && y < otherEnd.Y) && (start.X < x && x < end.X))
                {
                    return true;
                }
            }
            else
            {
                slope = (double)Slope;
                otherSlope = (double)otherEdge.Slope;
                if (swap)
                {
                    slope = slope * -1;
                }
                if (otherSwap)
                {
                    otherSlope = otherSlope * -1;
                }

                try
                {
                    xIntersection = (double)((otherStart.Y - otherSlope * otherStart.X - start.Y + slope * start.X) / (slope - otherSlope));
                }
                catch (DivideByZeroException)
                {
                    // Parallel lines
                    return false;
                }

                if (Math.Max(start.X, otherStart.X) < xIntersection && xIntersection < Math.Min(end.X, otherEnd.X))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
