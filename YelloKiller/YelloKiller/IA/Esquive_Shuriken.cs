using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace YelloKiller.IA
{
    static class Esquive_Shuriken
    {
        public static void Boss_Esquive_Shuriken(Hero hero, Boss boss, List<Shuriken> shuriken, Carte carte, Rectangle camera)
        {
            foreach (Shuriken _shuriken in shuriken)
            {
                if (boss.VaEnHaut && boss.VaEnBas && boss.VaADroite && boss.VaAGauche)
                {
                    if (_shuriken.Direction == Vector2.UnitX && boss.Regarde_Gauche && Math.Abs(boss.X - _shuriken.X) < 4 && boss.Y == _shuriken.Y)
                    // si le shuriken va a droite , le shuriken est a moins de 4 cases du boss et que le shuriken
                    // et le boss sont sur la meme position en Y alors :
                    {
                        if (boss.Y + 1 < Taille_Map.HAUTEUR_MAP && carte.Cases[boss.Y + 1, boss.X].EstFranchissable)
                        // si le boss n est pas tout en bas de la map ou coller vers le bas a une texture non franchissable :
                        {
                            boss.positionDesiree.Y += 28; // il descend 
                            boss.VaEnBas = false;
                            break; // Jpense que c'est inutile, mais on sait jamais
                        }
                        // si il est colle a une texture non franchissable :
                        else if (boss.Y - 1 >= 0 && carte.Cases[boss.Y - 1, boss.X].EstFranchissable)
                        {
                            boss.positionDesiree.Y -= 28; // il monte
                            boss.VaEnHaut = false;
                            break;
                        }
                    }
                    else if (_shuriken.Direction == Vector2.UnitY && boss.Regarde_Haut && Math.Abs(boss.Y - _shuriken.Y) < 4 && boss.X == _shuriken.X)
                    // si le shuriken va en bas, le shuriken est a moins de 7 cases du boss et que le shuriken
                    // et le boss sont sur la meme position en X alors :
                    {
                        if (boss.X + 1 < Taille_Map.LARGEUR_MAP && carte.Cases[boss.Y, boss.X + 1].EstFranchissable)
                        {
                            boss.positionDesiree.X += 28; // il va a droite
                            boss.VaADroite = false;
                            break;
                        }
                        // si il est colle a une texture non franchissable :
                        else if (boss.X - 1 >= 0 && carte.Cases[boss.Y, boss.X - 1].EstFranchissable)
                        {
                            boss.positionDesiree.X -= 28; // il va a gauche
                            boss.VaAGauche = false;
                            break;
                        }
                    }
                    else if (_shuriken.Direction == -Vector2.UnitX && boss.Regarde_Droite && Math.Abs(boss.X - _shuriken.X) < 4 && boss.Y == _shuriken.Y)
                    // si le shuriken va a droite , le shuriken est a moins de 7 cases du boss et que le shuriken
                    // et le boss sont sur la meme position en Y alors :
                    {
                        if (boss.Y + 1 < Taille_Map.HAUTEUR_MAP && carte.Cases[boss.Y + 1, boss.X].EstFranchissable)
                        // si le boss n est pas tout en bas de la map ou coller vers le bas a une texture non franchissable :
                        {
                            boss.positionDesiree.Y -= 28; // il descend 
                            boss.VaEnHaut = false;
                            break; // Jpense que c'est inutile, mais on sait jamais
                        }
                        // si il est colle a une texture non franchissable :
                        else if (boss.Y - 1 >= 0 && carte.Cases[boss.Y - 1, boss.X].EstFranchissable)
                        {
                            boss.positionDesiree.Y += 28; // il monte
                            boss.VaEnBas = false;
                            break;
                        }
                    }
                    else if (_shuriken.Direction == -Vector2.UnitY && boss.Regarde_Bas && Math.Abs(boss.Y - _shuriken.Y) < 4 && boss.X == _shuriken.X)
                    // si le shuriken va en bas, le shuriken est a moins de 7 cases du boss et que le shuriken
                    // et le boss sont sur la meme position en X alors :
                    {
                        if (boss.X + 1 < Taille_Map.LARGEUR_MAP && carte.Cases[boss.Y, boss.X + 1].EstFranchissable)
                        {
                            boss.positionDesiree.X -= 28; // il va a droite
                            boss.VaAGauche = false;
                            break;
                        }
                        // si il est colle a une texture non franchissable :
                        else if (boss.X - 1 >= 0 && carte.Cases[boss.Y, boss.X - 1].EstFranchissable)
                        {
                            boss.positionDesiree.X += 28; // il va a gauche
                            boss.VaADroite = false;
                            break;
                        }
                    }
                }
            }
        }
    }
}
