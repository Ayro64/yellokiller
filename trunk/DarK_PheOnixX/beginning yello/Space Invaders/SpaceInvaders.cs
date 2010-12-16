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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
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

            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;



            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            player = new Hero(this);
            screenwidth = GraphicsDevice.Viewport.Width;
            screenheight = GraphicsDevice.Viewport.Height;
            // TODO: Add your initialization logic here
           

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



        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            player.Update(screenheight, screenwidth, this);

            // TODO: Add your update logic here

            
                if (invaders_pos[1, 14 ].bot_position.X > 778 )
                {
                    dir = -1;
                }
            else if (invaders_pos[0, 0].bot_position.X < 0)
            {
                dir = 1;
                }

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

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            int j;
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            // TODO: Add your drawing code here
            

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
