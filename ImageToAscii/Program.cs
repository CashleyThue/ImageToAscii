using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageToAscii
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1 && (args[0] == "--help" || args[0] == "-h"))
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  ImageToAscii <input> <scale> <output>");
                return;
            }
            
            if (args.Length != 3)
            {
                Console.Error.WriteLine(
                    "Usage: ImageToAscii <PathToInput> <Scale> <PathToOutput>");
                Environment.Exit(1);
            }
            
            if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine($"File not found: {args[0]}");
                Environment.Exit(1);
            }

            if (!int.TryParse(args[1], out int scale))
            {
                Console.Error.WriteLine("Scale must be an integer.");
                Environment.Exit(1);
            }
            
            string inputPath = args[0];
            string outputPath = args[2];
            
            var img = Image.Load<Rgba32>(inputPath);
            
            if (scale <= 0)
                throw new ArgumentException("Scale must be positive.");

            if (img.Width / scale == 0 || img.Height / scale == 0)
                throw new ArgumentException("Scale too large for image size.");
            
            int width = img.Width; int height = img.Height;
            
            double aspectRatio = 2;
            
            img.Mutate(x => x.Resize(
                width/scale, (int)(height / (scale * aspectRatio))
                ));
            
            width = img.Width; height = img.Height;

            string[,] grid = new string[height,width];
            
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Rgba32 pixel = img[x, y];
                    
                    int colorSum = pixel.R + pixel.G + pixel.B;
                    
                    switch (colorSum)
                    {
                        case 0:
                            grid[y, x] += '#';
                            break;
                        case int n when (0 < n && n <= 100):
                            grid[y, x] += 'X';
                            break;
                        case int n when (100 < n && n <= 200):
                            grid[y, x] += '%';
                            break;
                        case int n when (200 < n && n <= 300):
                            grid[y, x] += '&';
                            break;
                        case int n when (300 < n && n <= 400):
                            grid[y, x] += '*';
                            break;
                        case int n when (400 < n && n <= 500):
                            grid[y, x] += '+';
                            break;
                        case int n when (500 < n && n <= 600):
                            grid[y, x] += '/';
                            break;
                        case int n when (600 < n && n <= 700):
                            grid[y, x] += '(';
                            break;
                        case int n when (700 < n && n <= 750):
                            grid[y, x] += "'";
                            break;
                        default:
                            grid[y, x] += ' ';
                            break;
                    }
                }
            }
            
            File.WriteAllText(outputPath, "");
            for (int y = 0; y < height; y++)
            {
                string row = "";

                for (int x = 0; x < width; x++)
                {
                    row += grid[y, x];
                }

                Console.WriteLine(row);
                File.AppendAllText(outputPath, row + Environment.NewLine);
            }
        }
    }
}