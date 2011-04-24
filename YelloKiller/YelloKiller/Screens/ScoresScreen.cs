using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace YelloKiller
{
    class ScoresScreen : MenuScreen
    {
        #region Fields

        ContentManager content;
        Texture2D scoresTexture, scroll, blankTexture;
        YellokillerGame game;

        Color Color;

        #endregion
        
        #region Initialization

        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        /// 
        public ScoresScreen(YellokillerGame game)
            : base(Langue.tr("Scores"))
        {
            this.game = game;
        }

        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, wheras if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            //Ecran Scores.
            scoresTexture = content.Load<Texture2D>("ScoresTex");

            //Carré noir.
            blankTexture = content.Load<Texture2D>("blank");

            // Parchemin
            scroll = content.Load<Texture2D>("ScoresScrollFil");
        }

        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draws the Scores screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            Color = new Color(0, 0, 0, TransitionAlpha);
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            SpriteFont font = ScreenManager.Font;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            byte fade = TransitionAlpha;

            spriteBatch.Begin();

            spriteBatch.Draw(scoresTexture, fullscreen,
                             new Color(fade, fade, fade));

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);


            spriteBatch.Draw(scroll,
                             new Rectangle(viewport.Width / 5, 60, viewport.Width / 5 * 3, viewport.Height / 5 * 3),
                             new Color(fade, fade, fade));

            // Draw the menu title.
            Vector2 titlePosition = new Vector2(viewport.Width / 2, 100);
            Vector2 titleOrigin = font.MeasureString(MenuTitle) / 2;
            float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, MenuTitle, titlePosition, Color, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }

        #endregion

    }
}
