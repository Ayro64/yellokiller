using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Yellokiller
{
    class Pausebckground : GameScreen
    {
        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public Pausebckground()
        {
        }

        #endregion

        #region Draw


        /// <summary>
        /// Draws the pause menu screen. This darkens down the gameplay screen
        /// that is underneath us, and then chains to the base MenuScreen.Draw.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);
        }

        #endregion

        public static void Quit(PlayerIndex playerIndex, ScreenManager screenManager)
        {
            screenManager.GetScreens()[1].ExitScreen();
        }
    }
}
