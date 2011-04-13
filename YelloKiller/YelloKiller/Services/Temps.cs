using System;

namespace YelloKiller
{
    static class Temps
    {
        /* Cette classe ne contient qu'une seule fonction qui permet de convertir un nombre de seconde en minutes et secondes, le tout proprement, 
         * j'ai trouvé ca sur Internet. Adrien*/

        public static string Conversion(double seconde)
        {
            TimeSpan t = TimeSpan.FromSeconds(seconde);

            if (t.Seconds < 59 && t.Minutes == 0)
                return string.Format("{0:D2}", t.Seconds);
            else if (t.Minutes < 59 && t.Hours == 0)
                return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
            else
                return string.Format("{0:D1}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
        }
    }
}
