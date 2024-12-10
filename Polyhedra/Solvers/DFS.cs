using Polyhedra.DataStructs3D;
using Polyhedra.DataStructs2D.Nets;

namespace Unfolder.Polyhedra.Solvers
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
            List<NetMove> moves = net.GetMoves();
            foreach (var move in moves)
            {
                net.MakeMove(move);
                StepsTaken++;
                var status = net.GetStatus();

                if (status == NetStatus.Complete || (StepsTaken >= StepsToDo && !MainPageViewModel.DoUnfoldAnimation))
                {
                    return net;
                }
                else if (status == NetStatus.Invalid)
                {
                    net.Undo();
                }
                else if (status == NetStatus.Valid)
                {
                    var solvedNet = Solve(net);
                    if (solvedNet.GetStatus() == NetStatus.Complete || (StepsTaken >= StepsToDo && !MainPageViewModel.DoUnfoldAnimation))
                    {
                        StepsToDo = StepsTaken;
                        return solvedNet;
                    }
                    net.Undo();
                }
            }
            return net;
        }
    }
}
