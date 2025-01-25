using PolyhedraUnfolding.DataStructs3D;
using PolyhedraUnfolding.DataStructs2D.Nets;

namespace PolyhedraUnfolding.Solvers
{
    public class BFS : Solver
    {

        public BFS(Net2D net) : base(net)
        {
        }

        public BFS(Polyhedron polyhedron) : base(polyhedron)
        {
        }

        public override Net2D Solve(Net2D net)
        {
            return net;
        }
    }
}
