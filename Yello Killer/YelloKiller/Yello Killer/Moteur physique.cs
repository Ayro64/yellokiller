using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Yellokiller
{
    static class Moteur_physique
    {
        static public void Collision(Rectangle hero1, Rectangle hero2, ref bool droite1, ref bool gauche1, ref bool monter1, ref bool descendre1)
        {
            droite1 = true;
            gauche1 = true;
            monter1 = true;
            descendre1 = true;

            if (hero1.Intersects(hero2))
            {
                if (hero1.Top == hero2.Bottom - 1 || hero1.Top == hero2.Bottom - 2 || hero1.Top == hero2.Bottom - 4)
                {
                    monter1 = false;
                }

                if (hero1.Bottom == hero2.Top + 1 || hero1.Bottom == hero2.Top + 2 || hero1.Bottom == hero2.Top + 4)
                {
                    descendre1 = false;
                }

                if (hero1.Left == hero2.Right - 1 || hero1.Left == hero2.Right - 2 || hero1.Left == hero2.Right - 4)
                {
                    gauche1 = false;
                }

                if (hero1.Right == hero2.Left + 1 || hero1.Right == hero2.Left + 2 || hero1.Right == hero2.Left + 4)
                {
                    droite1 = false;
                }
            }
        }
    }
}