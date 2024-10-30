using MIConvexHull;
using System.Diagnostics;

namespace Unfolding.Client.Polyhedra.DataStructs
{
    public class Face : ConvexFace<Point3D, Face>
    {
        // Contains Adjacency as Face[]
        // Normal as double[]
        // Vertices as Point3D[]
    }
}
