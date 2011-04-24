#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
#endregion

namespace YelloKiller
{
    /// <summary>
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    class PauseMenuScreen : MenuScreen
    {
        int mod;
        YellokillerGame game;

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

            // Create our menu entries.
            MenuEntry resumeGameMenuEntry;
            MenuEntry optionsGameMenuEntry = new MenuEntry(Langue.tr("Options"));
            MenuEntry quitGameMenuEntry;


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
                MenuEntry saveMapMenuEntry = new MenuEntry(Langue.tr("PausEditSave"));
                MenuEntry loadMapMenuEntry = new MenuEntry(Langue.tr("PausEditLoad"));
                saveMapMenuEntry.Selected += SaveMapMenuEntrySelected;
                loadMapMenuEntry.Selected += LoadMapMenuEntrySelected;
                MenuEntries.Add(saveMapMenuEntry);
                MenuEntries.Add(loadMapMenuEntry);
            }

            MenuEntries.Add(optionsGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
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
        /// Event handler for when the Save Map menu entry is selected.
        /// </summary>

        public void SaveMapMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
        }

        /// <summary>
        /// Event handler for when the Load Map menu entry is selected.
        /// </summary>

        public void LoadMapMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new LoadMapMenuScreen(game), e.PlayerIndex, true);
            ScreenManager.RemoveScreen(this);
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>

        public void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(mod, game), e.PlayerIndex, true);
            ScreenManager.RemoveScreen(this);
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

        #region Draw


        /// <summary>
        /// Draws the pause menu screen. This chains to the base MenuScreen.Draw.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }


        #endregion
    }
}
