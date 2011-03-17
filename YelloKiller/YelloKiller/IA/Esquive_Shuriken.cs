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
                    if (_shuriken.Direction == Vector2.UnitX && (int)(boss.position.X / 28) - (int)(_shuriken.position.X) / 28 < 7
                        && (int)(boss.position.Y) / 28 == (int)(_shuriken.position.Y) / 28)
                    {
                        if (boss.position.Y < 28 * (Taille_Map.HAUTEUR_MAP - 1) &&
                            (int)carte.Cases[(int)(boss.position.Y) / 28 + 1, (int)(boss.position.X) / 28].Type > 0)
                        {
                            boss.positionDesiree.Y += 28;
                            boss.Descendre = false;
                            break; // Jpense que c'est inutile, mais on sait jamais
                        }
                        else if (boss.position.Y > 0 && (int)carte.Cases[(int)(boss.position.Y) / 28 - 1, (int)(boss.position.X) / 28].Type > 0)
                        {
                            boss.positionDesiree.Y -= 28;
                            boss.Monter = false;
                            break;
                        }
                    }

                    if (_shuriken.Direction == Vector2.UnitY && (int)(boss.position.Y / 28) - (int)(_shuriken.position.Y) / 28 < 7
                        && (int)(boss.position.X) / 28 == (int)(_shuriken.position.X) / 28)
                    {
                        if (boss.position.X < 28 * Taille_Map.LARGEUR_MAP - 1 && (int)carte.Cases[(int)(boss.position.Y) / 28, (int)(boss.position.X) / 28 + 1].Type > 0)
                        {
                            boss.positionDesiree.X += 28;
                            boss.Droite = false;
                            break;
                        }
                        else if (boss.position.X > 0 && (int)carte.Cases[(int)(boss.position.Y) / 28, (int)(boss.position.X) / 28 - 1].Type > 0)
                        {
                            boss.positionDesiree.X -= 28;
                            boss.Gauche = false;
                            break;
                        }
                    }

                    if (_shuriken.Direction == -Vector2.UnitX && (int)(boss.position.X / 28) - (int)(_shuriken.position.X) / 28 < 7
                        && (int)(boss.position.Y) / 28 == (int)(_shuriken.position.Y) / 28)
                    {
                        if (boss.position.Y < 28 * (Taille_Map.HAUTEUR_MAP - 1) &&
                            (int)carte.Cases[(int)(boss.position.Y) / 28 + 1, (int)(boss.position.X) / 28].Type > 0)
                        {
                            boss.positionDesiree.Y += 28;
                            boss.Monter = false;
                            break;
                        }
                        else if (boss.position.Y > 0 && (int)carte.Cases[(int)(boss.position.Y) / 28 - 1, (int)(boss.position.X) / 28].Type > 0)
                        {
                            boss.positionDesiree.Y -= 28;
                            boss.Descendre = false;
                            break;
                        }
                    }

                    if (_shuriken.Direction == -Vector2.UnitY && (int)(boss.position.Y / 28) - (int)(_shuriken.position.Y) / 28 < 7
                        && (int)(boss.position.X) / 28 == (int)(_shuriken.position.X) / 28)
                    {
                        if (boss.position.X < 28 * Taille_Map.LARGEUR_MAP - 1 && (int)carte.Cases[(int)(boss.position.Y) / 28, (int)(boss.position.X) / 28 + 1].Type > 0)
                        {
                            boss.positionDesiree.X += 28;
                            boss.Gauche = false;
                            break;
                        }
                        else if (boss.position.X > 0 && (int)carte.Cases[(int)(boss.position.Y) / 28, (int)(boss.position.X) / 28 - 1].Type > 0)
                        {
                            boss.positionDesiree.X -= 28;
                            boss.Droite = false;
                            break;
                        }
                    }
                }
            }
        }
    }
}
