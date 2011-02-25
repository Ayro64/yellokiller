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
        finMurDroite = -7,
        finMurFN = -8,
        finMurGauche = -9,
        fondNoir = -10,
        commode = -11,
        GrandeTable = -12,
        Lit = -13,
        TableMoyenne = -14,

        bois = 1,
        boisCarre = 2,
        tapisRougeBC = 3,
        herbe = 4,
        herbeFoncee = 5,
        piedDeMurBois = 6,
        terre = 7,
        carlageNoir = 8,

        fond = -6,

        Joueur1 = 100,
        Joueur2 = 101,
        Garde = 102,
        Patrouilleur = 103,
        Patrouilleur_a_cheval = 104,
        Boss = 105,
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
        List<Garde> _gardes;
        List<Patrouilleur> _patrouilleurs;
        List<Patrouilleur_a_cheval> _patrouilleurs_a_chevaux;
        List<Boss> _boss;

        Player audio;
        MoteurAudio moteurAudio;
        double temps;
        string nomDeCarte;

        #endregion

        #region Initialization

        public GameplayScreenSolo(string nomDeCarte)
        {
            this.nomDeCarte = nomDeCarte;

            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            audio = new Player();
            moteurAudio = new MoteurAudio();

            carte = new Carte(new Vector2(Taille_Map.LARGEUR_MAP, Taille_Map.HAUTEUR_MAP));
            carte.OuvrirCarte(nomDeCarte);

            camera = new Rectangle(0, 0, 32, 24);

            _shuriken = new List<Shuriken>();

            hero = new Hero(new Vector2(28 * carte.OrigineJoueur1.X + 5, 28 * carte.OrigineJoueur1.Y + 1), Keys.Up, Keys.Down, Keys.Right, Keys.Left, Keys.RightControl, Keys.RightShift, 1, 50);

            // Centre la camera sur le personnage.
            if (28 * carte.OrigineJoueur1.X - 440 < 0)
                camera.X = 0;
            else if (28 * carte.OrigineJoueur1.X + 440 > 28 * Taille_Map.LARGEUR_MAP)
                camera.X = 28 * (Taille_Map.LARGEUR_MAP - 32);
            else
                camera.X = 28 * (int)carte.OrigineJoueur1.X - 500;

            if (28 * carte.OrigineJoueur1.Y - 322 < 0)
                camera.Y = 0;
            else if (28 * carte.OrigineJoueur1.Y + 322 > 28 * Taille_Map.HAUTEUR_MAP)
                camera.Y = 28 * (Taille_Map.HAUTEUR_MAP - 28);
            else
                camera.Y = 28 * (int)carte.OrigineJoueur1.Y - 400;

            _gardes = new List<Garde>();
            foreach (Vector2 position in carte.OriginesGardes)
                _gardes.Add(new Garde(new Vector2(28 * position.X + 5, 28 * position.Y)));

            _patrouilleurs = new List<Patrouilleur>();
            foreach (Vector2 position in carte.OriginesPatrouilleurs)
                _patrouilleurs.Add(new Patrouilleur(new Vector2(28 * position.X + 5, 28 * position.Y)));
            
            _patrouilleurs_a_chevaux = new List<Patrouilleur_a_cheval>();
            foreach (Vector2 position in carte.OriginesPatrouilleursAChevaux)
                _patrouilleurs_a_chevaux.Add(new Patrouilleur_a_cheval(new Vector2(28 * position.X + 5, 28 * position.Y)));

            _boss = new List<Boss>();
            foreach (Vector2 position in carte.OriginesBoss)
                _boss.Add(new Boss(new Vector2(28 * position.X + 5, 28 * position.Y)));

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

            foreach (Garde mechant in _gardes)
                mechant.LoadContent(content, 2);

            foreach (Patrouilleur mechant in _patrouilleurs)
                mechant.LoadContent(content, 2);

            foreach (Patrouilleur_a_cheval mechant in _patrouilleurs_a_chevaux)
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

                hero.Update(gameTime, carte, ref camera, _shuriken, moteurAudio, content, null);

                foreach (Garde pasgentil in _gardes)
                    pasgentil.Update(gameTime, carte, hero, camera);

                ServiceHelper.Game.Window.Title = "Camera.X = " + (int)camera.X + " ; Camera.Y = " + (int)camera.Y + " _ Hero.X = " + hero.position.X + " ; Hero.Y = " + hero.position.Y;

                foreach (Patrouilleur pasgentil in _patrouilleurs)
                    pasgentil.Update(gameTime, carte, hero, camera);

                foreach (Patrouilleur_a_cheval pasgentil in _patrouilleurs_a_chevaux)
                    pasgentil.Update(gameTime, carte, hero, camera);
                /*
                foreach (Boss pasgentil in _boss)
                    pasgentil.Update(gameTime);*/

                Moteur_physique.Collision_Shuriken_Ennemis(_gardes, _patrouilleurs, _patrouilleurs_a_chevaux, _boss, _shuriken, moteurAudio.SoundBank);

                if (Moteur_physique.Collision_Garde_Hero(_gardes, hero, moteurAudio.SoundBank))
                    LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameOverScreen(nomDeCarte));
                if (Moteur_physique.Collision_Patrouilleur_Hero(_patrouilleurs, hero, moteurAudio.SoundBank))
                    LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameOverScreen(nomDeCarte));
                if (Moteur_physique.Collision_PatrouilleurACheval_Hero(_patrouilleurs_a_chevaux, hero, moteurAudio.SoundBank))
                    LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameOverScreen(nomDeCarte));
                if (Moteur_physique.Collision_Boss_Hero(_boss, hero, moteurAudio.SoundBank))
                    LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameOverScreen(nomDeCarte));

                if (_boss.Count == 0)
                {
                    LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameWin(nomDeCarte));
                    audio.Close();
                    moteurAudio.SoundBank.PlayCue("11 Fanfare");
                }

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
            hero.Draw(spriteBatch, gameTime, camera);

            spriteBatch.DrawString(ScreenManager.font, "Il reste " + _gardes.Count.ToString() + " ennemis.", new Vector2(0, Taille_Ecran.HAUTEUR_ECRAN - 25), Color.BurlyWood);

            spriteBatch.DrawString(ScreenManager.font, Temps.Conversion(temps), new Vector2(Taille_Ecran.LARGEUR_ECRAN - 60, Taille_Ecran.HAUTEUR_ECRAN - 25), Color.DarkRed);

            foreach (Garde connard in _gardes)
                connard.Draw(spriteBatch, camera);

            foreach (Patrouilleur connard in _patrouilleurs)
                connard.Draw(spriteBatch, camera);

            foreach (Patrouilleur_a_cheval connard in _patrouilleurs_a_chevaux)
                connard.Draw(spriteBatch, camera);

            foreach (Boss connard in _boss)
                connard.Draw(spriteBatch, camera);

            for (int i = 0; i < _shuriken.Count; i++)
            {
                _shuriken[i].Update(gameTime, carte);
                _shuriken[i].Draw(spriteBatch, camera);

                if (_shuriken[i].ShurikenExists == false)
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
                moteurAudio.SoundBank.PlayCue("menuBouge");
                ScreenManager.AddScreen(new Pausebckground(), ControllingPlayer, true);
                ScreenManager.AddScreen(new PauseMenuScreen(0, 1), ControllingPlayer, true);
            }

            if (ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.G))
            {
                LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameOverScreen(nomDeCarte));
            }

            if (ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.W))
            {
                audio.Close();
                moteurAudio.SoundBank.PlayCue("11 Fanfare");
                LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameWin(nomDeCarte));
            }

            // Looks up input for the Media Player.
            audio.HandleInput();
        }
        #endregion
    }
}