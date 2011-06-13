using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{

    public class YellokillerGame : Microsoft.Xna.Framework.Game
    {
        #region Fields

        GraphicsDeviceManager graphics;
        ScreenManager screenManager;

        #endregion

        #region Initialization

        public YellokillerGame()
        {
            EventInput.EventInput.Initialize(this.Window);

            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferHeight = Taille_Ecran.HAUTEUR_ECRAN;
            graphics.PreferredBackBufferWidth = Taille_Ecran.LARGEUR_ECRAN;
          this.graphics.IsFullScreen = true;

            ServiceHelper.Game = this;
            Components.Add(new KeyboardService(this));
            Components.Add(new MouseService(this));
            Components.Add(new GamePadService(this));

            // Create the screen manager component.
            screenManager = new ScreenManager(this);


            Components.Add(screenManager);

            // Activate the first screens.
            screenManager.AddScreen(new IntroScreen(this), null);
        }
        #endregion

    }

    #region Entry Point


    static class Program
    {
        static void Main()
        {
            using (YellokillerGame game = new YellokillerGame())
            {
                game.Run();
            }
        }
    }

    #endregion
}