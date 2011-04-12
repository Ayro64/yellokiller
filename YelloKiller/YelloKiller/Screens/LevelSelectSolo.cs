﻿using System;
using System.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    class LevelSelectSolo : GameScreen
    {
        #region Fields

        List<MenuEntry> levels = new List<MenuEntry>();
        List<Carte> miniCartes = new List<Carte>();
        MenuEntry abortMenuEntry;
        YellokillerGame game;
        MoteurAudio moteurAudio;

        string menuTitle = Langue.tr("Solo"), level = Langue.tr("Level");
        int selectedEntry = 0;
        ContentManager content;
        Texture2D levelSelectBkground, blankTexture;
        Color Color;


        #endregion

        #region Initialization

        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public LevelSelectSolo(YellokillerGame game)
        {
            this.game = game;
            string[] fileEntries = Directory.GetFiles(System.Windows.Forms.Application.StartupPath);
            foreach (string str in fileEntries)
            {
                if (str.Substring(str.Length - 3) == "txt" && str[str.Length - 10] == 'S')
                {
                    MenuEntry menuEntry = new MenuEntry(str.Substring(str.Length - 10, 6));
                    menuEntry.Selected += LevelMenuEntrySelected;
                    levels.Add(menuEntry);

                    Carte map = new Carte(new Vector2(Taille_Map.LARGEUR_MAP, Taille_Map.HAUTEUR_MAP));
                    map.OuvrirCarte(str.Substring(str.Length - 10));
                    miniCartes.Add(map);
                }
            }
            //Durée de la transition.
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            // Create our menu entries.
            abortMenuEntry = new MenuEntry(Langue.tr("BckToMenu"));

            // Hook up menu event handlers.
            abortMenuEntry.Selected += AbortMenuEntrySelected;

            // Add entries to the menu.
            levels.Add(abortMenuEntry);

            moteurAudio = new MoteurAudio();
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
            levelSelectBkground = content.Load<Texture2D>("LevelSelection");

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
            // Move to the left menu entry?
            if (input.IsMenuLeft(ControllingPlayer))
            {
                moteurAudio.SoundBank.PlayCue("sonMenuBoutton");
                if (selectedEntry > 0)
                    selectedEntry--;
            }

            // Move to the right menu entry?
            if (input.IsMenuRight(ControllingPlayer))
            {
                moteurAudio.SoundBank.PlayCue("sonMenuBoutton");
                if (selectedEntry < (levels.Count - 1))
                    selectedEntry++;
            }

            // Move to the up menu entry?
            if (input.IsMenuUp(ControllingPlayer))
            {
                moteurAudio.SoundBank.PlayCue("sonMenuBoutton");
                if (selectedEntry > 2)
                    selectedEntry -= 3;
                else
                    selectedEntry = levels.Count - 1;
            }

            // Move to the down menu entry?
            if (input.IsMenuDown(ControllingPlayer))
            {
                moteurAudio.SoundBank.PlayCue("sonMenuBoutton");
                if (selectedEntry < levels.Count - 3)
                    selectedEntry += 3;
                else if (selectedEntry < levels.Count - 1)
                    selectedEntry = levels.Count - 1;
                else
                    selectedEntry = selectedEntry % 3;

            }

            // Accept or cancel the menu? We pass in our ControllingPlayer, which may
            // either be null (to accept input from any player) or a specific index.
            // If we pass a null controlling player, the InputState helper returns to
            // us which player actually provided the input. We pass that through to
            // OnSelectEntry and OnCancel, so they can tell which player triggered them.
            PlayerIndex playerIndex;

            if (input.IsMenuSelect(ControllingPlayer, out playerIndex))
            {
                moteurAudio.SoundBank.PlayCue("menuBouge");
                OnSelectEntry(selectedEntry, playerIndex);
            }
        }

        /// <summary>
        /// Handler for when the user has chosen a menu entry.
        /// </summary>
        protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
        {
            levels[entryIndex].OnSelectEntry(playerIndex);
        }

        /// <summary>
        /// Event handler for when the Level X menu entry is selected.
        /// </summary>
        void LevelMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            MenuEntry selectedLevel = (MenuEntry)sender;
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen(selectedLevel.Text + ".txt", game));
        }

        /// <summary>
        /// This uses the loading screen to transition from the game over screen back to the main menu screen.
        /// </summary>

        void AbortMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                           new MainMenuScreen(game));
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
            for (int i = 0; i < levels.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);
                levels[i].Update(this, isSelected, gameTime);
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        /// <summary>
        /// Draws the Level Select screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            Color = new Color(255, 0, 0, TransitionAlpha);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            Vector2 position = new Vector2(130, 300);

            byte fade = TransitionAlpha;

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            if (ScreenState == ScreenState.TransitionOn)
                position.X -= transitionOffset * 256;
            else
                position.X += transitionOffset * 512;

            spriteBatch.Begin();

            spriteBatch.Draw(levelSelectBkground, fullscreen,
                             new Color(fade, fade, fade));

            // Draw each menu entry in turn.
            for (int i = 0; i < levels.Count - 1; i++)
            {
                MenuEntry menuEntry = levels[i];

                bool isSelected = IsActive && (i == selectedEntry);

                menuEntry.Draw(this, position, isSelected, gameTime, Color.Black);

                // Miniatures
                miniCartes[i].DrawInMenu(spriteBatch, content, new Vector2(position.X - 45, position.Y - 150));

                if ((i % 3 == 0) || (i % 3 == 1))
                    position.X += 250;
                else
                {
                    position.Y += (menuEntry.GetHeight(this) + 150);
                    position.X -= 500;
                }
            }

            // Bouton Retour
            if ((levels.Count % 3 == 1) || (levels.Count % 3 == 2))
                position.X -= ((levels.Count - 1) % 3) * 250;
            position.X -= 30;
            position.Y = 580;
            abortMenuEntry.Draw(this, position, (IsActive && (levels.Count - 1 == selectedEntry)), gameTime, Color.Black);

            // Draw the menu title.
            Vector2 titlePosition = new Vector2(450, 100);
            Vector2 titleOrigin = font.MeasureString(menuTitle) / 2;
            Color titleColor = new Color(0, 0, 0, TransitionAlpha);
            float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }

        #endregion
    }
}
