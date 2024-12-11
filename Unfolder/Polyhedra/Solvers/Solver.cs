using Polyhedra.DataStructs3D;
using Polyhedra.DataStructs2D.Nets;

namespace Unfolder.Polyhedra.Solvers
{
    public abstract class Solver
    {
        private Net2D netToSolve;
        public static int StepsToDo = 1;
        public int StepsTaken = 0;
        public static bool UseSteps = true;

        public Solver(Net2D net)
        {
            netToSolve = net;
        }

        public Solver(Polyhedron polyhedron)
        {
            netToSolve = polyhedron.ToNet2D();
        }

        public abstract Net2D Solve(Net2D net);

        public Net2D Solve()
        {
            return Solve(netToSolve);
        }
    }
}
