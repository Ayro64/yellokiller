using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    class Garde : Ennemi
    {
        List<Case> chemin;
        Case depart;
        Case arrivee;

        public Garde(Vector2 position)
            : base(position)
        {
            Position = position;
            SourceRectangle = new Rectangle(24, 64, 16, 24);
            Rectangle = new Rectangle((int)position.X + 1, (int)position.Y + 1, 16, 24);
            positionDesiree = position;
            chemin = new List<Case>();
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            base.LoadContent(content, "Garde");
            MaxIndex = maxIndex;
        }

        public void Update(GameTime gameTime, Carte carte, Hero hero, Rectangle camera)
        {
            base.Update(gameTime, new Rectangle((int)Index * 24, 0, 16, 24), new Rectangle((int)Index * 24, 64, 16, 24), new Rectangle((int)Index * 24, 97, 16, 24), new Rectangle((int)Index * 24, 33, 16, 24));

          //  if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.Enter))
            if (Math.Abs((int)(hero.position.X / 28) - (int)(position.X / 28)) < 4 && Math.Abs((int)(hero.position.Y / 28) - (int)(position.Y / 28)) < 4
                || ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.Enter))
            {
                Console.WriteLine("Camera : X = " + camera.X / 28 + " Y = " + camera.Y / 28);
                Console.WriteLine("Origines\nGarde : X = " + (int)position.X / 28 + " Y = " + (int)position.Y / 28 + " Hero : X = " + (int)hero.position.X / 28 + " Y = " + (int)hero.position.Y / 28);
                depart = carte.Cases[((int)position.Y) / 28 /*- (camera.X / 28)*/, ((int)position.X) / 28 /*- (camera.Y / 28)*/];
                arrivee = carte.Cases[((int)hero.position.Y) / 28 /*- (camera.X / 28)*/, ((int)hero.position.X) / 28 /*- (camera.Y / 28)*/];
                chemin = Pathfinding.CalculChemin(carte, depart, arrivee);

                if (chemin == null)
                    Console.WriteLine("CHEMIN NULL");

                Console.WriteLine("Depart : X = " + depart.X + " ; Y = " + depart.Y + " _ Arrivee : X = " + arrivee.X + " ; Y = " + arrivee.Y);
                Console.WriteLine("Debut pathfinding");
                if (chemin != null)
                    foreach (Case sisi in chemin)
                        Console.WriteLine("X = " + (int)sisi.X + " ; Y = " + (int)sisi.Y);

                Console.WriteLine("Fin pathfinding");
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
        }
    }
}