using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    class GameWin : GameScreen
    {
        #region Fields

        List<MenuEntry> menuEntries = new List<MenuEntry>();
        MenuEntry nextMenuEntry, restartMenuEntry, abortMenuEntry;
        int selectedEntry = 0, comingfrom;
        ContentManager content;
        Texture2D winTexture, blankTexture;
        string WinMessage = Langue.tr("WinMsg");

        Color Color;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public GameWin(int comingfrom)
        {
            this.comingfrom = comingfrom;

            //Durée de la transition.
            TransitionOnTime = TimeSpan.FromSeconds(1.2);
            TransitionOffTime = TimeSpan.FromSeconds(1.2);


            // Create our menu entries.
            nextMenuEntry = new MenuEntry(Langue.tr("WiNext"));
            restartMenuEntry = new MenuEntry(Langue.tr("WiRetry"));
            abortMenuEntry = new MenuEntry(Langue.tr("BckToMenu"));

            // Hook up menu event handlers.
            nextMenuEntry.Selected += NextMenuEntrySelected;
            restartMenuEntry.Selected += RestartMenuEntrySelected;
            abortMenuEntry.Selected += AbortMenuEntrySelected;

            // Add entries to the menu.
            menuEntries.Add(nextMenuEntry);
            menuEntries.Add(restartMenuEntry);
            menuEntries.Add(abortMenuEntry);
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
            winTexture = content.Load<Texture2D>("WinTex");

            //Carré noir.
            blankTexture = content.Load<Texture2D>("blank");
        }

        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Responds to user input, changing the selected entry and accepting
        /// or cancelling the menu.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            // Move to the previous menu entry?
            if (input.IsMenuLeft(ControllingPlayer))
            {
                selectedEntry--;

                if (selectedEntry < 0)
                    selectedEntry = menuEntries.Count - 1;
            }

            // Move to the next menu entry?
            if (input.IsMenuRight(ControllingPlayer))
            {
                selectedEntry++;

                if (selectedEntry >= menuEntries.Count)
                    selectedEntry = 0;
            }

            // Accept or cancel the menu? We pass in our ControllingPlayer, which may
            // either be null (to accept input from any player) or a specific index.
            // If we pass a null controlling player, the InputState helper returns to
            // us which player actually provided the input. We pass that through to
            // OnSelectEntry and OnCancel, so they can tell which player triggered them.
            PlayerIndex playerIndex;

            if (input.IsMenuSelect(ControllingPlayer, out playerIndex))
                OnSelectEntry(selectedEntry, playerIndex);
        }

        /// <summary>
        /// Handler for when the user has chosen a menu entry.
        /// </summary>
        protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
        {
            menuEntries[entryIndex].OnSelectEntry(playerIndex);
        }

        /// <summary>
        /// Event handler for when the Restart menu entry is selected.
        /// </summary>
        void NextMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (comingfrom == 1)
                LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreenSolo());
            else
                LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreenCoop());
        }

        /// <summary>
        /// Event handler for when the Restart menu entry is selected.
        /// </summary>
        void RestartMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (comingfrom == 1)
                LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreenSolo());
            else
                LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreenCoop());
        }

        /// <summary>
        /// This uses the loading screen to transition from the game over screen back to the main menu screen.
        /// </summary>

        void AbortMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                           new MainMenuScreen());
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Updates the menu.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            // Update each nested MenuEntry object.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);
                menuEntries[i].Update(this, isSelected, gameTime);
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        /// <summary>
        /// Draws the Win screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            Color = new Color(255, 0, 0, TransitionAlpha);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            byte fade = TransitionAlpha;

            //Entrées Menu
            Vector2 positionL = new Vector2(viewport.Width / 2 - ((font.MeasureString(nextMenuEntry.Text).X + font.MeasureString(restartMenuEntry.Text).X + font.MeasureString(abortMenuEntry.Text).X + 60) / 2), 610);
            Vector2 positionM = new Vector2(positionL.X + font.MeasureString(nextMenuEntry.Text).X + 30, 610);
            Vector2 positionR = new Vector2(positionM.X + font.MeasureString(restartMenuEntry.Text).X + 30, 610);
            Vector2 WinPosition = new Vector2(viewport.Width / 2, 65);

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            if (ScreenState == ScreenState.TransitionOn)
            {
                positionL.X -= transitionOffset * 256;
                positionM.Y += transitionOffset * 100;
                positionR.X += transitionOffset * 256;
            }
            else
            {
                positionL.X -= transitionOffset * 512;
                positionM.Y += transitionOffset * 100;
                positionR.X += transitionOffset * 512;
            }

            spriteBatch.Begin();

            spriteBatch.Draw(winTexture, fullscreen,
                             new Color(fade, fade, fade));

            // Rectangle noir
            spriteBatch.Draw(blankTexture,
                             new Rectangle(220, 530, 450, 110),
                             new Color(0, 0, 0, (byte)(fade * 2 / 3)));
            spriteBatch.Draw(blankTexture,
                             new Rectangle((int)WinPosition.X - (int)(font.MeasureString(WinMessage).X / 1.4f), (int)(WinPosition.Y - font.MeasureString(WinMessage).Y), (int)(font.MeasureString(WinMessage).X * 1.4f), 45),
                             new Color(0, 0, 0, (byte)(fade * 2 / 3)));


            // Draw each menu entry in turn.

            bool isSelected = IsActive && (0 == selectedEntry);
            nextMenuEntry.Draw(this, positionL, isSelected, gameTime, Color);
            isSelected = IsActive && (1 == selectedEntry);
            restartMenuEntry.Draw(this, positionM, isSelected, gameTime, Color);
            isSelected = IsActive && (2 == selectedEntry);
            abortMenuEntry.Draw(this, positionR, isSelected, gameTime, Color);


            // Draw the menu title.
            
            Vector2 WinOrigin = font.MeasureString(WinMessage) / 2;
            float WinScale = 1.25f;

            WinPosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, WinMessage, WinPosition, Color, 0,
                                   WinOrigin, WinScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }

        #endregion

    }
}
