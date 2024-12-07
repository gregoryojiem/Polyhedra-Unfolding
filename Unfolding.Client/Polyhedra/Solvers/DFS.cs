using Unfolding.Client.Polyhedra.DataStructs;

namespace Unfolding.Client.Polyhedra.Solvers
{
    public class DFS : Solver
    {
        public int StepsTaken = 0;

        public DFS(Net2D net) : base(net)
        {
        }

        public DFS(Polyhedron polyhedron) : base(polyhedron)
        {
        }

        public override Net2D Solve(Net2D net)
        {
            Console.WriteLine(net.Placements.Count);
            List<(Polygon, Polygon?)> moves = net.GetMoves();
            foreach (var move in moves)
            {
                net.PlacePolygon(move.Item1, move.Item2);
                StepsTaken++;
                var status = net.GetStatus();

                if (status == NetStatus.Complete || StepsTaken >= StepsToDo)
                {
                    return net;
                }
                else if (status == NetStatus.Valid) {
                    Solve(net);
                    if (net.GetStatus() == NetStatus.Complete || StepsTaken >= StepsToDo)
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
