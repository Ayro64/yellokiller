#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
#endregion

namespace Yellokiller
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        MenuEntry languageMenuEntry;
        MenuEntry sonMenuEntry;
        MenuEntry soMenuEntry;
        MenuEntry elfMenuEntry;

        int mod;

        enum Language
        {
            Français,
            Anglais,
            Allemand,
        }

        static Language currentLanguage = Language.Français;

        static string[] son = { "Défault", "Player", "Aucun" };
        static int currentSon = 0;

        static bool So = true;

        static int elf = 23;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen(int mode)
            : base("Options")
        {
            mod = mode;

            // Create our menu entries.
            languageMenuEntry = new MenuEntry(string.Empty);
            sonMenuEntry = new MenuEntry(string.Empty);
            soMenuEntry = new MenuEntry(string.Empty);
            elfMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry backMenuEntry = new MenuEntry("Retour");

            // Hook up menu event handlers.
            languageMenuEntry.Selected += LanguageMenuEntrySelected;
            sonMenuEntry.Selected += SonMenuEntrySelected;
            soMenuEntry.Selected += SoMenuEntrySelected;
            elfMenuEntry.Selected += ElfMenuEntrySelected;
            backMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(languageMenuEntry);
            MenuEntries.Add(sonMenuEntry);
            MenuEntries.Add(soMenuEntry);
            MenuEntries.Add(elfMenuEntry);
            MenuEntries.Add(backMenuEntry);
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            languageMenuEntry.Text = "Langage: " + currentLanguage;
            sonMenuEntry.Text = "Mode de son: " + son[currentSon];
            soMenuEntry.Text = "Blah?: " + (So ? "Oui" : "Non");
            elfMenuEntry.Text = "elf: " + elf;
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Ungulate menu entry is selected.
        /// </summary>
        void LanguageMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentLanguage++;

            if (currentLanguage > Language.Allemand)
                currentLanguage = 0;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Language menu entry is selected.
        /// </summary>
        void SonMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentSon = (currentSon + 1) % son.Length;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Frobnicate menu entry is selected.
        /// </summary>
        void SoMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            So = !So;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Elf menu entry is selected.
        /// </summary>
        void ElfMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            elf++;
            SetMenuEntryText();
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            if (IsPopup)
                ScreenManager.AddScreen(new PauseMenuScreen(1, mod), playerIndex, true);
            ExitScreen();
        }


        #endregion
    }
}
