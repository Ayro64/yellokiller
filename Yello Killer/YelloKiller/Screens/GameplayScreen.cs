#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace Yellokiller
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteFont gameFont;
        SpriteBatch spriteBatch;

        Hero1 hero1;
        Hero2 hero2;
        Map carte;

        MediaLibrary sampleMediaLibrary;
        Random rand;
        int i, j;

        KeyboardState keyboardState, lastKeyboardState;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            sampleMediaLibrary = new MediaLibrary();
            rand = new Random();

            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            carte = new Map("map.txt");
            hero1 = new Hero1(new Vector2(336, 10), new Rectangle(25, 133, 16, 25));
            hero2 = new Hero2(new Vector2(346, 10), new Rectangle(25, 133, 16, 25));

            i = rand.Next(0, sampleMediaLibrary.Albums.Count - 1);
            j = rand.Next(0, sampleMediaLibrary.Albums[i].Songs.Count);
            MediaPlayer.Play(sampleMediaLibrary.Albums[i].Songs[j]);



        }


        protected void Initialize()
        {
        }

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            spriteBatch = ScreenManager.SpriteBatch;

            gameFont = content.Load<SpriteFont>("courier");
            hero1.LoadContent(content, 2);
            hero2.LoadContent(content, 2);
            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion



        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (IsActive)
            {
                hero1.Update(gameTime, carte.map, carte.hauteurMap, carte.largeurMap);
                hero2.Update(gameTime, carte.map, carte.hauteurMap, carte.largeurMap);
                base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            }

            if (MediaPlayer.State == MediaState.Stopped)
            {
                i = rand.Next(0, sampleMediaLibrary.Albums.Count - 1);
                j = rand.Next(0, sampleMediaLibrary.Albums[i].Songs.Count);
                MediaPlayer.Play(sampleMediaLibrary.Albums[i].Songs[j]);
            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;


            lastKeyboardState = keyboardState;
            keyboardState = input.CurrentKeyboardStates[playerIndex];

            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new Pausebckground(), ControllingPlayer, true);
                ScreenManager.AddScreen(new PauseMenuScreen(0, 0), ControllingPlayer, true);
            }
            else
            {
                // Otherwise move the player position.
                Vector2 movement = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.Left))
                    movement.X--;

                if (keyboardState.IsKeyDown(Keys.Right))
                    movement.X++;

                if (keyboardState.IsKeyDown(Keys.Up))
                    movement.Y--;

                if (keyboardState.IsKeyDown(Keys.Down))
                    movement.Y++;

                if (keyboardState.IsKeyDown(Keys.G))
                {
                    LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameOverScreen());
                }

                if (keyboardState.IsKeyDown(Keys.K))
                {
                    i = rand.Next(0, sampleMediaLibrary.Albums.Count - 1);
                    j = rand.Next(0, sampleMediaLibrary.Albums[i].Songs.Count);
                    MediaPlayer.Play(sampleMediaLibrary.Albums[i].Songs[j]);
                }
                if (keyboardState.IsKeyDown(Keys.L) && lastKeyboardState.IsKeyUp(Keys.L))
                {
                    if (MediaPlayer.State == MediaState.Playing)
                        MediaPlayer.Pause();
                    else
                        MediaPlayer.Resume();
                }
                
            }
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.DarkOrchid, 0, 0);

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0)
                ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);

            spriteBatch.Begin();

            carte.Draw(spriteBatch, content);
            hero1.Draw(spriteBatch);
            hero2.Draw(spriteBatch);
            spriteBatch.DrawString(gameFont, sampleMediaLibrary.Albums[i].Songs[j].Artist + " - " + sampleMediaLibrary.Albums[i].Songs[j], new Vector2(10, 10), Color.Red);

            spriteBatch.End();
            base.Draw(gameTime);
        }


        #endregion
    }
}
