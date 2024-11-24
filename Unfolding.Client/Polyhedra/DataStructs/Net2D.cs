namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Net2D
    {
        PolyhedraFace[] faces;
        public Net2D(PolyhedraFace[] faces)
        {
            this.faces = faces;
        }

        private static Net2D GenerateNet(PolyhedraFace[] faces)
        {
            PolyhedraFace largestPoly = faces[0];
            for (int i = 1; i < faces.Length; i++)
            {
                if (faces[i].Vertices.Length > largestPoly.Vertices.Length)
                {
                    largestPoly = faces[i];
                }
            }
            Net2D net = new(faces);
            return GenerateNetBacktrack(net);
        }

        private static Net2D GenerateNetBacktrack(Net2D net)
        {
            return null;
        }


        //private static bool LineIntersection(Edge l1, Edge l2)
        //{
        //    try
        //    {
        //        //l2.slope = (ps[1].y - ps[0].y) / (ps[1].x - ps[0].x)
        //        XIntersection = (l2.Z - l2.slope * l2.X - l1.Z + l1.slope * l1.X) / (l1.slope - l2.slope);
        //    }
        //    catch (DivideByZeroException e)
        //    {

        //    }

        //    if (x <= intersect <= Math.Min(l1.points[1].x, l2.points[1].x))
        //    {
        //        return true;
        //    }
        //    return false;
        //}
    }
}
