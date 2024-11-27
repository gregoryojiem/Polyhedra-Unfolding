using Unfolding.Client.Polyhedra.DataStructs;

namespace Unfolding.Client.Polyhedra.Solvers
{
    public abstract class Solver
    {
        public abstract Net2D Solve(Net2D net);
    }
}
