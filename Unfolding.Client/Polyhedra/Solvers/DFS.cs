using Unfolding.Client.Polyhedra.DataStructs;

namespace Unfolding.Client.Polyhedra.Solvers
{
    public class DFS : Solver
    {
        public override Net2D Solve(Net2D net)
        {
            List<(Polygon, Polygon)> moves = net.GetMoves();
            foreach (var move in moves)
            {
                net.PlacePolygon(move.Item1, move.Item2);

                if (net.GetStatus() == 1)
                {
                    return net;
                }
                else if (net.GetStatus() == 0) {
                    Solve(net);
                    if (net.GetStatus() == 1)
                    {
                        return net;
                    }
                }
                else
                {
                    net.Undo();
                }
            }
            return net;
        }
    }
}
