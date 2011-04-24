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
        YellokillerGame game;

        public LoadMapMenuScreen(YellokillerGame game)
            : base(Langue.tr("PausEditLoad"))
        {
            this.game = game;
            string[] fileEntries = Directory.GetFiles(System.Windows.Forms.Application.StartupPath, "*.txt");
            foreach (string str in fileEntries)
            {
                    MenuEntry menuEntry = new MenuEntry(str.Substring(str.Length - 10));
                    menuEntry.Selected += MenuEntrySelected;
                    MenuEntries.Add(menuEntry);
            }
        }

        void MenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            // MenuEntry selected = (MenuEntry) sender;
            MenuEntry selected = sender as MenuEntry;
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new EditorScreen(selected.Text, game));
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            ScreenManager.AddScreen(new PauseMenuScreen(2, 2, game), playerIndex, true);
            ExitScreen();
        }
    }
}
