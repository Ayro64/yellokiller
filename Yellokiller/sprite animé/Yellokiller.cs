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

namespace sprite_animé
{
    public class Yellokiller : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Hero1 hero1;
        Hero2 hero2;
        Map carte;

        public Yellokiller()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            ServiceHelper.Game = this;
            Components.Add(new KeyboardService(this));
            graphics.PreferredBackBufferHeight = 476;
            graphics.PreferredBackBufferWidth = 616;
        }

        protected override void Initialize()
        {
            carte = new Map("map.txt");
            hero1 = new Hero1(new Vector2(336, 10), new Rectangle(25, 133, 16, 25));
            hero2 = new Hero2(new Vector2(346, 10), new Rectangle(25, 133, 16, 25));
            base.Initialize();
        }

     
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            hero1.LoadContent(Content, "NinjaTrans",2 );
            hero2.LoadContent(Content, "NinjaTrans", 2);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            hero1.Update(gameTime, carte.map, carte.hauteurMap, carte.largeurMap);
            hero2.Update(gameTime, carte.map, carte.hauteurMap, carte.largeurMap);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            carte.Draw(spriteBatch, Content);
            hero1.Draw(spriteBatch);
            hero2.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
