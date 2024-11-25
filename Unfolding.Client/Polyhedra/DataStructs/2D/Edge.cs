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

        public bool Intersection(Edge otherEdge)
        {
            bool verticalCase = false;
            try
            {
                if (Slope == null || otherEdge.Slope == null)
                {
                    verticalCase = true;
                }
                else
                {
                    double xIntersection = (double)((otherEdge.Start.Y - otherEdge.Slope * otherEdge.Start.X - Start.Y + Slope * Start.X) / (Slope - otherEdge.Slope));
                }
            }
            catch (DivideByZeroException)
            {
                // Parallel lines
                return false;
            }

            if (verticalCase)
            {
                // TODO handle vertical line case, very common case with example polyhedra
            }
            else
            {
                // TODO implement regular case
                //    if (x <= intersect <= Math.Min(l1.points[1].x, l2.points[1].x))
                //    {
                //        return true;
                //    }
                //    return false;
            }
            return false;
        }
    }
}
