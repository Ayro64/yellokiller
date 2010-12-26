using System;

namespace sprite_animé
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Yellokiller game = new Yellokiller())
            {
                game.Run();
            }
        }
    }
}

