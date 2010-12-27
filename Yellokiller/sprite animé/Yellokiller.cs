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

namespace Yellokiller
{
    public class Yellokiller : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Hero1 hero1;
        Hero2 hero2;
        Map carte;

        MediaLibrary sampleMediaLibrary;
        Random rand;
        KeyboardState keyboardState, lastKeyboardState;
        int i, j;

        public Yellokiller()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "content";
            ServiceHelper.Game = this;
            Components.Add(new KeyboardService(this));
            graphics.PreferredBackBufferHeight = 476;
            graphics.PreferredBackBufferWidth = 616;
            sampleMediaLibrary = new MediaLibrary();
            rand = new Random();
        }

        protected override void Initialize()
        {
            carte = new Map("map.txt");
            hero1 = new Hero1(new Vector2(330, 10), new Rectangle(25, 133, 16, 25));
            hero2 = new Hero2(new Vector2(350, 10), new Rectangle(25, 133, 16, 25));           

            i = rand.Next(0, sampleMediaLibrary.Albums.Count - 1);
            j = rand.Next(0, sampleMediaLibrary.Albums[i].Songs.Count);
            MediaPlayer.Play(sampleMediaLibrary.Albums[i].Songs[j]);
            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            hero2.LoadContent(Content, 2);
            hero1.LoadContent(Content, 2);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.K))
            {
                i = rand.Next(0, sampleMediaLibrary.Albums.Count - 1);
                j = rand.Next(0, sampleMediaLibrary.Albums[i].Songs.Count);
                MediaPlayer.Play(sampleMediaLibrary.Albums[i].Songs[j]);
            }
            if (keyboardState.IsKeyDown(Keys.Space) && lastKeyboardState.IsKeyUp(Keys.Space))
            {
                if (MediaPlayer.State == MediaState.Playing)
                    MediaPlayer.Pause();
                else
                    MediaPlayer.Resume();
            }

            hero1.Update(gameTime, carte.map, carte.hauteurMap, carte.largeurMap);
            hero2.Update(gameTime, carte.map, carte.hauteurMap, carte.largeurMap);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            carte.Draw(spriteBatch, Content);
            hero2.Draw(spriteBatch);
            hero1.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
