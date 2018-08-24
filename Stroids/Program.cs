using System;

namespace Stroids
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            using (var game = AsteroidsGame.Instance)
            {
                game.Run();
            }
        }
    }
}
