using System;

namespace YelloKiller
{
    static class Program
    {
        static void Main(string[] args)
        {
            using (YelloKiller game = new YelloKiller())
            {
                game.Run();
            }
        }
    }
}

