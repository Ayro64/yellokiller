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

            //Ecran Game Over.
            scoresTexture = content.Load<Texture2D>("Game Over");

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
            Color = new Color(255, 0, 0, TransitionAlpha);
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            SpriteFont font = ScreenManager.Font;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            byte fade = TransitionAlpha;

            spriteBatch.Begin();

            spriteBatch.Draw(scoresTexture, fullscreen,
                             new Color(fade, fade, fade));

            // Rectangle noir
            spriteBatch.Draw(blankTexture, new Rectangle(100, 100, 100, 100), new Color(0, 0, 0, (byte)(fade * 2 / 3)));

            // Draw the menu title.
            Vector2 ScoPosition = new Vector2(viewport.Width / 2, 100);
            Vector2 ScOrigin = font.MeasureString(MenuTitle) / 2;

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            ScoPosition.Y -= transitionOffset * 100;

            spriteBatch.Draw(scroll,
                             new Rectangle(viewport.Width / 4, 40, (int)(font.MeasureString("1. XXXXXXXXXX ----- XXXXXXXXXX").X + 30), viewport.Height / 2),
                             new Color(fade, fade, fade));

            spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion

    }
}
