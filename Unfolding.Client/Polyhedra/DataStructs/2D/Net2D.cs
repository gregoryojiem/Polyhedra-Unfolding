namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Net2D
    {
        private readonly Polygon[] Polygons;
        private readonly int[] Placements;

        public Net2D(Polygon[] polygons)
        {
            Polygons = polygons;
            Placements = new int[polygons.Length];
        }

        // assumes current polygon is placed, and adjacent hasn't been placed
        public void PlacePolygon(Polygon currentPolygon, Polygon adjacentPolygon)
        {
            
        }

        public void Undo()
        {

        }

        public bool Validate(Polygon adjacentPolygon)
        {
            // TODO optimize by checking bounding boxes
            for (int j = 0; j < Polygons.Length; j++)
            {
                var polygon = Polygons[j];
                if (polygon.HasBeenPlaced)
                {
                    foreach (Edge2D setEdge in Polygons[j].Edges)
                    {
                        foreach (Edge2D edge in adjacentPolygon.Edges)
                        {
                            if (edge.Intersection(setEdge))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public List<(Polygon, Polygon)> GetMoves()
        {
            return null;
        }
    }
}
