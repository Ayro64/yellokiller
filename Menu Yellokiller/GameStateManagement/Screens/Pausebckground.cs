using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Yellokiller
{
    class Pausebckground : MenuScreen
    {
        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public Pausebckground()
            : base("")
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
      

            base.Draw(gameTime);
        }


        #endregion
    }
}
