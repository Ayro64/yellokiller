using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace YelloKiller
{
    class DelMapMenuScreen : MenuScreen
    {
        YellokillerGame game;
        string ToDelete;

        public DelMapMenuScreen(YellokillerGame game)
            : base(Langue.tr("PausEditLoad"))
        {
            this.game = game;
            string[] fileEntries = LoadMapMenuScreen.ConcatenerTableaux(Directory.GetFiles(System.Windows.Forms.Application.StartupPath + "\\Levels", "*.solo"), Directory.GetFiles(System.Windows.Forms.Application.StartupPath + "\\Levels", "*.coop"));
            foreach (string str in fileEntries)
            {
                MenuEntry menuEntry = new MenuEntry(str.Substring(str.LastIndexOf('\\') + 1));
                menuEntry.Selected += MenuEntrySelected;
                MenuEntries.Add(menuEntry);
            }
        }

        void MenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            MenuEntry selected = sender as MenuEntry;
            ToDelete = selected.Text;
            string message = Langue.tr("MapDel") + ToDelete + " ?";
            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);
            confirmExitMessageBox.Accepted += MessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, e.PlayerIndex);

        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to delete" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        void MessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            File.Delete(System.Windows.Forms.Application.StartupPath + "\\Levels\\" + ToDelete);
            this.ExitScreen();
        }

    }
}
