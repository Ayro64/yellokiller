﻿using System;
using System.Collections.Generic;
using System.Linq;
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

    class Hero2
    {
        KeyboardState lastKeyboardState, keyboardState;
        Vector2 position;
        float vitesse_animation = 0.008f;
        int vitesse_sprite = 2;
        float index = 0;
        int maxIndex = 0;
        Rectangle? sourceRectangle = null;
        Texture2D texture;

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

        public Hero2(Vector2 position)
        {
            this.position = position;
        }
        public Hero2(Vector2 position, Rectangle? sourceRectangle)
        {
            this.position = position;

            this.sourceRectangle = sourceRectangle;
        }
        public Hero2(float x, float y, Rectangle? sourceRectangle)
        {
            position = new Vector2(x, y);
            this.sourceRectangle = sourceRectangle;
        }

        public void LoadContent(ContentManager content, string assetName, int maxIndex)
        {
            texture = content.Load<Texture2D>(assetName);
            this.maxIndex = maxIndex;
        }

        public void Update(GameTime gameTime, int[,] map, int hauteurMap, int largeurMap)
        {
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            if (Keyboard.GetState().IsKeyUp(Keys.Up))                        // arreter le sprite
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

                if (position.Y > 0 && keyboardState.IsKeyDown(Keys.Up) &&
            ((map[(int)(position.Y - 1) / 28, (int)position.X / 28] == 0 && map[(int)(position.Y - 1) / 28, (int)(position.X + 15) / 28] == 0 || map[(int)(position.Y - 1) / 28, (int)position.X / 28] == 1 && map[(int)(position.Y - 1) / 28, (int)(position.X + 15) / 28] == 1)
          || (map[(int)(position.Y - 1) / 28, (int)position.X / 28] == 0 && map[(int)(position.Y - 1) / 28, (int)(position.X + 15) / 28] == 1 || map[(int)(position.Y - 1) / 28, (int)position.X / 28] == 1 && map[(int)(position.Y - 1) / 28, (int)(position.X + 15) / 28] == 0)))
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
            }

            if (position.Y < 28 * (hauteurMap - 1) && keyboardState.IsKeyDown(Keys.Down) &&
                ((map[(int)(position.Y / 28) + 1, (int)(position.X) / 28] == 0 && map[(int)(position.Y / 28) + 1, (int)(position.X + 15) / 28] == 0 || map[(int)(position.Y / 28) + 1, (int)position.X / 28] == 1 && map[(int)(position.Y / 28) + 1, (int)(position.X + 15) / 28] == 1)
              || (map[(int)(position.Y / 28) + 1, (int)(position.X) / 28] == 0 && map[(int)(position.Y / 28) + 1, (int)(position.X + 15) / 28] == 1 || map[(int)(position.Y / 28) + 1, (int)position.X / 28] == 1 && map[(int)(position.Y / 28) + 1, (int)(position.X + 15) / 28] == 0)))
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
            }

            if (position.X > 0 && keyboardState.IsKeyDown(Keys.Left) &&
               ((map[(int)position.Y / 28, (int)(position.X - 1) / 28] == 0 && map[(int)(position.Y + 27) / 28, (int)(position.X - 1) / 28] == 0 || map[(int)position.Y / 28, (int)(position.X - 1) / 28] == 1 && map[(int)(position.Y + 27) / 28, (int)(position.X - 1) / 28] == 1)
             || (map[(int)position.Y / 28, (int)(position.X - 1) / 28] == 0 && map[(int)(position.Y + 27) / 28, (int)(position.X - 1) / 28] == 1 || map[(int)position.Y / 28, (int)(position.X - 1) / 28] == 1 && map[(int)(position.Y + 27) / 28, (int)(position.X - 1) / 28] == 0)))

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
            }

            if (position.X < 28 * largeurMap - 16 && keyboardState.IsKeyDown(Keys.Right) &&
               ((map[(int)position.Y / 28, (int)((position.X - 12) / 28) + 1] == 0 && map[(int)(position.Y + 27) / 28, (int)((position.X - 12) / 28) + 1] == 0 || map[(int)position.Y / 28, (int)((position.X - 12) / 28) + 1] == 1 && map[(int)(position.Y + 27) / 28, (int)((position.X - 12) / 28) + 1] == 1)
             || (map[(int)position.Y / 28, (int)((position.X - 12) / 28) + 1] == 0 && map[(int)(position.Y + 27) / 28, (int)((position.X - 12) / 28) + 1] == 1 || map[(int)position.Y / 28, (int)((position.X - 12) / 28) + 1] == 1 && map[(int)(position.Y + 27) / 28, (int)((position.X - 12) / 28) + 1] == 0)))

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
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, Color.White);
        }
    }
}
