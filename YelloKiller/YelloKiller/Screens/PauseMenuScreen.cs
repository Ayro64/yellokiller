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
        public event EventHandler<PlayerIndexEventArgs> F1;
        public event EventHandler<PlayerIndexEventArgs> F2;
        public event EventHandler<PlayerIndexEventArgs> F3;
        public event EventHandler<PlayerIndexEventArgs> F4;
        public event EventHandler<PlayerIndexEventArgs> F5;
        public event EventHandler<PlayerIndexEventArgs> F6;
        public event EventHandler<PlayerIndexEventArgs> F7;
        public event EventHandler<PlayerIndexEventArgs> F8;

        MenuEntry resumeGameMenuEntry;
        MenuEntry saveMapMenuEntry;
        MenuEntry loadMapMenuEntry;
        MenuEntry deleteMapMenuEntry;
        MenuEntry presetsMenuEntry;
        MenuEntry optionsGameMenuEntry;
        MenuEntry quitGameMenuEntry;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public PauseMenuScreen(int comingfrom, int mode, YellokillerGame game)
            : base(Langue.tr("PauseTitle"))
        {
            this.game = game;
            //Conserve la s�lection
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
                deleteMapMenuEntry = new MenuEntry(Langue.tr("PausEditDel"));
                presetsMenuEntry = new MenuEntry(Langue.tr("Presets"));
                loadMapMenuEntry.Selected += LoadMapMenuEntrySelected;
                deleteMapMenuEntry.Selected += DeleteMapMenuEntrySelected;
                presetsMenuEntry.Selected += PresetsMapMenuEntrySelected;
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
        /// Event handler for when the Delete Map menu entry is selected.
        /// </summary>

        public void DeleteMapMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new DelMapMenuScreen(game), e.PlayerIndex, true);
        }

        void PresetsMapMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Presets presetsScreen = new Presets(game);
            presetsScreen.F1Selected += F1;
            presetsScreen.F1Selected += OnCancel;
            presetsScreen.F2Selected += F2;
            presetsScreen.F2Selected += OnCancel;
            presetsScreen.F3Selected += F3;
            presetsScreen.F3Selected += OnCancel;
            presetsScreen.F4Selected += F4;
            presetsScreen.F4Selected += OnCancel;
            presetsScreen.F5Selected += F5;
            presetsScreen.F5Selected += OnCancel;
            presetsScreen.F6Selected += F6;
            presetsScreen.F6Selected += OnCancel;
            presetsScreen.F7Selected += F7;
            presetsScreen.F7Selected += OnCancel;
            presetsScreen.F8Selected += F8;
            presetsScreen.F8Selected += OnCancel;
            ScreenManager.AddScreen(presetsScreen, e.PlayerIndex, true);
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        public void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
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
                MenuEntries.Add(deleteMapMenuEntry);
                MenuEntries.Add(presetsMenuEntry);
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
                deleteMapMenuEntry.Text = Langue.tr("PausEditDel");
                presetsMenuEntry.Text = Langue.tr("Presets");
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
