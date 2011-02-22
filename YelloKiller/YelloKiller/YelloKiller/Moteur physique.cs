using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using YelloKiller.YelloKiller;
using System;

namespace YelloKiller
{
    static class Moteur_physique
    {
        static public void Collision_Shuriken_Ennemi(List<Garde> listeGardes, List<Shuriken> listeShuriken, SoundBank soundBank)
        {
            if (listeGardes.Count != 0)
            {
                for (int i = 0; i < listeGardes.Count; i++)
                    for (int j = 0; j < listeShuriken.Count; j++)
                        if (listeGardes[i].rectangle.Intersects(listeShuriken[j].Rectangle))
                        {
                            soundBank.PlayCue("cri");
                            listeGardes.Remove(listeGardes[i]);
                            listeShuriken.Remove(listeShuriken[j]);
                            break;
                        }
            }
        }

        //Commentaire destine a Etienne : Bite avec un 'T'
        
        static public bool Collision_Ennemi_Heros(List<Garde> listeGardes, Hero1 hero1, Hero2 hero2, SoundBank soundBank)
        {
            for (int b = 0; b < listeGardes.Count; b++)
            {
                if (listeGardes[b].Rectangle.Intersects(hero1.Rectangle) || listeGardes[b].rectangle.Intersects(hero2.Rectangle))
                {
                    soundBank.PlayCue("CriMortHero");
                    return true;
                }
            }
            return false;
        }

        static public bool Collision_Ennemi_Hero(List<Garde> listeGardes, Hero hero, SoundBank soundBank)
        {
            for (int b = 0; b < listeGardes.Count; b++)
            {
                if (listeGardes[b].rectangle.Intersects(hero.Rectangle))
                {
                    soundBank.PlayCue("CriMortHero");
                    return true;
                }
            }
            return false;
        }
    }
}