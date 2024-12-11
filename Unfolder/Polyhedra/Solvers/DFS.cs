using Polyhedra.DataStructs3D;
using Polyhedra.DataStructs2D.Nets;

namespace Unfolder.Polyhedra.Solvers
{
    public class DFS : Solver
    {
        public DFS(Net2D net) : base(net)
        {
        }

        public DFS(Polyhedron polyhedron) : base(polyhedron)
        {
        }

        public override Net2D Solve(Net2D net)
        {
            Stack<Net2D> returnNets = new Stack<Net2D>();
            var moveTracker = new int[net.Polygons.Length];
            var moveTrackerIndex = 0;

            while (moveTracker[0] != 2)
            {
                var moves = net.GetMoves();
                var currentMoveIndex = moveTracker[moveTrackerIndex];

                if (currentMoveIndex > moves.Count - 1) // Dead end
                {
                    net.Undo();
                    moveTracker[moveTrackerIndex] = 0;
                    moveTrackerIndex--;
                }

                var currentMove = moves[currentMoveIndex];

                Console.WriteLine(net.Placements.Count + 1);
                if (UseSteps && StepsTaken >= StepsToDo)
                {
                    return net;
                }
                StepsTaken++;

                net.MakeMove(currentMove);

                var status = net.GetStatus();

                if (status == NetStatus.Complete)
                {
                    return net;
                }
                else if (status == NetStatus.Valid)
                {
                    moveTracker[moveTrackerIndex]++;
                    moveTrackerIndex++;
                }
                else if (status == NetStatus.Invalid)
                {
                    net.Undo();
                    moveTracker[moveTrackerIndex]++;
                }
            }

            throw new NotImplementedException("Net was unable to be solved");
        }
    }
}
