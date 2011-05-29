using System;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using YelloKiller.Moteur_Particule;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{    
    public class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        public ContentManager Content { get { return content; } }
        SpriteFont gameFont;
        SpriteBatch spriteBatch;
        Hero hero1, hero2;
        uint ennemisTues, retries;
        Carte carte;
        Rectangle camera;
        MoteurParticule moteurparticule;
        List<Shuriken> _shuriken;
        List<Garde> _gardes;
        List<Patrouilleur> _patrouilleurs;
        List<Patrouilleur_a_cheval> _patrouilleurs_a_chevaux;
        List<Boss> _boss;
        List<Statue> _statues;
        List<Bonus> _bonus;
        List<Rectangle> gardesMorts, patrouilleursAChevauxMorts, patrouilleursMorts, bossMorts;
        Texture2D textureGardeMort, texturePatrouilleurAChevalMort, texturePatrouilleurMort, textureBossMort, textureBasFond;
        public static bool Alerte { get; set; }

        static double timer_update_collision = 0;
        public static double Timer_Update_Collision
        {
            get { return timer_update_collision; }
            set { timer_update_collision = value; }
        }

        Player audio;
        double temps = 0;
        string nomDeCarte;
        bool jeuEnCoop;

        #endregion

        #region Timer hero 1

        private static double timer_hero1 = 0;

        public static double Timer_Hero1
        {
            get { return timer_hero1; }
            set { timer_hero1 = value; }
        }

        private static bool enable_timer_hero1 = false;
        public static bool Enable_Timer_Hero1
        {
            get { return enable_timer_hero1; }
            set { enable_timer_hero1 = value; }
        }
        #endregion

        #region Timer hero 2
        private static double timer_hero2 = 0;

        public static double Timer_Hero2
        {
            get { return timer_hero2; }
            set { timer_hero2 = value; }
        }

        private static bool enable_timer_hero2 = false;
        public static bool Enable_Timer_Hero2
        {
            get { return enable_timer_hero2; }
            set { enable_timer_hero2 = value; }
        }
        #endregion

        #region Initialization

        YellokillerGame game = null;
        public GameplayScreen(string nomDeCarte, YellokillerGame game, uint retries)
        {
            this.retries = retries;
            this.game = game;
            this.nomDeCarte = nomDeCarte;
            jeuEnCoop = nomDeCarte[nomDeCarte.Length - 1] == 'p';
            MoteurParticule.Camera = new Vector2(camera.X, camera.Y) ;
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            audio = new Player();

            carte = new Carte(new Vector2(Taille_Map.LARGEUR_MAP, Taille_Map.HAUTEUR_MAP));
            carte.OuvrirCarte(nomDeCarte);

            _shuriken = new List<Shuriken>();

            camera = new Rectangle(0, 0, 36, 24);

            hero1 = new Hero(new Vector2(28 * carte.OrigineJoueur1.X + 5, 28 * carte.OrigineJoueur1.Y + 1), Keys.Up, Keys.Down, Keys.Right, Keys.Left, Keys.RightAlt, Keys.RightControl, Keys.RightShift, 1);
            if (jeuEnCoop)
                hero2 = new Hero(new Vector2(28 * carte.OrigineJoueur2.X + 5, 28 * carte.OrigineJoueur2.Y + 1), Keys.Z, Keys.S, Keys.D, Keys.Q, Keys.A, Keys.Space, Keys.LeftShift, 2);

            // Centre la camera sur le personnage... plus ou moins...
            if (carte.OrigineJoueur1.X - 16 < 0)
                camera.X = 0;
            else if (carte.OrigineJoueur1.X + 16 > Taille_Map.LARGEUR_MAP - 1)
                camera.X = 28 * (Taille_Map.LARGEUR_MAP - 36);
            else
                camera.X = 28 * ((int)carte.OrigineJoueur1.X - 16);

            if (carte.OrigineJoueur1.Y - 12 < 0)
                camera.Y = 0;
            else if (carte.OrigineJoueur1.Y + 12 > Taille_Map.HAUTEUR_MAP - 1)
                camera.Y = 28 * (Taille_Map.HAUTEUR_MAP - 24);
            else
                camera.Y = 28 * ((int)carte.OrigineJoueur1.Y - 12);

            _gardes = new List<Garde>();
            foreach (Vector2 position in carte.OriginesGardes)
                _gardes.Add(new Garde(new Vector2(28 * position.X + 5, 28 * position.Y), carte));

            _patrouilleurs = new List<Patrouilleur>();
            foreach (List<Vector2> parcours in carte.OriginesPatrouilleurs)
            {
                _patrouilleurs.Add(new Patrouilleur(new Vector2(28 * parcours[0].X + 5, 28 * parcours[0].Y), carte));
                foreach (Vector2 passage in parcours)
                    _patrouilleurs[_patrouilleurs.Count - 1].Parcours.Add(carte.Cases[(int)passage.Y, (int)passage.X]);

                _patrouilleurs[_patrouilleurs.Count - 1].CreerTrajet(carte);
            }

            _patrouilleurs_a_chevaux = new List<Patrouilleur_a_cheval>();
            foreach (List<Vector2> parcours in carte.OriginesPatrouilleursAChevaux)
            {
                _patrouilleurs_a_chevaux.Add(new Patrouilleur_a_cheval(new Vector2(28 * parcours[0].X + 5, 28 * parcours[0].Y), carte));
                foreach (Vector2 passage in parcours)
                    _patrouilleurs_a_chevaux[_patrouilleurs_a_chevaux.Count - 1].Parcours.Add(carte.Cases[(int)passage.Y, (int)passage.X]);

                _patrouilleurs_a_chevaux[_patrouilleurs_a_chevaux.Count - 1].CreerTrajet(carte);
            }

            _boss = new List<Boss>();
            foreach (Vector2 position in carte.OriginesBoss)
                _boss.Add(new Boss(new Vector2(28 * position.X + 5, 28 * position.Y), carte));

            _statues = new List<Statue>();
            for (int l = 0; l < carte.OriginesStatues.Count; l++)
                _statues.Add(new Statue(28 * new Vector2(carte.OriginesStatues[l].X, carte.OriginesStatues[l].Y), carte, carte.RotationsDesStatues[l]));

            _bonus = new List<Bonus>();
            foreach (Vector2 bonus in carte.BonusShurikens)
                _bonus.Add(new Bonus(28 * bonus, TypeBonus.shuriken));

            foreach (Vector2 bonus in carte.BonusHadokens)
                _bonus.Add(new Bonus(28 * bonus, TypeBonus.hadoken));

            gardesMorts = new List<Rectangle>();
            patrouilleursAChevauxMorts = new List<Rectangle>();
            patrouilleursMorts = new List<Rectangle>();
            bossMorts = new List<Rectangle>();

            ennemisTues = (uint)(_gardes.Count + _patrouilleurs.Count + _patrouilleurs_a_chevaux.Count);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            spriteBatch = ScreenManager.SpriteBatch;
            gameFont = content.Load<SpriteFont>("courier");

            moteurparticule = new MoteurParticule(game, spriteBatch, carte, hero1, hero2, _statues);

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

            foreach (Statue statue in _statues)
                statue.LoadContent(content);

            foreach (Bonus bonus in _bonus)
                bonus.LoadContent(content);

            textureGardeMort = content.Load<Texture2D>(@"Menu Editeur de Maps\gardeMort");
            texturePatrouilleurAChevalMort = content.Load<Texture2D>(@"Menu Editeur de Maps\patrouilleurAChevalMort");
            texturePatrouilleurMort = content.Load<Texture2D>(@"Menu Editeur de Maps\Patrouilleur mort");
            textureBossMort = content.Load<Texture2D>(@"Menu Editeur de Maps\Boss mort");

            textureBasFond = content.Load<Texture2D>("Bas fond");
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
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            if (IsActive)
            {
                if (Enable_Timer_Hero1)
                    timer_hero1 += gameTime.ElapsedGameTime.TotalSeconds;
                if (Enable_Timer_Hero2)
                    timer_hero2 += gameTime.ElapsedGameTime.TotalSeconds;

                temps += gameTime.ElapsedGameTime.TotalSeconds;
                AudioEngine.Update();

                hero1.Update(gameTime, carte, ref camera, moteurparticule, _shuriken, content, hero2);
                if (jeuEnCoop)
                    hero2.Update(gameTime, carte, ref camera, moteurparticule, _shuriken, content, hero1);

                foreach (Garde garde in _gardes)
                    garde.Update(gameTime, carte, hero1, hero2, camera, gardesMorts, patrouilleursMorts, patrouilleursAChevauxMorts, bossMorts);

                foreach (Patrouilleur patrouilleur in _patrouilleurs)
                    patrouilleur.Update(gameTime, carte, hero1, hero2, camera, gardesMorts, patrouilleursMorts, patrouilleursAChevauxMorts, bossMorts);

                foreach (Patrouilleur_a_cheval patrouilleurACheval in _patrouilleurs_a_chevaux)
                    patrouilleurACheval.Update(gameTime, carte, hero1, hero2, camera, gardesMorts, patrouilleursMorts, patrouilleursAChevauxMorts, bossMorts);

                foreach (Boss boss in _boss)
                    boss.Update(gameTime, _shuriken, carte, hero1, hero2, camera, gardesMorts, patrouilleursMorts, patrouilleursAChevauxMorts, bossMorts);

                foreach (Statue statue in _statues)
                    statue.Update(gameTime, moteurparticule);

                if (timer_update_collision > 0)
                {
                    Moteur_physique.Collision_Armes_Ennemis(hero1, hero2, _gardes, _patrouilleurs, _patrouilleurs_a_chevaux, _boss, _shuriken, moteurparticule, AudioEngine.SoundBank, ref gardesMorts, ref patrouilleursAChevauxMorts, ref patrouilleursMorts, ref bossMorts);
                    if (timer_update_collision > 5)
                        timer_update_collision = 0;
                }

                if (Moteur_physique.Collision_Garde_Heros(_gardes, hero1, hero2, AudioEngine.SoundBank))
                    LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameOverScreen(nomDeCarte, game, retries));

                if (Moteur_physique.Collision_Patrouilleur_Heros(_patrouilleurs, hero1, hero2, AudioEngine.SoundBank))
                    LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameOverScreen(nomDeCarte, game, retries));

                if (Moteur_physique.Collision_PatrouilleurACheval_Heros(_patrouilleurs_a_chevaux, hero1, hero2, AudioEngine.SoundBank))
                    LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameOverScreen(nomDeCarte, game, retries));

                if (Moteur_physique.Collision_Boss_Heros(_boss, hero1, hero2, AudioEngine.SoundBank))
                    LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameOverScreen(nomDeCarte, game, retries));

                if (Moteur_physique.Collision_Heros_ExplosionStatues(_statues, hero1, hero2, moteurparticule, AudioEngine.SoundBank))
                    LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameOverScreen(nomDeCarte, game, retries));

                Moteur_physique.Collision_Heros_Bonus(ref hero1, ref hero2, ref _bonus, AudioEngine.SoundBank);
                if (_boss.Count == 0)
                {
                    LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameWin(nomDeCarte, (uint)carte.Salaire, temps, ennemisTues - (uint)(_gardes.Count + _patrouilleurs.Count + _patrouilleurs_a_chevaux.Count), retries, game));
                    audio.Close();
                }

                if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.C))
                    SauvegarderCheckPoint();

                audio.Update(gameTime);
            }
        }


        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0)
                ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);

            spriteBatch.Begin();
            carte.DrawInGame(gameTime, spriteBatch, content, camera);

            foreach (Garde garde in _gardes)
                //garde.Draw(spriteBatch, camera);
                garde.Draw(camera, spriteBatch);

            foreach (Patrouilleur patrouilleur in _patrouilleurs)
                //patrouilleur.Draw(spriteBatch, camera);
                patrouilleur.Draw(camera, spriteBatch);

            foreach (Patrouilleur_a_cheval patrouilleurACheval in _patrouilleurs_a_chevaux)
                // patrouilleurACheval.Draw(spriteBatch, camera);
                patrouilleurACheval.Draw(camera, spriteBatch);

            foreach (Boss boss in _boss)
                //boss.Draw(spriteBatch, camera);
                boss.Draw(camera, spriteBatch);

            foreach (Statue statue in _statues)
                statue.Draw(spriteBatch, camera);

            foreach (Bonus bonus in _bonus)
                bonus.Draw(spriteBatch, camera);

            foreach (Rectangle position in gardesMorts)
                spriteBatch.Draw(textureGardeMort, new Vector2(position.X, position.Y) - new Vector2(camera.X, camera.Y), Color.White);

            foreach (Rectangle position in patrouilleursAChevauxMorts)
                spriteBatch.Draw(texturePatrouilleurAChevalMort, new Vector2(position.X, position.Y) - new Vector2(camera.X, camera.Y), Color.White);

            foreach (Rectangle position in patrouilleursMorts)
                spriteBatch.Draw(texturePatrouilleurMort, new Vector2(position.X, position.Y) - new Vector2(camera.X, camera.Y), Color.White);

            foreach (Rectangle position in bossMorts)
                spriteBatch.Draw(textureBossMort, new Vector2(position.X, position.Y) - new Vector2(camera.X, camera.Y), Color.White);

            for (int i = 0; i < _shuriken.Count; i++)
            {
                _shuriken[i].Update(gameTime, carte);
                _shuriken[i].Draw(spriteBatch, camera);

                if (_shuriken[i].ShurikenExists == false)
                {
                    _shuriken.RemoveAt(i);
                    AudioEngine.SoundBank.PlayCue("shurikenCollision");
                }
            }

            hero1.Draw_Hero(spriteBatch, camera);
            if (jeuEnCoop)
                hero2.Draw_Hero(spriteBatch, camera);

            spriteBatch.Draw(textureBasFond, new Vector2(0, Taille_Ecran.HAUTEUR_ECRAN - 84), null, Color.PapayaWhip, 0, Vector2.Zero, new Vector2((float)Taille_Ecran.LARGEUR_ECRAN / (float)textureBasFond.Width, 1), SpriteEffects.None, 1);
            
            hero1.Draw_Infos(spriteBatch);
            if (jeuEnCoop)
                hero2.Draw_Infos(spriteBatch);            

            spriteBatch.DrawString(ScreenManager.font, "Ennemis restants : " + (_gardes.Count + _patrouilleurs.Count + _patrouilleurs_a_chevaux.Count).ToString(), new Vector2(10, Taille_Ecran.HAUTEUR_ECRAN - 25), Color.DarkBlue);

            spriteBatch.DrawString(ScreenManager.font, Temps.Conversion(temps), new Vector2(Taille_Ecran.LARGEUR_ECRAN - 60, Taille_Ecran.HAUTEUR_ECRAN - 25), Color.DarkRed);

            if(Alerte)
                spriteBatch.DrawString(ScreenManager.font, "ALERTE ! VOUS AVEZ ETE REPERE !", new Vector2(200, 100), Color.Red);
            
            spriteBatch.End();
            audio.Draw(gameTime);
            base.Draw(gameTime);
        }

        #endregion

        #region Handle Input

        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                AudioEngine.SoundBank.PlayCue("menuBouge");
                ScreenManager.AddScreen(new Pausebckground(), ControllingPlayer, true);
                ScreenManager.AddScreen(new PauseMenuScreen(0, 1, game), ControllingPlayer, true);
            }

            if (ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.W))
            {
                audio.Close();
                LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameWin(nomDeCarte, (uint)carte.Salaire, temps, ennemisTues - (uint)(_gardes.Count + _patrouilleurs.Count + _patrouilleurs_a_chevaux.Count), retries, game));
            }

            // Looks up input for the Media Player.
            audio.HandleInput();
        }
        #endregion

        private void SauvegarderCheckPoint()
        {
            StreamWriter file = new StreamWriter("checkTemp.txt");
            hero1.SauvegarderCheckPoint(ref file);

            if (hero2 != null)
                hero2.SauvegarderCheckPoint(ref file);

            file.WriteLine("Gardes");
            foreach (Garde garde in _gardes)
                garde.SauvegarderCheckPoint(ref file);

            file.WriteLine("Gardes morts");
            foreach (Rectangle mort in gardesMorts)
                file.WriteLine(mort.X.ToString() + "," + mort.Y.ToString());   

            file.WriteLine("Patrouilleurs");
            foreach (Patrouilleur patrouilleur in _patrouilleurs)
                patrouilleur.SauvegarderCheckPoint(ref file);

            file.WriteLine("Patrouilleurs morts");
            foreach (Rectangle mort in patrouilleursMorts)
                file.WriteLine(mort.X.ToString() + "," + mort.Y.ToString());   

            file.WriteLine("Patrouilleurs A Chevaux");
            foreach (Patrouilleur_a_cheval cheval in _patrouilleurs_a_chevaux)
                cheval.SauvegarderCheckPoint(ref file);

            file.WriteLine("Patrouilleurs a chevaux morts");
            foreach (Rectangle mort in patrouilleursAChevauxMorts)
                file.WriteLine(mort.X.ToString() + "," + mort.Y.ToString());   

            file.WriteLine("Boss");
            foreach (Boss boss in _boss)
                boss.SauvegarderCheckPoint(ref file);

            file.WriteLine("Boss morts");
            foreach (Rectangle mort in bossMorts)
                file.WriteLine(mort.X.ToString() + "," + mort.Y.ToString());   

            file.Close();
        }
    }
}