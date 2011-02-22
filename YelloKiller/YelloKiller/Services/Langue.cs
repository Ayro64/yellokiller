using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YelloKiller
{
    class Langue
    {
        public static string tr(string sentence)
        {
            switch (Properties.Settings.Default.Language)
            {
                case (0):
                    switch (sentence)
                    {
                        case ("MainMenuTitle"):
                            return " Yello Killer\nMenu Principal";
                        case ("MainMenuSolo"):
                            return "Mode Solo";
                        case ("MainMenuCoop"):
                            return "Mode Co-op";
                        case ("MainMenuEditor"):
                            return "Editeur de cartes";
                        case ("MainMenuOptions"):
                            return "Options";
                        case ("MainMenuQuit"):
                            return "Quitter";
                        case ("MainQuitMsg"):
                            return "Êtes-vous sûr de vouloir quitter le jeu?\n";
                        default:
                            return "";
                    }
                default:
                    return "";
            }
        }
    }
}
