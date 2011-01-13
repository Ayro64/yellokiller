using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Yellokiller
{
    class Hero1
    {
        Vector2 position;
        float vitesse_animation = 0.008f;
        int vitesse_sprite = 1;
        float index = 0;
        int maxIndex = 0;
        Rectangle? sourceRectangle = null;
        Rectangle rectangle;
        Texture2D texture;
        KeyboardState lastKeyboardState, keyboardState;
        public bool monter = true, descendre = true, droite = true, gauche = true;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
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

        public Hero1(Vector2 position, Rectangle? sourceRectangle)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            texture = content.Load<Texture2D>("NinjaTrans");
            this.maxIndex = maxIndex;
        }

        public void Update(GameTime gameTime, char[,] map, int hauteurMap, int largeurMap, Hero2 hero2)
        {
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            rectangle = new Rectangle((int)Position.X, (int)Position.Y, 18, 28);
            Moteur_physique.Collision(this.rectangle, hero2.Rectangle, ref droite, ref gauche, ref monter, ref descendre);

            if (Keyboard.GetState().IsKeyUp(Keys.Z))                        // arreter le sprite
            {
                if (sourceRectangle.Value.Y == 133)
                    sourceRectangle = new Rectangle(24, 133, 16, 28);
                if (sourceRectangle.Value.Y == 198)
                    sourceRectangle = new Rectangle(24, 197, 16, 28);
                if (sourceRectangle.Value.Y == 230)
                    sourceRectangle = new Rectangle(24, 229, 16, 28);
                if (sourceRectangle.Value.Y == 166)
                    sourceRectangle = new Rectangle(24, 165, 16, 28);
            }

            if (keyboardState.IsKeyUp(Keys.LeftShift))
                vitesse_animation = 0.008f;

            if (position.Y > 0 && keyboardState.IsKeyDown(Keys.Z) && monter &&
               (map[(int)(position.Y + 6) / 28, (int)(position.X + 15) / 28] == 'h' ||
                map[(int)(position.Y + 6) / 28, (int)(position.X + 15) / 28] == 'a') &&
               (map[(int)(position.Y + 6) / 28, (int)position.X / 28] == 'h' ||
                map[(int)(position.Y + 6) / 28, (int)position.X / 28] == 'a'))
            {
                index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;
                if (index < maxIndex)
                {
                    sourceRectangle = new Rectangle((int)index * 48, 133, 16, 28);
                    position.Y -= 1 * vitesse_sprite;
                }
                else
                {
                    index = 0f;
                }

                if (keyboardState.IsKeyDown(Keys.LeftShift))
                {
                    position.Y -= 1 * vitesse_sprite * 2;
                    vitesse_animation = 0.008f * 2;
                }
            }

            if (position.Y < 28 * (hauteurMap - 1) && keyboardState.IsKeyDown(Keys.S) && descendre &&
                (map[(int)(position.Y / 28) + 1, (int)(position.X + 15) / 28] == 'h' ||
                 map[(int)(position.Y / 28) + 1, (int)(position.X + 15) / 28] == 'a') &&
                (map[(int)(position.Y / 28) + 1, (int)(position.X) / 28] == 'h' ||
                 map[(int)(position.Y / 28) + 1, (int)position.X / 28] == 'a'))
            {
                index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;

                if (index < maxIndex)
                {
                    sourceRectangle = new Rectangle((int)index * 48, 198, 16, 28);
                    position.Y += 1 * vitesse_sprite;
                }
                else
                {
                    index = 0f;
                }

                if (keyboardState.IsKeyDown(Keys.LeftShift))
                {
                    position.Y += 1 * vitesse_sprite * 2;
                    vitesse_animation = 0.008f * 2;
                }
            }

            if (position.X > 0 && keyboardState.IsKeyDown(Keys.Q) && gauche &&
               (map[(int)(position.Y + 27) / 28, (int)(position.X - 1) / 28] == 'h' ||
                map[(int)(position.Y + 27) / 28, (int)(position.X - 1) / 28] == 'a') &&
               (map[(int)(position.Y + 7) / 28, (int)(position.X - 1) / 28] == 'h' ||
                map[(int)(position.Y + 7) / 28, (int)(position.X - 1) / 28] == 'a'))
            {
                index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;
                if (index < maxIndex)
                {
                    sourceRectangle = new Rectangle((int)index * 48, 230, 16, 28);
                    position.X -= 1 * vitesse_sprite;
                }
                else
                {
                    index = 0f;
                }

                if (keyboardState.IsKeyDown(Keys.LeftShift))
                {
                    position.X -= 1 * vitesse_sprite * 2;
                    vitesse_animation = 0.008f * 2;
                }
            }

            if (position.X < 28 * largeurMap - 16 && keyboardState.IsKeyDown(Keys.D) && droite &&
                (map[(int)(position.Y + 27) / 28, (int)((position.X - 12) / 28) + 1] == 'h' ||
                 map[(int)(position.Y + 27) / 28, (int)((position.X - 12) / 28) + 1] == 'a') &&
                (map[(int)(position.Y + 7) / 28, (int)((position.X - 12) / 28) + 1] == 'h' ||
                 map[(int)(position.Y + 7) / 28, (int)((position.X - 12) / 28) + 1] == 'a'))
            {
                index += gameTime.ElapsedGameTime.Milliseconds * vitesse_animation;
                if (index < maxIndex)
                {
                    sourceRectangle = new Rectangle((int)index * 48, 166, 16, 28);
                    position.X += 1 * vitesse_sprite;
                }
                else
                {
                    index = 0f;
                }

                if (keyboardState.IsKeyDown(Keys.LeftShift))
                {
                    position.X += 1 * vitesse_sprite * 2;
                    vitesse_animation = 0.008f * 2;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, Color.White);
        }
    }
}