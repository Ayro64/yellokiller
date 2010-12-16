using System;
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

namespace Space_Invaders
{
    public class SpaceInvaders : Microsoft.Xna.Framework.Game
    {
        Invaders[,] invaders_pos = new Invaders[4, 15];
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
       
        int screenwidth, screenheight;
        Hero player;
        int dir = 1;
        
        public SpaceInvaders()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            ServiceHelper.Game = this;
            Components.Add(new KeyboardService(this));

            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;



        }

        protected override void Initialize()
        {
            player = new Hero(this);
            screenwidth = GraphicsDevice.Viewport.Width;
            screenheight = GraphicsDevice.Viewport.Height;
           

            for (int j = 0; j < 15; j++)
            {
                if (j < 12)
                    invaders_pos[0, j] = new Soucoupes(new Vector2(90 + j * 50, 20), this, Color.BlueViolet);

                invaders_pos[1, j] = new Poulpes(new Vector2(100 + j * 40, 70), this, Color.BlueViolet);

                invaders_pos[2, j] = new Seches(new Vector2(100 + j * 40, 115), this, Color.BlueViolet);

                invaders_pos[3, j] = new Crabes(new Vector2(100 + j * 40, 160), this, Color.BlueViolet);
            }
            base.Initialize();
        }



        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            player.Update(screenheight, screenwidth, this);
                        
            if (invaders_pos[1, 14 ].bot_position.X > 778)
               dir = -1;

            else if (invaders_pos[0, 0].bot_position.X < 0)
                dir = 1;

            for (int j = 0; j < 15; j++)
            {
                    if (j < 12)
                        invaders_pos[0, j].bot_position.X+=dir;

                    invaders_pos[1, j].bot_position.X+=dir;
                    invaders_pos[2, j].bot_position.X+=dir;
                    invaders_pos[3, j].bot_position.X+=dir;
                              
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            int j;
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            spriteBatch.Begin();  

            player.Draw(spriteBatch);

            for (j = 0; j < 12; j++)
            {
                invaders_pos[0, j].draw(spriteBatch);
            }

            for (j = 0; j < 15; j++)
            {
                invaders_pos[1, j].draw(spriteBatch);
            }

            for (j = 0; j < 15; j++)
            {
                invaders_pos[2, j].draw(spriteBatch);
            }

            for (j = 0; j < 15; j++)
            {
                invaders_pos[3, j].draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
