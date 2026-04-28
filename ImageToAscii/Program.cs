using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageToAscii
{
    static class Program
    {
        static void Main(string[] args)
        {
            string ramp = @" .'`^"",:;Il!i><~+_-?][}{1)(|\/*tfjrxnuvczXYUJCLQ0OZmwqpdbkhao#MW&8%B@$";
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
            
            using var img = Image.Load<Rgba32>(inputPath);
            
            if (img.Width / scale == 0 ||
                img.Height / scale == 0)
            {
                Console.Error.WriteLine(
                    "Scale too large for image size.");
                Environment.Exit(1);
            }

            const double aspectRatio = 2;
            int originalWidth = img.Width;
            int originalHeight = img.Height;

            img.Mutate(x => x.Resize(
                originalWidth / scale,
                Math.Max(1, (int)(originalHeight / (scale * aspectRatio)))
            ));

            StringBuilder str = new();

            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    Rgba32 pixel = img[x, y];

                    int gray = (int)(
                        0.299 * pixel.R +
                        0.587 * pixel.G +
                        0.114 * pixel.B
                    );

                    int i = gray * (ramp.Length - 1) / 255;
                    
                    str.Append(ramp[i]);
                }
                str.AppendLine();
            }
            File.WriteAllText(outputPath, str.ToString(), Encoding.UTF8);
        }
    }
}