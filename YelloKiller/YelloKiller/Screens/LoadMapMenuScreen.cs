using System;
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
                    if (str[str.Length - 10] == 'C')
                        menuEntry.Selected += CoopMenuEntrySelected;
                    else if (str[str.Length - 10] == 'S')
                        menuEntry.Selected += SoloMenuEntrySelected;
                    MenuEntries.Add(menuEntry);
                }
            }
        }

        void CoopMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreenCoop());
        }

        void SoloMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreenSolo());
        }
    }
}
