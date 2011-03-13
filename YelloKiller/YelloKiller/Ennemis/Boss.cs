using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Input;

namespace YelloKiller
{
    class Boss : Ennemi
    {
        int vie;
        List<Case> chemin;
        Case depart;
        Case arrivee;


        public Boss(Vector2 position)
            : base(position)
        {
            Position = position;
            SourceRectangle = new Rectangle(26, 64, 18, 26);
            Rectangle = new Rectangle((int)position.X + 1, (int)position.Y + 1, 18, 26);
            MaxIndex = 0;
            positionDesiree = position;
            vie = 5;
            chemin = new List<Case>();

        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            base.LoadContent(content, "Boss");
            MaxIndex = maxIndex;
        }

        public int Vie
        {
            get { return vie; }
            set { vie = value; }
        }

        public void Update(GameTime gameTime, List<Shuriken> shuriken, Carte carte, Hero hero, Rectangle camera)
        {
            base.Update(gameTime, new Rectangle((int)Index * 24, 0, 16, 24), new Rectangle((int)Index * 24, 64, 16, 24), new Rectangle((int)Index * 24, 97, 16, 24), new Rectangle((int)Index * 24, 33, 16, 24));

            if (Math.Abs((int)(hero.position.X / 28) - (int)(position.X / 28)) < 4 && Math.Abs((int)(hero.position.Y / 28) - (int)(position.Y / 28)) < 4)
            {                
                depart = carte.Cases[((int)position.Y) / 28 /*- (camera.X / 28)*/, ((int)position.X) / 28 /*- (camera.Y / 28)*/];
                arrivee = carte.Cases[((int)hero.position.Y) / 28 /*- (camera.X / 28)*/, ((int)hero.position.X) / 28 /*- (camera.Y / 28)*/];
                chemin = Pathfinding.CalculChemin(carte, depart, arrivee);               
            }

            if (chemin != null && chemin.Count != 0)
            {
                if (Monter && Descendre && Droite && Gauche)
                {
                    Console.WriteLine("Position : X = " + (int)Position.X / 28 + " ; Y = " + (int)Position.Y / 28 + " _ Chemin : X = " + (int)chemin[chemin.Count - 1].Position.X / 28 + " ; Y = " + (int)chemin[chemin.Count - 1].Position.Y / 28);

                    if ((int)chemin[chemin.Count - 1].X < (int)Position.X / 28)
                    {
                        Console.WriteLine("Je vais a gauche");
                        positionDesiree.X -= 28;
                        Gauche = false;
                        chemin.RemoveAt(chemin.Count - 1);
                    }
                    else if ((int)chemin[chemin.Count - 1].X > (int)Position.X / 28)
                    {
                        Console.WriteLine("Je vais a droite");
                        positionDesiree.X += 28;
                        Droite = false;
                        chemin.RemoveAt(chemin.Count - 1);
                    }
                    else if ((int)chemin[chemin.Count - 1].Y < (int)Position.Y / 28)
                    {
                        Console.WriteLine("Je vais en haut");
                        positionDesiree.Y -= 28;
                        Monter = false;
                        chemin.RemoveAt(chemin.Count - 1);
                    }
                    else if ((int)chemin[chemin.Count - 1].Y > (int)Position.Y / 28)
                    {
                        Console.WriteLine("Je vais en bas");
                        positionDesiree.Y += 28;
                        Descendre = false;
                        chemin.RemoveAt(chemin.Count - 1);
                    }
                }
            }

            foreach (Shuriken _shuriken in shuriken)
            {
                //Console.WriteLine("_shuriken = " + (int)(_shuriken.position.Y / 28) + " , boss = " + (int)(this.position.Y / 28));

                if (Monter && Descendre && Droite && Gauche)
                {
                    if (_shuriken.Direction == Vector2.UnitX && (int)(position.X / 28) - (int)(_shuriken.position.X) / 28 < 7
                        && (int)(position.Y) / 28 == (int)(_shuriken.position.Y) / 28)
                    {
                        if (position.Y < 28 * (Taille_Map.HAUTEUR_MAP - 1) &&
                            (int)carte.Cases[(int)(position.Y) / 28 + 1, (int)(position.X) / 28].Type > 0)
                        {
                            Console.WriteLine("Bas !");
                            positionDesiree.Y += 28;
                            Descendre = false;
                            break; // Jpense que c'est inutile, mais on sait jamais
                        }
                        else if (position.Y > 0 && (int)carte.Cases[(int)(position.Y) / 28 - 1, (int)(position.X) / 28].Type > 0)
                        {
                            Console.WriteLine("Haut !");
                            positionDesiree.Y -= 28;
                            Monter = false;
                            break;
                        }
                    }

                    if (_shuriken.Direction == Vector2.UnitY && (int)(position.Y / 28) - (int)(_shuriken.position.Y) / 28 < 7
                        && (int)(position.X) / 28 == (int)(_shuriken.position.X) / 28)
                    {
                        if (position.X < 28 * Taille_Map.LARGEUR_MAP - 1 && (int)carte.Cases[(int)(position.Y) / 28, (int)(position.X) / 28 + 1].Type > 0)
                        {
                            Console.WriteLine("Droite !");
                            positionDesiree.X += 28;
                            Droite = false;
                            break;
                        }
                        else if (position.X > 0 && (int)carte.Cases[(int)(position.Y) / 28, (int)(position.X) / 28 - 1].Type > 0)
                        {
                            Console.WriteLine("Gauche !");
                            positionDesiree.X -= 28;
                            Gauche = false;
                            break;
                        }
                    }
                }
            }
        }
    }
}