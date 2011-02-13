using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Yellokiller.Yello_Killer
{
    class Ennemi : Case
    {
        Vector2 position, positionDesiree;
        float vitesse_animation;
        int vitesse_sprite;
        float index;
        int maxIndex;
        Rectangle? sourceRectangle;
        Rectangle rectangle;
        Texture2D texture;
        bool monter, descendre, droite, gauche;
        List<Case> walkingList;
        float autochemin;
        //int msElapsed;

        public Ennemi(Vector2 position, Rectangle? sourceRectangle, TypeCase type)
            : base(position, sourceRectangle, type)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            rectangle = new Rectangle((int)position.X, (int)position.Y, 16, 28);
            vitesse_animation = 0.008f;
            vitesse_sprite = 1;
            index = 0;
            maxIndex = 0;
            autochemin = 0;
            //msElapsed = 0;
            positionDesiree = position;
            monter = descendre = droite = gauche = true;
            walkingList = new List<Case>();
        }

        public List<Case> WalkingList
        {
            set { if (value != null) walkingList = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
        }

        public Rectangle? SourceRectangle
        {
            get { return sourceRectangle; }
            set { sourceRectangle = value; }
        }

        public Rectangle Rectangle
        {
            get { return rectangle; }
        }

        public Vector2 PositionDesiree
        {
            get { return positionDesiree; }
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            texture = content.Load<Texture2D>("ennemi");
            this.maxIndex = maxIndex;
        }

        public void Update(GameTime gameTime, Carte carte/*, GameplayScreenCoop yk, Hero1 hero1, Hero2 hero2*/)
        {
            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;

            autochemin += gameTime.ElapsedGameTime.Milliseconds * 0.001f;
                     
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

            /*msElapsed += gameTime.ElapsedGameTime.Milliseconds;
            if (walkingList.Count != 0)
            {
                if (msElapsed >= 100)
                {
                    msElapsed = 0;
                    position.X = walkingList[walkingList.Count - 1].Position.X;
                    position.Y = walkingList[walkingList.Count - 1].Position.Y;
                    Position = walkingList[walkingList.Count - 1].Position;
                    walkingList.RemoveAt(walkingList.Count - 1);
                }
            }*/
        }

        /*public void UpdateInSolo(GameTime gameTime, Carte carte, GameplayScreenSolo yk, Hero hero)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, 18, 28);
            autochemin += gameTime.ElapsedGameTime.Milliseconds * 0.001f;

            if (this.position.Y - hero.Position.Y < 5 && this.position.Y - hero.Position.Y > -5)
            {
                autochemin += gameTime.ElapsedGameTime.Milliseconds * 0.001f;
                Console.WriteLine("position = " + (this.position.Y - hero.Position.Y));
            }

            if (sourceRectangle.Value.Y == 1)
                sourceRectangle = new Rectangle(5, 1, 16, 23);
            if (sourceRectangle.Value.Y == 33)
                sourceRectangle = new Rectangle(5, 33, 16, 23);
            if (sourceRectangle.Value.Y == 65)
                sourceRectangle = new Rectangle(5, 65, 16, 23);
            if (sourceRectangle.Value.Y == 98)
                sourceRectangle = new Rectangle(5, 98, 16, 23);

            if (autochemin < 5 && position.X < 28 * Taille_Map.LARGEUR_MAP - 18 && droite &&
                (int)carte.Cases[(int)(position.Y + 27) / 28, (int)((position.X - 12) / 28) + 1].Type > 0 &&
                (int)carte.Cases[(int)(position.Y + 7) / 28, (int)((position.X - 12) / 28) + 1].Type > 0)
            {

                index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;
                if (index < maxIndex)
                {
                    sourceRectangle = new Rectangle((int)index * 5, 33, 16, 23);
                    position.X += vitesse_sprite;
                }
                else
                    index = 0f;
            }
            else if (autochemin < 10 && position.Y < 28 * (Taille_Map.HAUTEUR_MAP - 1) && descendre &&
                     (int)carte.Cases[(int)(position.Y / 28) + 1, (int)(position.X + 15) / 28].Type > 0 &&
                     (int)carte.Cases[(int)(position.Y / 28) + 1, (int)position.X / 28].Type > 0)
            {
                index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;
                if (index < maxIndex)
                {
                    sourceRectangle = new Rectangle((int)index * 5, 65, 16, 23);
                    position.Y += vitesse_sprite;
                }
                else
                    index = 0f;
            }
            else if (autochemin < 15 && position.X > 0 && gauche &&
                     (int)carte.Cases[(int)(position.Y + 27) / 28, (int)(position.X - 1) / 28].Type > 0 &&
                     (int)carte.Cases[(int)(position.Y + 7) / 28, (int)(position.X - 1) / 28].Type > 0)
            {

                index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;
                if (index < maxIndex)
                {
                    sourceRectangle = new Rectangle((int)index * 5, 98, 16, 23);
                    position.X -= vitesse_sprite;
                }
                else
                    index = 0f;
            }
            else if (autochemin < 20 && position.Y > 0 && monter &&
                     (int)carte.Cases[(int)(position.Y + 6) / 28, (int)(position.X + 15) / 28].Type > 0 &&
                     (int)carte.Cases[(int)(position.Y + 6) / 28, (int)position.X / 28].Type > 0)
            {
                index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;
                if (index < maxIndex)
                {
                    sourceRectangle = new Rectangle((int)index * 5, 1, 16, 23);
                    position.Y -= vitesse_sprite;
                }
                else
                    index = 0f;
            }
            else
            {
                autochemin = 0;
            }

            msElapsed += gameTime.ElapsedGameTime.Milliseconds;
            if (walkingList.Count != 0)
            {
                if (msElapsed >= 100)
                {
                    msElapsed = 0;
                    position.X = walkingList[walkingList.Count - 1].Position.X;
                    position.Y = walkingList[walkingList.Count - 1].Position.Y;
                    Position = walkingList[walkingList.Count - 1].Position;
                    walkingList.RemoveAt(walkingList.Count - 1);
                }
            }
        }*/

        public void Draw(SpriteBatch spriteBatch, Rectangle camera)
        {
            spriteBatch.Draw(texture, new Vector2(position.X - camera.X, position.Y - camera.Y), sourceRectangle, Color.White);
        }
    }
}