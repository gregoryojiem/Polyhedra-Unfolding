namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Net2D
    {
        Polygon[] polygons;
        bool[] setPolygons;
        public Net2D(Polygon[] polys)
        {
            polygons = polys;
            setPolygons = new bool[polygons.Length];
        }

        private static Net2D GenerateNet(Polygon[] polys)
        {
            Polygon largestPoly = polys[0];
            for (int i = 1; i < polys.Length; i++)
            {
                if (polys[i].Vertices.Length > largestPoly.Vertices.Length)
                {
                    largestPoly = polys[i];
                }
            }
            Net2D net = new(polys);
            return GenerateNetBacktrack(net);
        }

        private static Net2D GenerateNetBacktrack(Net2D net)
        {
            return null;
        }
    }
}
