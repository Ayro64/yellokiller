#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
#endregion

namespace YelloKiller
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        KeyboardState keyboardState, lastKeyboardState;

        MenuEntry languageMenuEntry;
        MenuEntry fullScreenMenuEntry;
        MenuEntry sonMenuEntry;
        MenuEntry soundVolumeMenuEntry;
        MenuEntry fxVolumeMenuEntry;
        MenuEntry eraseMenuEntry;
        MenuEntry backMenuEntry;
        YellokillerGame game;

        int mod;

        string[] language = { "Français", "Deutsch", "English" };
        string[] son = { Langue.tr("SoundDefault"), "Player", Langue.tr("SoundNone") };

        uint currentLanguage;
        bool fullScreen;
        bool ToggleOK;
        uint currentSon;
        float soundVolume;
        uint fxVolume;

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
        public OptionsMenuScreen(int mode, YellokillerGame game)
            : base(Langue.tr("Options"))
        {
            this.game = game;
            // Fetches Settings.
            currentLanguage = Properties.Settings.Default.Language;
            fullScreen = Properties.Settings.Default.FullScreen;
            ToggleOK = true;
            currentSon = Properties.Settings.Default.AudioType;

            // soundVolume = (uint)(MediaPlayer.Volume * 10);
            soundVolume = Properties.Settings.Default.MusicVolume;
            fxVolume = Properties.Settings.Default.FXVolume;

            mod = mode;

            // Create our menu entries.
            languageMenuEntry = new MenuEntry(string.Empty);
            fullScreenMenuEntry = new MenuEntry(string.Empty);
            sonMenuEntry = new MenuEntry(string.Empty);
            soundVolumeMenuEntry = new MenuEntry(string.Empty);
            fxVolumeMenuEntry = new MenuEntry(string.Empty);
            eraseMenuEntry = new MenuEntry(string.Empty);
            backMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            // Hook up menu event handlers.
            languageMenuEntry.Selected += LanguageMenuEntrySelected;

            fullScreenMenuEntry.Selected += FullScreenMenuEntrySelected;

            sonMenuEntry.Selected += SonMenuEntrySelected;
            eraseMenuEntry.Selected += EraseMenuEntrySelected;
            backMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(languageMenuEntry);
            MenuEntries.Add(fullScreenMenuEntry);
            MenuEntries.Add(sonMenuEntry);
            MenuEntries.Add(soundVolumeMenuEntry);
            MenuEntries.Add(fxVolumeMenuEntry);
            MenuEntries.Add(eraseMenuEntry);
            MenuEntries.Add(backMenuEntry);
        }



        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            Properties.Settings.Default.Language = currentLanguage;
            Properties.Settings.Default.FullScreen = fullScreen;
            Properties.Settings.Default.AudioType = currentSon;
            Properties.Settings.Default.MusicVolume = soundVolume;
            Properties.Settings.Default.FXVolume = fxVolume;

            this.TitleUpdate(Langue.tr("Options"));
            languageMenuEntry.Text = Langue.tr("OptLan") + language[currentLanguage];
            fullScreenMenuEntry.Text = Langue.tr("FullScr") + (fullScreen ? Langue.tr("Yes") : Langue.tr("No"));
            sonMenuEntry.Text = Langue.tr("OptSound") + son[currentSon];
            soundVolumeMenuEntry.Text = Langue.tr("OptMusic") + (uint)soundVolume;
            fxVolumeMenuEntry.Text = Langue.tr("OptFX") + fxVolume;
            eraseMenuEntry.Text = Langue.tr("Erase");
            backMenuEntry.Text = Langue.tr("Back");
        }

        // Sert qu'aux rectangles
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            blankTexture = content.Load<Texture2D>("blank");
        }

        public override void UnloadContent()
        {
            content.Unload();
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
                soundVolume = (MediaPlayer.Volume * 10);
                SetMenuEntryText();
            }
            if (MenuEntries[selectedEntry] == soundVolumeMenuEntry
                && input.IsMenuRight(ControllingPlayer) && soundVolume < 10)
            {
                MediaPlayer.Volume += 0.0999f;
                soundVolume = (MediaPlayer.Volume * 10);
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
        /// Event handler for when the Language menu entry is selected.
        /// </summary>
        void LanguageMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentLanguage = (uint)((currentLanguage + 1) % language.Length);
            SetMenuEntryText();
        }

        /// <summary>
        /// Event handler for when the Full Screen menu entry is selected.
        /// </summary>
        void FullScreenMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if (ToggleOK)
            {
                fullScreen = !fullScreen;
                game.Toggle();
            }
            SetMenuEntryText();
            ToggleOK = !ToggleOK;
        }

        /// <summary>
        /// Event handler for when the Language menu entry is selected.
        /// </summary>
        void SonMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentSon = (uint)((currentSon + 1) % son.Length);
            SetMenuEntryText();
        }

        void EraseMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            string message = Langue.tr("SaveDel");
            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);
            confirmExitMessageBox.Accepted += MessageBoxAccepted;
            ScreenManager.AddScreen(confirmExitMessageBox, e.PlayerIndex);
        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to delete" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        void MessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            Properties.Unlocked.Default.C2 = false;
            Properties.Unlocked.Default.C3 = false;
            Properties.Unlocked.Default.C4 = false;
            Properties.Unlocked.Default.C5 = false;
            Properties.Unlocked.Default.C6 = false;
            Properties.Unlocked.Default.S2 = false;
            Properties.Unlocked.Default.S3 = false;
            Properties.Unlocked.Default.S4 = false;
            Properties.Unlocked.Default.S5 = false;
            Properties.Unlocked.Default.S6 = false;

            Properties.Unlocked.Default.Save();
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            Properties.Settings.Default.Save();
            ExitScreen();
        }

        #endregion

        #region Update and Draw

        public override void Draw(GameTime gameTime)
        {
            if (mod == 0)
            {
                Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
                SpriteFont font = ScreenManager.Font;
                float titleSize = font.MeasureString(Langue.tr("Options")).X;

                // Tout ça ne sert qu'à faire les pauvres rectangles noirs.
                spriteBatch = ScreenManager.SpriteBatch;

                spriteBatch.Begin();
                // Rectangle noir des entrées menu.
                spriteBatch.Draw(blankTexture,
                                 new Rectangle(115, 210, 300, 230),
                                 new Color(0, 0, 0, (byte)(TransitionAlpha * 2 / 3)));

                // Celui du titre.
                spriteBatch.Draw(blankTexture,
                             new Rectangle((int)((viewport.Width / 2) - ((titleSize * 1.25f / 2) + 10)), 75, (int)(titleSize * 1.25f) + 20, 50),
                             new Color(0, 0, 0, (byte)(TransitionAlpha * 2 / 3)));

                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        #endregion
    }
}
