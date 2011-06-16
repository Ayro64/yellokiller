﻿using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace YelloKiller
{
    class LevelSelectMulti : GameScreen
    {
        #region Fields

        List<MenuEntry> levels = new List<MenuEntry>();
        List<Carte> miniCartes = new List<Carte>();
        MenuEntry abortMenuEntry;
        YellokillerGame game;

        string menuTitle = Langue.tr("Multi"), level = Langue.tr("Level");
        int selectedEntry = 0;
        ContentManager content;
        Texture2D levelSelectBkground, blankTexture, padlock;
        Color Color;
        Color titleColor;

        bool[] unlocked;
        List<string> storyMissions;
        List<string> fileEntries;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public LevelSelectMulti(YellokillerGame game)
        {
            this.game = game; 

            storyMissions = new List<string>();
            fileEntries = new List<string>();
            foreach (string file in Directory.GetFiles(System.Windows.Forms.Application.StartupPath + "\\Story", "*.coop"))
            {
                storyMissions.Add(file);
                fileEntries.Add(file);
            }

            foreach (string file in Directory.GetFiles(System.Windows.Forms.Application.StartupPath + "\\Levels", "*.coop"))
                fileEntries.Add(file);

            unlocked = new bool[6];
            unlocked[0] = Properties.Unlocked.Default.C1;
            unlocked[1] = Properties.Unlocked.Default.C2;
            unlocked[2] = Properties.Unlocked.Default.C3;
            unlocked[3] = Properties.Unlocked.Default.C4;
            unlocked[4] = Properties.Unlocked.Default.C5;
            unlocked[5] = Properties.Unlocked.Default.C6;

            foreach (string str in fileEntries)
            {
                string entryName = str.Substring(str.LastIndexOf('\\') + 1);

                Carte map = new Carte(new Vector2(Taille_Map.LARGEUR_MAP, Taille_Map.HAUTEUR_MAP));
                map.OuvrirCartePourMenu(entryName);
                miniCartes.Add(map);

                entryName = entryName.Substring(0, entryName.LastIndexOf('.'));
                MenuEntry menuEntry = new MenuEntry(entryName);

                if (storyMissions.Contains(str))
                    menuEntry.IsLocked = !unlocked[(int.Parse(entryName[0].ToString())) - 1];

                menuEntry.Selected += LevelMenuEntrySelected;
                levels.Add(menuEntry);

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
            levelSelectBkground = content.Load<Texture2D>("Level Select Multi");

            //Carré noir.
            blankTexture = content.Load<Texture2D>("blank");

            // Cadenas.
            padlock = content.Load<Texture2D>("cadenas");
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
                AudioEngine.SoundBank.PlayCue("sonMenuBoutton");
                if (selectedEntry > 0)
                    selectedEntry--;
            }

            // Move to the right menu entry?
            if (input.IsMenuRight(ControllingPlayer))
            {
                AudioEngine.SoundBank.PlayCue("sonMenuBoutton");
                if (selectedEntry < (levels.Count - 1))
                    selectedEntry++;
            }

            // Move to the up menu entry?
            if (input.IsMenuUp(ControllingPlayer))
            {
                AudioEngine.SoundBank.PlayCue("sonMenuBoutton");
                if (selectedEntry == levels.Count - 1)
                    selectedEntry -= 1;
                else if (selectedEntry > 3)
                    selectedEntry -= 4;
                else
                    selectedEntry = levels.Count - 1;
            }

            // Move to the down menu entry?
            if (input.IsMenuDown(ControllingPlayer))
            {
                AudioEngine.SoundBank.PlayCue("sonMenuBoutton");
                if (selectedEntry < levels.Count - 4)
                    selectedEntry += 4;
                else if (selectedEntry < levels.Count - 1)
                    selectedEntry = levels.Count - 1;
                else
                    selectedEntry = 0;
            }

            // Accept or cancel the menu? We pass in our ControllingPlayer, which may
            // either be null (to accept input from any player) or a specific index.
            // If we pass a null controlling player, the InputState helper returns to
            // us which player actually provided the input. We pass that through to
            // OnSelectEntry and OnCancel, so they can tell which player triggered them.
            PlayerIndex playerIndex;

            if (input.IsMenuSelect(ControllingPlayer, out playerIndex))
            {
                OnSelectEntry(selectedEntry, playerIndex);
                AudioEngine.SoundBank.PlayCue("menuBouge");
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
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen(selectedLevel.Text + ".coop", game, 0));
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
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Update each nested MenuEntry object.
            for (int i = 0; i < levels.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);
                levels[i].Update(this, isSelected, gameTime);
            }
        }

        /// <summary>
        /// Draws the Level Select screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            Color = new Color(255, 0, 0, TransitionAlpha);
            titleColor = new Color(192, 192, 192, TransitionAlpha);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            Vector2 position = new Vector2(viewport.Width / 8, viewport.Height / 2);

            byte fade = TransitionAlpha;

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            /*
            if (ScreenState == ScreenState.TransitionOn)
                position.X -= transitionOffset * 256;
            else
                position.X += transitionOffset * 512;
            */

            spriteBatch.Begin();

            spriteBatch.Draw(levelSelectBkground, fullscreen,
                             new Color(fade, fade, fade));

            // Draw each menu entry in turn.
            for (int i = 0; i < levels.Count - 1; i++)
            {
                MenuEntry menuEntry = levels[i];

                bool isSelected = IsActive && (i == selectedEntry);

                menuEntry.CDraw(this, position, isSelected, gameTime, titleColor, TransitionPosition);

                // Miniatures
                miniCartes[i].DrawInMenu(spriteBatch, content, new Vector2(position.X - 120, position.Y - 200));

                if (menuEntry.IsLocked)
                    spriteBatch.Draw(padlock, new Vector2(position.X - 120, position.Y - 200), Color.White);

                if ((i % 4 == 0) || (i % 4 == 1) || (i % 4 == 2))
                    position.X += 250;
                else
                {
                    position.Y += (menuEntry.GetHeight(this) + 200);
                    position.X -= 750;
                }
            }

            // Bouton Retour
            position.X -= (((levels.Count - 1) % 4) * 250) + 30;
            position.Y = viewport.Height - 50;
            abortMenuEntry.Draw(this, position, (IsActive && (levels.Count - 1 == selectedEntry)), gameTime, titleColor, TransitionPosition);

            // Draw the menu title.
            Vector2 titlePosition = new Vector2(viewport.Width / 2, 100);
            Vector2 titleOrigin = font.MeasureString(menuTitle) / 2;
            float titleScale = 1.25f;
            float titleSize = font.MeasureString(menuTitle).X;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.Draw(blankTexture,
                             new Rectangle((int)((viewport.Width / 2) - ((titleSize * 1.25f / 2) + 10)), 60, (int)(titleSize * 1.25f) + 20, 80),
                             new Color(0, 0, 0, (byte)(TransitionAlpha * 2 / 3)));

            spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }

        #endregion
    }
}
