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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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

        Player audio;
        KeyboardState keyboardState, lastKeyboardState;

        MenuEntry languageMenuEntry;
        MenuEntry sonMenuEntry;
        MenuEntry soMenuEntry;
        MenuEntry soundVolumeMenuEntry;
        MenuEntry fxVolumeMenuEntry;

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

        static int soundVolume = (int)(MediaPlayer.Volume * 10);
        static int fxVolume = 25;


        // Sert à keud', XNA.Content et XNA.Graphics à dégager.
        ContentManager content;
        SpriteBatch spriteBatch;
        Texture2D blankTexture;
        //

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen(int mode)
            : base("Options")
        {
            audio = new Player(soundVolume / 10);
            mod = mode;

            // Create our menu entries.
            languageMenuEntry = new MenuEntry(string.Empty);
            sonMenuEntry = new MenuEntry(string.Empty);
            soMenuEntry = new MenuEntry(string.Empty);
            soundVolumeMenuEntry = new MenuEntry(string.Empty);
            fxVolumeMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry backMenuEntry = new MenuEntry("Retour");

            // Hook up menu event handlers.
            languageMenuEntry.Selected += LanguageMenuEntrySelected;
            sonMenuEntry.Selected += SonMenuEntrySelected;
            soMenuEntry.Selected += SoMenuEntrySelected;
            backMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(languageMenuEntry);
            MenuEntries.Add(sonMenuEntry);
            MenuEntries.Add(soMenuEntry);
            MenuEntries.Add(soundVolumeMenuEntry);
            MenuEntries.Add(fxVolumeMenuEntry);
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
            soundVolumeMenuEntry.Text = "Volume de la musique : " + soundVolume;
            fxVolumeMenuEntry.Text = "Volume des sons : " + fxVolume;
        }

        // Sert qu'aux rectangles
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            blankTexture = content.Load<Texture2D>("blank");

            base.LoadContent();
        }
        //

        #endregion

        #region Handle Input

        /// <summary>
        /// Responds to user input, changing the selected entry and accepting
        /// or cancelling the menu.
        /// </summary>
        public override void HandleInput(InputState input)
        {


            // Accept or cancel the menu? We pass in our ControllingPlayer, which may
            // either be null (to accept input from any player) or a specific index.
            // If we pass a null controlling player, the InputState helper returns to
            // us which player actually provided the input. We pass that through to
            // OnSelectEntry and OnCancel, so they can tell which player triggered them.

            // Look up inputs for the active player profile.
            PlayerIndex PlayerIndex;
            int playerIndex = (int)ControllingPlayer.Value;

            lastKeyboardState = keyboardState;
            keyboardState = input.CurrentKeyboardStates[playerIndex];

            audio.HandleInput(keyboardState, lastKeyboardState);

            if (input.IsMenuSelect(ControllingPlayer, out PlayerIndex))
            {
                OnSelectEntry(selectedEntry, PlayerIndex);
            }
            else if (input.IsMenuCancel(ControllingPlayer, out PlayerIndex))
            {
                OnCancel(PlayerIndex);
            }

            // Event handler for when the Sound Volume menu entry is selected.
            if (MenuEntries[selectedEntry] == soundVolumeMenuEntry
                && input.IsMenuLeft(ControllingPlayer) && soundVolume > 0)
            {
                MediaPlayer.Volume -= 0.0999f;
                soundVolume = (int)(MediaPlayer.Volume * 10);
                SetMenuEntryText();
            }
            if (MenuEntries[selectedEntry] == soundVolumeMenuEntry
                && input.IsMenuRight(ControllingPlayer) && soundVolume < 10)
            {
                MediaPlayer.Volume += 0.0999f;
                soundVolume = (int)(MediaPlayer.Volume * 10);
                SetMenuEntryText();
            }

            // Event handler for when the Sound FX Volume menu entry is selected.
            if (input.IsMenuLeft(ControllingPlayer) && MenuEntries[selectedEntry] == fxVolumeMenuEntry)
            {
                fxVolume--;
                SetMenuEntryText();
            }
            if (input.IsMenuRight(ControllingPlayer) && MenuEntries[selectedEntry] == fxVolumeMenuEntry)
            {
                fxVolume++;
                SetMenuEntryText();
            }

            base.HandleInput(input);
        }
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

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            if (IsPopup)
                ScreenManager.AddScreen(new PauseMenuScreen(1, mod), playerIndex, true);
            audio.Close();
            ExitScreen();
        }

        /// <summary>
        /// Updates the menu.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            audio.Update(gameTime);
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            audio.Draw(gameTime);

            // Tout ça ne sert qu'à faire les pauvres rectangles noirs.
            spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            // Rectangle noir des entrées menu
            spriteBatch.Draw(blankTexture,
                             new Rectangle(115, 210, 300, 235),
                             new Color(0, 0, 0, (byte)(TransitionAlpha * 2 / 3)));

            // Celui du titre
            spriteBatch.Draw(blankTexture,
                            new Rectangle(510, 60, 230, 80),
                            new Color(0, 0, 0, (byte)(TransitionAlpha * 2 / 3)));

            spriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion
    }
}
