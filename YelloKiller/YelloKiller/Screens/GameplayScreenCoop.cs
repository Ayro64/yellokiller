using System;
using System.Threading;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    public class GameplayScreenCoop : GameScreen
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

        List<Shuriken> _shuriken;
        List<Garde> _gardes;
        List<Patrouilleur> _patrouilleurs;
        List<patrouilleur_a_cheval> _patrouilleurs_a_chevaux;
        List<Boss> _boss;

        Player audio;
        MoteurAudio moteurAudio;
        double temps;

        #endregion

        #region Initialization

        public GameplayScreenCoop()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            audio = new Player();
            moteurAudio = new MoteurAudio();

            carte = new Carte(new Vector2(Taille_Map.LARGEUR_MAP, Taille_Map.HAUTEUR_MAP));
            carte.OuvrirCarte("Csave0.txt", 2);

            _shuriken = new List<Shuriken>();

            camera = new Rectangle(0, 0, 32, 24);

            hero1 = new Hero1(new Vector2(28 * carte.OrigineJoueur1.X + 5, 28 * carte.OrigineJoueur1.Y), new Rectangle(25, 133, 16, 25), TypeCase.Joueur1);
            hero2 = new Hero2(new Vector2(28 * carte.OrigineJoueur2.X + 5, 28 * carte.OrigineJoueur2.Y), new Rectangle(25, 133, 16, 25), TypeCase.Joueur1);

            // Centre la camera sur le personnage.
            if (28 * carte.OrigineJoueur1.X - 440 < 0)
                camera.X = 0;
            else if (28 * carte.OrigineJoueur1.X + 440 > 28 * Taille_Map.LARGEUR_MAP)
                camera.X = 28 * (Taille_Map.LARGEUR_MAP - 34);
            else
                camera.X = 28 * (int)carte.OrigineJoueur1.X - 500;

            if (28 * carte.OrigineJoueur1.Y - 322 < 0)
                camera.Y = 0;
            else if (28 * carte.OrigineJoueur1.Y + 322 > 28 * Taille_Map.HAUTEUR_MAP)
                camera.Y = 28 * (Taille_Map.HAUTEUR_MAP - 26);
            else
                camera.Y = 28 * (int)carte.OrigineJoueur1.Y - 400;

            _gardes = new List<Garde>();
            foreach (Vector2 position in carte.OriginesGardes)
                _gardes.Add(new Garde(new Vector2(28 * position.X + 5, 28 * position.Y), new Rectangle(24, 64, 16, 24), TypeCase.Garde));

            _patrouilleurs = new List<Patrouilleur>();
            foreach (Vector2 position in carte.OriginesPatrouilleurs)
                _patrouilleurs.Add(new Patrouilleur(new Vector2(28 * position.X + 5, 28 * position.Y), new Rectangle(24, 0, 19, 26), TypeCase.Patrouilleur));

            _patrouilleurs_a_chevaux = new List<patrouilleur_a_cheval>();
            foreach (Vector2 position in carte.OriginesPatrouilleursAChevaux)
                _patrouilleurs_a_chevaux.Add(new patrouilleur_a_cheval(new Vector2(28 * position.X + 5, 28 * position.Y), new Rectangle(24, 0, 23, 30), TypeCase.Patrouilleur_a_cheval));

            _boss = new List<Boss>();
            foreach (Vector2 position in carte.OriginesBoss)
                _boss.Add(new Boss(new Vector2(28 * position.X + 5, 28 * position.Y), new Rectangle(26, 64, 18, 26), TypeCase.Boss));

            temps = 0;
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

            foreach (Garde mechant in _gardes)
                mechant.LoadContent(content, 2);

            foreach (Patrouilleur mechant in _patrouilleurs)
                mechant.LoadContent(content, 2);

            foreach (patrouilleur_a_cheval mechant in _patrouilleurs_a_chevaux)
                mechant.LoadContent(content, 2);

            foreach (Boss mechant in _boss)
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

                hero1.Update(gameTime, carte, hero2, this, ref camera, _shuriken, moteurAudio);
                hero2.Update(gameTime, carte, hero1, this, _shuriken, moteurAudio);

                foreach (Garde pasgentil in _gardes)
                    pasgentil.UpdateInCoop(gameTime, carte, hero1, hero2);

                foreach (Patrouilleur pasgentil in _patrouilleurs)
                    pasgentil.UpdateInCoop(gameTime, carte, hero1, hero2);

                foreach (patrouilleur_a_cheval pasgentil in _patrouilleurs_a_chevaux)
                    pasgentil.UpdateInCoop(gameTime, carte, hero1, hero2);

                foreach (Boss pasgentil in _boss)
                    pasgentil.UpdateInCoop(gameTime, carte, hero1, hero2);

                Moteur_physique.Collision_Shuriken_Ennemis(_gardes, _patrouilleurs, _patrouilleurs_a_chevaux, _boss, _shuriken, moteurAudio.SoundBank);

                if (Moteur_physique.Collision_Garde_Heros(_gardes, hero1, hero2, moteurAudio.SoundBank))
                    LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameOverScreen(0));

                if (Moteur_physique.Collision_Patrouilleur_Heros(_patrouilleurs, hero1, hero2, moteurAudio.SoundBank))
                    LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameOverScreen(0));

                if (Moteur_physique.Collision_PatrouilleurACheval_Heros(_patrouilleurs_a_chevaux, hero1, hero2, moteurAudio.SoundBank))
                    LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameOverScreen(0));

                if (Moteur_physique.Collision_Boss_Heros(_boss, hero1, hero2, moteurAudio.SoundBank))
                    LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameOverScreen(0));

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
            hero1.Draw(spriteBatch, gameTime, camera, carte);
            hero2.Draw(spriteBatch, gameTime, camera, carte);

            spriteBatch.DrawString(ScreenManager.font, "Il reste " + _gardes.Count.ToString() + " ennemis.", new Vector2(0, Taille_Ecran.HAUTEUR_ECRAN - 25), Color.BurlyWood);

            spriteBatch.DrawString(ScreenManager.font, Temps.Conversion(temps), new Vector2(Taille_Ecran.LARGEUR_ECRAN - 60, Taille_Ecran.HAUTEUR_ECRAN - 25), Color.DarkRed);

            foreach (Garde connard in _gardes)
                connard.Draw(spriteBatch, camera);

            foreach (Patrouilleur connard in _patrouilleurs)
                connard.Draw(spriteBatch, camera);

            foreach (patrouilleur_a_cheval connard in _patrouilleurs_a_chevaux)
                connard.Draw(spriteBatch, camera);

            foreach (Boss connard in _boss)
                connard.Draw(spriteBatch, camera);

            for (int i = 0; i < _shuriken.Count; i++)
            {
                _shuriken[i].Update(gameTime, carte);
                _shuriken[i].Draw(spriteBatch, camera);

                if (_shuriken[i].existshuriken == false)
                {
                    _shuriken.Remove(_shuriken[i]);
                    Console.WriteLine("suppresion shuriken");
                    moteurAudio.SoundBank.PlayCue("shurikenCollision");
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
                moteurAudio.SoundBank.PlayCue("menuBouge");
            }

            if (ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.G))
            {
                LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameOverScreen(2));
            }

            if (ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.W))
            {
                LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameWin(2));
            }

            // Looks up input for the Media Player.
            audio.HandleInput();
        }
        #endregion
    }
}