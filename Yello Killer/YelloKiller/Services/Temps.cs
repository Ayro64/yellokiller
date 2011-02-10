﻿using System;

namespace Yellokiller
{
    static class Temps
    {
        /* Cette classe ne contient qu'une seule fonction qui permet de convertir un nombre de seconde en minutes et secondes, le tout proprement, 
         * j'ai trouvé ca sur Internet. Adrien*/ 

        public static string Conversion(double seconde)
        {
            TimeSpan t = TimeSpan.FromSeconds(seconde);

            return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
        }
    }
}