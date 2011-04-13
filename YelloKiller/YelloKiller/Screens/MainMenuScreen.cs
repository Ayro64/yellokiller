#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ParticleSample;
#endregion

namespace YelloKiller
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        // Sert à keud'
        #region Properties

        SpriteBatch spriteBatch;
        ContentManager content;
        Texture2D blankTexture;

        #endregion
        //

        #region Fields

        MenuEntry soloMenuEntry;
        MenuEntry coopMenuEntry;
        MenuEntry editorMenuEntry;
        MenuEntry optionsMenuEntry;
        MenuEntry exitMenuEntry;
        YellokillerGame game;
        SmokePlumeParticleSystem fume; // fumigene

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen(YellokillerGame game)
            : base(Langue.tr("MainMenuTitle"))
        {
            
            this.game = game;

            // Create our menu entries.
            soloMenuEntry = new MenuEntry(Langue.tr("MainMenuSolo"));
            coopMenuEntry = new MenuEntry(Langue.tr("MainMenuCoop"));
            editorMenuEntry = new MenuEntry(Langue.tr("MainMenuEditor"));
            optionsMenuEntry = new MenuEntry(Langue.tr("Options"));
            exitMenuEntry = new MenuEntry(Langue.tr("MainMenuQuit"));

            // Hook up menu event handlers.
            soloMenuEntry.Selected += SoloMenuEntrySelected;
            coopMenuEntry.Selected += CoopMenuEntrySelected;
            editorMenuEntry.Selected += EditorMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(soloMenuEntry);
            MenuEntries.Add(coopMenuEntry);
            MenuEntries.Add(editorMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        /// <summary>
        /// Fills in the latest values for the main menu screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            this.TitleUpdate(Langue.tr("MainMenuTitle"));
            soloMenuEntry.Text = Langue.tr("MainMenuSolo");
            coopMenuEntry.Text = Langue.tr("MainMenuCoop");
            editorMenuEntry.Text = Langue.tr("MainMenuEditor");
            optionsMenuEntry.Text = Langue.tr("Options");
            exitMenuEntry.Text = Langue.tr("MainMenuQuit");
        }

        // Sert qu'aux rectangles
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            blankTexture = content.Load<Texture2D>("blank");

            fume = new SmokePlumeParticleSystem(game, 9);
            game.Components.Add(fume);
        }
        //

        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Play Solo Game menu entry is selected.
        /// </summary>
        void SoloMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, e.PlayerIndex, new LevelSelectSolo(game));
        }

        /// <summary>
        /// Event handler for when the Play Coop Game menu entry is selected.
        /// </summary>
        void CoopMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new LevelSelectMulti(game));
        }

        ///<summary>
        ///Event handler for when the Map Editor menu entry is selected.
        ///</summary>
        void EditorMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new EditorScreen("", game));
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(0, game), e.PlayerIndex);
        }


        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            string message = Langue.tr("MainQuitMsg");

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

        #region Update and Draw

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            SetMenuEntryText();
            fume.AddParticles(new Vector2(Taille_Ecran.HAUTEUR_ECRAN / 2, Taille_Ecran.LARGEUR_ECRAN));
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

        }

        public override void Draw(GameTime gameTime)
        {
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            SpriteFont font = ScreenManager.Font;
            float titleSize = font.MeasureString(Langue.tr("MainMenuTitle")).X;

            // Sert qu'aux rectangles noirs.
            spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            // Rectangle noir des entrées menu
            spriteBatch.Draw(blankTexture,
                             new Rectangle(115, 220, 230, 190),
                             new Color(0, 0, 0, (byte)(TransitionAlpha * 2 / 3)));

            // Celui du titre
            spriteBatch.Draw(blankTexture,
                             new Rectangle((int)((viewport.Width / 2) - ((titleSize * 1.25f / 2) + 10)) , 60, (int)(titleSize * 1.25f) + 20, 80),
                             new Color(0, 0, 0, (byte)(TransitionAlpha * 2 / 3)));

            spriteBatch.End();

            base.Draw(gameTime);
        }

        //

        #endregion
    }
}
