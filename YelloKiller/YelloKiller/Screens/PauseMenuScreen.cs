#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;
#endregion

namespace YelloKiller
{
    /// <summary>
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    class PauseMenuScreen : MenuScreen
    {
        #region Fields

        int mod;
        uint lan;
        YellokillerGame game;
        public event EventHandler<PlayerIndexEventArgs> SaveMapMenuEntrySelected;
        MenuEntry resumeGameMenuEntry;
        MenuEntry saveMapMenuEntry;
        MenuEntry optionsGameMenuEntry;
        MenuEntry quitGameMenuEntry;
        MenuEntry loadMapMenuEntry;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public PauseMenuScreen(int comingfrom, int mode, YellokillerGame game)
            : base(Langue.tr("PauseTitle"))
        {
            this.game = game;
            //Conserve la sélection
            selectedEntry = comingfrom;

            mod = mode;
            lan = Properties.Settings.Default.Language;

            // Create our menu entries.
            optionsGameMenuEntry = new MenuEntry(Langue.tr("Options"));


            if (mode == 2)
            {
                resumeGameMenuEntry = new MenuEntry(Langue.tr("PausEditRes"));
                quitGameMenuEntry = new MenuEntry(Langue.tr("PausEditQuit"));
            }
            else
            {
                resumeGameMenuEntry = new MenuEntry(Langue.tr("PausGameRes"));
                quitGameMenuEntry = new MenuEntry(Langue.tr("PausGameQuit"));
            }

            // Hook up menu event handlers.
            resumeGameMenuEntry.Selected += OnCancel;
            optionsGameMenuEntry.Selected += OptionsMenuEntrySelected;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(resumeGameMenuEntry);

            if (mode == 2)
            {
                saveMapMenuEntry = new MenuEntry(Langue.tr("PausEditSave"));
                loadMapMenuEntry = new MenuEntry(Langue.tr("PausEditLoad"));
                loadMapMenuEntry.Selected += LoadMapMenuEntrySelected;
                MenuEntries.Add(loadMapMenuEntry);
            }

            if (mode != 2)
            {
                MenuEntries.Add(optionsGameMenuEntry);
                MenuEntries.Add(quitGameMenuEntry);
            }
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            string message;

            if (mod == 2)
                message = Langue.tr("EditMsgBox");

            else
                message = Langue.tr("GameMsgBox");

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        /// <summary>
        /// Event handler for when the Load Map menu entry is selected.
        /// </summary>

        public void LoadMapMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new LoadMapMenuScreen(game), e.PlayerIndex, true);
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        public void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            OptionsMenuScreen Options = new OptionsMenuScreen(mod, game);

            ScreenManager.AddScreen(new OptionsMenuScreen(mod, game), e.PlayerIndex, true);
        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to quit" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                           new MainMenuScreen(game));
            MediaPlayer.Stop();
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            base.OnCancel(playerIndex);
            Pausebckground.Quit(playerIndex, ScreenManager);
        }

        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (Properties.Settings.Default.Language != lan)
            {
                SetMenuEntryText();
                lan = Properties.Settings.Default.Language;
            }

            if (mod == 2 && !saveMapMenuEntry.IsEvent)
            {
                saveMapMenuEntry.Selected += SaveMapMenuEntrySelected;
                MenuEntries.Add(saveMapMenuEntry);
                MenuEntries.Add(optionsGameMenuEntry);
                MenuEntries.Add(quitGameMenuEntry);
            }
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            if (this == ScreenManager.GetScreens()[ScreenManager.GetScreens().GetLength(0) - 1] || ScreenManager.GetScreens()[ScreenManager.GetScreens().GetLength(0) - 1].Type == "Pop")
                base.Draw(gameTime);
        }

        /// <summary>
        /// Fills in the latest values for the pause screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            this.TitleUpdate(Langue.tr("PauseTitle"));
            optionsGameMenuEntry.Text = Langue.tr("Options");

            if (mod == 2)
            {
                resumeGameMenuEntry.Text = Langue.tr("PausEditRes");
                quitGameMenuEntry.Text = Langue.tr("PausEditQuit");
                saveMapMenuEntry.Text = Langue.tr("PausEditSave");
                loadMapMenuEntry.Text = Langue.tr("PausEditLoad");
            }
            else
            {
                resumeGameMenuEntry.Text = Langue.tr("PausGameRes");
                quitGameMenuEntry.Text = Langue.tr("PausGameQuit");
            }
        }


        #endregion
    }
}
