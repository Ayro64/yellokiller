#region File Description
//-----------------------------------------------------------------------------
// PauseMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

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

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public PauseMenuScreen(int comingfrom, int mode)
            : base("En pause")
        {
            //Conserve la sélection
            selectedEntry = comingfrom;

            mod = mode;

            // Create our menu entries.
            MenuEntry resumeGameMenuEntry;
            MenuEntry optionsGameMenuEntry = new MenuEntry("Options");
            MenuEntry quitGameMenuEntry;

            if (mode == 2)
            {
                resumeGameMenuEntry = new MenuEntry("Reprendre l'édition");
                quitGameMenuEntry = new MenuEntry("Quitter l'édition");
            }
            else
            {
                resumeGameMenuEntry = new MenuEntry("Reprendre le jeu");
                quitGameMenuEntry = new MenuEntry("Quitter la partie");
            }


            // Hook up menu event handlers.
            resumeGameMenuEntry.Selected += OnCancel;
            optionsGameMenuEntry.Selected += OptionsMenuEntrySelected;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(resumeGameMenuEntry);
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
                message = "Êtes-vous sûr de vouloir quitter l'édition?\nToute édition non sauvegardée sera perdue.\n";

            else
                message = "Êtes-vous sûr de vouloir quitter la partie?\n";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }


        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>

        public void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(mod), e.PlayerIndex, true);
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
                                                           new MainMenuScreen());
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
