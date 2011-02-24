using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace YelloKiller
{
    class LoadMapMenuScreen : MenuScreen
    {
        public LoadMapMenuScreen()
            : base(Langue.tr("PausEditLoad"))
        {
            string[] fileEntries = Directory.GetFiles(System.Windows.Forms.Application.StartupPath);
            foreach (string str in fileEntries)
            {
                if (str.Substring(str.Length - 3) == "txt")
                {
                    MenuEntry menuEntry = new MenuEntry(str.Substring(str.Length - 10));
                    menuEntry.Selected += MenuEntrySelected;
                    MenuEntries.Add(menuEntry);
                }
            }
        }

        void MenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            // MenuEntry selected = (MenuEntry) sender;
            MenuEntry selected = sender as MenuEntry;
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new EditorScreen(selected.Text));
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            ScreenManager.AddScreen(new PauseMenuScreen(2, 2), playerIndex, true);
            ExitScreen();
        }
    }
}
