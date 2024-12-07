using Unfolding.Client.Polyhedra.DataStructs;

namespace Unfolding.Client.Polyhedra.Solvers
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
            // TODO needs proper thought to work properly
            List<(Polygon, Polygon)> moves = net.GetMoves();
            foreach (var move in moves)
            {
                net.PlacePolygon(move.Item1, move.Item2);
                if (net.GetStatus() == NetStatus.Invalid)
                {
                    net.Undo();
                }
            }
            Solve(net); // Solve on every valid move
            if (net.GetStatus() == NetStatus.Complete)
            {
                return net;
            }
            return null;
        }
    }
}
