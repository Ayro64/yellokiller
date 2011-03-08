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

            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.Enter))
            {
                Console.WriteLine("Camera.X = " + (int)camera.X / 28 + " ; Camera.Y = " + (int)camera.Y / 28);
                chemin = Pathfinding.CalculChemin(carte, carte.Cases[25, 18], carte.Cases[42, 21]);

                if (chemin == null)
                    Console.WriteLine("CHEMIN NULL");

                Console.WriteLine("Depart : X = " + carte.Cases[25, 18].X + " ; Y = " + carte.Cases[25, 18].Y + " _ Arrivee : X = " + carte.Cases[42, 21].X + " ; Y = " + carte.Cases[42, 21].Y);
                if (chemin != null)
                    foreach (Case sisi in chemin)
                        Console.WriteLine("X = " + (int)sisi.Position.X / 28 + " ; Y = " + (int)sisi.Position.Y / 28);
            }
            
            if (chemin != null && chemin.Count != 0)
            {
                if (Monter && Descendre && Droite && Gauche)
                {
                    Console.WriteLine("Position : X = " + (int)Position.X / 28 + " ; Y = " + (int)Position.Y / 28 + " _ Chemin : X = " + (int)chemin[chemin.Count - 1].Position.X / 28 + " ; Y = " + (int)chemin[chemin.Count - 1].Position.Y / 28);

                    if ((int)chemin[chemin.Count - 1].Position.X / 28 < (int)Position.X / 28)
                    {
                        positionDesiree.X -= 28;
                        Gauche = false;
                        chemin.RemoveAt(chemin.Count - 1);
                    }
                    else if ((int)chemin[chemin.Count - 1].Position.X / 28 > (int)Position.X / 28)
                    {
                        positionDesiree.X += 28;
                        Droite = false;
                        chemin.RemoveAt(chemin.Count - 1);
                    }
                    else if ((int)chemin[chemin.Count - 1].Position.Y / 28 < (int)Position.Y / 28)
                    {
                        positionDesiree.Y -= 28;
                        Monter = false;
                        chemin.RemoveAt(chemin.Count - 1);
                    }
                    else if ((int)chemin[chemin.Count - 1].Position.Y / 28 > (int)Position.Y / 28)
                    {
                        positionDesiree.Y += 28;
                        Descendre = false;
                        chemin.RemoveAt(chemin.Count - 1);
                    }
                }
            }
        }
    }
}