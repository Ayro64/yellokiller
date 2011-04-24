﻿using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using YelloKiller.Moteur_Particule;
using Microsoft.Xna.Framework.Audio;

namespace YelloKiller
{
    static class Moteur_physique
    {
        static public void Collision_Armes_Ennemis(Hero hero1, Hero hero2, List<Garde> _gardes, List<Patrouilleur> _Patrouilleurs, List<Patrouilleur_a_cheval> _PatrouilleursAChevaux, List<Boss> _Boss, List<Shuriken> listeShuriken, MoteurParticule particule, SoundBank soundBank)
        {
            if (_gardes.Count != 0)
            {
                for (int i = 0; i < _gardes.Count; i++)
                {
                    if (_gardes[i].Rectangle.Intersects(particule.Rectangle_Hadoken_hero1(hero1)))
                    {
                        soundBank.PlayCue("cri");
                        _gardes.Remove(_gardes[i]);
                        break;
                    }
                    else if (_gardes[i].Rectangle.Intersects(particule.Rectangle_Ball_hero1(hero1)))
                    {
                        soundBank.PlayCue("cri");
                        _gardes.Remove(_gardes[i]);
                        break;
                    }
                    if (hero2 != null)
                    {
                        if (_gardes[i].Rectangle.Intersects(particule.Rectangle_Hadoken_hero2(hero2)))
                        {
                            soundBank.PlayCue("cri");
                            _gardes.Remove(_gardes[i]);
                            break;
                        }
                        else if (_gardes[i].Rectangle.Intersects(particule.Rectangle_Ball_hero2(hero2)))
                        {
                            soundBank.PlayCue("cri");
                            _gardes.Remove(_gardes[i]);
                            break;
                        }
                    }
                    for (int j = 0; j < listeShuriken.Count; j++)
                        if (_gardes[i].Rectangle.Intersects(listeShuriken[j].Rectangle))
                        {
                            soundBank.PlayCue("cri");
                            ServiceHelper.Get<IGamePadService>().Vibration(20);
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
                    if (_Patrouilleurs[i].Rectangle.Intersects(particule.Rectangle_Hadoken_hero1(hero1)))
                    {
                        soundBank.PlayCue("cri");
                        _Patrouilleurs.Remove(_Patrouilleurs[i]);
                        break;
                    }
                    else if (_Patrouilleurs[i].Rectangle.Intersects(particule.Rectangle_Ball_hero1(hero1)))
                    {
                        soundBank.PlayCue("cri");
                        _Patrouilleurs.Remove(_Patrouilleurs[i]);
                        break;
                    }
                    if (hero2 != null)
                    {
                        if (_Patrouilleurs[i].Rectangle.Intersects(particule.Rectangle_Hadoken_hero2(hero2)))
                        {
                            soundBank.PlayCue("cri");
                            _Patrouilleurs.Remove(_Patrouilleurs[i]);
                            break;
                        }
                        else if (_Patrouilleurs[i].Rectangle.Intersects(particule.Rectangle_Ball_hero2(hero2)))
                        {
                            soundBank.PlayCue("cri");
                            _Patrouilleurs.Remove(_Patrouilleurs[i]);
                            break;
                        }
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
                    if (_PatrouilleursAChevaux[i].Rectangle.Intersects(particule.Rectangle_Hadoken_hero1(hero1)))
                    {
                        soundBank.PlayCue("cri");
                        _PatrouilleursAChevaux.Remove(_PatrouilleursAChevaux[i]);
                        break;
                    }
                    else if (_PatrouilleursAChevaux[i].Rectangle.Intersects(particule.Rectangle_Ball_hero1(hero1)))
                    {
                        soundBank.PlayCue("cri");
                        _PatrouilleursAChevaux.Remove(_PatrouilleursAChevaux[i]);
                        break;
                    }
                    if (hero2 != null)
                    {
                        if (_PatrouilleursAChevaux[i].Rectangle.Intersects(particule.Rectangle_Hadoken_hero2(hero2)))
                        {
                            soundBank.PlayCue("cri");
                            _PatrouilleursAChevaux.Remove(_PatrouilleursAChevaux[i]);
                            break;
                        }
                        else if (_PatrouilleursAChevaux[i].Rectangle.Intersects(particule.Rectangle_Ball_hero2(hero2)))
                        {
                            soundBank.PlayCue("cri");
                            _PatrouilleursAChevaux.Remove(_PatrouilleursAChevaux[i]);
                            break;
                        }
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
                {
                    if (_Boss[i].Vie < 0)
                    {
                        _Boss[i].Vie = 5;
                        _Boss.Remove(_Boss[i]);
                        soundBank.PlayCue("cri");
                    }
                    else if (_Boss[i].Rectangle.Intersects(particule.Rectangle_Hadoken_hero1(hero1)))
                    {
                        _Boss[i].Vie = _Boss[i].Vie - 2;
                        // des que le boss est touche par le hadoken je supprime le rectangle jusqu au prochain
                        // appel sinon le boss perd sa vie d un coup.
                        particule.Rectangle_Hadoken_Est_Present_Hero1 = false;
                    }
                    else if (_Boss[i].Rectangle.Intersects(particule.Rectangle_Ball_hero1(hero1)))
                    {
                        _Boss[i].Vie = _Boss[i].Vie - 2;
                        // des que le boss est touche par le hadoken je supprime le rectangle jusqu au prochain
                        // appel sinon le boss perd sa vie d un coup.
                        particule.Rectangle_Ball_Est_Present_Hero1 = false;
                    }
                    else if (hero2 != null)
                    {
                        if (_Boss[i].Rectangle.Intersects(particule.Rectangle_Hadoken_hero2(hero2)))
                        {
                            _Boss[i].Vie = _Boss[i].Vie - 2;
                            // des que le boss est touche par le hadoken je supprime le rectangle jusqu au prochain
                            // appel sinon le boss perd sa vie d un coup.
                            particule.Rectangle_Hadoken_Est_Present_Hero2 = false;
                        }
                        else if (_Boss[i].Rectangle.Intersects(particule.Rectangle_Ball_hero2(hero2)))
                        {
                            _Boss[i].Vie = _Boss[i].Vie - 2;
                            // des que le boss est touche par le hadoken je supprime le rectangle jusqu au prochain
                            // appel sinon le boss perd sa vie d un coup.
                            particule.Rectangle_Ball_Est_Present_Hero2 = false;
                        }
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
                    ServiceHelper.Get<IGamePadService>().Vibration(50);
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
                        ServiceHelper.Get<IGamePadService>().Vibration(50);
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
                    ServiceHelper.Get<IGamePadService>().Vibration(50);
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
                        ServiceHelper.Get<IGamePadService>().Vibration(50);
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
                    ServiceHelper.Get<IGamePadService>().Vibration(50);
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
                        ServiceHelper.Get<IGamePadService>().Vibration(50);
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
                    ServiceHelper.Get<IGamePadService>().Vibration(50);
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
                        ServiceHelper.Get<IGamePadService>().Vibration(50);
                        soundBank.PlayCue("CriMortHero");
                        return true;
                    }
                }
            }
            return false;
        }

        static public bool Collision_Heros_ExplosionStatues(List<Statue> _statues, Hero hero1, Hero hero2, MoteurParticule particule, SoundBank soundBank)
        {
            if (_statues.Count != 0)
            {
                for (int i = 0; i < _statues.Count; i++)
                {
                    Console.WriteLine("statue =" + _statues[i].SourceRectangle.Value.Y);
                    Console.WriteLine("rectangle statue =" + particule.Rectangle_Hadoken_Statue(_statues[i]));
                    Console.WriteLine("rectangle hero  =" + hero1.Rectangle);
                    if (hero1.Rectangle.Intersects(particule.Rectangle_Hadoken_Statue(_statues[i])))
                    {                       
                        ServiceHelper.Get<IGamePadService>().Vibration(50);
                        soundBank.PlayCue("CriMortHero");
                        return true;
                    }
                    if (hero2 != null)
                    {
                        if (hero2.Rectangle.Intersects(particule.Rectangle_Hadoken_Statue(_statues[i])))
                        {
                            ServiceHelper.Get<IGamePadService>().Vibration(50);
                            soundBank.PlayCue("CriMortHero");
                            return true;
                        }

                    }
                }
            }
            return false;
        }

        static public void Collision_Heros_Bonus(ref Hero hero1, ref Hero hero2, ref List<Bonus> bonus)
        {
            for (int u = 0; u < bonus.Count; u++)
            {
                if (hero1.X == bonus[u].X && hero1.Y == bonus[u].Y)
                {
                    switch (bonus[u].TypeBonus)
                    {
                        case TypeBonus.shuriken:
                            hero1.NombreShurikens += 3;
                            bonus.RemoveAt(u);
                            break;
                        case TypeBonus.hadoken:
                            hero1.NombreHadokens++;
                            bonus.RemoveAt(u);
                            break;
                    }
                }
            }

            if (hero2 != null)
            {
                for (int u = 0; u < bonus.Count; u++)
                {
                    if (hero2.X == bonus[u].X && hero2.Y == bonus[u].Y)
                    {
                        switch (bonus[u].TypeBonus)
                        {
                            case TypeBonus.shuriken:
                                hero2.NombreShurikens += 3;
                                bonus.RemoveAt(u);
                                break;
                            case TypeBonus.hadoken:
                                hero2.NombreHadokens++;
                                bonus.RemoveAt(u);
                                break;
                        }
                    }
                }
            }
        }
    }
}