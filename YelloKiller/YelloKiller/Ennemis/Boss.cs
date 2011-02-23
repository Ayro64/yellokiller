﻿using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using YelloKiller.YelloKiller;

namespace YelloKiller
{
    class Boss : Ennemi
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

        public Boss(Vector2 position, Rectangle? sourceRectangle, TypeCase type)
            : base(position, sourceRectangle, type)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            rectangle = new Rectangle((int)position.X, (int)position.Y, 18, 26);
            vitesse_animation = 0.008f;
            vitesse_sprite = 1;
            index = 0;
            maxIndex = 0;
            positionDesiree = position;
            monter = descendre = droite = gauche = true;
            chemin = new List<Case>();
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            texture = content.Load<Texture2D>("Boss");
            this.maxIndex = maxIndex;
        }

        public void UpdateInSolo(GameTime gameTime, Carte carte, Hero hero, Rectangle camera)
        {
            Position = position;
            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;
            Rectangle = rectangle;

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

            if (sourceRectangle.Value.Y == 0)
                sourceRectangle = new Rectangle(26, 0, 18, 26);
            if (sourceRectangle.Value.Y == 34)
                sourceRectangle = new Rectangle(26, 34, 18, 26);
            if (sourceRectangle.Value.Y == 64)
                sourceRectangle = new Rectangle(26, 64, 18, 26);
            if (sourceRectangle.Value.Y == 98)
                sourceRectangle = new Rectangle(26, 98, 18, 26);

            if (!monter)
            {
                if (position != positionDesiree)
                {
                    position.Y -= vitesse_sprite;
                    sourceRectangle = new Rectangle((int)index * 26, 0, 18, 26);
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
                    sourceRectangle = new Rectangle((int)index * 26, 64, 18, 26);
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
                    sourceRectangle = new Rectangle((int)index * 26, 98, 18, 26);
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
                    sourceRectangle = new Rectangle((int)index * 26, 34, 18, 26);
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
        }

        public void UpdateInCoop(GameTime gameTime, Carte carte, Hero1 hero1, Hero2 hero2)
        {
            Position = position;
            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;
            Rectangle = rectangle;

            if (sourceRectangle.Value.Y == 0)
                sourceRectangle = new Rectangle(26, 0, 18, 26);
            if (sourceRectangle.Value.Y == 34)
                sourceRectangle = new Rectangle(26, 34, 18, 26);
            if (sourceRectangle.Value.Y == 64)
                sourceRectangle = new Rectangle(26, 64, 18, 26);
            if (sourceRectangle.Value.Y == 97)
                sourceRectangle = new Rectangle(26, 98, 18, 26);
            if (!monter)
            {
                if (position != positionDesiree)
                {
                    position.Y -= vitesse_sprite;
                    sourceRectangle = new Rectangle((int)index * 26, 0, 18, 26);
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
                    sourceRectangle = new Rectangle((int)index * 26, 64, 18, 26);
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
                    sourceRectangle = new Rectangle((int)index * 26, 98, 18, 26);
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
                    sourceRectangle = new Rectangle((int)index * 26, 34, 18, 26);
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
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle camera)
        {
            spriteBatch.Draw(texture, new Vector2(position.X - camera.X, position.Y - camera.Y), sourceRectangle, Color.White);
        }
    }
}