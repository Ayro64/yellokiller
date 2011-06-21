using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using YelloKiller.Moteur_Particule;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace YelloKiller
{
    static class Moteur_physique
    {
        static public void Collision_Armes_Ennemis(Heros heros1, Heros heros2, List<Garde> _gardes, List<Patrouilleur> _Patrouilleurs, List<Patrouilleur_a_cheval> _PatrouilleursAChevaux, List<Boss> _Boss, List<Shuriken> listeShuriken, MoteurParticule particule, SoundBank soundBank, ref List<EnnemiMort> morts, ContentManager content)
        {
            if (_gardes.Count != 0)
            {
                for (int i = 0; i < _gardes.Count; i++)
                {
                    if (_gardes[i].Rectangle.Intersects(particule.Rectangle_Hadoken_heros1(heros1)) ||
                        _gardes[i].Rectangle.Intersects(particule.Rectangle_Ball_heros1(heros1)) ||
                        heros1.AttaqueAuSabre(_gardes[i].X, _gardes[i].Y))
                    {
                        soundBank.PlayCue("cri");
                        morts.Add(new EnnemiMort(new Vector2(28 * _gardes[i].X, 28 * _gardes[i].Y), content, EnnemiMort.TypeEnnemiMort.garde));
                        if (_gardes[i].Alerte)
                            GameplayScreen.Alerte = false;
                        _gardes.RemoveAt(i);
                        break;
                    }
                    if (heros2 != null)
                    {
                        if (_gardes[i].Rectangle.Intersects(particule.Rectangle_Hadoken_heros2(heros2)) ||
                            _gardes[i].Rectangle.Intersects(particule.Rectangle_Ball_heros2(heros2)) ||
                            heros2.AttaqueAuSabre(_gardes[i].X, _gardes[i].Y))
                        {
                            soundBank.PlayCue("cri");
                            morts.Add(new EnnemiMort(new Vector2(28 * _gardes[i].X, 28 * _gardes[i].Y), content, EnnemiMort.TypeEnnemiMort.garde));
                            if (_gardes[i].Alerte)
                                GameplayScreen.Alerte = false;
                            _gardes.RemoveAt(i);
                            break;
                        }
                    }
                    for (int j = 0; j < listeShuriken.Count; j++)
                        if (_gardes[i].Rectangle.Intersects(listeShuriken[j].Rectangle))
                        {
                            soundBank.PlayCue("cri");
                            ServiceHelper.Get<IGamePadService>().Vibration(20);
                            morts.Add(new EnnemiMort(new Vector2(28 * _gardes[i].X, 28 * _gardes[i].Y), content, EnnemiMort.TypeEnnemiMort.garde));
                            if (_gardes[i].Alerte)
                                GameplayScreen.Alerte = false;
                            _gardes.RemoveAt(i);
                            listeShuriken.RemoveAt(j);
                            break;
                        }
                }
            }

            if (_Patrouilleurs.Count != 0)
            {
                for (int i = 0; i < _Patrouilleurs.Count; i++)
                {
                    if (_Patrouilleurs[i].Rectangle.Intersects(particule.Rectangle_Hadoken_heros1(heros1)) ||
                        _Patrouilleurs[i].Rectangle.Intersects(particule.Rectangle_Ball_heros1(heros1)) ||
                        heros1.AttaqueAuSabre(_Patrouilleurs[i].X, _Patrouilleurs[i].Y))
                    {
                        soundBank.PlayCue("Bruitage patrouilleur");
                        morts.Add(new EnnemiMort(new Vector2(28 * _Patrouilleurs[i].X, 28 * _Patrouilleurs[i].Y), content, EnnemiMort.TypeEnnemiMort.patrouilleur));
                        if (_Patrouilleurs[i].Alerte)
                            GameplayScreen.Alerte = false;
                        _Patrouilleurs.RemoveAt(i);
                        break;
                    }
                    if (heros2 != null)
                    {
                        if (_Patrouilleurs[i].Rectangle.Intersects(particule.Rectangle_Hadoken_heros2(heros2)) ||
                            _Patrouilleurs[i].Rectangle.Intersects(particule.Rectangle_Ball_heros2(heros2)) ||
                            heros2.AttaqueAuSabre(_Patrouilleurs[i].X, _Patrouilleurs[i].Y))
                        {
                            soundBank.PlayCue("Bruitage patrouilleur");
                            morts.Add(new EnnemiMort(new Vector2(28 * _Patrouilleurs[i].X, 28 * _Patrouilleurs[i].Y), content, EnnemiMort.TypeEnnemiMort.patrouilleur));
                            if (_Patrouilleurs[i].Alerte)
                                GameplayScreen.Alerte = false;
                            _Patrouilleurs.RemoveAt(i);
                            break;
                        }
                    }
                    for (int j = 0; j < listeShuriken.Count; j++)
                        if (_Patrouilleurs[i].Rectangle.Intersects(listeShuriken[j].Rectangle))
                        {
                            soundBank.PlayCue("Bruitage patrouilleur");
                            morts.Add(new EnnemiMort(new Vector2(28 * _Patrouilleurs[i].X, 28 * _Patrouilleurs[i].Y), content, EnnemiMort.TypeEnnemiMort.patrouilleur));
                            if (_Patrouilleurs[i].Alerte)
                                GameplayScreen.Alerte = false;
                            _Patrouilleurs.RemoveAt(i);
                            listeShuriken.RemoveAt(j);
                            break;
                        }
                }
            }

            if (_PatrouilleursAChevaux.Count != 0)
            {
                for (int i = 0; i < _PatrouilleursAChevaux.Count; i++)
                {
                    if ((_PatrouilleursAChevaux[i].Rectangle.Intersects(particule.Rectangle_Hadoken_heros1(heros1)) || 
                        _PatrouilleursAChevaux[i].Rectangle.Intersects(particule.Rectangle_Ball_heros1(heros1))) ||
                        heros1.AttaqueAuSabre(_PatrouilleursAChevaux[i].X, _PatrouilleursAChevaux[i].Y))
                    {
                        soundBank.PlayCue("Bruitage cheval");
                        morts.Add(new EnnemiMort(new Vector2(28 * _PatrouilleursAChevaux[i].X, 28 * _PatrouilleursAChevaux[i].Y), content, EnnemiMort.TypeEnnemiMort.patrouilleurACheval));
                        if (_PatrouilleursAChevaux[i].Alerte)
                            GameplayScreen.Alerte = false;
                        _PatrouilleursAChevaux.RemoveAt(i);
                        break;
                    }
                    if (heros2 != null)
                    {
                        if (_PatrouilleursAChevaux[i].Rectangle.Intersects(particule.Rectangle_Hadoken_heros2(heros2)) ||
                            _PatrouilleursAChevaux[i].Rectangle.Intersects(particule.Rectangle_Ball_heros2(heros2)) ||
                            heros2.AttaqueAuSabre(_PatrouilleursAChevaux[i].X, _PatrouilleursAChevaux[i].Y))
                        {
                            soundBank.PlayCue("Bruitage cheval");
                            morts.Add(new EnnemiMort(new Vector2(28 * _PatrouilleursAChevaux[i].X, 28 * _PatrouilleursAChevaux[i].Y), content, EnnemiMort.TypeEnnemiMort.patrouilleurACheval));
                            if (_PatrouilleursAChevaux[i].Alerte)
                                GameplayScreen.Alerte = false;
                            _PatrouilleursAChevaux.RemoveAt(i);
                            break;
                        }
                    }
                    for (int j = 0; j < listeShuriken.Count; j++)
                        if (_PatrouilleursAChevaux[i].Rectangle.Intersects(listeShuriken[j].Rectangle))
                        {
                            soundBank.PlayCue("Bruitage cheval");
                            morts.Add(new EnnemiMort(new Vector2(28 * _PatrouilleursAChevaux[i].X, 28 * _PatrouilleursAChevaux[i].Y), content, EnnemiMort.TypeEnnemiMort.patrouilleurACheval));
                            if (_PatrouilleursAChevaux[i].Alerte)
                                GameplayScreen.Alerte = false;
                            _PatrouilleursAChevaux.RemoveAt(i);
                            listeShuriken.RemoveAt(j);
                            break;
                        }
                }
            }

            if (_Boss.Count != 0)
            {
                for (int i = 0; i < _Boss.Count; i++)
                {
                    if (_Boss[i].Vie < 0 || heros1.AttaqueAuSabre(_Boss[i].X, _Boss[i].Y))
                    {
                        _Boss[i].Vie = 5;
                        morts.Add(new EnnemiMort(new Vector2(28 * _Boss[i].X, 28 * _Boss[i].Y), content, EnnemiMort.TypeEnnemiMort.boss));
                        _Boss.RemoveAt(i);
                        soundBank.PlayCue("cri");
                    }
                    else if (_Boss[i].Rectangle.Intersects(particule.Rectangle_Hadoken_heros1(heros1)))
                    {
                        _Boss[i].Vie = _Boss[i].Vie - 2;
                        soundBank.PlayCue("Bruitage boss touche");
                        particule.Rectangle_Hadoken_Est_Present_Hero1 = false;
                    }
                    else if (_Boss[i].Rectangle.Intersects(particule.Rectangle_Ball_heros1(heros1)))
                    {
                        _Boss[i].Vie = _Boss[i].Vie - 2;
                        soundBank.PlayCue("Bruitage boss touche");
                        particule.Rectangle_Ball_Est_Present_Hero1 = false;
                    }
                    else if (heros2 != null)
                    {
                        if (_Boss[i].Vie < 0 || heros2.AttaqueAuSabre(_Boss[i].X, _Boss[i].Y))
                        {
                            _Boss[i].Vie = 5;
                            morts.Add(new EnnemiMort(new Vector2(28 * _Boss[i].X, 28 * _Boss[i].Y), content, EnnemiMort.TypeEnnemiMort.boss));
                            _Boss.RemoveAt(i);
                            soundBank.PlayCue("cri");
                        }
                        else if (_Boss[i].Rectangle.Intersects(particule.Rectangle_Hadoken_heros2(heros2)))
                        {
                            _Boss[i].Vie = _Boss[i].Vie - 2;
                            soundBank.PlayCue("Bruitage boss touche");
                            particule.Rectangle_Hadoken_Est_Present_Hero2 = false;
                        }
                        else if (_Boss[i].Rectangle.Intersects(particule.Rectangle_Ball_heros2(heros2)))
                        {
                            _Boss[i].Vie = _Boss[i].Vie - 2;
                            soundBank.PlayCue("Bruitage boss touche");
                            particule.Rectangle_Ball_Est_Present_Hero2 = false;
                        }
                    }
                    for (int j = 0; j < listeShuriken.Count; j++)
                        if (_Boss[i].Rectangle.Intersects(listeShuriken[j].Rectangle))
                        {
                            // une fois que le shuriken a touché le boss, le boss regarder vers le heros
                            if (listeShuriken[j].Direction == Vector2.UnitY)
                                _Boss[i].SourceRectangle = new Rectangle(26, 0, 16, 24);
                            else if (listeShuriken[j].Direction == -Vector2.UnitX)
                                _Boss[i].SourceRectangle = new Rectangle(26, 33, 16, 24);
                            else if (listeShuriken[j].Direction == -Vector2.UnitY)
                                _Boss[i].SourceRectangle = new Rectangle(26, 64, 16, 24);
                            else if (listeShuriken[j].Direction == Vector2.UnitX)
                                _Boss[i].SourceRectangle = new Rectangle(26, 97, 16, 24);

                            soundBank.PlayCue("Bruitage boss touche");
                            _Boss[i].Vie--;
                            listeShuriken.RemoveAt(j);
                            break;
                        }
                }
            }
        }
        
        static public bool Collision_Garde_Heros(List<Garde> _gardes, Heros heros1, Heros heros2, SoundBank soundBank)
        {
            for (int b = 0; b < _gardes.Count; b++)
            {
                if (_gardes[b].Rectangle.Intersects(heros1.Rectangle))
                {
                    ServiceHelper.Get<IGamePadService>().Vibration(50);
                    soundBank.PlayCue("CriMortHero");
                    return true;
                }
            }
            if (heros2 != null)
            {
                for (int b = 0; b < _gardes.Count; b++)
                {
                    if (_gardes[b].Rectangle.Intersects(heros2.Rectangle))
                    {
                        ServiceHelper.Get<IGamePadService>().Vibration(50);
                        soundBank.PlayCue("CriMortHero");
                        return true;
                    }
                }
            }
            return false;
        }
        
        static public bool Collision_Patrouilleur_Heros(List<Patrouilleur> _patrouilleurs, Heros heros1, Heros heros2, SoundBank soundBank)
        {
            for (int b = 0; b < _patrouilleurs.Count; b++)
            {
                if (_patrouilleurs[b].Rectangle.Intersects(heros1.Rectangle))
                {
                    ServiceHelper.Get<IGamePadService>().Vibration(50);
                    soundBank.PlayCue("CriMortHero");
                    return true;
                }
            }
            if (heros2 != null)
            {
                for (int b = 0; b < _patrouilleurs.Count; b++)
                {
                    if (_patrouilleurs[b].Rectangle.Intersects(heros2.Rectangle))
                    {
                        ServiceHelper.Get<IGamePadService>().Vibration(50);
                        soundBank.PlayCue("CriMortHero");
                        return true;
                    }
                }
            }
            return false;
        }
        
        static public bool Collision_PatrouilleurACheval_Heros(List<Patrouilleur_a_cheval> _patrouilleursAChevaux, Heros heros1, Heros heros2, SoundBank soundBank)
        {
            for (int b = 0; b < _patrouilleursAChevaux.Count; b++)
            {
                if (_patrouilleursAChevaux[b].Rectangle.Intersects(heros1.Rectangle))
                {
                    ServiceHelper.Get<IGamePadService>().Vibration(50);
                    soundBank.PlayCue("CriMortHero");
                    return true;
                }
            }
            if (heros2 != null)
            {
                for (int b = 0; b < _patrouilleursAChevaux.Count; b++)
                {
                    if (_patrouilleursAChevaux[b].Rectangle.Intersects(heros2.Rectangle))
                    {
                        ServiceHelper.Get<IGamePadService>().Vibration(50);
                        soundBank.PlayCue("CriMortHero");
                        return true;
                    }
                }
            }
            return false;
        }
        
        static public bool Collision_Boss_Heros(List<Boss> _Boss, Heros heros1, Heros heros2, SoundBank soundBank)
        {
            for (int b = 0; b < _Boss.Count; b++)
            {
                if (_Boss[b].Rectangle.Intersects(heros1.Rectangle))
                {
                    ServiceHelper.Get<IGamePadService>().Vibration(50);
                    soundBank.PlayCue("CriMortHero");
                    return true;
                }
            }
            if (heros2 != null)
            {
                for (int b = 0; b < _Boss.Count; b++)
                {
                    if (_Boss[b].Rectangle.Intersects(heros2.Rectangle))
                    {
                        ServiceHelper.Get<IGamePadService>().Vibration(50);
                        soundBank.PlayCue("CriMortHero");
                        return true;
                    }
                }
            }
            return false;
        }
        
        static public bool Collision_Heros_ExplosionStatues(List<Statue> _statues, Heros heros1, Heros heros2, MoteurParticule particule, SoundBank soundBank)
        {
            if (_statues.Count != 0)
            {
                for (int i = 0; i < _statues.Count; i++)
                {
                    if (heros1.Rectangle.Intersects(particule.Rectangle_Hadoken_Statue(_statues[i])))
                    {                       
                        ServiceHelper.Get<IGamePadService>().Vibration(50);
                        soundBank.PlayCue("CriMortHero");
                        return true;
                    }
                    if (heros2 != null)
                    {
                        if (heros2.Rectangle.Intersects(particule.Rectangle_Hadoken_Statue(_statues[i])))
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
        
        static public bool Collision_Heros_Bonus(ref Heros heros1, ref Heros heros2, ref List<Bonus> bonus, SoundBank soundBank)
        {
            for (int u = 0; u < bonus.Count; u++)
            {
                if (heros1.X == bonus[u].X && heros1.Y == bonus[u].Y)
                {
                    switch (bonus[u].TypeBonus)
                    {
                        case TypeBonus.shuriken:
                            heros1.NombreShuriken += 3;
                            bonus.RemoveAt(u);
                            soundBank.PlayCue("shurikenobt");
                            break;
                        case TypeBonus.hadoken:
                            heros1.NombreHadoken++;
                            bonus.RemoveAt(u);
                            soundBank.PlayCue("hadokenobt");
                            break;
                        case TypeBonus.checkPoint:
                            bonus.RemoveAt(u);
                            soundBank.PlayCue("CheckPoint");
                            return true;
                    }
                }
            }

            if (heros2 != null)
            {
                for (int u = 0; u < bonus.Count; u++)
                {
                    if (heros2.X == bonus[u].X && heros2.Y == bonus[u].Y)
                    {
                        switch (bonus[u].TypeBonus)
                        {
                            case TypeBonus.shuriken:
                                heros2.NombreShuriken += 3;
                                bonus.RemoveAt(u);
                                soundBank.PlayCue("shurikenobt");
                                break;
                            case TypeBonus.hadoken:
                                heros2.NombreHadoken++;
                                bonus.RemoveAt(u);
                                soundBank.PlayCue("hadokenobt");
                                break;
                            case TypeBonus.checkPoint:
                                bonus.RemoveAt(u);
                                soundBank.PlayCue("CheckPoint");
                                return true;
                        }
                    }
                }
            }
            return false;
        }

        static public void Collisions_Heros_Interrupteurs(Heros heros1, Heros heros2, ref List<Interrupteur> interrupteurs, SoundBank soundBank, Carte carte)
        {
            foreach (Interrupteur bouton in interrupteurs)
                if (heros1.X == bouton.position.X && heros1.Y == bouton.position.Y || heros2 != null && heros2.X == bouton.position.X && heros2.Y == bouton.position.Y)
                    bouton.OuvrirPorte(soundBank, carte);
        }

        static public bool Collision_Heros_Dark_Hero(Heros heros, Dark_Hero dark, SoundBank SB)
        {
            if (dark != null && heros.Rectangle.Intersects(dark.Rectangle))
            {
                SB.PlayCue("dark");
                return true;
            }
            return false;
        }
    }
}