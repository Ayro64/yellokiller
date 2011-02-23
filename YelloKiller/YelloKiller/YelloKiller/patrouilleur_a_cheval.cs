﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace YelloKiller
{
    class patrouilleur_a_cheval : Ennemi
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
        float autochemin;

        public patrouilleur_a_cheval(Vector2 position, Rectangle? sourceRectangle, TypeCase type)
            : base(position, sourceRectangle, type)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            rectangle = new Rectangle((int)position.X, (int)position.Y, 23, 32);
            vitesse_animation = 0.008f;
            vitesse_sprite = 4;
            index = 0;
            maxIndex = 0;
            autochemin = 0;
            positionDesiree = position;
            monter = descendre = droite = gauche = true;
            chemin = new List<Case>();
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            texture = content.Load<Texture2D>("Patrouilleur_a_cheval");
            this.maxIndex = maxIndex;
        }

        public void UpdateInSolo(GameTime gameTime, Carte carte, Hero hero, Rectangle camera)
        {
            Position = position;
            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;
            Rectangle = rectangle;

            autochemin += gameTime.ElapsedGameTime.Milliseconds * 0.001f;

            if (sourceRectangle.Value.Y == 0)
                sourceRectangle = new Rectangle(24, 0, 23, 30);
            if (sourceRectangle.Value.Y == 34)
                sourceRectangle = new Rectangle(24, 34, 23, 30);
            if (sourceRectangle.Value.Y == 65)
                sourceRectangle = new Rectangle(24, 65, 23, 30);
            if (sourceRectangle.Value.Y == 98)
                sourceRectangle = new Rectangle(24, 98, 23, 30);

            if (!monter)
            {
                if (position != positionDesiree)
                {
                    position.Y -= vitesse_sprite;
                    sourceRectangle = new Rectangle((int)index * 24, 0, 23, 30);
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
                    sourceRectangle = new Rectangle((int)index * 24, 65, 23, 30);
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
                    sourceRectangle = new Rectangle((int)index * 24, 98, 23, 30);
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
                    sourceRectangle = new Rectangle((int)index * 24, 34, 23, 30);
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
            }


        }

        public void UpdateInCoop(GameTime gameTime, Carte carte, Hero1 hero1, Hero2 hero2/*, GameplayScreenCoop yk, Hero1 hero1, Hero2 hero2*/)
        {
            Position = position;
            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;
            Rectangle = rectangle;

            autochemin += gameTime.ElapsedGameTime.Milliseconds * 0.001f;

            if (sourceRectangle.Value.Y == 1)
                sourceRectangle = new Rectangle(5, 0, 23, 30);
            if (sourceRectangle.Value.Y == 33)
                sourceRectangle = new Rectangle(5, 34, 23, 30);
            if (sourceRectangle.Value.Y == 65)
                sourceRectangle = new Rectangle(5, 65, 23, 30);
            if (sourceRectangle.Value.Y == 98)
                sourceRectangle = new Rectangle(5, 98, 23, 30);

            if (!monter)
            {
                if (position != positionDesiree)
                {
                    position.Y -= vitesse_sprite;
                    sourceRectangle = new Rectangle((int)index * 5, 34, 23, 30);
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
                    sourceRectangle = new Rectangle((int)index * 5, 65, 23, 30);
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
                    sourceRectangle = new Rectangle((int)index * 5, 98, 23, 30);
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
                    sourceRectangle = new Rectangle((int)index * 5, 0, 23, 30);
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
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle camera)
        {
            spriteBatch.Draw(texture, new Vector2(position.X - camera.X, position.Y - camera.Y), sourceRectangle, Color.White);
        }
    }
}