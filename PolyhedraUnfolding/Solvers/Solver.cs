using PolyhedraUnfolding.DataStructs3D;
using PolyhedraUnfolding.DataStructs2D.Nets;

namespace PolyhedraUnfolding.Solvers
{
    public abstract class Solver
    {
        private Net2D netToSolve;
        public static bool UseNetSteps = false;

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
