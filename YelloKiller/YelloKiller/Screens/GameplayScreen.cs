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
        GameOverScreen GOScreen;

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
        List<Interrupteur> interrupteurs;
        List<EnnemiMort> ennemisMorts;
        Dark_Hero Dark_Hero;
        Texture2D textureBasFond, modeAlerte;
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
            MoteurParticule.Camera = new Vector2(camera.X, camera.Y);
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            audio = new Player();

            carte = new Carte(new Vector2(Taille_Map.LARGEUR_MAP, Taille_Map.HAUTEUR_MAP));
            carte.OuvrirCarte(nomDeCarte, content);

            _shuriken = new List<Shuriken>();

            camera = new Rectangle(0, 0, 36, 24);

            hero1 = new Hero(new Vector2(28 * carte.OrigineJoueur1.X + 5, 28 * carte.OrigineJoueur1.Y + 1), Keys.Up, Keys.Down, Keys.Right, Keys.Left, Keys.RightAlt, Keys.RightControl, Keys.RightShift, 1);
            hero1.NombreShuriken = carte.Munitions[0];
            hero1.NombreHadoken = carte.Munitions[1];
            hero1.NombreFumigene = carte.Munitions[2];
            hero1.NombreBall = carte.Munitions[3];

            if (jeuEnCoop)
            {
                hero2 = new Hero(new Vector2(28 * carte.OrigineJoueur2.X + 5, 28 * carte.OrigineJoueur2.Y + 1), Keys.Z, Keys.S, Keys.D, Keys.Q, Keys.A, Keys.Space, Keys.LeftShift, 2);
                hero2.NombreShuriken = carte.Munitions[4];
                hero2.NombreHadoken = carte.Munitions[5];
                hero2.NombreFumigene = carte.Munitions[6];
                hero2.NombreBall = carte.Munitions[7];
            }
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
            for (int i = 0; i < carte.OriginesPatrouilleurs.Count; i++)
            {
                _patrouilleurs.Add(new Patrouilleur(new Vector2(28 * carte.OriginesPatrouilleurs[i][0].X + 5, 28 * carte.OriginesPatrouilleurs[i][0].Y), carte, i));

                foreach (Vector2 passage in carte.OriginesPatrouilleurs[i])
                    _patrouilleurs[_patrouilleurs.Count - 1].Parcours.Add(carte.Cases[(int)passage.Y, (int)passage.X]);

                _patrouilleurs[_patrouilleurs.Count - 1].CreerTrajet(carte);
            }

            _patrouilleurs_a_chevaux = new List<Patrouilleur_a_cheval>();
            for (int i = 0; i < carte.OriginesPatrouilleursAChevaux.Count; i++)
            {
                _patrouilleurs_a_chevaux.Add(new Patrouilleur_a_cheval(new Vector2(28 * carte.OriginesPatrouilleursAChevaux[i][0].X + 5, 28 * carte.OriginesPatrouilleursAChevaux[i][0].Y), carte, i));

                foreach (Vector2 passage in carte.OriginesPatrouilleursAChevaux[i])
                    _patrouilleurs_a_chevaux[_patrouilleurs_a_chevaux.Count - 1].Parcours.Add(carte.Cases[(int)passage.Y, (int)passage.X]);

                _patrouilleurs_a_chevaux[_patrouilleurs_a_chevaux.Count - 1].CreerTrajet(carte);
            }

            if (carte.OrigineDarkHero != -Vector2.One)
                Dark_Hero = new Dark_Hero(carte.OrigineDarkHero, carte);

            _boss = new List<Boss>();
            foreach (Vector2 position in carte.OriginesBoss)
                _boss.Add(new Boss(new Vector2(28 * position.X + 5, 28 * position.Y), carte));

            _statues = new List<Statue>();
            for (int l = 0; l < carte.OriginesStatues.Count; l++)
                _statues.Add(new Statue(28 * new Vector2(carte.OriginesStatues[l].X, carte.OriginesStatues[l].Y), carte, carte.RotationsDesStatues[l]));

            _bonus = new List<Bonus>();
            foreach (Bonus extra in carte.Bonus)
                _bonus.Add(new Bonus(28 * new Vector2(extra.X, extra.Y), extra.TypeBonus));

            interrupteurs = new List<Interrupteur>();
            foreach (Interrupteur bouton in carte.Interrupteurs)
            {
                interrupteurs.Add(new Interrupteur(bouton.Position));
                interrupteurs[interrupteurs.Count - 1].PortePosition = bouton.PortePosition;
                interrupteurs[interrupteurs.Count - 1].rotation = bouton.rotation;
            }

            ennemisMorts = new List<EnnemiMort>();
            ennemisTues = (uint)(_gardes.Count + _patrouilleurs.Count + _patrouilleurs_a_chevaux.Count);
            Alerte = false;
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

            if (Dark_Hero != null)
                Dark_Hero.LoadContent(content, 2);

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

            foreach (Interrupteur bouton in interrupteurs)
                bouton.LoadContent(content);

            textureBasFond = content.Load<Texture2D>("Bas fond");
            modeAlerte = content.Load<Texture2D>("modealerte");
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

                hero1.Update(gameTime, carte, ref camera, moteurparticule, _shuriken, content, hero2, interrupteurs);
                if (jeuEnCoop)
                    hero2.Update(gameTime, carte, ref camera, moteurparticule, _shuriken, content, hero1, interrupteurs);

                if (carte.OrigineDarkHero != -Vector2.One)
                    Dark_Hero.Update(gameTime, carte, hero1, hero2, camera, ennemisMorts, moteurparticule.Rectangle_Fumigene(hero1));

                foreach (Garde garde in _gardes)
                    garde.Update(gameTime, carte, hero1, hero2, camera, ennemisMorts, moteurparticule.Rectangle_Fumigene(hero1));

                foreach (Patrouilleur patrouilleur in _patrouilleurs)
                    patrouilleur.Update(gameTime, carte, hero1, hero2, camera, ennemisMorts, moteurparticule.Rectangle_Fumigene(hero1));

                foreach (Patrouilleur_a_cheval patrouilleurACheval in _patrouilleurs_a_chevaux)
                    patrouilleurACheval.Update(gameTime, carte, hero1, hero2, camera, ennemisMorts, moteurparticule.Rectangle_Fumigene(hero1));

                foreach (Boss boss in _boss)
                    boss.Update(gameTime, _shuriken, carte, hero1, hero2, camera, ennemisMorts, moteurparticule.Rectangle_Fumigene(hero1));

                foreach (Statue statue in _statues)
                    statue.Update(gameTime, moteurparticule);

                if (timer_update_collision > 0)
                {
                    Moteur_physique.Collision_Armes_Ennemis(hero1, hero2, _gardes, _patrouilleurs, _patrouilleurs_a_chevaux, _boss, _shuriken, moteurparticule, AudioEngine.SoundBank, ref ennemisMorts, content);
                    if (timer_update_collision > 5)
                        timer_update_collision = 0;
                }

                if (Moteur_physique.Collision_Garde_Heros(_gardes, hero1, hero2, AudioEngine.SoundBank) ||
                    Moteur_physique.Collision_Patrouilleur_Heros(_patrouilleurs, hero1, hero2, AudioEngine.SoundBank) ||
                    Moteur_physique.Collision_PatrouilleurACheval_Heros(_patrouilleurs_a_chevaux, hero1, hero2, AudioEngine.SoundBank) ||
                    Moteur_physique.Collision_Boss_Heros(_boss, hero1, hero2, AudioEngine.SoundBank) ||
                    Moteur_physique.Collision_Heros_ExplosionStatues(_statues, hero1, hero2, moteurparticule, AudioEngine.SoundBank))
                    Mourir();

                if (Moteur_physique.Collision_Heros_Bonus(ref hero1, ref hero2, ref _bonus, AudioEngine.SoundBank))
                    SauvegarderCheckPoint();

                Moteur_physique.Collisions_Heros_Interrupteurs(hero1, hero2, ref interrupteurs, AudioEngine.SoundBank);

                if (_boss.Count == 0 && (hero1.EstAuPointDeDepart || hero2 != null && hero2.EstAuPointDeDepart))
                {
                    LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new GameWin(nomDeCarte, (uint)carte.Salaire, temps, ennemisTues - (uint)(_gardes.Count + _patrouilleurs.Count + _patrouilleurs_a_chevaux.Count), retries, game));
                    audio.Close();
                }

                if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.C))
                    SauvegarderCheckPoint();

                if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.O))
                    ChargerCheckPoint();

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
                garde.Draw(spriteBatch, camera);

            foreach (Patrouilleur patrouilleur in _patrouilleurs)
                patrouilleur.Draw(spriteBatch, camera);

            foreach (Patrouilleur_a_cheval patrouilleurACheval in _patrouilleurs_a_chevaux)
                patrouilleurACheval.Draw(spriteBatch, camera);

            if (Dark_Hero != null)
                Dark_Hero.Draw(spriteBatch, camera);

            foreach (Boss boss in _boss)
                boss.Draw(spriteBatch, camera);

            foreach (Statue statue in _statues)
                statue.Draw(spriteBatch, camera);

            foreach (Bonus bonus in _bonus)
                bonus.Draw(spriteBatch, camera);

            foreach (Interrupteur bouton in interrupteurs)
                bouton.DrawInGame(spriteBatch, camera);

            foreach (EnnemiMort mort in ennemisMorts)
                mort.Draw(spriteBatch, camera);

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

            spriteBatch.Draw(textureBasFond, new Vector2(0, Taille_Ecran.HAUTEUR_ECRAN - 84), null, Color.PapayaWhip, 0, Vector2.Zero, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width/* (float)Taille_Ecran.LARGEUR_ECRAN *// (float)textureBasFond.Width, 1.3f), SpriteEffects.None, 1);

            hero1.Draw_Infos(spriteBatch);
            if (jeuEnCoop)
                hero2.Draw_Infos(spriteBatch);

            spriteBatch.DrawString(ScreenManager.font, "Ennemis restants : " + (_gardes.Count + _patrouilleurs.Count + _patrouilleurs_a_chevaux.Count).ToString(), new Vector2(Taille_Ecran.LARGEUR_ECRAN - 400, Taille_Ecran.HAUTEUR_ECRAN - 25), Color.DarkBlue);

            spriteBatch.DrawString(ScreenManager.font, Temps.Conversion(temps), new Vector2(Taille_Ecran.LARGEUR_ECRAN - 80, Taille_Ecran.HAUTEUR_ECRAN - 25), Color.DarkRed);

            if (Alerte)
                spriteBatch.Draw(modeAlerte, Vector2.Zero, null, Color.White, 0, Vector2.Zero, new Vector2(((float)ScreenManager.GraphicsDevice.Viewport.Width / (float)modeAlerte.Width), (((float)ScreenManager.GraphicsDevice.Viewport.Height - 84) / (float)modeAlerte.Height)), SpriteEffects.None, 1);

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

            if (ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.G))
            {
                audio.Close();
                Mourir();
            }

            // Looks up input for the Media Player.
            audio.HandleInput();
        }
        #endregion

        private void SauvegarderCheckPoint()
        {
            StreamWriter file = new StreamWriter("checkTemp.tmp");

            file.WriteLine("Hero 1");
            hero1.SauvegarderCheckPoint(ref file);
            if (hero2 != null)
            {
                file.WriteLine("Hero 2");
                hero2.SauvegarderCheckPoint(ref file);
            }

            file.WriteLine("Gardes");
            foreach (Garde garde in _gardes)
                garde.SauvegarderCheckPoint(ref file);

            file.WriteLine("Patrouilleurs");
            foreach (Patrouilleur patrouilleur in _patrouilleurs)
                patrouilleur.SauvegarderCheckPointPat(ref file);

            file.WriteLine("Patrouilleurs A Chevaux");
            foreach (Patrouilleur_a_cheval cheval in _patrouilleurs_a_chevaux)
                cheval.SauvegarderCheckPointPat(ref file);

            file.WriteLine("Boss");
            foreach (Boss boss in _boss)
                boss.SauvegarderCheckPoint(ref file);

            file.WriteLine("Morts");
            foreach (EnnemiMort mort in ennemisMorts)
                file.WriteLine(mort.X.ToString() + "," + mort.Y.ToString() + "," + ((int)mort.Type).ToString());

            file.WriteLine("Bonus");
            foreach (Bonus bonus in _bonus)
                bonus.SauvegarderCheckPoint(ref file);

            file.WriteLine("Interrupteurs");
            foreach (Interrupteur bouton in interrupteurs)
                bouton.SauvegarderCheckPoint(ref file);

            file.WriteLine("Camera");
            file.WriteLine(camera.X + "," + camera.Y);

            file.Close();
        }

        private void ChargerCheckPoint()
        {
            StreamReader file = new StreamReader("checkTemp.tmp");
            string banana = "";
            string[] dessert = null;

            file.ReadLine();

            hero1.ChargerCheckPoint(ref file);
            banana = file.ReadLine();
            if (banana == "Hero 2")
            {
                hero2.ChargerCheckPoint(ref file);
                banana = file.ReadLine();
            }

            banana = file.ReadLine();
            // Gardes
            _gardes = new List<Garde>();
            while (banana != "Patrouilleurs")
            {
                dessert = banana.Split(',');
                _gardes.Add(new Garde(new Vector2(28 * Convert.ToInt32(dessert[0]), 28 * Convert.ToInt32(dessert[1])), carte));
                banana = file.ReadLine();
            }

            // Patrouilleurs
            banana = file.ReadLine();

            _patrouilleurs = new List<Patrouilleur>();
            while (banana != "Patrouilleurs A Chevaux")
            {
                dessert = banana.Split(',');
                _patrouilleurs.Add(new Patrouilleur(new Vector2(28 * Convert.ToInt32(dessert[0]), 28 * Convert.ToInt32(dessert[1])), carte, Convert.ToInt32(dessert[2])));

                foreach (Vector2 passage in carte.OriginesPatrouilleurs[_patrouilleurs[_patrouilleurs.Count - 1].Identifiant])
                    _patrouilleurs[_patrouilleurs.Count - 1].Parcours.Add(carte.Cases[(int)passage.Y, (int)passage.X]);

                _patrouilleurs[_patrouilleurs.Count - 1].Etape = Convert.ToInt32(dessert[3]);
                _patrouilleurs[_patrouilleurs.Count - 1].CreerTrajet(carte);

                banana = file.ReadLine();
            }

            // Patrouilleurs a chevaux
            banana = file.ReadLine();

            _patrouilleurs_a_chevaux = new List<Patrouilleur_a_cheval>();
            while (banana != "Boss")
            {
                dessert = banana.Split(',');
                _patrouilleurs_a_chevaux.Add(new Patrouilleur_a_cheval(new Vector2(28 * Convert.ToInt32(dessert[0]), 28 * Convert.ToInt32(dessert[1])), carte, Convert.ToInt32(dessert[2])));

                foreach (Vector2 passage in carte.OriginesPatrouilleursAChevaux[_patrouilleurs_a_chevaux[_patrouilleurs_a_chevaux.Count - 1].Identifiant])
                    _patrouilleurs_a_chevaux[_patrouilleurs_a_chevaux.Count - 1].Parcours.Add(carte.Cases[(int)passage.Y, (int)passage.X]);

                _patrouilleurs_a_chevaux[_patrouilleurs_a_chevaux.Count - 1].Etape = Convert.ToInt32(dessert[3]);
                _patrouilleurs_a_chevaux[_patrouilleurs_a_chevaux.Count - 1].CreerTrajet(carte);

                banana = file.ReadLine();
            }

            // Boss
            banana = file.ReadLine();

            _boss = new List<Boss>();
            while (banana != "Morts")
            {
                dessert = banana.Split(',');
                _boss.Add(new Boss(new Vector2(28 * Convert.ToInt32(dessert[0]), 28 * Convert.ToInt32(dessert[1])), carte));
                banana = file.ReadLine();
            }

            banana = file.ReadLine();

            ennemisMorts = new List<EnnemiMort>();
            while (banana != "Bonus")
            {
                dessert = banana.Split(',');
                ennemisMorts.Add(new EnnemiMort(new Vector2(28 * Convert.ToInt32(dessert[0]), 28 * Convert.ToInt32(dessert[1])), content, (EnnemiMort.TypeEnnemiMort)Convert.ToInt32(dessert[2])));
                banana = file.ReadLine();
            }

            banana = file.ReadLine();
            _bonus = new List<Bonus>();
            while (banana != "Interrupteurs")
            {
                dessert = banana.Split(',');
                _bonus.Add(new Bonus(new Vector2(Convert.ToInt32(dessert[0]), Convert.ToInt32(dessert[1])), (TypeBonus)Convert.ToInt32(dessert[2])));
                banana = file.ReadLine();
            }

            banana = file.ReadLine();
            interrupteurs = new List<Interrupteur>();
            while (banana != "Camera")
            {
                dessert = banana.Split(',');
                interrupteurs.Add(new Interrupteur(new Vector2(Convert.ToInt32(dessert[0]), Convert.ToInt32(dessert[1]))));
                interrupteurs[interrupteurs.Count - 1].PortePosition = new Vector2(Convert.ToInt32(dessert[2]), Convert.ToInt32(dessert[3]));
                interrupteurs[interrupteurs.Count - 1].rotation = Convert.ToByte(dessert[4]);
                interrupteurs[interrupteurs.Count - 1].PorteOuverte = Convert.ToBoolean(dessert[5]);
                banana = file.ReadLine();
            }

            banana = file.ReadLine();
            dessert = banana.Split(',');
            camera.X = Convert.ToInt32(dessert[0]);
            camera.Y = Convert.ToInt32(dessert[1]);

            foreach (Garde garde in _gardes)
                garde.LoadContent(content, 2);

            foreach (Patrouilleur patrouilleur in _patrouilleurs)
                patrouilleur.LoadContent(content, 2);

            foreach (Patrouilleur_a_cheval patrouilleurACheval in _patrouilleurs_a_chevaux)
                patrouilleurACheval.LoadContent(content, 2);

            foreach (Boss boss in _boss)
                boss.LoadContent(content, 2);

            foreach (Bonus bonus in _bonus)
                bonus.LoadContent(content);

            foreach (Interrupteur bouton in interrupteurs)
                bouton.LoadContent(content);

            file.Close();
        }

        void ChargerChkPoint(object sender, PlayerIndexEventArgs e)
        {
            GOScreen.ExitScreen();
            ChargerCheckPoint();
        }

        void Mourir()
        {
            GOScreen = new GameOverScreen(nomDeCarte, game, retries);
            GOScreen.Chkpoint += ChargerChkPoint;
            ScreenManager.AddScreen(GOScreen, ControllingPlayer);
        }
    }
}