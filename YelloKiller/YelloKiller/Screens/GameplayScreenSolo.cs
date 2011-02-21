using System;
using System.Threading;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    public enum TypeCase // Valeur strictement positive pour les cases franchissables, negative sinon.
    {
        arbre = -1,
        arbre2 = -2,
        buissonSurHerbe = -3,
        murBlanc = -4,
        tableauMurBlanc = -5,

        bois = 1,
        boisCarre = 2,
        tapisRougeBC = 3,
        herbe = 4,
        herbeFoncee = 5,
        piedDeMurBois = 6,
        terre = 7,

        fond = -6,

        Ennemi = 100,
        Joueur1 = 101,
        Joueur2 = 102
    }

    public class GameplayScreenSolo : GameScreen
    {
        #region Fields

        ContentManager content;
        public ContentManager Content { get { return content; } }
        SpriteFont gameFont;
        SpriteBatch spriteBatch;
        Hero hero;
        Carte carte;
        Rectangle camera;
        List<Shuriken> _shuriken;
        List<Ennemi> _ennemis;
        Player audio;
        MoteurAudio moteurAudio;
        double temps;

        #endregion

        #region Initialization

        public GameplayScreenSolo()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            audio = new Player();
            moteurAudio = new MoteurAudio();

            carte = new Carte(new Vector2(Taille_Map.LARGEUR_MAP, Taille_Map.HAUTEUR_MAP));
            carte.OuvrirCarteSolo("Ssave0.txt");

            camera = new Rectangle(0, 0, 32, 24);

            _shuriken = new List<Shuriken>();

            hero = new Hero(new Vector2(28 * carte.origineJoueur1.X + 5, 28 * carte.origineJoueur1.Y + 1), new Rectangle(25, 133, 16, 25), TypeCase.Joueur1);

            if (28 * carte.origineJoueur1.X - 440 >= 0)
                camera.X = 28 * (int)carte.origineJoueur1.X - 440;
            else
                camera.X = 0;

            if (28 * carte.origineJoueur1.Y - 322 >= 0)
                camera.Y = 28 * (int)carte.origineJoueur1.Y - 322;
            else
                camera.Y = 0;
 
            _ennemis = new List<Ennemi>();

            foreach (Vector2 position in carte._originesEnnemis)
                _ennemis.Add(new Ennemi(new Vector2(28 * position.X + 5, 28 * position.Y), new Rectangle(5, 1, 16, 23), TypeCase.Ennemi));

            temps = 0;
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            spriteBatch = ScreenManager.SpriteBatch;
            gameFont = content.Load<SpriteFont>("courier");

            audio.LoadContent(content);

            hero.LoadContent(content, 2);

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
            if (IsActive)
            {
                temps += gameTime.ElapsedGameTime.TotalSeconds;

                moteurAudio.Update();

                hero.Update(gameTime, carte, this, ref camera, _shuriken);

                foreach (Ennemi pasgentil in _ennemis)
                    pasgentil.Update(gameTime, carte/*, this, hero*/);
                
                Moteur_physique.Collision_Shuriken_Ennemi(_ennemis, _shuriken, moteurAudio.SoundBank);

                if(Moteur_physique.Collision_Ennemi_Hero(_ennemis, hero, moteurAudio.SoundBank))
                    LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameOverScreen(1));

                audio.Update(gameTime);
            }
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }



        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.DarkOrchid, 0, 0);

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0)
                ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);

            spriteBatch.Begin();

            carte.DrawInGame(spriteBatch, content, camera);
            hero.Draw(spriteBatch, gameTime, camera, carte, hero);

            spriteBatch.DrawString(ScreenManager.font, "Il reste " + _ennemis.Count.ToString() + " ennemis.", new Vector2(0, Taille_Ecran.HAUTEUR_ECRAN - 25), Color.BurlyWood);

            spriteBatch.DrawString(ScreenManager.font, Temps.Conversion(temps), new Vector2(Taille_Ecran.LARGEUR_ECRAN - 60, Taille_Ecran.HAUTEUR_ECRAN - 25), Color.DarkRed);

            foreach (Ennemi connard in _ennemis)
                connard.Draw(spriteBatch, camera);

            for (int i = 0; i < _shuriken.Count; i++)
            {
                Shuriken m = _shuriken[i];
                m.Update(gameTime, carte);
                m.Draw(spriteBatch, camera);

                if (m.Position.X > Taille_Map.LARGEUR_MAP * 28 || _shuriken[i].existshuriken == false)
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
            {
                LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameOverScreen(1));
            }

            // Looks up input for the Media Player.
            audio.HandleInput();
        }
        #endregion
    }
}