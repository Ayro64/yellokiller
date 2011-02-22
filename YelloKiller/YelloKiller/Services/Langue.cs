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
                        case ("Options"):
                            return "Options";
                        case ("MainMenuQuit"):
                            return "Quitter";
                        case ("MainQuitMsg"):
                            return "Êtes-vous sûr de vouloir quitter le jeu?\n";
                        case("EditorExCharacters"):
                            return "Le ou les personnages n'a / n'ont pas été placé.\n\nVeuillez placer un ou deux personnages avant de sauvegarder.";
                        case("EditorSave1"):
                            return "Fichier sauvegardé sous ";
                        case("EditorSave2"):
                            return ".txt\n\nAppuyez sur ECHAP pour quitter.";
                        case("GORetry"):
                            return "Réessayer";
                        case("GOAbort"):
                            return "Quitter";
                        case("GOMsg"):
                            return "Vous avez été capturé!";
                        case("Multi"):
                            return "Choix du Niveau\n     Co-op";
                        case("Solo"):
                            return "Choix du Niveau\n      Solo";
                        case("Level"):
                            return "Niveau";
                        case("BckToMenu"):
                            return "Retour au Menu";
                        case("Loading"):
                            return "Chargement...";
                        case("PauseTitle"):
                            return "En Pause";
                        case("PausEditRes"):
                            return "Reprendre l'édition";
                        case("PausEditQuit"):
                            return "Quitter l'édition";
                        case("PausGameRes"):
                            return "Reprendre le jeu";
                        case("PausGameQuit"):
                            return "Quitter la partie";
                        case("MsgBox"):
                            return "\nBouton A, Espace, Entrée : OK" +
                                   "\nBouton B, Echap : Annuler";
                        case("EditMsgBox"):
                            return "Êtes-vous sûr de vouloir quitter l'édition?\nToute édition non sauvegardée sera perdue.\n";
                        case("GameMsgBox"):
                            return "Êtes-vous sûr de vouloir quitter la partie?\n";
                        default:
                            return "";
                    }
                default:
                    return "";
            }
        }
    }
}
