using Unfolding.Client.Polyhedra.DataStructs;
namespace Unfolding.Client.Polyhedra
{
    public class Align
    {
        public static PolyhedraFace Rotate3DToAlign(PolyhedraFace face)
        {
            Point3D centroid = getCentroid(face);
            face.TranslateToOrigin();

            Vec3D planeNormalVec = new(face.Normal[0], face.Normal[1], face.Normal[2]);
            Vec3D targetVector = new(0, 1, 0);

            double angle = Math.Acos(planeNormalVec.Dot(targetVector));
            double crossProd = planeNormalVec.Cross(targetVector).Y;
            
            if (crossProd > 0)
            {
                angle = -angle;
            }

            Point3D[] newPoints = [];
            foreach (Point3D p in face.Vertices)
            {
                //todo call matrix rotation
                Point3D rotP = p.Rotate(angle, "y");
                newPoints.Append(rotP);
            }
            return new PolyhedraFace(newPoints);
        }

        private static Point3D getCentroid(PolyhedraFace face)
        {
            Point3D[] vertices = face.Vertices;
            int numVertices = vertices.Length;
            double sumX = 0, sumY = 0, sumZ = 0;
            for (int i = 0; i < numVertices; i++)
            {
                Point3D currVertex = vertices[i];
                sumX += currVertex.X; sumY += currVertex.Y; sumZ += currVertex.Z;
            }
            return new Point3D(sumX / numVertices, sumY / numVertices, sumZ / numVertices);
        }
    }
}
