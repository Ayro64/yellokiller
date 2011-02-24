using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    class Player
    {
        public bool IsPlayer
        {
            get { return isPlayer; }
            protected internal set { isPlayer = value; }
        }

        bool isPlayer = true;

        float Volume;
        int musique, album;
        string songName;
        MediaLibrary sampleMediaLibrary;
        Random rand;
        SpriteBatch spriteBatch;
        SpriteFont font;

        Texture2D blankTexture;

        #region Initialization

        public Player()
        {
            sampleMediaLibrary = new MediaLibrary();
            rand = new Random();

            album = 0;
            musique = rand.Next(0, sampleMediaLibrary.Albums[album].Songs.Count);
            MediaPlayer.Play(sampleMediaLibrary.Albums[album].Songs[musique]);
            songName = sampleMediaLibrary.Albums[album].Songs[musique].Artist + " - " + sampleMediaLibrary.Albums[album].Songs[musique];
            Volume = (Properties.Settings.Default.MusicVolume / 10);
            MediaPlayer.Volume = (float)Volume;
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

        public void HandleInput()
        {
            // Change track
            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.K))
            {
                musique = rand.Next(0, sampleMediaLibrary.Albums[album].Songs.Count);
                MediaPlayer.Play(sampleMediaLibrary.Albums[album].Songs[musique]);
                songName = sampleMediaLibrary.Albums[album].Songs[musique].Artist + " - " + sampleMediaLibrary.Albums[album].Songs[musique];
            }

            // Pause player
            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.L))
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
                musique = rand.Next(0, sampleMediaLibrary.Albums[album].Songs.Count);
                MediaPlayer.Play(sampleMediaLibrary.Albums[album].Songs[musique]);
                songName = sampleMediaLibrary.Albums[album].Songs[musique].Artist + " - " + sampleMediaLibrary.Albums[album].Songs[musique];
            }
        }

        public void Draw(GameTime gameTime)
        {
            // Song name.
            spriteBatch = ScreenManager.spriteBatch;
            font = ScreenManager.font;

            spriteBatch.Begin();

            spriteBatch.Draw(blankTexture, new Rectangle(0, 0, (songName.Length * 12), 45), new Color(0, 0, 0, (byte)(170)));

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