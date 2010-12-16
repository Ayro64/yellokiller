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
    public class Yello_Killer : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        Player1 max1;
        Player2 max2;
        
        public Yello_Killer()
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
            int screenwidth = GraphicsDevice.Viewport.Width;
            int screenheight = GraphicsDevice.Viewport.Height;

            max1 = new Player1(new Vector2(350,300));
            max2 = new Player2(new Vector2(450,500));
                       
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            max1.LoadContent(Content, "Mario");
            max2.LoadContent(Content, "joueur");
            max1.Scale = new Vector2(0.2f, 0.2f);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            max1.Update(gameTime);
            max2.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            spriteBatch.Begin();

            max1.Draw(spriteBatch);
            max2.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
