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
                        case ("EditorExCharacters"):
                            return "Le ou les personnages n'a / n'ont pas été placé.\n\nVeuillez placer un ou deux personnages avant de sauvegarder.";
                        case ("EditorSave1"):
                            return "Fichier sauvegardé sous ";
                        case ("EditorSave2"):
                            return ".txt\n\nAppuyez sur ECHAP pour quitter.";
                        case ("GORetry"):
                            return "Réessayer";
                        case ("GOAbort"):
                            return "Quitter";
                        case ("GOMsg"):
                            return "Vous avez été capturé!";
                        case ("Multi"):
                            return "Choix du Niveau\n     Co-op";
                        case ("Solo"):
                            return "Choix du Niveau\n      Solo";
                        case ("Level"):
                            return "Niveau";
                        case ("BckToMenu"):
                            return "Retour au Menu Principal";
                        case ("Back"):
                            return "Retour";
                        case ("Loading"):
                            return "Chargement...";
                        case ("PauseTitle"):
                            return "En Pause";
                        case ("PausEditRes"):
                            return "Reprendre l'édition";
                        case ("PausEditQuit"):
                            return "Quitter l'édition";
                        case ("PausGameRes"):
                            return "Reprendre le jeu";
                        case ("PausGameQuit"):
                            return "Quitter la partie";
                        case ("MsgBox"):
                            return "\nBouton A, Espace, Entrée : OK" +
                                   "\nBouton B, Echap : Annuler";
                        case ("EditMsgBox"):
                            return "Êtes-vous sûr de vouloir quitter l'édition?\nToute édition non sauvegardée sera perdue.\n";
                        case ("GameMsgBox"):
                            return "Êtes-vous sûr de vouloir quitter la partie?\n";
                        case ("SoundDefault"):
                            return "Défault";
                        case ("SoundNone"):
                            return "Aucun";
                        case ("OptLan"):
                            return "Langage: ";
                        case ("OptSound"):
                            return "Mode de son: ";
                        case ("OptMusic"):
                            return "Volume de la musique : ";
                        case ("OptFX"):
                            return "Volume des sons : ";
                        default:
                            return "";
                    }
                case (1):
                    switch (sentence)
                    {
                        case ("MainMenuTitle"):
                            return " Yello Killer\nHauptmenü";
                        case ("MainMenuSolo"):
                            return "Allein spielen";
                        case ("MainMenuCoop"):
                            return "Zusammen spielen";
                        case ("MainMenuEditor"):
                            return "Karteneditor";
                        case ("Options"):
                            return "Einstellungen";
                        case ("MainMenuQuit"):
                            return "Quitter";
                        case ("MainQuitMsg"):
                            return "Êtes-vous sûr de vouloir quitter le jeu?\n";
                        case ("EditorExCharacters"):
                            return "Le ou les personnages n'a / n'ont pas été placé.\n\nVeuillez placer un ou deux personnages avant de sauvegarder.";
                        case ("EditorSave1"):
                            return "Fichier sauvegardé sous ";
                        case ("EditorSave2"):
                            return ".txt\n\nAppuyez sur ECHAP pour quitter.";
                        case ("GORetry"):
                            return "Réessayer";
                        case ("GOAbort"):
                            return "Quitter";
                        case ("GOMsg"):
                            return "Vous avez été capturé!";
                        case ("Multi"):
                            return "Choix du Niveau\n     Co-op";
                        case ("Solo"):
                            return "Choix du Niveau\n      Solo";
                        case ("Level"):
                            return "Niveau";
                        case ("BckToMenu"):
                            return "Retour au Menu Principal";
                        case ("Back"):
                            return "Retour";
                        case ("Loading"):
                            return "Chargement...";
                        case ("PauseTitle"):
                            return "En Pause";
                        case ("PausEditRes"):
                            return "Reprendre l'édition";
                        case ("PausEditQuit"):
                            return "Quitter l'édition";
                        case ("PausGameRes"):
                            return "Reprendre le jeu";
                        case ("PausGameQuit"):
                            return "Quitter la partie";
                        case ("MsgBox"):
                            return "\nBouton A, Espace, Entrée : OK" +
                                   "\nBouton B, Echap : Annuler";
                        case ("EditMsgBox"):
                            return "Êtes-vous sûr de vouloir quitter l'édition?\nToute édition non sauvegardée sera perdue.\n";
                        case ("GameMsgBox"):
                            return "Êtes-vous sûr de vouloir quitter la partie?\n";
                        case ("SoundDefault"):
                            return "Défault";
                        case ("SoundNone"):
                            return "Aucun";
                        case ("OptLan"):
                            return "Langage: ";
                        case ("OptSound"):
                            return "Mode de son: ";
                        case ("OptMusic"):
                            return "Volume de la musique : ";
                        case ("OptFX"):
                            return "Volume des effets sonores : ";
                        default:
                            return "";
                    }
                case (2):
                    switch (sentence)
                    {
                        case ("MainMenuTitle"):
                            return " Yello Killer\nMain Menu";
                        case ("MainMenuSolo"):
                            return "Play in Solo Mode";
                        case ("MainMenuCoop"):
                            return "Play in Co-op Mode";
                        case ("MainMenuEditor"):
                            return "Map Editor";
                        case ("Options"):
                            return "Settings";
                        case ("MainMenuQuit"):
                            return "Exit Game";
                        case ("MainQuitMsg"):
                            return "Are you sure you want to exit the game?\n";
                        case ("EditorExCharacters"):
                            return "The character(s) has not been placed yet\n\nPlease set a starting point before saving.";
                        case ("EditorSave1"):
                            return "File saved as ";
                        case ("EditorSave2"):
                            return ".txt\n\nPres ESC to exit.";
                        case ("GORetry"):
                            return "Retry";
                        case ("GOAbort"):
                            return "Abort";
                        case ("GOMsg"):
                            return "You've been captured!";
                        case ("Multi"):
                            return "Select a level\n     Co-op";
                        case ("Solo"):
                            return "Select a level\n      Solo";
                        case ("Level"):
                            return "Level";
                        case ("BckToMenu"):
                            return "Back to Main Menu";
                        case ("Back"):
                            return "Back";
                        case ("Loading"):
                            return "Loading...";
                        case ("PauseTitle"):
                            return "Paused";
                        case ("PausEditRes"):
                            return "Resume editing";
                        case ("PausEditQuit"):
                            return "Stop editing";
                        case ("PausGameRes"):
                            return "Resume play";
                        case ("PausGameQuit"):
                            return "Stop playing";
                        case ("MsgBox"):
                            return "\nA Button, Space bar, Enter : OK" +
                                   "\nB Button, ESC : Cancel";
                        case ("EditMsgBox"):
                            return "Are you sure you want to quit the map editor?\nAll unsaved changes will be lost.\n";
                        case ("GameMsgBox"):
                            return "Are you sure you want to quit the game?\n";
                        case ("SoundDefault"):
                            return "Default";
                        case ("SoundNone"):
                            return "None";
                        case ("OptLan"):
                            return "Language: ";
                        case ("OptSound"):
                            return "Audio mode: ";
                        case ("OptMusic"):
                            return "Music Volume : ";
                        case ("OptFX"):
                            return "FX Volume : ";
                        default:
                            return "";
                    }
                default:
                    return "";
            }
        }
    }
}
