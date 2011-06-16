using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace YelloKiller
{
    class HelpScreen : GameScreen
    {
        #region Fields

        YellokillerGame game;
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

            current = contentManager.Load<Texture2D>("QuickHelp\\Commandes");
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

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            byte fade = TransitionAlpha;

            spriteBatch.Begin();

            spriteBatch.Draw(current, fullscreen,
                             new Color(fade, fade, fade));

            spriteBatch.End();
        }

        #endregion

        #region Handle Input

        public override void HandleInput(InputState input)
        {
            if (current == ennemis && (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.Enter) || ServiceHelper.Get<IGamePadService>().Tirer()))
            {
                LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                               new MainMenuScreen(game));
            }
        }

        #endregion
    }
}
