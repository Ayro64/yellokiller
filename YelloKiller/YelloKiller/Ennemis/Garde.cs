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
            if (ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.Enter))
            {
                chemin = Pathfinding.CalculChemin(carte, new Vector2((int)Position.X / 28 + (int)camera.X / 28, (int)Position.Y / 28 + (int)camera.Y / 28), new Vector2((int)hero.Position.X / 28 + (int)camera.X / 28, (int)hero.Position.Y / 28 + (int)camera.Y / 28), camera);

                Console.WriteLine("Depart : X = " + (int)Position.X / 28 + " ; Y = " + (int)Position.Y / 28 + " _ Arrivee : X = " + (int)hero.Position.X / 28 + " ; Y = " + (int)hero.Position.Y / 28);
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

            if (SourceRectangle.Value.Y == 0)
                SourceRectangle = new Rectangle(24, 0, 16, 24);
            if (SourceRectangle.Value.Y == 33)
                SourceRectangle = new Rectangle(24, 33, 16, 24);
            if (SourceRectangle.Value.Y == 64)
                SourceRectangle = new Rectangle(24, 64, 16, 24);
            if (SourceRectangle.Value.Y == 97)
                SourceRectangle = new Rectangle(24, 97, 16, 24);
        }
    }
}