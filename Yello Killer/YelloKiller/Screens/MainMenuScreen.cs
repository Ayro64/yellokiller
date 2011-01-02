#region File Description
//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace Yellokiller
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        // Sert à keud'
        #region Properties (provisoire)

        ContentManager content;
        SpriteBatch spriteBatch;
        Texture2D blankTexture;

        #endregion
        //

        #region Initialization


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base(" Yello Killer\nMenu Principal")
        {
            // Create our menu entries.
            MenuEntry jouerMenuEntry = new MenuEntry("\"Jouer\"");
            MenuEntry editorMenuEntry = new MenuEntry("Editeur de cartes");
            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry exitMenuEntry = new MenuEntry("Quitter");

            // Hook up menu event handlers.
            jouerMenuEntry.Selected += JouerMenuEntrySelected;
            editorMenuEntry.Selected += EditorMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(jouerMenuEntry);
            MenuEntries.Add(editorMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
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
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void JouerMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen());
        }


        ///<summary>
        ///Event handler for when the Map Editor menu entry is selected.
        ///</summary>
        void EditorMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                                new EditorScreen());
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(0), e.PlayerIndex);
        }


        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Êtes-vous sûr de vouloir quitter le jeu?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            // Tout ça ne sert qu'à faire les pauvres rectangles noirs.
            spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            // Rectangle noir des entrées menu
            spriteBatch.Draw(blankTexture,
                             new Rectangle(115, 210, 215, 190),
                             new Color(0, 0, 0, (byte)(TransitionAlpha * 2 / 3)));

            // Celui du titre
            spriteBatch.Draw(blankTexture,
                            new Rectangle(510, 60, 230, 80),
                            new Color(0, 0, 0, (byte)(TransitionAlpha * 2 / 3)));

            spriteBatch.End();

            base.Draw(gameTime);
        }

        //

        #endregion
    }
}
