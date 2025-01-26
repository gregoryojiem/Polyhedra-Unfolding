using PolyhedraUnfolding;
using PolyhedraUnfolding.DataStructs3D;
using PolyhedraUnfolding.Solvers;
using System.Diagnostics;
using CommandLine;

namespace NetGenerator
{
    internal class Program
    {
        public class Options
        {
            [Value(0, MetaName = "polyhedra-name", Required = true, 
                HelpText = "Name of the polyhedron " +
                "(e.g. Dodecahedron, Truncated Dodecahedron, Snub Cube, Sphere, Random, etc.)")]
            public string ShapeName { get; set; }

            [Value(1, MetaName = "output-name", Required = true,
                HelpText = "The name to give the output file (e.g. dodecahedron.png)")]
            public string OutputFileName { get; set; }

            [Option('s', "sphere-params", Required = false, 
                HelpText = "Determines how many slices and stacks compose the sphere (e.g. 5,5). " +
                "Larger numbers give a better approximation, but they may take a long time to generate when going above ~10,000 faces.")]
            public string SphereParams { get; set; }

            [Option('p', "random-point-count", Required = false, 
                HelpText = "Determines how many points compose the random polyhedron (e.g. 1000)")]
            public string RandomPointCount { get; set; }

            [Option('r', "desired-resolution", Required = false, HelpText = "The resolution of the output image (e.g. 1920×1080). " +
                "If not specified, defaults to 1920×1080")]
            public string Resolution { get; set; }
        }

        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "testLargeSphere")
            {
                var imagePath = TestSuite.RunPerformanceTests();
                DisplayImage(imagePath);
                return;
            }

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParsingErrors);
        }

        public static void RunOptions(Options opts)
        {
            Polyhedron polyhedron;
            var shapeName = opts.ShapeName.ToLowerInvariant();

            switch (shapeName)
            {
                case "sphere":
                    if (opts.SphereParams == null)
                    {
                        PrintHelpMessage();
                        return;
                    }
                    var sphereParams = opts.SphereParams.Split(',').Select(int.Parse).ToArray();
                    polyhedron = PolyhedronExamples.GetSphere(sphereParams[0], sphereParams[1], 1);
                    break;
                case "random":
                    if (opts.RandomPointCount == null)
                    {
                        PrintHelpMessage();
                        return;
                    }
                    if (!int.TryParse(opts.RandomPointCount, out int randomPointCount))
                    {
                        Console.WriteLine("Could not parse the random point count given.");
                        PrintHelpMessage();
                        return;
                    }
                    polyhedron = PolyhedronExamples.GetRandomPolyhedron(randomPointCount, 1);
                    break;
                default:
                    if (!PolyhedronExamples.GetShapeNames().Contains(shapeName, StringComparer.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("The given shape '" + shapeName + "' is invalid. Choose from the available shapes!");
                        PrintHelpMessage();
                        return;
                    }
                    polyhedron = PolyhedronExamples.GetPolyhedron(shapeName);
                    break;
            }

            if (!ValidateExtension(opts.OutputFileName))
            {
                return;
            }

            var (width, height) = GetResolution(opts.Resolution);
            GenerateNetImage(polyhedron, opts.ShapeName, opts.OutputFileName, width, height);
        }

        public static (int, int) GetResolution(string resolutionString)
        {
            if (string.IsNullOrEmpty(resolutionString))
            {
                return (1920, 1080);
            }

            string[] dimensions = resolutionString.Split('x');
            if (dimensions.Length == 2 && 
                int.TryParse(dimensions[0], out int width) && 
                int.TryParse(dimensions[1], out int height))
            {
                return (width, height);
            }
            else
            {
                Console.WriteLine("Error: Given resolution was invalid. Use the format WidthxHeight (e.g. '1920x1080')");
                Console.WriteLine("Defaulting to 1920x1080...");
                return (1920, 1080);
            }
        }

        public static bool ValidateExtension(string fileName)
        {
            string extension = Path.GetExtension(fileName).TrimStart('.').ToLowerInvariant();
            var imageFormats = SixLabors.ImageSharp.Configuration.Default.ImageFormats;
            var supportedExtensions = imageFormats.SelectMany(f => f.FileExtensions).ToArray();
            if (!supportedExtensions.Contains(extension))
            {
                Console.WriteLine("Error: Output file must given with a supported image extension (e.g. file.png or file.jpg).");
                Console.WriteLine("Supported extensions are: " + string.Join(", ", supportedExtensions));
                return false;
            }
            return true;
        }

        public static void GenerateNetImage(Polyhedron polyhedron, string shapeName, string fileName, int width, int height)
        {
            Console.WriteLine("Generating net for a " + shapeName);
            Console.WriteLine("Face count : " + polyhedron.Faces.Length);
            Console.WriteLine("Vertex count : " + polyhedron.CountVertices());

            var solver = new DFS(polyhedron);
            var net = solver.Solve();
            var imagePath = Visualization.SaveNetToImage(net, fileName, width, height);

            DisplayImage(imagePath);
        }

        public static void DisplayImage(string imagePath)
        {
            var photoWindow = new Process();
            photoWindow.StartInfo.FileName = imagePath;
            photoWindow.StartInfo.UseShellExecute = true;
            photoWindow.Start();
        }

        public static void HandleParsingErrors(IEnumerable<Error> errs)
        {
            PrintHelpMessage();
        }

        public static void PrintHelpMessage()
        {
            Console.WriteLine("Usage: <shape> [options] <output.png/jpg/valid extension>");
            Console.WriteLine("The 'sphere' and 'random' polyhedra options require extra input. See help for more.");
            Console.WriteLine("Available shapes: " + string.Join(", ", PolyhedronExamples.GetShapeNames()));
        }
    }
}
