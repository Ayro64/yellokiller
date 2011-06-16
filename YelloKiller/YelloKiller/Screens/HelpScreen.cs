using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    class HelpScreen : GameScreen
    {
        #region Fields

        ContentManager contentManager;
        SpriteBatch spriteBatch;
        Texture2D manette, ennemis, current;

        #endregion

        #region Initialization

        public HelpScreen(YellokillerGame game)
        {
            this.game = game;
            spriteBatch = ScreenManager.spriteBatch;
        }

        public override void LoadContent()
        {
            if (contentManager == null)
                contentManager = new ContentManager(ScreenManager.Game.Services, "Content");
        }

        public override void UnloadContent()
        {
            contentManager.Unload();
        }

        #endregion

        #region Draw

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();


            spriteBatch.End();
        }

        #endregion

        #region Handle Input

        public override void HandleInput(InputState input)
        {
            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.Enter) || ServiceHelper.Get<IGamePadService>().Tirer())
            {
                LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                               new MainMenuScreen(game));
                this.ExitScreen();
            }
        }

        #endregion
    }
}
