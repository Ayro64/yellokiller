using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using S = YelloKiller.Properties.Scores;

namespace YelloKiller
{
    class ScoresScreen : MenuScreen
    {
        #region Fields

        ContentManager content;
        Texture2D scoresTexture, scroll, blankTexture, kane;
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

            // Kanji argent.
            kane = content.Load<Texture2D>("Kane");
        }

        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }

        #endregion

        protected override void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
        {
            OnCancel(playerIndex);
        }

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
            Rectangle scrolltangle = new Rectangle(viewport.Width / 5, 150, viewport.Width / 5 * 3, viewport.Height / 5 * 3);
            byte fade = TransitionAlpha;
            Vector2 ScoresPos = new Vector2(scrolltangle.X + 100, scrolltangle.Y + 100);

            spriteBatch.Begin();

            spriteBatch.Draw(scoresTexture, fullscreen,
                             new Color(fade, fade, fade));

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);


            spriteBatch.Draw(scroll,
                             scrolltangle,
                             new Color(fade, fade, fade));

            // Draw the menu title.
            Vector2 titlePosition = new Vector2(viewport.Width / 2, 190);
            Vector2 titleOrigin = font.MeasureString(MenuTitle) / 2;
            float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, MenuTitle, titlePosition, Color, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.DrawString(font, "1. " + S.Default.Name_01, ScoresPos, Color);
            ScoresPos.X += 155;
            spriteBatch.DrawString(font, S.Default.Map_01, ScoresPos, Color);
            ScoresPos.X += 150;
            spriteBatch.DrawString(font, S.Default.Score_01.ToString(), ScoresPos, Color);
            spriteBatch.Draw(kane, new Rectangle((int)(ScoresPos.X + font.MeasureString(S.Default.Score_01.ToString()).X + 5), (int)ScoresPos.Y + 5, 20, 10), Color);
            ScoresPos += new Vector2(-305, 25);

            spriteBatch.DrawString(font, "2. " + S.Default.Name_02, ScoresPos, Color);
            ScoresPos.X += 155;
            spriteBatch.DrawString(font, S.Default.Map_02, ScoresPos, Color);
            ScoresPos.X += 150;
            spriteBatch.DrawString(font, S.Default.Score_02.ToString(), ScoresPos, Color);
            spriteBatch.Draw(kane, new Rectangle((int)(ScoresPos.X + font.MeasureString(S.Default.Score_02.ToString()).X + 5), (int)ScoresPos.Y + 5, 20, 10), Color);
            ScoresPos += new Vector2(-305, 25);

            spriteBatch.DrawString(font, "3. " + S.Default.Name_03, ScoresPos, Color);
            ScoresPos.X += 155;
            spriteBatch.DrawString(font, S.Default.Map_03, ScoresPos, Color);
            ScoresPos.X += 150;
            spriteBatch.DrawString(font, S.Default.Score_03.ToString(), ScoresPos, Color);
            spriteBatch.Draw(kane, new Rectangle((int)(ScoresPos.X + font.MeasureString(S.Default.Score_03.ToString()).X + 5), (int)ScoresPos.Y + 5, 20, 10), Color);
            ScoresPos += new Vector2(-305, 25);

            spriteBatch.DrawString(font, "4. " + S.Default.Name_04, ScoresPos, Color);
            ScoresPos.X += 155;
            spriteBatch.DrawString(font, S.Default.Map_04, ScoresPos, Color);
            ScoresPos.X += 150;
            spriteBatch.DrawString(font, S.Default.Score_04.ToString(), ScoresPos, Color);
            spriteBatch.Draw(kane, new Rectangle((int)(ScoresPos.X + font.MeasureString(S.Default.Score_04.ToString()).X + 5), (int)ScoresPos.Y + 5, 20, 10), Color);
            ScoresPos += new Vector2(-305, 25);

            spriteBatch.DrawString(font, "5. " + S.Default.Name_05, ScoresPos, Color);
            ScoresPos.X += 155;
            spriteBatch.DrawString(font, S.Default.Map_05, ScoresPos, Color);
            ScoresPos.X += 150;
            spriteBatch.DrawString(font, S.Default.Score_05.ToString(), ScoresPos, Color);
            spriteBatch.Draw(kane, new Rectangle((int)(ScoresPos.X + font.MeasureString(S.Default.Score_05.ToString()).X + 5), (int)ScoresPos.Y + 5, 20, 10), Color);
            ScoresPos += new Vector2(-305, 25);

            spriteBatch.DrawString(font, "6. " + S.Default.Name_06, ScoresPos, Color);
            ScoresPos.X += 155;
            spriteBatch.DrawString(font, S.Default.Map_06, ScoresPos, Color);
            ScoresPos.X += 150;
            spriteBatch.DrawString(font, S.Default.Score_06.ToString(), ScoresPos, Color);
            spriteBatch.Draw(kane, new Rectangle((int)(ScoresPos.X + font.MeasureString(S.Default.Score_06.ToString()).X + 5), (int)ScoresPos.Y + 5, 20, 10), Color);
            ScoresPos += new Vector2(-305, 25);

            spriteBatch.DrawString(font, "7. " + S.Default.Name_07, ScoresPos, Color);
            ScoresPos.X += 155;
            spriteBatch.DrawString(font, S.Default.Map_07, ScoresPos, Color);
            ScoresPos.X += 150;
            spriteBatch.DrawString(font, S.Default.Score_07.ToString(), ScoresPos, Color);
            spriteBatch.Draw(kane, new Rectangle((int)(ScoresPos.X + font.MeasureString(S.Default.Score_07.ToString()).X + 5), (int)ScoresPos.Y + 5, 20, 10), Color);
            ScoresPos += new Vector2(-305, 25);

            spriteBatch.DrawString(font, "8. " + S.Default.Name_08, ScoresPos, Color);
            ScoresPos.X += 155;
            spriteBatch.DrawString(font, S.Default.Map_08, ScoresPos, Color);
            ScoresPos.X += 150;
            spriteBatch.DrawString(font, S.Default.Score_08.ToString(), ScoresPos, Color);
            spriteBatch.Draw(kane, new Rectangle((int)(ScoresPos.X + font.MeasureString(S.Default.Score_08.ToString()).X + 5), (int)ScoresPos.Y + 5, 20, 10), Color);
            ScoresPos += new Vector2(-305, 25);

            spriteBatch.DrawString(font, "9. " + S.Default.Name_09, ScoresPos, Color);
            ScoresPos.X += 155;
            spriteBatch.DrawString(font, S.Default.Map_09, ScoresPos, Color);
            ScoresPos.X += 150;
            spriteBatch.DrawString(font, S.Default.Score_09.ToString(), ScoresPos, Color);
            spriteBatch.Draw(kane, new Rectangle((int)(ScoresPos.X + font.MeasureString(S.Default.Score_09.ToString()).X + 5), (int)ScoresPos.Y + 5, 20, 10), Color);
            ScoresPos += new Vector2(-316, 25);

            spriteBatch.DrawString(font, "10. " + S.Default.Name_10, ScoresPos, Color);
            ScoresPos.X += 166;
            spriteBatch.DrawString(font, S.Default.Map_10, ScoresPos, Color);
            ScoresPos.X += 150;
            spriteBatch.DrawString(font, S.Default.Score_10.ToString(), ScoresPos, Color);
            spriteBatch.Draw(kane, new Rectangle((int)(ScoresPos.X + font.MeasureString(S.Default.Score_10.ToString()).X + 5), (int)ScoresPos.Y + 5, 20, 10), Color);
            ScoresPos += new Vector2(-305, 25);

            spriteBatch.End();
        }

        #endregion

    }
}
