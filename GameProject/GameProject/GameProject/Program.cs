using System;

namespace GameProject
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GameController game = new GameController())
            {
                game.Run();
            }
        }
    }
#endif
}

