﻿using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Yellokiller
{
    class Player
    {
        #region Properties

        public bool IsPlayer
        {
            get { return isPlayer; }
            protected internal set { isPlayer = value; }
        }

        bool isPlayer = true;
  
        #endregion

        #region Fields

        float Volume;
        int m, n;
        MediaLibrary sampleMediaLibrary;
        Random rand;
        SpriteBatch spriteBatch;
        SpriteFont font;

        #endregion

        #region Initialization

        public Player(int soundVolume)
        {
            sampleMediaLibrary = new MediaLibrary();
            rand = new Random();

            m = rand.Next(0, sampleMediaLibrary.Albums.Count - 1);
            n = rand.Next(0, sampleMediaLibrary.Albums[m].Songs.Count);
            MediaPlayer.Play(sampleMediaLibrary.Albums[m].Songs[n]);
            Volume = soundVolume;
            MediaPlayer.Volume = Volume;
        }

        public void LoadContent()
        {
        }

        public void UnloadContent()
        {
        }


        #endregion

        #region Handle Input

        public void HandleInput(KeyboardState keyboardState, KeyboardState lastKeyboardState)
        {
            // Change track
            if (keyboardState.IsKeyDown(Keys.K))
            {
                m = rand.Next(0, sampleMediaLibrary.Albums.Count - 1);
                n = rand.Next(0, sampleMediaLibrary.Albums[m].Songs.Count);
                MediaPlayer.Play(sampleMediaLibrary.Albums[m].Songs[n]);
            }

            // Pause player
            if (keyboardState.IsKeyDown(Keys.L) && lastKeyboardState.IsKeyUp(Keys.L))
            {
                if (MediaPlayer.State == MediaState.Playing)
                    MediaPlayer.Pause();
                else
                    MediaPlayer.Resume();
            }
        }

        #endregion

        #region Update and Draw

        public void Update(GameTime gameTime)
        {
            // Change to the next track when the last one ends.
            if (MediaPlayer.State == MediaState.Stopped)
            {
                m = rand.Next(0, sampleMediaLibrary.Albums.Count - 1);
                n = rand.Next(0, sampleMediaLibrary.Albums[m].Songs.Count);
                MediaPlayer.Play(sampleMediaLibrary.Albums[m].Songs[n]);
            }
        }

        public void Draw(GameTime gameTime)
        {
            // Song name.
            spriteBatch = ScreenManager.spriteBatch;
            font = ScreenManager.font;
            spriteBatch.DrawString(font, sampleMediaLibrary.Albums[m].Songs[n].Artist + " - " + sampleMediaLibrary.Albums[m].Songs[n], new Vector2(10, 10), Color.Red);
        }

        public void Close()
        {
            MediaPlayer.Stop();
        }

        #endregion
    }
}