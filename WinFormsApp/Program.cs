using System;

namespace WinFormsApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Game8())
            {
                game.IsMouseVisible = true;
                game.Run();
            }
        }
    }
}
