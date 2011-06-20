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
            try
            {
                string[] fileEntries = ConcatenerTableaux(Directory.GetFiles(System.Windows.Forms.Application.StartupPath + "\\Levels", "*.solo"), Directory.GetFiles(System.Windows.Forms.Application.StartupPath + "\\Levels", "*.coop"));

                foreach (string str in fileEntries)
                {
                    MenuEntry menuEntry = new MenuEntry(str.Substring(str.LastIndexOf('\\') + 1));
                    menuEntry.Selected += MenuEntrySelected;
                    MenuEntries.Add(menuEntry);
                }
            }
            catch (DirectoryNotFoundException)
            {
 
            }
        }

        public static string[] ConcatenerTableaux(string[] tab1, string[] tab2)
        {
            string[] res = new string[tab1.Length + tab2.Length];
            int index = tab1.Length;

            for (int k = 0; k < tab1.Length; k++)
                res[k] = tab1[k];

            for (int j = 0; j < tab2.Length; j++)
            {
                res[index] = tab2[j];
                index++;
            }

            return res;
        }

        void MenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            // MenuEntry selected = (MenuEntry) sender; <-- très beau aussi!
            MenuEntry selected = sender as MenuEntry;
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new EditorScreen(selected.Text, game));
        }
    }
}
