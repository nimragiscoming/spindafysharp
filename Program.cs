using spindafysharp;
using System.Drawing;
using System.Drawing.Imaging;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Input image path to Spindafy...");

        string path = Console.ReadLine();

        Bitmap source = new Bitmap(path);

        Bitmap spinda = new Bitmap(@"images\spinda_base.png");
        Bitmap spindaMask = new Bitmap(@"images\spinda_mask.png");

        Bitmap[] spots = new Bitmap[4];
        spots[0] = new Bitmap(@"images\spot_1.png");
        spots[1] = new Bitmap(@"images\spot_2.png");
        spots[2] = new Bitmap(@"images\spot_3.png");
        spots[3] = new Bitmap(@"images\spot_4.png");


        Bitmap map = new Bitmap(source.Width,source.Height);

        Graphics g = Graphics.FromImage(map);

        g.Clear(Color.White);

        SpindaDrawer drawer = new SpindaDrawer(g, map,spinda,spindaMask, spots);

        int countX = source.Width/32;
        int countY = source.Height / 32;

        for (int i = 0; i < countX; i++)
        {
            for (int j = 0; j < countY; j++)
            {
                Point point = new Point(i * 32, j * 32);

                SpindaDrawer.Spinda sp = drawer.GenerateSpinda(point, source);

                drawer.DrawSpinda(point, sp);

                Console.WriteLine("Drawn Spinda " + (1 + j + i * countY) + "/" + countX * countY);
            }
        }

        map.Save(@"images\spindafied.png", ImageFormat.Png);

        Console.WriteLine(@"Successfully Spindafied and saved to images\spindafied.png");
    }
}