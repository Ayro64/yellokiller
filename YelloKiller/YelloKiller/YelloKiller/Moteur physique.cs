using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace YelloKiller
{
    static class Moteur_physique
    {
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
                            break;
                        }
            }
        }

        //Commentaire destine a Etienne : Bite avec un 'T'
        
        static public bool Collision_Ennemi_Heros(List<Ennemi> listeEnnemis, Hero1 hero1, Hero2 hero2)
        {
            for (int b = 0; b < listeEnnemis.Count; b++)
            {
                if (listeEnnemis[b].Rectangle.Intersects(hero1.Rectangle) || listeEnnemis[b].Rectangle.Intersects(hero2.Rectangle))
                    return true;
            }
            return false;
        }

        static public bool Collision_Ennemi_Hero(List<Ennemi> listeEnnemis, Hero hero)
        {
            for (int b = 0; b < listeEnnemis.Count; b++)
            {
                if (listeEnnemis[b].Rectangle.Intersects(hero.Rectangle))
                    return true;
            }
            return false;
        }
    }
}