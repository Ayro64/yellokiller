using Microsoft.Xna.Framework;
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
        byte numero;
        Texture2D blankTexture;

        #region Initialization

        public Player()
        {
            Volume = (Properties.Settings.Default.MusicVolume / 10);
            MediaPlayer.Volume = (float)Volume;
            numero = 0;
        }

        public void LoadContent(ContentManager content)
        {
            blankTexture = content.Load<Texture2D>("blank");
        }

        #endregion

        public void PlayMusique(Song[] musiques)
        {
            MediaPlayer.Play(musiques[numero]);
        }

        #region Update and Draw

        public void Update(GameTime gameTime)
        {
            if (MediaPlayer.State == MediaState.Stopped)
            {
                numero++;
                numero %= 2;
                PlayMusique(ScreenManager.AudioEngine.Musiques);
            }
        }

        public void Close()
        {
            MediaPlayer.Stop();
        }

        #endregion
    }
}