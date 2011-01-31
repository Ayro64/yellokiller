using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;

namespace Yellokiller.Yello_Killer
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
                    monter1 = false;

                if (hero1.Bottom == hero2.Top + 1 || hero1.Bottom == hero2.Top + 2 || hero1.Bottom == hero2.Top + 4)
                    descendre1 = false;

                if (hero1.Left == hero2.Right - 1 || hero1.Left == hero2.Right - 2 || hero1.Left == hero2.Right - 4)
                    gauche1 = false;

                if (hero1.Right == hero2.Left + 1 || hero1.Right == hero2.Left + 2 || hero1.Right == hero2.Left + 4)
                    droite1 = false;
            }
        }

        static public void Collision_Shuriken_Ennemi(List<Ennemi> listeEnnemis, List<Shuriken> listeShuriken)
        {
            if (listeEnnemis.Count != 0)
            {
                for (int i = 0; i < listeEnnemis.Count; i++)
                    for (int j = 0; j < listeShuriken.Count; j++)
                        if (listeEnnemis[i].Rectangle.Intersects(listeShuriken[j].Rectangle))
                        {
                            listeEnnemis.Remove(listeEnnemis[i]);
                            listeShuriken.Remove(listeShuriken[j]);
                        }
            }
        }

        //Commentaire destine a Etienne : Bitte


        static public bool Collision_Ennemi_Heros(List<Ennemi> listeEnnemis, Hero1 hero1, Hero2 hero2)
        {
            for (int b = 0; b < listeEnnemis.Count; b++)
            {
                if (listeEnnemis[b].Rectangle.Intersects(hero1.Rectangle) || listeEnnemis[b].Rectangle.Intersects(hero2.Rectangle))
                    return true;
            }
            return false;
        }
    }
}