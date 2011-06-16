using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace YelloKiller
{
    class IntroScreen : GameScreen
    {
        #region Fields

        VideoPlayer VLC;
        Video cinematique;
        YellokillerGame game;

        ContentManager contentManager;
        SpriteBatch spriteBatch;

        #endregion

        #region Initialization

        public IntroScreen(YellokillerGame game)
        {
            this.game = game;
            VLC = new VideoPlayer();
            spriteBatch = ScreenManager.spriteBatch;
        }

        public override void LoadContent()
        {
            if (contentManager == null)
                contentManager = new ContentManager(ScreenManager.Game.Services, "Content");

            cinematique = contentManager.Load<Video>(@"Cinématique\Cinematique");
            VLC.Play(cinematique);
        }

        public override void UnloadContent()
        {
            contentManager.Unload();
        }

        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (VLC.State == MediaState.Stopped)
            {
                this.ExitScreen();
                LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                               new MainMenuScreen(game));
            }
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            if (VLC.State == MediaState.Playing)
                spriteBatch.Draw(VLC.GetTexture(), new Vector2(0, Taille_Ecran.HAUTEUR_ECRAN / 2 - ((float)Taille_Ecran.LARGEUR_ECRAN / (float)cinematique.Width) * cinematique.Height / 2), null, Color.White, 0, Vector2.Zero, (float)Taille_Ecran.LARGEUR_ECRAN / (float)cinematique.Width, SpriteEffects.None, 1);

            spriteBatch.End();
        }

        #endregion

        #region Handle Input

        public override void HandleInput(InputState input)
        {
            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.Enter) || ServiceHelper.Get<IGamePadService>().Tirer())
            {
                VLC.Stop();
                LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                               new MainMenuScreen(game));
            }
        }

        #endregion
    }
}