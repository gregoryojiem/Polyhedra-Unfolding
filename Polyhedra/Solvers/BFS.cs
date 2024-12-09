using Polyhedra.DataStructs2D;
using Polyhedra.DataStructs3D;
using Polyhedra.DataStructs2D.Nets;

namespace Polyhedra.Solvers
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
