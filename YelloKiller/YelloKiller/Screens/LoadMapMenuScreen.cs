﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace YelloKiller
{
    class LoadMapMenuScreen : MenuScreen
    {
        string path = System.Windows.Forms.Application.StartupPath;

        public LoadMapMenuScreen()
            : base(Langue.tr("PausEditLoad"))
        {
            string[] fileEntries = Directory.GetFiles(path);
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
            MenuEntry selected = sender as MenuEntry;
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new EditorScreen(selected.Text));
        }
    }
}
