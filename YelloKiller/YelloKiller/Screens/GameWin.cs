using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using S = YelloKiller.Properties.Scores;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace YelloKiller
{
    class GameWin : GameScreen
    {
        #region Fields

        List<MenuEntry> menuEntries = new List<MenuEntry>();
        MenuEntry nextMenuEntry;
        MenuEntry restartMenuEntry;
        MenuEntry abortMenuEntry;
        int selectedEntry = 0;
        uint salaire, deaths, restart;
        ContentManager content;
        Texture2D winTexture, blankTexture, scroll, kane;
        YellokillerGame game;
        string WinMessage, comingfrom, baseSalary, time, killed, retries, score, penalties, next;
        double temps;
        bool HiScore = false, ShowHiScore = false, soundPlayed = false;

        Color Color;
        Color EntriesColor;

        Cue Fanfare;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public GameWin(string comingfrom, uint salaire, double levelTime, uint deaths, uint retries, YellokillerGame game)
        {
            this.game = game;
            this.comingfrom = comingfrom;
            this.salaire = salaire;
            this.deaths = deaths;
            this.restart = retries;

            temps = levelTime;

            baseSalary = Langue.tr("BaseSalary") + salaire;
            penalties = Langue.tr("Penalties");
            WinMessage = Langue.tr("WinMsg");
            time = Langue.tr("Time") + Temps.Conversion(levelTime) + " --> " + (int)(-levelTime * 100);
            killed = Langue.tr("Killed") + deaths + " x 1 000 --> " + (-deaths * 1000);
            this.retries = Langue.tr("Retries") + retries + " x 10 000 --> " + (-retries * 10000);

            this.salaire -= (deaths * 1000) + (uint)(levelTime * 100) + (retries * 10000);
            if (this.salaire < 0)
                this.salaire = 0;
            score = Langue.tr("Score") + this.salaire;

            string[] fileEntries = LoadMapMenuScreen.ConcatenerTableaux(Directory.GetFiles(System.Windows.Forms.Application.StartupPath + "\\Story", "*." + comingfrom.Substring(comingfrom.Length - 4)), Directory.GetFiles(System.Windows.Forms.Application.StartupPath + "\\Levels", "*." + comingfrom.Substring(comingfrom.Length - 4)));
            
            for (int i = 0; i < fileEntries.Length; i++)
            {
                if (fileEntries[i].Substring(fileEntries[i].LastIndexOf('\\') + 1) == comingfrom)
                    next = fileEntries[i + 1].Substring(fileEntries[i + 1].LastIndexOf('\\') + 1);
            }


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

            if (this.salaire > S.Default.Score_10)
            {
                HiScore = true;
            }
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
            // Kanji argent.
            kane = content.Load<Texture2D>("Kane");
            // Parchemin
            scroll = content.Load<Texture2D>("ScoresScrollFil");
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
                AudioEngine.SoundBank.PlayCue("sonMenuBoutton");
                selectedEntry--;

                if (selectedEntry < 0)
                    selectedEntry = menuEntries.Count - 1;
            }

            // Move to the next menu entry?
            if (input.IsMenuRight(ControllingPlayer))
            {
                AudioEngine.SoundBank.PlayCue("sonMenuBoutton");
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
            Fanfare.Stop(AudioStopOptions.Immediate);
            if (File.Exists("checkTemp.tmp"))
                File.Delete("checkTemp.tmp");
            menuEntries[entryIndex].OnSelectEntry(playerIndex);
        }

        /// <summary>
        /// Event handler for when the Restart menu entry is selected.
        /// </summary>
        void NextMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen(next, game, 0));
        }

        /// <summary>
        /// Event handler for when the Restart menu entry is selected.
        /// </summary>
        void RestartMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen(comingfrom, game, 0));
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

            if (!soundPlayed)
            {
                Fanfare = AudioEngine.SoundBank.GetCue("11 Fanfare");
                Fanfare.Play();
                soundPlayed = true;
            }

            if (ScreenState == ScreenState.Active)
                ShowHiScore = true;
            if (ShowHiScore && HiScore)
            {
                ScreenManager.AddScreen(new NewHiScore(salaire, comingfrom.Substring(0, comingfrom.Length - 5)), ControllingPlayer);
                HiScore = false;
            }

            // Update each nested MenuEntry object.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);
                menuEntries[i].Update(this, isSelected, gameTime);
            }
        }

        /// <summary>
        /// Draws the Win screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            Color = new Color(0, 0, 0, TransitionAlpha);
            EntriesColor = new Color(255, 0, 0, TransitionAlpha);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            byte fade = TransitionAlpha;

            //Entrées Menu
            Vector2 positionL = new Vector2(viewport.Width / 2 - ((font.MeasureString(nextMenuEntry.Text).X + font.MeasureString(restartMenuEntry.Text).X + font.MeasureString(abortMenuEntry.Text).X + 60) / 2), viewport.Height - 45);
            Vector2 positionM = new Vector2(positionL.X + font.MeasureString(nextMenuEntry.Text).X + 30, viewport.Height - 45);
            Vector2 positionR = new Vector2(positionM.X + font.MeasureString(restartMenuEntry.Text).X + 30, viewport.Height - 45);
            Vector2 WinPosition = new Vector2(viewport.Width / 2, 85);
            Vector2 ScoresPosition = new Vector2((int)(viewport.Width - (font.MeasureString(baseSalary).X + 60)) / 2, 160);

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            /*

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
            
            */

            spriteBatch.Begin();

            spriteBatch.Draw(winTexture, fullscreen,
                             new Color(fade, fade, fade));

            spriteBatch.Draw(scroll,
                             new Rectangle((int)(viewport.Width - ((font.MeasureString(baseSalary).X * 2.5f) + 30)) / 2, 40, (int)(font.MeasureString(baseSalary).X * 2.7f + 30), (int)(viewport.Height / 1.4f)),
                             new Color(fade, fade, fade));

            // Draw each menu entry in turn.

            bool isSelected = IsActive && (0 == selectedEntry);
            nextMenuEntry.Draw(this, positionL, isSelected, gameTime, EntriesColor, TransitionPosition);
            isSelected = IsActive && (1 == selectedEntry);
            restartMenuEntry.Draw(this, positionM, isSelected, gameTime, EntriesColor, TransitionPosition);
            isSelected = IsActive && (2 == selectedEntry);
            abortMenuEntry.Draw(this, positionR, isSelected, gameTime, EntriesColor, TransitionPosition);


            // Draw the menu title.

            Vector2 WinOrigin = font.MeasureString(WinMessage) / (2 * 1.25f);
            Vector2 SalOrigin = new Vector2((font.MeasureString(baseSalary).X + 30) / 4, font.MeasureString(baseSalary).Y) / 2;
            Vector2 PenOrigin = new Vector2(font.MeasureString(penalties).X / 4, font.MeasureString(penalties).Y) / 2;
            Vector2 ScOrigin = new Vector2(0, 0);

            float WinScale = 1.25f;
            float SalScale = 1.75f;
            float Scorale = 2f;

            WinPosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, WinMessage, WinPosition, Color, 0,
                                   WinOrigin, WinScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, baseSalary, ScoresPosition, Color, 0,
                                   SalOrigin, SalScale, SpriteEffects.None, 0);
            spriteBatch.Draw(kane, new Rectangle((int)(ScoresPosition.X + (font.MeasureString(baseSalary).X * 1.525f)), (int)ScoresPosition.Y - 15, 30, 20), Color);
            ScoresPosition.Y += 55;
            ScoresPosition.X -= 15;
            spriteBatch.DrawString(font, penalties, ScoresPosition, Color, 0,
                                   PenOrigin, WinScale, SpriteEffects.None, 0);
            ScoresPosition.Y += 40;
            ScoresPosition.X -= 10;
            spriteBatch.DrawString(font, time, ScoresPosition, Color);
            spriteBatch.Draw(kane, new Rectangle((int)(ScoresPosition.X + font.MeasureString(time).X * 1.05f), (int)ScoresPosition.Y, 20, 17), Color);
            ScoresPosition.Y += 50;
            spriteBatch.DrawString(font, killed, ScoresPosition, Color);
            spriteBatch.Draw(kane, new Rectangle((int)(ScoresPosition.X + (font.MeasureString(killed).X) * 1.025f), (int)ScoresPosition.Y, 20, 17), Color);
            ScoresPosition.Y += 50;
            spriteBatch.DrawString(font, retries, ScoresPosition, Color);
            spriteBatch.Draw(kane, new Rectangle((int)(ScoresPosition.X + (font.MeasureString(retries).X) * 1.05f), (int)ScoresPosition.Y, 20, 17), Color);
            ScoresPosition.Y += 60;
            ScoresPosition.X += 40;
            spriteBatch.DrawString(font, score, ScoresPosition, Color, 0,
                                   ScOrigin, Scorale, SpriteEffects.None, 0);
            spriteBatch.Draw(kane, new Rectangle((int)(ScoresPosition.X + (font.MeasureString(score).X * 2.025f)), (int)ScoresPosition.Y + 5, 37, 27), Color);

            spriteBatch.End();
        }

        #endregion

    }
}
