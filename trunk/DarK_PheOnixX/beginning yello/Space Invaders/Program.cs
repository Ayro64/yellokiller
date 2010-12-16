using System;

namespace Space_Invaders
{
    static class Program
    {
        static void Main(string[] args)
        {
            using (Yello_Killer game = new Yello_Killer())
            {
                game.Run();
            }
        }
    }
}

