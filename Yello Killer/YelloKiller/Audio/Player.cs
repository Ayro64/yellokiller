using Microsoft.Xna.Framework.Audio;
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
        int n;
	string songName;
        MediaLibrary sampleMediaLibrary;
        Random rand;
        SpriteBatch spriteBatch;
        SpriteFont font;

	Texture2D blankTexture;

        #endregion

        #region Initialization

        public Player(int soundVolume)
        {
            sampleMediaLibrary = new MediaLibrary();
            rand = new Random();

            n = rand.Next(0, sampleMediaLibrary.Albums[1].Songs.Count);
            MediaPlayer.Play(sampleMediaLibrary.Albums[1].Songs[n]);
	    songName = sampleMediaLibrary.Albums[m].Songs[n].Artist + " - " + sampleMediaLibrary.Albums[m].Songs[n]; 
            Volume = soundVolume;
            MediaPlayer.Volume = Volume;
        }

        public void LoadContent(ContentManager content)
        {
	    blankTexture = content.Load<Texture2D>("blank");
        }

        public void UnloadContent()
        {
        }


        #endregion

        #region Handle Input

        public void HandleInput(KeyboardState keyboardState, KeyboardState lastKeyboardState)
        {
            // Change track
            if (keyboardState.IsKeyDown(Keys.K) && lastKeyboardState.IsKeyUp(Keys.K))
            {
                n = rand.Next(0, sampleMediaLibrary.Albums[1].Songs.Count);
                MediaPlayer.Play(sampleMediaLibrary.Albums[1].Songs[n]);
		songName = sampleMediaLibrary.Albums[m].Songs[n].Artist + " - " + sampleMediaLibrary.Albums[m].Songs[n];
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
                n = rand.Next(0, sampleMediaLibrary.Albums[1].Songs.Count);
                MediaPlayer.Play(sampleMediaLibrary.Albums[1].Songs[n]);
		songName = sampleMediaLibrary.Albums[m].Songs[n].Artist + " - " + sampleMediaLibrary.Albums[m].Songs[n];
            }
        }

        public void Draw(GameTime gameTime)
        {
            // Song name.
            spriteBatch = ScreenManager.spriteBatch;
            font = ScreenManager.font;

            spriteBatch.Begin();

	    spriteBatch.Draw(blankTexture,
                             new Rectangle(0, 0, (songName.Length * 12), 45),
                             new Color(0, 0, 0, (byte)(170)));

            spriteBatch.DrawString(font, songName, new Vector2(10, 10), Color.Red);
            spriteBatch.End();
        }

        public void Close()
        {
            MediaPlayer.Stop();
        }

        #endregion
    }
}