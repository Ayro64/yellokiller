using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using YelloKiller.YelloKiller;
using System;

namespace YelloKiller
{
    static class Moteur_physique
    {
        static public void Collision_Shuriken_Ennemis(List<Garde> _gardes, List<Patrouilleur> _Patrouilleurs, List<patrouilleur_a_cheval> _PatrouilleursAChevaux, List<Shuriken> listeShuriken, SoundBank soundBank)
        {
            if (_gardes.Count != 0)
            {
                for (int i = 0; i < _gardes.Count; i++)
                    for (int j = 0; j < listeShuriken.Count; j++)
                        if (_gardes[i].Rectangle.Intersects(listeShuriken[j].Rectangle))
                        {
                            soundBank.PlayCue("cri");
                            _gardes.Remove(_gardes[i]);
                            listeShuriken.Remove(listeShuriken[j]);
                            break;
                        }
            }
            if (_Patrouilleurs.Count != 0)
            {
                for (int i = 0; i < _Patrouilleurs.Count; i++)
                    for (int j = 0; j < listeShuriken.Count; j++)
                        if (_Patrouilleurs[i].Rectangle.Intersects(listeShuriken[j].Rectangle))
                        {
                            soundBank.PlayCue("cri");
                            _Patrouilleurs.Remove(_Patrouilleurs[i]);
                            listeShuriken.Remove(listeShuriken[j]);
                            break;
                        }
            }
            if (_PatrouilleursAChevaux.Count != 0)
            {
                for (int i = 0; i < _PatrouilleursAChevaux.Count; i++)
                    for (int j = 0; j < listeShuriken.Count; j++)
                        if (_PatrouilleursAChevaux[i].Rectangle.Intersects(listeShuriken[j].Rectangle))
                        {
                            soundBank.PlayCue("cri");
                            _PatrouilleursAChevaux.Remove(_PatrouilleursAChevaux[i]);
                            listeShuriken.Remove(listeShuriken[j]);
                            break;
                        }
            }
        }

        //Commentaire destine a Etienne : Bite avec un 'T'

        static public bool Collision_Garde_Heros(List<Garde> _gardes, Hero1 hero1, Hero2 hero2, SoundBank soundBank)
        {
            for (int b = 0; b < _gardes.Count; b++)
            {
                if (_gardes[b].Rectangle.Intersects(hero1.Rectangle) || _gardes[b].rectangle.Intersects(hero2.Rectangle))
                {
                    soundBank.PlayCue("CriMortHero");
                    return true;
                }
            }
            return false;
        }

        static public bool Collision_Patrouilleur_Heros(List<Patrouilleur> _patrouilleurs, Hero1 hero1, Hero2 hero2, SoundBank soundBank)
        {
            for (int b = 0; b < _patrouilleurs.Count; b++)
            {
                if (_patrouilleurs[b].Rectangle.Intersects(hero1.Rectangle) || _patrouilleurs[b].rectangle.Intersects(hero2.Rectangle))
                {
                    soundBank.PlayCue("CriMortHero");
                    return true;
                }
            }
            return false;
        }

        static public bool Collision_PatrouilleurACheval_Heros(List<patrouilleur_a_cheval> _patrouilleursAChevaux, Hero1 hero1, Hero2 hero2, SoundBank soundBank)
        {
            for (int b = 0; b < _patrouilleursAChevaux.Count; b++)
            {
                if (_patrouilleursAChevaux[b].Rectangle.Intersects(hero1.Rectangle) || _patrouilleursAChevaux[b].rectangle.Intersects(hero2.Rectangle))
                {
                    soundBank.PlayCue("CriMortHero");
                    return true;
                }
            }
            return false;
        }
        static public bool Collision_Garde_Hero(List<Garde> _gardes, Hero hero, SoundBank soundBank)
        {
            for (int b = 0; b < _gardes.Count; b++)
            {
                if (_gardes[b].Rectangle.Intersects(hero.Rectangle))
                {
                    soundBank.PlayCue("CriMortHero");
                    return true;
                }
            }
            return false;
        }

        static public bool Collision_Patrouilleur_Hero(List<Patrouilleur> _patrouilleurs, Hero hero, SoundBank soundBank)
        {
            for (int b = 0; b < _patrouilleurs.Count; b++)
            {
                if (_patrouilleurs[b].Rectangle.Intersects(hero.Rectangle))
                {
                    soundBank.PlayCue("CriMortHero");
                    return true;
                }
            }
            return false;
        }

        static public bool Collision_PatrouilleurACheval_Hero(List<patrouilleur_a_cheval> _patrouilleursAChevaux, Hero hero, SoundBank soundBank)
        {
            for (int b = 0; b < _patrouilleursAChevaux.Count; b++)
            {
                if (_patrouilleursAChevaux[b].Rectangle.Intersects(hero.Rectangle))
                {
                    soundBank.PlayCue("CriMortHero");
                    return true;
                }
            }
            return false;
        }
/*
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
        }*/
    }
}