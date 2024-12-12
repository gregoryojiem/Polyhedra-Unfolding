using Polyhedra.DataStructs2D.Nets;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace PerformanceTesting
{
    public class Visualization
    {
        private readonly static int displaySize = 2000;
        private static int counter = 1;

        public static void SaveNetToImage(Net2D net)
        {
            var (width, height) = net.PrepForImageConversion(displaySize, displaySize/8);
            var image = new Image<Rgba32>(width, height);
            var pen = Pens.Solid(Color.White, 3);
            var brush = Brushes.Solid(Color.Red);

            image.Mutate(x => x.BackgroundColor(Color.Black));
            foreach (var polygon in net.Polygons)
            {
                var points = polygon.Vertices.Select(v => new PointF((float)v.X, (float)v.Y)).ToArray();
                image.Mutate(x => { x.FillPolygon(brush, points); });
                image.Mutate(x => { x.DrawPolygon(pen, points); });
            }

            SaveImage(image);
        }

        public static void SaveImage(Image image)
        {
            var baseDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            DirectoryInfo? projectRoot = baseDirectory.Parent?.Parent?.Parent;
            if (projectRoot == null)
            {
                throw new DirectoryNotFoundException("Couldn't find project root from: " + baseDirectory);
            }

            var imageRoot = Path.Combine(projectRoot.FullName, "images");

            if (!Directory.Exists(imageRoot))
            {
                throw new DirectoryNotFoundException("Image directory not found: " + imageRoot);
            }

            var fileName = counter++.ToString() + ".png";
            var filePath = Path.Combine(imageRoot, fileName);
            image.SaveAsPng(filePath);
        }
    }
}
