using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace YelloKiller
{
    class EditorSavePop : MessageBoxScreen
    {
        #region Initialization

        public string nomSauvegarde;
        uint lastIntTimer, mod;
        Texture2D gradientTexture;
        static double timer = 0;
        bool underscore;
        public new event EventHandler<PlayerIndexEventArgs> Accepted;

        public EditorSavePop(string nomSauvegarde, uint mod)
            : base(Langue.tr("SavePop-Up"), false)
        {
            this.mod = mod;
            this.Type = "Pop";
            this.nomSauvegarde = nomSauvegarde;
            this.UsageText = Langue.tr("ScoresBox");
            lastIntTimer = 0;
            underscore = false;
            EventInput.EventInput.CharEntered += new EventInput.CharEnteredHandler(EventInput_CharEntered);
        }


        /// <summary>
        /// Loads graphics content for this screen. This uses the shared ContentManager
        /// provided by the Game class, so the content will remain loaded forever.
        /// Whenever a subsequent MessageBoxScreen tries to load this same content,
        /// it will just get back another reference to the already loaded data.
        /// </summary>
        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;
            gradientTexture = content.Load<Texture2D>("gradient");
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Responds to user input, accepting or cancelling the message box.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;

            // We pass in our ControllingPlayer, which may either be null (to
            // accept input from any player) or a specific index. If we pass a null
            // controlling player, the InputState helper returns to us which player
            // actually provided the input. We pass that through to our Accepted and
            // Cancelled events, so they can tell which player triggered them.
            if (input.IsScoreSelect(ControllingPlayer, out playerIndex))
            {
                AudioEngine.SoundBank.PlayCue("menuBouge");
                // Raise the accepted event, then exit the message box.
                if (nomSauvegarde.Length > 0)
                {
                    Accepted(this, new PlayerIndexEventArgs(playerIndex));
                    ExitScreen();
                }
            }
            else if (input.IsMenuCancel(ControllingPlayer, out playerIndex))
            {
                AudioEngine.SoundBank.PlayCue("menuBouge");
                ExitScreen();
            }

            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.Back) && (nomSauvegarde.Length > 0))
            { nomSauvegarde = nomSauvegarde.Remove(nomSauvegarde.Length - 1); }
        }

        void EventInput_CharEntered(object sender, EventInput.CharacterEventArgs e)
        {
            if (e.Character != '\b' && e.Character != '\r' && e.Character != '\t' && nomSauvegarde.Length < 15 && !(ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.LeftControl)) && !(ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.RightControl)))
                nomSauvegarde += e.Character;
        }

        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            timer += gameTime.ElapsedGameTime.TotalSeconds;

            if ((int)timer != lastIntTimer)
            {
                lastIntTimer++;
                underscore = !underscore;
            }
        }


        /// <summary>
        /// Draws the message box.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            if (mod == 0)
            {
                // Darken down any other screens that were drawn beneath the popup.
                ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);
            }

            // Center the message text in the viewport.
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(Message);
            textSize.Y += font.MeasureString(UsageText).Y;
            Vector2 textPosition = (viewportSize - textSize) / 2;

            // The background includes a border somewhat larger than the text itself.
            const int hPad = 32;
            const int vPad = 16;

            Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - hPad,
                                                          (int)textPosition.Y - vPad,
                                                          (int)textSize.X + hPad * 2,
                                                          (int)textSize.Y + vPad * 2);

            // Fade the popup alpha during transitions.
            Color color = new Color(255, 255, 255, TransitionAlpha);
            Color nameColor = new Color(137, 124, 55, TransitionAlpha);

            spriteBatch.Begin();

            // Draw the background rectangle.
            spriteBatch.Draw(gradientTexture, backgroundRectangle, color);

            // Draw the message box text.
            spriteBatch.DrawString(font, Message, textPosition, color);
            textPosition.Y += font.MeasureString(Message).Y - (font.MeasureString("\n").Y / 2);
            spriteBatch.DrawString(font, nomSauvegarde + (underscore ? "_\n" : "\n"), textPosition, nameColor);
            textPosition.Y += font.MeasureString(nomSauvegarde + 'A').Y;
            spriteBatch.DrawString(font, UsageText, textPosition, color);

            spriteBatch.End();
        }


        #endregion
    }
}
