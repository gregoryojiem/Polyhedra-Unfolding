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

                if (net.GetStatus() == NetStatus.Complete)
                {
                    return net;
                }
                else if (net.GetStatus() == NetStatus.Valid) {
                    Solve(net);
                    if (net.GetStatus() == NetStatus.Complete)
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
