using PolyhedraUnfolding.DataStructs2D.Nets;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace NetGenerator
{
    public class Visualization
    {
        private const float PenScalar = 800f;

        public static string SaveNetToImage(Net2D net, string fileName, int width, int height)
        {
            net.PrepForImageConversion(width, height);
            var image = new Image<Rgba32>(width, height);
            var outlineSize = Math.Max(width, height) / PenScalar;
            var pen = Pens.Solid(Color.White, outlineSize);
            var brush = Brushes.Solid(Color.Red);

            image.Mutate(x => x.BackgroundColor(Color.Black));
            foreach (var polygon in net.Polygons)
            {
                var points = polygon.Vertices.Select(v => new PointF((float)v.X, (float)v.Y)).ToArray();
                image.Mutate(x => { x.FillPolygon(brush, points); });
                image.Mutate(x => { x.DrawPolygon(pen, points); });
            }

            var baseDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            var filePath = Path.Combine(baseDirectory.FullName, fileName);
            image.SaveAsPng(filePath);
            return filePath;
        }
    }
}
