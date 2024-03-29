﻿using System;
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
                        case("Help"):
                            return "Instructions";
                        case ("Options"):
                            return "Options";
                        case("Yes"):
                            return "Oui";
                        case("No"):
                            return "Non";
                        case ("MainMenuQuit"):
                            return "Quitter";
                        case ("MainQuitMsg"):
                            return "Êtes-vous sûr de vouloir quitter le jeu?\n";
                        case ("EditorExCharacters"):
                            return "Aucun boss ou héros n'a été placé.\n\nVeuillez placer au moins un héros et un boss avant de sauvegarder.";
                        case ("EditorSave1"):
                            return "Fichier sauvegardé sous ";
                        case ("EditorSave2"):
                            return ".\nAppuyez sur ECHAP pour quitter.";
                        case ("GORetry"):
                            return "Recommencer";
                        case ("GOAbort"):
                            return "Quitter";
                        case ("GOchk"):
                            return "Réessayer";
                        case ("GOMsg"):
                            return "Vous avez été capturé!";
                        case ("Multi"):
                            return "Choix du Niveau\n     Co-op";
                        case ("Solo"):
                            return "Choix du Niveau\n     Solo";
                        case ("BckToMenu"):
                            return "Retour au Menu Principal";
                        case ("Back"):
                            return "Retour";
                        case ("Loading"):
                            return "Chargement...";
                        case("Remaining"):
                            return "Cibles restantes :";
                        case("GoHome"):
                            return "Retournez au point de départ!";
                        case("Joueur"):
                            return "Joueur";
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
                        case ("PausEditDel"):
                            return "Supprimer une carte";
                        case("MapDel"):
                            return "Êtes-vous sûr de vouloir supprimer ";
                        case ("SaveDel"):
                            return "Êtes-vous sûr de vouloir effacer\nles données sauvegardées?\n";
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
                        case ("OptLan"):
                            return "Langage: ";
                        case ("OptSound"):
                            return "Mode de son: ";
                        case ("OptMusic"):
                            return "Volume de la musique : ";
                        case ("OptFX"):
                            return "Volume des sons : ";
                        case("Erase"):
                            return "Effacer la sauvegarde";
                        case("FullScr"):
                            return "Plein écran : ";
                        case ("WinMsg"):
                            return "Cible éliminée!";
                        case ("WiNext"):
                            return "Niveau suivant";
                        case ("WiRetry"):
                            return "Refaire le niveau";
                        case ("Time"):
                            return "Temps : ";
                        case ("Killed"):
                            return "Gardes tués : ";
                        case ("Retries"):
                            return "Tentatives : ";
                        case ("Score"):
                            return "Solde : ";
                        case ("BaseSalary"):
                            return "Salaire de base : ";
                        case ("Penalties"):
                            return "Pénalités : ";
                        case("Scores"):
                            return "Classement des assassins";
                        case("HiScore"):
                            return "Vous venez de remplir un contrat juteux!\nSous quel nom désirez-vous être connu?\n";
                        case ("ScoresBox"):
                            return "Bouton A, Entrée : Enregistrer" +
                                   "\nBouton B, Echap : Ne pas enregistrer";
                        case("SavePop-Up"):
                            return "Sous quel nom enregistrer la carte?\n";
                        case("FileExists1"):
                            return "La carte ";
                        case("FileExists2"):
                            return " existe déjà.\nVeuillez retenter la sauvegarde avec un nouveau nom.";
                        case("Presets"):
                            return "Appliquer une transformation";
                        case("F1"):
                            return "Recouvrir d'herbe";
                        case("F2"):
                            return "Recouvrir d'herbe foncée";
                        case("F3"):
                            return "Recouvrir de terre";
                        case("F4"):
                            return "Recouvrir de parquet";
                        case("F5"):
                            return "Créer un très grand labyrinthe";
                        case("F6"):
                            return "Créer un grand labyrinthe";
                        case("F7"):
                            return "Créer un petit labyrinthe";
                        case("F8"):
                            return "Créer un très petit labyrinthe";
                        default:
                            return "";
                    }
                case (1):
                    switch (sentence)
                    {
                        case ("MainMenuTitle"):
                            return "Yello Killer\n Hauptmenu";
                        case ("MainMenuSolo"):
                            return "Einzelspiel";
                        case ("MainMenuCoop"):
                            return "Koop-Modus";
                        case ("MainMenuEditor"):
                            return "Karteneditor";
                        case ("Help"):
                            return "Hilfe";
                        case ("Options"):
                            return "Einstellungen";
                        case("Yes"):
                            return "Ja";
                        case("No"):
                            return "Nein";
                        case ("MainMenuQuit"):
                            return "Spiel verlassen";
                        case ("MainQuitMsg"):
                            return "Sind Sie sicher, dass Sie das Spiel verlassen möchten?\n";
                        case ("EditorExCharacters"):
                            return "Der Charakter/Die Charaktere oder der/die Endgegner wurde/wurden noch nicht plaziert.\n\nBitte platziere vor dem Speichern einen Startpunkt und mindestens ein Endgegner.";
                        case ("EditorSave1"):
                            return "Datei wurde gespeichert als ";
                        case ("EditorSave2"):
                            return ".\nESC drucken um der Karteneditor zu verlassen.";
                        case ("GORetry"):
                            return "Wieder Starten";
                        case ("GOAbort"):
                            return "Abbrechen";
                        case ("GOchk"):
                            return "Wiederholen";
                        case ("GOMsg"):
                            return "Sie wurden erwischt!";
                        case ("Multi"):
                            return "Kartenauswahl\n    Koop";
                        case ("Solo"):
                            return "Kartenauswahl\n    Solo";
                        case ("BckToMenu"):
                            return "Zuruck zum Hauptmenu";
                        case ("Back"):
                            return "Zuruck";
                        case ("Loading"):
                            return "Ladung...";
                        case ("Remaining"):
                            return "Ubrigen Ziele :";
                        case ("GoHome"):
                            return "Zuruck zum Startpunkt!";
                        case ("Joueur"):
                            return "Spieler";
                        case ("PauseTitle"):
                            return "Pause";
                        case ("PausEditRes"):
                            return "Weiter bearbeiten";
                        case ("PausEditQuit"):
                            return "Zuruck zum Hauptmenu";
                        case ("PausEditSave"):
                            return "Karte speichern";
                        case ("PausEditLoad"):
                            return "Karte laden";
                        case ("PausEditDel"):
                            return "Karte lösen";
                        case ("MapDel"):
                            return "Sind sie sicher, dass sie\ndie Karte lösen wollen ";
                        case ("SaveDel"):
                            return "Sind sie sicher, dass sie die\ngespeicherte Dateien lösen wollen?\n";
                        case ("PausGameRes"):
                            return "Weiter spielen";
                        case ("PausGameQuit"):
                            return "Zuruck zum Hauptmenu";
                        case ("MsgBox"):
                            return "\nA Taste, Leertaste, Enter : Ja" +
                                   "\nB Taste, Esc : Nein";
                        case ("EditMsgBox"):
                            return "Sind Sie sicher, dass sie zum bearbeiten aufhoren mochten?\nAlle ungespeicherte Anderungenser werden verliert.\n";
                        case ("GameMsgBox"):
                            return "Sind sie sicher, dass Sie den Spiel verlassen mochten?\n";
                        case ("OptLan"):
                            return "Sprache: ";
                        case ("OptSound"):
                            return "Audio Mode: ";
                        case ("OptMusic"):
                            return "Musik lautstarke : ";
                        case ("OptFX"):
                            return "Effekte lautstarke : ";
                        case ("Erase"):
                            return "Gespeicherte Dateien lösen";
                        case ("FullScr"):
                            return "Vollbild : ";
                        case ("WinMsg"):
                            return "Ziel erreischt!";
                        case ("WiNext"):
                            return "Nachste Karte";
                        case ("WiRetry"):
                            return "Noch mal";
                        case ("Time"):
                            return "Zeit : ";
                        case ("Killed"):
                            return "Wächter getotet : ";
                        case ("Retries"):
                            return "Versuche : ";
                        case ("Score"):
                            return "Lohn : ";
                        case ("BaseSalary"):
                            return "Anfang Gehalt : ";
                        case ("Penalties"):
                            return "Strafen : ";
                        case ("Scores"):
                            return "Attentäter Ruhmeshalle";
                        case ("HiScore"):
                            return "Sie haben gerade viel Geld gewonnen!\nWie wollen sie gennant werden?\n";
                        case ("ScoresBox"):
                            return "A Taste, Enter : Speichern" +
                                   "\nB Taste, Esc : Nicht speichern";
                        case ("SavePop-Up"):
                            return "Unter welche Name wollen Sie die Karte speichern?\n";
                        case ("FileExists1"):
                            return "Die Karte ";
                        case ("FileExists2"):
                            return " existiert schon.\nBitte versuchen Sie noch mal zu speichern.";
                        case ("Presets"):
                            return "Karte transformieren";
                        case ("F1"):
                            return "Mit Gras decken";
                        case ("F2"):
                            return "Mit dunkel Gras decken";
                        case ("F3"):
                            return "Mit Erde decken";
                        case ("F4"):
                            return "Mit Parkett decken";
                        case ("F5"):
                            return "Sehr grosses Labyrinth schaffen";
                        case ("F6"):
                            return "Grosses Labyrinth schaffen";
                        case ("F7"):
                            return "Kleines Labyrinth schaffen";
                        case ("F8"):
                            return "Sehr kleines Labyrinth schaffen";
                        default:
                            return "";
                    }
                case (2):
                    switch (sentence)
                    {
                        case ("MainMenuTitle"):
                            return "Yello Killer\n Main Menu";
                        case ("MainMenuSolo"):
                            return "Play in Solo Mode";
                        case ("MainMenuCoop"):
                            return "Play in Co-op Mode";
                        case ("MainMenuEditor"):
                            return "Map Editor";
                        case ("Help"):
                            return "Quick help";
                        case ("Options"):
                            return "Settings";
                        case ("Yes"):
                            return "Yes";
                        case ("No"):
                            return "No";
                        case ("MainMenuQuit"):
                            return "Exit Game";
                        case ("MainQuitMsg"):
                            return "Are you sure you want to exit the game?\n";
                        case ("EditorExCharacters"):
                            return "The character(s) or the target(s) has not been placed yet.\n\nPlease set a starting point and at least one target before saving.";
                        case ("EditorSave1"):
                            return "File saved as ";
                        case ("EditorSave2"):
                            return ".\nPres ESC to exit.";
                        case ("GORetry"):
                            return "Restart";
                        case ("GOAbort"):
                            return "Abort";
                        case ("GOchk"):
                            return "Retry";
                        case ("GOMsg"):
                            return "You've been captured!";
                        case ("Multi"):
                            return "Select a level\n     Co-op";
                        case ("Solo"):
                            return "Select a level\n     Solo";
                        case ("BckToMenu"):
                            return "Back to Main Menu";
                        case ("Back"):
                            return "Back";
                        case ("Loading"):
                            return "Loading...";
                        case ("Remaining"):
                            return "Remaining Targets :";
                        case ("GoHome"):
                            return "Go back to start!";
                        case ("Joueur"):
                            return "Player";
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
                        case("PausEditDel"):
                            return "Delete a map";
                        case ("MapDel"):
                            return "Are you sure you want to delete ";
                        case ("SaveDel"):
                            return "Are you sure you want to delete all saved data?\n";
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
                        case ("OptLan"):
                            return "Language: ";
                        case ("OptSound"):
                            return "Audio mode: ";
                        case ("OptMusic"):
                            return "Music Volume : ";
                        case ("OptFX"):
                            return "FX Volume : ";
                        case ("Erase"):
                            return "Erase unlocked levels";
                        case ("FullScr"):
                            return "Full Screen : ";
                        case ("WinMsg"):
                            return "Target terminated!";
                        case ("WiNext"):
                            return "Next level";
                        case ("WiRetry"):
                            return "Do this level again";
                        case ("Time"):
                            return "Time : ";
                        case ("Killed"):
                            return "Killed guards : ";
                        case ("Retries"):
                            return "Retries : ";
                        case ("Score"):
                            return "Pay : ";
                        case ("BaseSalary"):
                            return "Base Salary : ";
                        case ("Penalties"):
                            return "Penalties : ";
                        case ("Scores"):
                            return "Assassins Hall of Fame";
                        case ("HiScore"):
                            return "You just fulfilled a juicy request!\nUnder which name would you like to be know?\n";
                        case ("ScoresBox"):
                            return "A Button, Enter : Save" +
                                   "\nB Button, ESC : Don't save";
                        case ("SavePop-Up"):
                            return "Under which name should the map be saved?\n";
                        case ("FileExists1"):
                            return "The map ";
                        case ("FileExists2"):
                            return " already exists.\nPlease try again with a different name.";
                        case ("Presets"):
                            return "Apply transformation";
                        case ("F1"):
                            return "Cover with grass";
                        case ("F2"):
                            return "Cover with dark grass";
                        case ("F3"):
                            return "Cover with dirt";
                        case ("F4"):
                            return "Cover with parquet";
                        case ("F5"):
                            return "Create a huge maze";
                        case ("F6"):
                            return "Create a big maze";
                        case ("F7"):
                            return "Create a small maze";
                        case ("F8"):
                            return "Create a tiny maze";
                        default:
                            return "";
                    }
                default:
                    return "";
            }
        }
    }
}
