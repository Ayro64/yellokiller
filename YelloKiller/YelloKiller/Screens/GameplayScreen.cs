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

    public class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        public ContentManager Content { get { return content; } }
        SpriteFont gameFont;
        SpriteBatch spriteBatch;
        Hero hero1, hero2;
        Carte carte;
        Rectangle camera;
        Case caseDepart;

        List<Shuriken> _shuriken;
        List<Garde> _gardes;
        List<Patrouilleur> _patrouilleurs;
        List<Patrouilleur_a_cheval> _patrouilleurs_a_chevaux;
        List<Boss> _boss;

        Player audio;
        MoteurAudio moteurAudio;
        double temps;
        string nomDeCarte;
        bool jeuEnCoop;

        #endregion

        #region Initialization

        public GameplayScreen(string nomDeCarte)
        {
            this.nomDeCarte = nomDeCarte;
            jeuEnCoop = nomDeCarte[0] == 'C';

            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            audio = new Player();
            moteurAudio = new MoteurAudio();

            carte = new Carte(new Vector2(Taille_Map.LARGEUR_MAP, Taille_Map.HAUTEUR_MAP));
            carte.OuvrirCarte(nomDeCarte);

            _shuriken = new List<Shuriken>();

            camera = new Rectangle(0, 0, 36, 27);

            hero1 = new Hero(new Vector2(28 * carte.OrigineJoueur1.X + 5, 28 * carte.OrigineJoueur1.Y + 1), Keys.Up, Keys.Down, Keys.Right, Keys.Left, Keys.RightControl, Keys.RightShift, 1, 50);
            caseDepart = carte.Cases[(int)carte.OrigineJoueur1.Y, (int)carte.OrigineJoueur1.X];
            if (jeuEnCoop)
                hero2 = new Hero(new Vector2(28 * carte.OrigineJoueur2.X + 5, 28 * carte.OrigineJoueur2.Y + 1), Keys.Z, Keys.S, Keys.D, Keys.Q, Keys.Space, Keys.LeftShift, 2, 25);

            // Centre la camera sur le personnage... plus ou moins...
            if (carte.OrigineJoueur1.X - 16 < 0)
                camera.X = 0;
            else if (carte.OrigineJoueur1.X + 16 > Taille_Map.LARGEUR_MAP - 1)
                camera.X = 28 * (Taille_Map.LARGEUR_MAP - 37);
            else
                camera.X = 28 * ((int)carte.OrigineJoueur1.X - 16);

            if (carte.OrigineJoueur1.Y - 12 < 0)
                camera.Y = 0;
            else if (carte.OrigineJoueur1.Y + 12 > Taille_Map.HAUTEUR_MAP - 1)
                camera.Y = 28 * (Taille_Map.HAUTEUR_MAP - 28);
            else
                camera.Y = 28 * ((int)carte.OrigineJoueur1.Y - 12);

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

            hero1.LoadContent(content, 2);
            if (jeuEnCoop)
                hero2.LoadContent(content, 2);

            foreach (Garde garde in _gardes)
                garde.LoadContent(content, 2);

            foreach (Patrouilleur patrouilleur in _patrouilleurs)
                patrouilleur.LoadContent(content, 2);

            foreach (Patrouilleur_a_cheval patrouilleurACheval in _patrouilleurs_a_chevaux)
                patrouilleurACheval.LoadContent(content, 2);

            foreach (Boss boss in _boss)
                boss.LoadContent(content, 2);

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

                hero1.Update(gameTime, carte, ref camera, _shuriken, moteurAudio, content, hero2);
                if (jeuEnCoop)
                    hero2.Update(gameTime, carte, ref camera, _shuriken, moteurAudio, content, hero1);

                foreach (Garde garde in _gardes)
                    garde.Update(gameTime, carte, hero1, hero2, camera);

                foreach (Patrouilleur patrouilleur in _patrouilleurs)
                    patrouilleur.Update(gameTime, carte, hero1, camera);

                foreach (Patrouilleur_a_cheval patrouilleurACheval in _patrouilleurs_a_chevaux)
                    patrouilleurACheval.Update(gameTime, carte, hero1, camera);
                
                foreach (Boss boss in _boss)
                    boss.Update(gameTime, _shuriken, carte, hero1, hero2, camera);
                
                Moteur_physique.Collision_Shuriken_Ennemis(_gardes, _patrouilleurs, _patrouilleurs_a_chevaux, _boss, _shuriken, moteurAudio.SoundBank);

                if (Moteur_physique.Collision_Garde_Heros(_gardes, hero1, hero2, moteurAudio.SoundBank))
                    LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameOverScreen(nomDeCarte));

                if (Moteur_physique.Collision_Patrouilleur_Heros(_patrouilleurs, hero1, hero2, moteurAudio.SoundBank))
                    LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameOverScreen(nomDeCarte));

                if (Moteur_physique.Collision_PatrouilleurACheval_Heros(_patrouilleurs_a_chevaux, hero1, hero2, moteurAudio.SoundBank))
                    LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameOverScreen(nomDeCarte));

                if (Moteur_physique.Collision_Boss_Heros(_boss, hero1, hero2, moteurAudio.SoundBank))
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
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0)
                ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);

            spriteBatch.Begin();

            carte.DrawInGame(spriteBatch, content, camera);
            hero1.Draw(spriteBatch, gameTime, camera);
            if (jeuEnCoop)
                hero2.Draw(spriteBatch, gameTime, camera);

            spriteBatch.DrawString(ScreenManager.font, "Il reste " + _gardes.Count.ToString() + " ennemis.", new Vector2(0, Taille_Ecran.HAUTEUR_ECRAN - 25), Color.BurlyWood);

            spriteBatch.DrawString(ScreenManager.font, Temps.Conversion(temps), new Vector2(Taille_Ecran.LARGEUR_ECRAN - 60, Taille_Ecran.HAUTEUR_ECRAN - 25), Color.DarkRed);

            foreach (Garde garde in _gardes)
                garde.Draw(spriteBatch, camera);

            foreach (Patrouilleur patrouilleur in _patrouilleurs)
                patrouilleur.Draw(spriteBatch, camera);

            foreach (Patrouilleur_a_cheval patrouilleurACheval in _patrouilleurs_a_chevaux)
                patrouilleurACheval.Draw(spriteBatch, camera);

            foreach (Boss boss in _boss)
                boss.Draw(spriteBatch, camera);

            for (int i = 0; i < _shuriken.Count; i++)
            {
                _shuriken[i].Update(gameTime, carte);
                _shuriken[i].Draw(spriteBatch, camera);

                if (_shuriken[i].ShurikenExists == false)
                {
                    
                    _shuriken.Remove(_shuriken[i]);
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
                moteurAudio.SoundBank.PlayCue("metalgear");
                audio.Close();
                LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameOverScreen(nomDeCarte));
            }

            if (ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.W))
            {              
                moteurAudio.SoundBank.PlayCue("11 Fanfare");
                audio.Close();
                LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameWin(nomDeCarte));                
            }

            if (ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.M))
            {
                moteurAudio.SoundBank.PlayCue("metalgear");
                audio.Close();
            }

            // Looks up input for the Media Player.
            audio.HandleInput();
        }
        #endregion
    }
}