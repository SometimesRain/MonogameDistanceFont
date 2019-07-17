using System;

namespace DistanceFontExample
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Game1(800, 600))
                game.Run();
        }
    }
}
