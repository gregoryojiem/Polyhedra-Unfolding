using Unfolding.Client.Polyhedra.DataStructs;

namespace Unfolding.Client.Polyhedra.Solvers
{
    public class DFS : Solver
    {
        public int StepsTaken = 0;

        public override Net2D Solve(Net2D net)
        {
            List<(Polygon, Polygon?)> moves = net.GetMoves();
            foreach (var move in moves)
            {
                net.PlacePolygon(move.Item1, move.Item2);
                StepsTaken++;

                if (net.GetStatus() == NetStatus.Complete || StepsTaken >= PolyMain.StepsToDo)
                {
                    return net;
                }
                else if (net.GetStatus() == NetStatus.Valid) {
                    Solve(net);
                    if (net.GetStatus() == NetStatus.Complete || StepsTaken >= PolyMain.StepsToDo)
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
