namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Net2D
    {
        Polygon[] polygons;
        bool[] setPolygons;
        public Net2D(Polygon[] polys, int index)
        {
            polygons = polys;
            setPolygons = new bool[polygons.Length];
            setPolygons[index] = true;
        }

        private static Net2D GenerateNet(Polygon[] polys)
        {
            int largestIndex = 0;
            Polygon largestPoly = polys[largestIndex];
            for (int i = 1;  i < polys.Length; i++)
            {
                if (polys[i].Vertices.Length > largestPoly.Vertices.Length)
                {
                    largestPoly = polys[i];
                    largestIndex = i;
                }
            }
            Net2D net = new(polys, largestIndex);
            net.Test();
            return GenerateNetBacktrack(net);
        }

        private void Test()
        {
            //polygons.
            
        }

        private static Net2D GenerateNetBacktrack(Net2D net)
        {
            return null;
        }
    }
}
