#region Using Statements
#endregion

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using S = YelloKiller.Properties.Scores;

namespace YelloKiller
{
    class NewHiScore : MessageBoxScreen
    {
        #region Initialization

        string playerName, comingFrom;
        uint Score;
        uint lastIntTimer;
        Texture2D gradientTexture;
        static double timer = 0;
        bool underscore;

        public NewHiScore(uint score, string comingfrom)
            : base(Langue.tr("HiScore"), false)
        {
            comingFrom = comingfrom;
            this.UsageText = Langue.tr("ScoresBox");
            playerName = "";
            lastIntTimer = 0;
            Score = score;
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

        #region Event


        /// <summary>
        /// Event handler for when the user selects ok on the "Hi Score" message box.
        /// </summary>
        void HiScoreAccepted(object sender, PlayerIndexEventArgs e)
        {
            if (Score > S.Default.Score_09)
            {
                if (Score > S.Default.Score_08)
                {
                    if (Score > S.Default.Score_07)
                    {
                        if (Score > S.Default.Score_06)
                        {
                            if (Score > S.Default.Score_05)
                            {
                                if (Score > S.Default.Score_04)
                                {
                                    if (Score > S.Default.Score_03)
                                    {
                                        if (Score > S.Default.Score_02)
                                        {
                                            if (Score > S.Default.Score_01)
                                            {
                                                S.Default.Score_10 = S.Default.Score_09;
                                                S.Default.Name_10 = S.Default.Name_09;
                                                S.Default.Map_10 = S.Default.Map_09;

                                                S.Default.Score_09 = S.Default.Score_08;
                                                S.Default.Name_09 = S.Default.Name_08;
                                                S.Default.Map_09 = S.Default.Map_08;

                                                S.Default.Score_08 = S.Default.Score_07;
                                                S.Default.Name_08 = S.Default.Name_07;
                                                S.Default.Map_08 = S.Default.Map_07;

                                                S.Default.Score_07 = S.Default.Score_06;
                                                S.Default.Name_07 = S.Default.Name_06;
                                                S.Default.Map_07 = S.Default.Map_06;

                                                S.Default.Score_06 = S.Default.Score_05;
                                                S.Default.Name_06 = S.Default.Name_05;
                                                S.Default.Map_06 = S.Default.Map_05;

                                                S.Default.Score_05 = S.Default.Score_04;
                                                S.Default.Name_05 = S.Default.Name_04;
                                                S.Default.Map_05 = S.Default.Map_04;

                                                S.Default.Score_04 = S.Default.Score_03;
                                                S.Default.Name_04 = S.Default.Name_03;
                                                S.Default.Map_04 = S.Default.Map_03;

                                                S.Default.Score_03 = S.Default.Score_02;
                                                S.Default.Name_03 = S.Default.Name_02;
                                                S.Default.Map_03 = S.Default.Map_02;

                                                S.Default.Score_02 = S.Default.Score_01;
                                                S.Default.Name_02 = S.Default.Name_01;
                                                S.Default.Map_02 = S.Default.Map_01;

                                                S.Default.Score_01 = Score;
                                                S.Default.Name_01 = playerName;
                                                S.Default.Map_01 = comingFrom;
                                            }
                                            else
                                            {
                                                S.Default.Score_10 = S.Default.Score_09;
                                                S.Default.Name_10 = S.Default.Name_09;
                                                S.Default.Map_10 = S.Default.Map_09;

                                                S.Default.Score_09 = S.Default.Score_08;
                                                S.Default.Name_09 = S.Default.Name_08;
                                                S.Default.Map_09 = S.Default.Map_08;

                                                S.Default.Score_08 = S.Default.Score_07;
                                                S.Default.Name_08 = S.Default.Name_07;
                                                S.Default.Map_08 = S.Default.Map_07;

                                                S.Default.Score_07 = S.Default.Score_06;
                                                S.Default.Name_07 = S.Default.Name_06;
                                                S.Default.Map_07 = S.Default.Map_06;

                                                S.Default.Score_06 = S.Default.Score_05;
                                                S.Default.Name_06 = S.Default.Name_05;
                                                S.Default.Map_06 = S.Default.Map_05;

                                                S.Default.Score_05 = S.Default.Score_04;
                                                S.Default.Name_05 = S.Default.Name_04;
                                                S.Default.Map_05 = S.Default.Map_04;

                                                S.Default.Score_04 = S.Default.Score_03;
                                                S.Default.Name_04 = S.Default.Name_03;
                                                S.Default.Map_04 = S.Default.Map_03;

                                                S.Default.Score_03 = S.Default.Score_02;
                                                S.Default.Name_03 = S.Default.Name_02;
                                                S.Default.Map_03 = S.Default.Map_02;

                                                S.Default.Score_02 = Score;
                                                S.Default.Name_02 = playerName;
                                                S.Default.Map_02 = comingFrom;
                                            }
                                        }
                                        else
                                        {
                                            S.Default.Score_10 = S.Default.Score_09;
                                            S.Default.Name_10 = S.Default.Name_09;
                                            S.Default.Map_10 = S.Default.Map_09;

                                            S.Default.Score_09 = S.Default.Score_08;
                                            S.Default.Name_09 = S.Default.Name_08;
                                            S.Default.Map_09 = S.Default.Map_08;

                                            S.Default.Score_08 = S.Default.Score_07;
                                            S.Default.Name_08 = S.Default.Name_07;
                                            S.Default.Map_08 = S.Default.Map_07;

                                            S.Default.Score_07 = S.Default.Score_06;
                                            S.Default.Name_07 = S.Default.Name_06;
                                            S.Default.Map_07 = S.Default.Map_06;

                                            S.Default.Score_06 = S.Default.Score_05;
                                            S.Default.Name_06 = S.Default.Name_05;
                                            S.Default.Map_06 = S.Default.Map_05;

                                            S.Default.Score_05 = S.Default.Score_04;
                                            S.Default.Name_05 = S.Default.Name_04;
                                            S.Default.Map_05 = S.Default.Map_04;

                                            S.Default.Score_04 = S.Default.Score_03;
                                            S.Default.Name_04 = S.Default.Name_03;
                                            S.Default.Map_04 = S.Default.Map_03;

                                            S.Default.Score_03 = Score;
                                            S.Default.Name_03 = playerName;
                                            S.Default.Map_03 = comingFrom;
                                        }
                                    }
                                    else
                                    {
                                        S.Default.Score_10 = S.Default.Score_09;
                                        S.Default.Name_10 = S.Default.Name_09;
                                        S.Default.Map_10 = S.Default.Map_09;

                                        S.Default.Score_09 = S.Default.Score_08;
                                        S.Default.Name_09 = S.Default.Name_08;
                                        S.Default.Map_09 = S.Default.Map_08;

                                        S.Default.Score_08 = S.Default.Score_07;
                                        S.Default.Name_08 = S.Default.Name_07;
                                        S.Default.Map_08 = S.Default.Map_07;

                                        S.Default.Score_07 = S.Default.Score_06;
                                        S.Default.Name_07 = S.Default.Name_06;
                                        S.Default.Map_07 = S.Default.Map_06;

                                        S.Default.Score_06 = S.Default.Score_05;
                                        S.Default.Name_06 = S.Default.Name_05;
                                        S.Default.Map_06 = S.Default.Map_05;

                                        S.Default.Score_05 = S.Default.Score_04;
                                        S.Default.Name_05 = S.Default.Name_04;
                                        S.Default.Map_05 = S.Default.Map_04;

                                        S.Default.Score_04 = Score;
                                        S.Default.Name_04 = playerName;
                                        S.Default.Map_04 = comingFrom;
                                    }
                                }
                                else
                                {
                                    S.Default.Score_10 = S.Default.Score_09;
                                    S.Default.Name_10 = S.Default.Name_09;
                                    S.Default.Map_10 = S.Default.Map_09;

                                    S.Default.Score_09 = S.Default.Score_08;
                                    S.Default.Name_09 = S.Default.Name_08;
                                    S.Default.Map_09 = S.Default.Map_08;

                                    S.Default.Score_08 = S.Default.Score_07;
                                    S.Default.Name_08 = S.Default.Name_07;
                                    S.Default.Map_08 = S.Default.Map_07;

                                    S.Default.Score_07 = S.Default.Score_06;
                                    S.Default.Name_07 = S.Default.Name_06;
                                    S.Default.Map_07 = S.Default.Map_06;

                                    S.Default.Score_06 = S.Default.Score_05;
                                    S.Default.Name_06 = S.Default.Name_05;
                                    S.Default.Map_06 = S.Default.Map_05;

                                    S.Default.Score_05 = Score;
                                    S.Default.Name_05 = playerName;
                                    S.Default.Map_05 = comingFrom;
                                }
                            }
                            else
                            {
                                S.Default.Score_10 = S.Default.Score_09;
                                S.Default.Name_10 = S.Default.Name_09;
                                S.Default.Map_10 = S.Default.Map_09;

                                S.Default.Score_09 = S.Default.Score_08;
                                S.Default.Name_09 = S.Default.Name_08;
                                S.Default.Map_09 = S.Default.Map_08;

                                S.Default.Score_08 = S.Default.Score_07;
                                S.Default.Name_08 = S.Default.Name_07;
                                S.Default.Map_08 = S.Default.Map_07;

                                S.Default.Score_07 = S.Default.Score_06;
                                S.Default.Name_07 = S.Default.Name_06;
                                S.Default.Map_07 = S.Default.Map_06;

                                S.Default.Score_06 = Score;
                                S.Default.Name_06 = playerName;
                                S.Default.Map_06 = comingFrom;
                            }
                        }
                        else
                        {
                            S.Default.Score_10 = S.Default.Score_09;
                            S.Default.Name_10 = S.Default.Name_09;
                            S.Default.Map_10 = S.Default.Map_09;

                            S.Default.Score_09 = S.Default.Score_08;
                            S.Default.Name_09 = S.Default.Name_08;
                            S.Default.Map_09 = S.Default.Map_08;

                            S.Default.Score_08 = S.Default.Score_07;
                            S.Default.Name_08 = S.Default.Name_07;
                            S.Default.Map_08 = S.Default.Map_07;

                            S.Default.Score_07 = Score;
                            S.Default.Name_07 = playerName;
                            S.Default.Map_07 = comingFrom;
                        }
                    }
                    else
                    {
                        S.Default.Score_10 = S.Default.Score_09;
                        S.Default.Name_10 = S.Default.Name_09;
                        S.Default.Map_10 = S.Default.Map_09;

                        S.Default.Score_09 = S.Default.Score_08;
                        S.Default.Name_09 = S.Default.Name_08;
                        S.Default.Map_09 = S.Default.Map_08;

                        S.Default.Score_08 = Score;
                        S.Default.Name_08 = playerName;
                        S.Default.Map_08 = comingFrom;
                    }
                }
                else
                {
                    S.Default.Score_10 = S.Default.Score_09;
                    S.Default.Name_10 = S.Default.Name_09;
                    S.Default.Map_10 = S.Default.Map_09;

                    S.Default.Score_09 = Score;
                    S.Default.Name_09 = playerName;
                    S.Default.Map_09 = comingFrom;
                }
            }
            else
            {
                S.Default.Score_10 = Score;
                S.Default.Name_10 = playerName;
                S.Default.Map_10 = comingFrom;
            }

            S.Default.Save();
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
                if (playerName.Length > 0)
                {
                    HiScoreAccepted(this, new PlayerIndexEventArgs(playerIndex));
                    ExitScreen();
                }
            }
            else if (input.IsMenuCancel(ControllingPlayer, out playerIndex))
            {
                AudioEngine.SoundBank.PlayCue("menuBouge");
                ExitScreen();
            }

            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.Back) && (playerName.Length > 0))
            {
                playerName = playerName.Remove(playerName.Length - 1);
            }
        }

        void EventInput_CharEntered(object sender, EventInput.CharacterEventArgs e)
        {
            if (e.Character != '\b' && e.Character != '\r' && e.Character != '\t' && playerName.Length < 15 && !(ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.LeftControl)) && !(ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.RightControl)))
                playerName += e.Character;
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

            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            // Center the message text in the viewport.
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(Message);
            textSize.Y += font.MeasureString(UsageText).Y;
            Vector2 textPosition = (viewportSize - textSize) / 2;
            textPosition.Y -= textSize.Y;

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
            try
            {
                spriteBatch.DrawString(font, playerName + (underscore ? "_\n" : "\n"), textPosition, nameColor);
            }
            catch (ArgumentException)
            {
                playerName = playerName.Remove(playerName.Length - 1);
            }
            textPosition.Y += font.MeasureString(playerName + 'A').Y;
            spriteBatch.DrawString(font, UsageText, textPosition, color);

            spriteBatch.End();
        }


        #endregion
    }
}
