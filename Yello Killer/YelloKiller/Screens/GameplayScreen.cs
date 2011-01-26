#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Yellokiller.Yello_Killer;
#endregion

namespace Yellokiller
{
    public enum TypeCase
    {
        herbe = 'h',
        herbeFoncee = 'H',
        arbre = 'a',
        mur = 'm',
        maison = 'M',
        origineEnnemi = 'E',
        origineJoueur1 = 'o',
        origineJoueur2 = 'O'
    };

    public class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        public ContentManager Content { get { return content; } }
        SpriteFont gameFont;
        SpriteBatch spriteBatch;
        Hero1 hero1;
        Hero2 hero2;
        Carte carte;
        Rectangle camera;
        Player audio;
        List<Shuriken> _shuriken;
        List<Ennemi> _ennemis;


        #endregion

        #region Initialization

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            audio = new Player(1);
            carte = new Carte(new Vector2(Taille_Map.LARGEUR_MAP, Taille_Map.HAUTEUR_MAP));
            carte.OuvrirCarte("save0.txt");
            _shuriken = new List<Shuriken>();
            camera = new Rectangle(0, 0, 32, 24);
            hero1 = new Hero1(28 * carte.origineJoueur1, new Rectangle(25, 133, 16, 25), TypeCase.origineJoueur1);
            hero2 = new Hero2(28 * carte.origineJoueur2, new Rectangle(25, 133, 16, 25), TypeCase.origineJoueur1);
            _ennemis = new List<Ennemi>();

            foreach (Vector2 position in carte._originesEnnemis)
                _ennemis.Add(new Ennemi(28 * position, new Rectangle(5, 1, 16, 23), TypeCase.origineEnnemi));
        }


        protected void Initialize()
        {
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            spriteBatch = ScreenManager.SpriteBatch;
            gameFont = content.Load<SpriteFont>("courier");

            audio.LoadContent(content);
            hero1.LoadContent(content, 2);
            hero2.LoadContent(content, 2);
            foreach (Ennemi mechant in _ennemis)
                mechant.LoadContent(content, 2);

            Thread.Sleep(1000);
            ScreenManager.Game.ResetElapsedTime();
        }

        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            ScreenManager.Game.IsMouseVisible = true;
            if (IsActive)
            {


                hero1.Update(gameTime, carte, hero2, this, ref camera, _shuriken);
                hero2.Update(gameTime, carte, hero1, this, ref camera, _shuriken);
                foreach (Ennemi pasgentil in _ennemis)
                    pasgentil.Update(gameTime, carte, this, hero1, hero2);
                audio.Update(gameTime);
                base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            }
        }



        public override void Draw(GameTime gameTime)
        {

            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.DarkOrchid, 0, 0);

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0)
                ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);

            spriteBatch.Begin();

            carte.DrawInGame(spriteBatch, content, camera);
            hero1.Draw(spriteBatch, gameTime, camera, carte, hero1);
            hero2.Draw(spriteBatch, gameTime, camera, carte, hero2);
            foreach (Ennemi connard in _ennemis)
                connard.Draw(spriteBatch);
            for (int i = 0; i < _shuriken.Count; i++)
            {
                Shuriken m = _shuriken[i];
                m.Update(gameTime, carte);
                m.Draw(spriteBatch, camera);

                if (m.Get_X() > Taille_Map.LARGEUR_MAP * 28 || _shuriken[i].existshuriken == false)
                {
                    _shuriken.Remove(m);
                    Console.WriteLine("suppresion shuriken");
                }
            }
            spriteBatch.End();
            audio.Draw(gameTime);
            base.Draw(gameTime);
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new Pausebckground(), ControllingPlayer, true);
                ScreenManager.AddScreen(new PauseMenuScreen(0, 1), ControllingPlayer, true);
            }

            if (ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.G))
                LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameOverScreen());

            // Looks up input for the Media Player.
            audio.HandleInput();
        }
        #endregion
    }
}