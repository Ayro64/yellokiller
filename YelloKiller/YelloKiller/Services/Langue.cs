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
                        case ("PausEditSave"):
                            return "Sauver la carte";
                        case ("PausEditLoad"):
                            return "Charger une carte";
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
                        case("WinMsg"):
                            return "Cible éliminée!";
                        case("WiNext"):
                            return "Niveau suivant";
                        case("WiRetry"):
                            return "Refaire le niveau";
                        default:
                            return "";
                    }
                case (1):
                    switch (sentence)
                    {
                        case ("MainMenuTitle"):
                            return " Yello Killer\n  Hauptmenu";
                        case ("MainMenuSolo"):
                            return "Einzelspiel";
                        case ("MainMenuCoop"):
                            return "Koop-Modus";
                        case ("MainMenuEditor"):
                            return "Karteneditor";
                        case ("Options"):
                            return "Einstellungen";
                        case ("MainMenuQuit"):
                            return "Spiel verlassen";
                        case ("MainQuitMsg"):
                            return "Sind Sie sicher, dass Sie das Spiel verlassen möchten?\n";
                        case ("EditorExCharacters"):
                            return "Der Charakter/Die Charaktere wurde/wurden noch nicht plaziert.\n\nBitte platziere vor dem Speichern einen Startpunkt.";
                        case ("EditorSave1"):
                            return "Datei wurde gespeichert als ";
                        case ("EditorSave2"):
                            return ".txt.\n\nESC drucken um der Karteneditor zu verlassen.";
                        case ("GORetry"):
                            return "Wiederholen";
                        case ("GOAbort"):
                            return "Zuruck zum Hauptmenu";
                        case ("GOMsg"):
                            return "Sie wurden erwischt!";
                        case ("Multi"):
                            return "Choix du Niveau\n     Koop";
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
                        case ("PausEditSave"):
                            return "Sauver la carte";
                        case ("PausEditLoad"):
                            return "Charger une carte";
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
                        case ("WinMsg"):
                            return "Cible éliminée!";
                        case ("WiNext"):
                            return "Niveau suivant";
                        case ("WiRetry"):
                            return "Refaire le niveau";
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
                            return "The character(s) has not been placed yet.\n\nPlease set a starting point before saving.";
                        case ("EditorSave1"):
                            return "File saved as ";
                        case ("EditorSave2"):
                            return ".txt.\n\nPres ESC to exit.";
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
                        case ("PausEditSave"):
                            return "Save the map";
                        case ("PausEditLoad"):
                            return "Load a map";
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
                        case ("WinMsg"):
                            return "Target terminated!";
                        case ("WiNext"):
                            return "Next level";
                        case ("WiRetry"):
                            return "Do this level again";
                        default:
                            return "";
                    }
                default:
                    return "";
            }
        }
    }
}
