using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using System;
using YelloKiller.Moteur_Particule;

namespace YelloKiller
{
    static class Moteur_physique
    {

        static public void Collision_Armes_Ennemis(Hero hero, List<Garde> _gardes, List<Patrouilleur> _Patrouilleurs, List<Patrouilleur_a_cheval> _PatrouilleursAChevaux, List<Boss> _Boss, List<Shuriken> listeShuriken, MoteurParticule particule, SoundBank soundBank)
        {
            if (_gardes.Count != 0)
            {
                for (int i = 0; i < _gardes.Count; i++)
                {
                    if (_gardes[i].Rectangle.Intersects(particule.Rectangle_Hadoken(hero)))
                    {
                        soundBank.PlayCue("cri");
                        _gardes.Remove(_gardes[i]);
                        break;
                    }
                    for (int j = 0; j < listeShuriken.Count; j++)
                        if (_gardes[i].Rectangle.Intersects(listeShuriken[j].Rectangle))
                        {
                            soundBank.PlayCue("cri");
                            _gardes.Remove(_gardes[i]);
                            listeShuriken.Remove(listeShuriken[j]);
                            break;
                        }
                }
            }
            if (_Patrouilleurs.Count != 0)
            {
                for (int i = 0; i < _Patrouilleurs.Count; i++)
                {
                    if (_Patrouilleurs[i].Rectangle.Intersects(particule.Rectangle_Hadoken(hero)))
                    {
                        soundBank.PlayCue("cri");
                        _Patrouilleurs.Remove(_Patrouilleurs[i]);
                        break;
                    }
                    for (int j = 0; j < listeShuriken.Count; j++)
                        if (_Patrouilleurs[i].Rectangle.Intersects(listeShuriken[j].Rectangle))
                        {
                            soundBank.PlayCue("cri");
                            _Patrouilleurs.Remove(_Patrouilleurs[i]);
                            listeShuriken.Remove(listeShuriken[j]);
                            break;
                        }
                }
            }
            if (_PatrouilleursAChevaux.Count != 0)
            {
                for (int i = 0; i < _PatrouilleursAChevaux.Count; i++)
                {
                    if (_PatrouilleursAChevaux[i].Rectangle.Intersects(particule.Rectangle_Hadoken(hero)))
                    {
                        soundBank.PlayCue("cri");
                        _PatrouilleursAChevaux.Remove(_PatrouilleursAChevaux[i]);
                        break;
                    }
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

            if (_Boss.Count != 0)
            {
                for (int i = 0; i < _Boss.Count; i++)
                { // problème avec le boss, il perd toute sa vie d'un cou lors du hadoken.
                    Console.WriteLine(_Boss[i].Vie);
                    if (_Boss[i].Vie == 0)
                    {
                        _Boss[i].Vie = 5;
                        _Boss.Remove(_Boss[i]);
                        soundBank.PlayCue("cri");
                        

                    }
                    else if (_Boss[i].Rectangle.Intersects(particule.Rectangle_Hadoken(hero)))
                    {
                        _Boss[i].Vie--;
                        break;
                    } 

                    for (int j = 0; j < listeShuriken.Count; j++)
                        if (_Boss[i].Rectangle.Intersects(listeShuriken[j].Rectangle))
                        {
                            // une fois que le shuriken a touché le boss, le boss regarder vers le hero
                            if (listeShuriken[j].Direction == Vector2.UnitY)
                                _Boss[i].SourceRectangle = new Rectangle(26, 0, 16, 24);
                            else if (listeShuriken[j].Direction == -Vector2.UnitX)
                                _Boss[i].SourceRectangle = new Rectangle(26, 33, 16, 24);
                            else if (listeShuriken[j].Direction == -Vector2.UnitY)
                                _Boss[i].SourceRectangle = new Rectangle(26, 64, 16, 24);
                            else if (listeShuriken[j].Direction == Vector2.UnitX)
                                _Boss[i].SourceRectangle = new Rectangle(26, 97, 16, 24);

                            _Boss[i].Vie--;
                            listeShuriken.Remove(listeShuriken[j]);
                            break;
                        }
                }
            }
        }

        static public bool Collision_Garde_Heros(List<Garde> _gardes, Hero hero1, Hero hero2, SoundBank soundBank)
        {
            for (int b = 0; b < _gardes.Count; b++)
            {
                if (_gardes[b].Rectangle.Intersects(hero1.Rectangle))
                {
                    soundBank.PlayCue("CriMortHero");
                    return true;
                }
            }
            if (hero2 != null)
            {
                for (int b = 0; b < _gardes.Count; b++)
                {
                    if (_gardes[b].Rectangle.Intersects(hero2.Rectangle))
                    {
                        soundBank.PlayCue("CriMortHero");
                        return true;
                    }
                }
            }
            return false;
        }

        static public bool Collision_Patrouilleur_Heros(List<Patrouilleur> _patrouilleurs, Hero hero1, Hero hero2, SoundBank soundBank)
        {
            for (int b = 0; b < _patrouilleurs.Count; b++)
            {
                if (_patrouilleurs[b].Rectangle.Intersects(hero1.Rectangle))
                {
                    soundBank.PlayCue("CriMortHero");
                    return true;
                }
            }
            if (hero2 != null)
            {
                for (int b = 0; b < _patrouilleurs.Count; b++)
                {
                    if (_patrouilleurs[b].Rectangle.Intersects(hero2.Rectangle))
                    {
                        soundBank.PlayCue("CriMortHero");
                        return true;
                    }
                }
            }
            return false;
        }

        static public bool Collision_PatrouilleurACheval_Heros(List<Patrouilleur_a_cheval> _patrouilleursAChevaux, Hero hero1, Hero hero2, SoundBank soundBank)
        {
            for (int b = 0; b < _patrouilleursAChevaux.Count; b++)
            {
                if (_patrouilleursAChevaux[b].Rectangle.Intersects(hero1.Rectangle))
                {
                    soundBank.PlayCue("CriMortHero");
                    return true;
                }
            }
            if (hero2 != null)
            {
                for (int b = 0; b < _patrouilleursAChevaux.Count; b++)
                {
                    if (_patrouilleursAChevaux[b].Rectangle.Intersects(hero2.Rectangle))
                    {
                        soundBank.PlayCue("CriMortHero");
                        return true;
                    }
                }
            }
            return false;
        }

        static public bool Collision_Boss_Heros(List<Boss> _Boss, Hero hero1, Hero hero2, SoundBank soundBank)
        {
            for (int b = 0; b < _Boss.Count; b++)
            {
                if (_Boss[b].Rectangle.Intersects(hero1.Rectangle))
                {
                    soundBank.PlayCue("CriMortHero");
                    return true;
                }
            }
            if (hero2 != null)
            {
                for (int b = 0; b < _Boss.Count; b++)
                {
                    if (_Boss[b].Rectangle.Intersects(hero2.Rectangle))
                    {
                        soundBank.PlayCue("CriMortHero");
                        return true;
                    }
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

        static public bool Collision_PatrouilleurACheval_Hero(List<Patrouilleur_a_cheval> _patrouilleursAChevaux, Hero hero, SoundBank soundBank)
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

        static public bool Collision_Boss_Hero(List<Boss> _Boss, Hero hero, SoundBank soundBank)
        {
            for (int b = 0; b < _Boss.Count; b++)
            {
                if (_Boss[b].Rectangle.Intersects(hero.Rectangle))
                {
                    soundBank.PlayCue("CriMortHero");
                    return true;
                }
            }
            return false;
        }
    }
}