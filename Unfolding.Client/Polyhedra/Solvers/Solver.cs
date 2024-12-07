using Unfolding.Client.Polyhedra.DataStructs;

namespace Unfolding.Client.Polyhedra.Solvers
{
    public abstract class Solver
    {
        private Net2D netToSolve;
        public static int StepsToDo = 1;

        public Solver(Net2D net)
        {
            netToSolve = net;
        }

        public abstract Net2D Solve(Net2D net);

        public Net2D Solve()
        {
            return Solve(netToSolve);
        }
    }
}
