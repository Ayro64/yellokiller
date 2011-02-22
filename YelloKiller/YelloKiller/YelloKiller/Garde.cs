using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using YelloKiller.YelloKiller;

namespace YelloKiller.YelloKiller
{
    class Garde : Ennemi
    {
        Vector2 position, positionDesiree;
        float vitesse_animation;
        int vitesse_sprite;
        float index;
        int maxIndex;
        Rectangle? sourceRectangle;
        public Rectangle rectangle;
        Texture2D texture;
        bool monter, descendre, droite, gauche;
        List<Case> chemin;
        //float autochemin;

        public Garde(Vector2 position, Rectangle? sourceRectangle, TypeCase type)
            : base(position, sourceRectangle, type)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            rectangle = new Rectangle((int)position.X, (int)position.Y, 16, 28);
            vitesse_animation = 0.008f;
            vitesse_sprite = 1;
            index = 0;
            maxIndex = 0;
            //autochemin = 0;
            positionDesiree = position;
            monter = descendre = droite = gauche = true;
            chemin = new List<Case>();
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            texture = content.Load<Texture2D>("Garde");
            this.maxIndex = maxIndex;
        }

        public void UpdateInSolo(GameTime gameTime, Carte carte, Hero hero, Rectangle camera)
        {
            Position = position;

            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;

            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.Enter))
            {
                chemin = Pathfinding.CalculChemin(carte, carte.Cases[(int)position.Y / 28, (int)position.X / 28], carte.Cases[(int)hero.Position.Y / 28, (int)hero.Position.X / 28], camera);
                Console.WriteLine("Depart : X = " + (int)position.X / 28 + " ; Y = " + (int)position.Y / 28 + " _ Arrivee : X = " + (int)hero.Position.X / 28 + " ; Y = " + (int)hero.Position.Y / 28);
                if (chemin != null)
                    foreach (Case sisi in chemin)
                        Console.WriteLine("X = " + (int)sisi.Position.X / 28 + " ; Y = " + (int)sisi.Position.Y / 28);
            }
            
            if (chemin != null && chemin.Count != 0)
            {
                if (monter && descendre && droite && gauche)
                {
                    Console.WriteLine("Position : X = " + (int)position.X / 28 + " ; Y = " + (int)position.Y / 28 + " _ Chemin : X = " + (int)chemin[chemin.Count - 1].Position.X / 28 + " ; Y = " + (int)chemin[chemin.Count - 1].Position.Y / 28);

                    if ((int)chemin[chemin.Count - 1].Position.X / 28 < (int)position.X / 28)
                    {
                        positionDesiree.X = position.X - 28;
                        gauche = false;
                        chemin.RemoveAt(chemin.Count - 1);
                        Console.WriteLine("Je vais a gauche.");
                    }
                    else if ((int)chemin[chemin.Count - 1].Position.X / 28 > (int)position.X / 28)
                    {
                        positionDesiree.X = position.X + 28;
                        droite = false;
                        chemin.RemoveAt(chemin.Count - 1);
                        Console.WriteLine("Je vais a droite.");
                    }
                    else if ((int)chemin[chemin.Count - 1].Position.Y / 28 < (int)position.Y / 28)
                    {
                        positionDesiree.Y = position.Y - 28;
                        monter = false;
                        chemin.RemoveAt(chemin.Count - 1);
                        Console.WriteLine("Je vais en haut.");
                    }
                    else if ((int)chemin[chemin.Count - 1].Position.Y / 28 > (int)position.Y / 28)
                    {
                        positionDesiree.Y = position.Y + 28;
                        descendre = false;
                        chemin.RemoveAt(chemin.Count - 1);
                        Console.WriteLine("Je vais en bas.");
                    }
                }
            }

            //autochemin += gameTime.ElapsedGameTime.Milliseconds * 0.001f;

            if (sourceRectangle.Value.Y == 1)
                sourceRectangle = new Rectangle(5, 1, 16, 23);
            if (sourceRectangle.Value.Y == 33)
                sourceRectangle = new Rectangle(5, 33, 16, 23);
            if (sourceRectangle.Value.Y == 65)
                sourceRectangle = new Rectangle(5, 65, 16, 23);
            if (sourceRectangle.Value.Y == 98)
                sourceRectangle = new Rectangle(5, 98, 16, 23);

            if (!monter)
            {
                if (position != positionDesiree)
                {
                    position.Y -= vitesse_sprite;
                    sourceRectangle = new Rectangle((int)index * 5, 33, 16, 23);
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;

                    if (index >= maxIndex)
                        index = 0f;
                }

                else
                {
                    monter = true;
                    position = positionDesiree;
                    index = 0f;
                }
            }

            if (!descendre)
            {
                if (position != positionDesiree)
                {
                    position.Y += vitesse_sprite;
                    sourceRectangle = new Rectangle((int)index * 5, 65, 16, 23);
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;

                    if (index >= maxIndex)
                        index = 0f;
                }
                else
                {
                    descendre = true;
                    position = positionDesiree;
                    index = 0f;
                }
            }

            if (!gauche)
            {
                if (position != positionDesiree)
                {
                    position.X -= vitesse_sprite;
                    sourceRectangle = new Rectangle((int)index * 5, 98, 16, 23);
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;

                    if (index >= maxIndex)
                        index = 0f;
                }

                else
                {
                    gauche = true;
                    position = positionDesiree;
                    index = 0f;
                }
            }

            if (!droite)
            {
                if (position != positionDesiree)
                {
                    position.X += vitesse_sprite;
                    sourceRectangle = new Rectangle((int)index * 5, 1, 16, 23);
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;

                    if (index >= maxIndex)
                        index = 0f;
                }

                else
                {
                    droite = true;
                    position = positionDesiree;
                    index = 0f;
                }
            }
            /*if (monter && descendre && droite && gauche)
            {
                if (autochemin < 5 && position.X < 28 * Taille_Map.LARGEUR_MAP - 18 &&
                    (int)carte.Cases[(int)(position.Y) / 28, (int)(position.X + 28) / 28].Type > 0)
                {
                    positionDesiree.X = position.X + 28;
                    positionDesiree.Y = position.Y;
                    droite = false;
                }
                else if (autochemin < 10 && position.Y < 28 * (Taille_Map.HAUTEUR_MAP - 1) &&
                         (int)carte.Cases[(int)(position.Y + 28) / 28, (int)(position.X) / 28].Type > 0)
                {
                    positionDesiree.Y = position.Y + 28;
                    positionDesiree.X = position.X;
                    descendre = false;
                }
                else if (autochemin < 15 && position.X > 0 &&
                         (int)carte.Cases[(int)(position.Y) / 28, (int)(position.X - 28) / 28].Type > 0)
                {
                    positionDesiree.X = position.X - 28;
                    positionDesiree.Y = position.Y;
                    gauche = false;
                }
                else if (autochemin < 20 && position.Y > 0 &&
                         (int)carte.Cases[(int)(position.Y - 28) / 28, (int)(position.X) / 28].Type > 0)
                {
                    positionDesiree.Y = position.Y - 28;
                    positionDesiree.X = position.X;
                    monter = false;
                }
                else
                {
                    autochemin = 0;
                }
            }*/
        }

        public void UpdateInCoop(GameTime gameTime, Carte carte, Hero1 hero1, Hero2 hero2)
        {
            Position = position;

            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;

            if (sourceRectangle.Value.Y == 1)
                sourceRectangle = new Rectangle(5, 1, 16, 23);
            if (sourceRectangle.Value.Y == 33)
                sourceRectangle = new Rectangle(5, 33, 16, 23);
            if (sourceRectangle.Value.Y == 65)
                sourceRectangle = new Rectangle(5, 65, 16, 23);
            if (sourceRectangle.Value.Y == 98)
                sourceRectangle = new Rectangle(5, 98, 16, 23);

            if (!monter)
            {
                if (position != positionDesiree)
                {
                    position.Y -= vitesse_sprite;
                    sourceRectangle = new Rectangle((int)index * 5, 33, 16, 23);
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;

                    if (index >= maxIndex)
                        index = 0f;
                }

                else
                {
                    monter = true;
                    position = positionDesiree;
                    index = 0f;
                }
            }

            if (!descendre)
            {
                if (position != positionDesiree)
                {
                    position.Y += vitesse_sprite;
                    sourceRectangle = new Rectangle((int)index * 5, 65, 16, 23);
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;

                    if (index >= maxIndex)
                        index = 0f;
                }
                else
                {
                    descendre = true;
                    position = positionDesiree;
                    index = 0f;
                }
            }

            if (!gauche)
            {
                if (position != positionDesiree)
                {
                    position.X -= vitesse_sprite;
                    sourceRectangle = new Rectangle((int)index * 5, 98, 16, 23);
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;

                    if (index >= maxIndex)
                        index = 0f;
                }

                else
                {
                    gauche = true;
                    position = positionDesiree;
                    index = 0f;
                }
            }

            if (!droite)
            {
                if (position != positionDesiree)
                {
                    position.X += vitesse_sprite;
                    sourceRectangle = new Rectangle((int)index * 5, 1, 16, 23);
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;

                    if (index >= maxIndex)
                        index = 0f;
                }

                else
                {
                    droite = true;
                    position = positionDesiree;
                    index = 0f;
                }
            }
            if (monter && descendre && droite && gauche)
            {
                /*if (autochemin < 5 && position.X < 28 * Taille_Map.LARGEUR_MAP - 18 &&
                    (int)carte.Cases[(int)(position.Y) / 28, (int)(position.X + 28) / 28].Type > 0)
                {
                    positionDesiree.X = position.X + 28;
                    positionDesiree.Y = position.Y;
                    droite = false;
                }
                else if (autochemin < 10 && position.Y < 28 * (Taille_Map.HAUTEUR_MAP - 1) &&
                         (int)carte.Cases[(int)(position.Y + 28) / 28, (int)(position.X) / 28].Type > 0)
                {
                    positionDesiree.Y = position.Y + 28;
                    positionDesiree.X = position.X;
                    descendre = false;
                }
                else if (autochemin < 15 && position.X > 0 &&
                         (int)carte.Cases[(int)(position.Y) / 28, (int)(position.X - 28) / 28].Type > 0)
                {
                    positionDesiree.X = position.X - 28;
                    positionDesiree.Y = position.Y;
                    gauche = false;
                }
                else if (autochemin < 20 && position.Y > 0 &&
                         (int)carte.Cases[(int)(position.Y - 28) / 28, (int)(position.X) / 28].Type > 0)
                {
                    positionDesiree.Y = position.Y - 28;
                    positionDesiree.X = position.X;
                    monter = false;
                }
                else
                {
                    autochemin = 0;
                }*/
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle camera)
        {
            spriteBatch.Draw(texture, new Vector2(position.X - camera.X, position.Y - camera.Y), sourceRectangle, Color.White);
        }
    }
}