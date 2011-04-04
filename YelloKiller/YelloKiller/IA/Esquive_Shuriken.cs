using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace YelloKiller.IA
{
    static class Esquive_Shuriken
    {
        public static void Boss_Esquive_Shuriken(Hero hero, Boss boss, List<Shuriken> shuriken, Carte carte, Rectangle camera)
        {
            foreach (Shuriken _shuriken in shuriken)
            {


                if (boss.Monter && boss.Descendre && boss.Droite && boss.Gauche)
                {
                    if (_shuriken.Direction == Vector2.UnitX && boss.Regarder_Gauche && Math.Abs(boss.X - (int)(_shuriken.position.X) / 28) < 4
                        && boss.Y == (int)(_shuriken.position.Y) / 28)
                    // si le shuriken va a droite , le shuriken est a moins de 7 cases du boss et que le shuriken
                    // et le boss sont sur la meme position en Y alors :
                    {
                        if (boss.position.Y < 28 * (Taille_Map.HAUTEUR_MAP - 1) &&
                            (int)carte.Cases[boss.Y + 1, boss.X].Type > 0)
                        // si le boss n est pas tout en bas de la map ou coller vers le bas a une texture non franchissable :
                        {
                            boss.positionDesiree.Y += 28; // il descend 
                            boss.Descendre = false;
                            break; // Jpense que c'est inutile, mais on sait jamais
                        }
                        // si il est colle a une texture non franchissable :
                        else if (boss.position.Y > 0 && (int)carte.Cases[boss.Y - 1, boss.X].Type > 0)
                        {
                            boss.positionDesiree.Y -= 28; // il monte
                            boss.Monter = false;
                            break;
                        }
                    }
                    else if (_shuriken.Direction == Vector2.UnitY && boss.Regarder_Haut && Math.Abs(boss.Y - (int)(_shuriken.position.Y) / 28) < 4
                        && boss.X == (int)(_shuriken.position.X) / 28)
                    // si le shuriken va en bas, le shuriken est a moins de 7 cases du boss et que le shuriken
                    // et le boss sont sur la meme position en X alors :
                    {
                        if (boss.position.X < 28 * (Taille_Map.LARGEUR_MAP - 1) && (int)carte.Cases[boss.Y, boss.X + 1].Type > 0)
                        {
                            boss.positionDesiree.X += 28; // il va a droite
                            boss.Droite = false;
                            break;
                        }
                        // si il est colle a une texture non franchissable :
                        else if (boss.position.X > 0 && (int)carte.Cases[boss.Y, boss.X - 1].Type > 0)
                        {
                            boss.positionDesiree.X -= 28; // il va a gauche
                            boss.Gauche = false;
                            break;
                        }
                    }
                    else if (_shuriken.Direction == -Vector2.UnitX && boss.Regarder_Droite && Math.Abs(boss.X - (int)(_shuriken.position.X) / 28) < 4
                          && boss.Y == (int)(_shuriken.position.Y) / 28)
                    // si le shuriken va a droite , le shuriken est a moins de 7 cases du boss et que le shuriken
                    // et le boss sont sur la meme position en Y alors :
                    {
                        if (boss.position.Y > 0 &&
                            (int)carte.Cases[boss.X + 1, boss.X].Type > 0)
                        // si le boss n est pas tout en bas de la map ou coller vers le bas a une texture non franchissable :
                        {
                            boss.positionDesiree.Y -= 28; // il descend 
                            boss.Monter = false;
                            break; // Jpense que c'est inutile, mais on sait jamais
                        }
                        // si il est colle a une texture non franchissable :
                        else if (boss.position.Y < 28 * (Taille_Map.HAUTEUR_MAP - 1) && (int)carte.Cases[boss.Y - 1, boss.X].Type > 0)
                        {
                            boss.positionDesiree.Y += 28; // il monte
                            boss.Descendre = false;
                            break;
                        }
                    }
                    else if (_shuriken.Direction == -Vector2.UnitY && boss.Regarder_Bas && Math.Abs(boss.Y - (int)(_shuriken.position.Y) / 28) < 4
                  && boss.X == (int)(_shuriken.position.X) / 28)
                    // si le shuriken va en bas, le shuriken est a moins de 7 cases du boss et que le shuriken
                    // et le boss sont sur la meme position en X alors :
                    {
                        if (boss.position.X > 0 && (int)carte.Cases[boss.Y, boss.X + 1].Type > 0)
                        {
                            boss.positionDesiree.X -= 28; // il va a droite
                            boss.Gauche = false;
                            break;
                        }
                        // si il est colle a une texture non franchissable :
                        else if (boss.position.X < 28 * (Taille_Map.LARGEUR_MAP - 1) && (int)carte.Cases[boss.Y, boss.X - 1].Type > 0)
                        {
                            boss.positionDesiree.X += 28; // il va a gauche
                            boss.Droite = false;
                            break;
                        }
                    }
                }
            }
        }
    }
}
