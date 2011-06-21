using System;
using System.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    public class EditorScreen : GameScreen
    {
        SpriteBatch spriteBatch;
        ContentManager content;

        Carte carte;
        Curseur curseur;
        Menu menu;
        Ascenseur ascenseur;
        YellokillerGame game;
        StreamWriter sauvegarde;
        string ligne, nomSauvegarde, nomCarte, extension;
        Rectangle camera;
        Vector2 origine1, origine2, _origineDark_Hero;
        List<Vector2> _originesGardes, _originesBoss, _originesStatues;
        List<Interrupteur> interrupteurs;
        List<Bonus> bonus;
        List<byte> rotationsDesStatues;
        List<List<Vector2>> _originesPatrouilleurs, _originesPatrouilleursAChevaux;
        Texture2D pointDePassage, fond, textureStatue, textureBasFond;
        bool fileExist = false;
        int compteur;
        int[] munitions;
        bool enableOrigine1, enableOrigine2, enableDH, enableSave, afficherMessageErreur, afficherMessageSauvegarde;
        double chronometre = 0;
        Informations infos;

        public EditorScreen(string nomCarte, YellokillerGame game)
        {
            this.game = game;
            if (nomCarte == "")
            {
                nomSauvegarde = nomCarte;
                compteur = 1;
            }
            else
            {
                nomSauvegarde = nomCarte.Substring(0, nomCarte.Length - 5);
                compteur = nomCarte[nomCarte.Length - 6];
                extension = nomCarte.Substring(nomCarte.Length - 5);
            }

            this.nomCarte = nomCarte;

            ligne = "";
            enableSave = true;
            afficherMessageErreur = false;
            afficherMessageSauvegarde = false;
            camera = new Rectangle(0, 0, 33, 24);
            carte = new Carte(new Vector2(Taille_Map.LARGEUR_MAP, Taille_Map.HAUTEUR_MAP));

            if (nomCarte == "")
                carte.Initialisation(new Vector2(Taille_Map.LARGEUR_MAP, Taille_Map.HAUTEUR_MAP));
            else
                carte.OuvrirCarte(nomCarte, content);

            _originesGardes = carte.OriginesGardes;
            _originesPatrouilleurs = carte.OriginesPatrouilleurs;
            _originesPatrouilleursAChevaux = carte.OriginesPatrouilleursAChevaux;
            _origineDark_Hero = carte.OrigineDarkHero;
            _originesBoss = carte.OriginesBoss;
            _originesStatues = carte.OriginesStatues;
            rotationsDesStatues = carte.RotationsDesStatues;
            bonus = carte.Bonus;
            interrupteurs = carte.Interrupteurs;
            munitions = carte.Munitions;

            origine1 = carte.OrigineJoueur1;
            origine2 = carte.OrigineJoueur2;

            enableOrigine1 = (carte.OrigineJoueur1 == -Vector2.One);
            enableOrigine2 = (carte.OrigineJoueur2 == -Vector2.One);
            enableDH = (carte.OrigineDarkHero == -Vector2.One);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            menu = new Menu(content);
            curseur = new Curseur(content);
            ascenseur = new Ascenseur(content, ScreenManager.GraphicsDevice.Viewport.Width - 28);
            fond = content.Load<Texture2D>(@"Textures\Invisible");
            pointDePassage = content.Load<Texture2D>(@"Menu Editeur de Maps\pied");
            textureStatue = content.Load<Texture2D>(@"Feuilles de sprites\statue_dragon");
            textureBasFond = content.Load<Texture2D>("Bas fond");
            infos = new Informations();
            infos.LoadContent(content);

            foreach (Interrupteur bouton in interrupteurs)
                bouton.LoadContent(content);

            spriteBatch = ScreenManager.SpriteBatch;
        }

        public override void UnloadContent()
        {
            content.Unload();
        }


        #region Event

        /// <summary>
        /// Event handler for when the user selects ok on the "Save Map" message box.
        /// </summary>
        void SaveMapAccepted(object sender, PlayerIndexEventArgs e)
        {
            EditorSavePop savePop = (EditorSavePop)sender;
            nomSauvegarde = savePop.nomSauvegarde;
            SauvegardeMap();
        }

        void SaveMapMenuAccepted(object sender, PlayerIndexEventArgs e)
        {
            EditorSavePop editorSavePop = new EditorSavePop(nomSauvegarde, 1);
            editorSavePop.Accepted += SaveMapAccepted;
            ScreenManager.AddScreen(editorSavePop, ControllingPlayer);
        }

        void F1Selected(object sender, PlayerIndexEventArgs e)
        {
            ChangerStyle(TypeCase.herbe);
        }

        void F2Selected(object sender, PlayerIndexEventArgs e)
        {
            ChangerStyle(TypeCase.herbeFoncee);
        }

        void F3Selected(object sender, PlayerIndexEventArgs e)
        {
            ChangerStyle(TypeCase.terre);
        }

        void F4Selected(object sender, PlayerIndexEventArgs e)
        {
            ChangerStyle(TypeCase.parquet);
        }

        void F5Selected(object sender, PlayerIndexEventArgs e)
        {
            Labyrinthe.CreerLabyrintheSimple(carte);
        }

        void F6Selected(object sender, PlayerIndexEventArgs e)
        {
            Labyrinthe.CreerLabyrintheDouble(carte);
        }

        void F7Selected(object sender, PlayerIndexEventArgs e)
        {
            Labyrinthe.CreerLabyrintheTriple(carte);
        }

        void F8Selected(object sender, PlayerIndexEventArgs e)
        {
            Labyrinthe.CreerLabyrintheQuadruple(carte);
        }

        #endregion

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (afficherMessageErreur || afficherMessageSauvegarde)
                chronometre += gameTime.ElapsedGameTime.TotalSeconds;

            ascenseur.Update(ScreenManager.GraphicsDevice.Viewport.Width - 28);
            menu.Update(ascenseur, ScreenManager.GraphicsDevice.Viewport.Width);
            curseur.Update(content, menu);

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            int playerIndex = (int)ControllingPlayer.Value;

            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new Pausebckground(), ControllingPlayer, true);
                PauseMenuScreen pauseMenuScreen = new PauseMenuScreen(0, 2, game);
                pauseMenuScreen.SaveMapMenuEntrySelected += SaveMapMenuAccepted;
                pauseMenuScreen.F1 += F1Selected;
                pauseMenuScreen.F2 += F2Selected;
                pauseMenuScreen.F3 += F3Selected;
                pauseMenuScreen.F4 += F4Selected;
                pauseMenuScreen.F5 += F5Selected;
                pauseMenuScreen.F6 += F6Selected;
                pauseMenuScreen.F7 += F7Selected;
                pauseMenuScreen.F8 += F8Selected;
                ScreenManager.AddScreen(pauseMenuScreen, ControllingPlayer, true);
            }

            if (camera.X > 0 && ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.Left))
                camera.X--;

            else if (camera.X < Taille_Map.LARGEUR_MAP - camera.Width && ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.Right))
                camera.X++;

            else if (camera.Y > 0 && ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.Up))
                camera.Y--;

            else if (camera.Y < Taille_Map.HAUTEUR_MAP - camera.Height && ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.Down))
                camera.Y++;

            if (curseur.Type == TypeCase.Gomme && ServiceHelper.Get<IMouseService>().ClicBoutonGauche())
                Supprimer_Ennemi_Ou_Bonus();

            if (ServiceHelper.Get<IMouseService>().ClicBoutonGauche() && ServiceHelper.Get<IMouseService>().DansLaCarte() && CasePossible)
                Placer_Personnage_Ou_Bonus();

            if (ServiceHelper.Get<IMouseService>().ClicBoutonDroit() && ServiceHelper.Get<IMouseService>().DansLaCarte() && CasePossible)
            {
                if (curseur.Type == TypeCase.Patrouilleur && _originesPatrouilleurs.Count > 0)
                    _originesPatrouilleurs[_originesPatrouilleurs.Count - 1].Add(new Vector2(curseur.Position.X + camera.X, curseur.Position.Y + camera.Y));
                else if (curseur.Type == TypeCase.Patrouilleur_a_cheval && _originesPatrouilleursAChevaux.Count > 0)
                    _originesPatrouilleursAChevaux[_originesPatrouilleursAChevaux.Count - 1].Add(new Vector2(curseur.Position.X + camera.X, curseur.Position.Y + camera.Y));
                else if (_originesStatues.Count > 0 && curseur.Type == TypeCase.Statues && _originesStatues.Count > 0)
                    rotationsDesStatues[rotationsDesStatues.Count - 1] = (byte)((rotationsDesStatues[rotationsDesStatues.Count - 1] + 1) % 4);
                else if (curseur.Type == TypeCase.Interrupteur && interrupteurs.Count > 0)
                    interrupteurs[interrupteurs.Count - 1].ChangerPosition(carte, new Vector2(curseur.Position.X + camera.X, curseur.Position.Y + camera.Y));
            }

            if (ServiceHelper.Get<IMouseService>().ClicBoutonMilieu() && ServiceHelper.Get<IMouseService>().DansLaCarte() && curseur.Type == TypeCase.Interrupteur)
                interrupteurs[interrupteurs.Count - 1].Rotationner(carte);

            if (ServiceHelper.Get<IMouseService>().BoutonGauchePresse() && ServiceHelper.Get<IMouseService>().DansLaCarte())
                Placer_Case();

            if (ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.LeftControl) && ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.S))
            {
                if (enableOrigine1 && enableOrigine2 || _originesBoss.Count == 0)
                    afficherMessageErreur = true;
                else
                {
                    EditorSavePop editorSavePop = new EditorSavePop(nomSauvegarde, 0);
                    editorSavePop.Accepted += SaveMapAccepted;
                    ScreenManager.AddScreen(editorSavePop, ControllingPlayer);
                }
            }

            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.F1))
                ChangerStyle(TypeCase.herbe);

            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.F2))
                ChangerStyle(TypeCase.herbeFoncee);

            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.F3))
                ChangerStyle(TypeCase.terre);

            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.F4))
                ChangerStyle(TypeCase.parquet);

            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.F5))
                Labyrinthe.CreerLabyrintheSimple(carte);

            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.F6))
                Labyrinthe.CreerLabyrintheDouble(carte);

            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.F7))
                Labyrinthe.CreerLabyrintheTriple(carte);

            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.F8))
                Labyrinthe.CreerLabyrintheQuadruple(carte);

            infos.Update();

            ScreenManager.Game.IsMouseVisible = !ServiceHelper.Get<IMouseService>().DansLaCarte();
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Gray, 0, 0); ;

            spriteBatch.Begin();

            carte.DrawInMapEditor(spriteBatch, content, camera);

            if (!enableOrigine1)
                spriteBatch.Draw(menu.ListeTexturesGauche[0], 28 * new Vector2(origine1.X - camera.X + 1, origine1.Y - camera.Y), Color.White);
            if (!enableOrigine2)
                spriteBatch.Draw(menu.ListeTexturesGauche[1], 28 * new Vector2(origine2.X - camera.X + 1, origine2.Y - camera.Y), Color.White);
            if (!enableDH)
                spriteBatch.Draw(menu.ListeTexturesGauche[5], 28 * new Vector2(_origineDark_Hero.X - camera.X + 1, _origineDark_Hero.Y - camera.Y), Color.White);

            foreach (Vector2 position in _originesGardes)
                spriteBatch.Draw(menu.ListeTexturesGauche[2], 28 * new Vector2(position.X - camera.X + 1, position.Y - camera.Y), Color.White);

            foreach (List<Vector2> parcours in _originesPatrouilleurs)
                for (int z = 0; z < parcours.Count; z++)
                {
                    if (z == 0)
                        spriteBatch.Draw(menu.ListeTexturesGauche[3], 28 * new Vector2(parcours[z].X - camera.X + 1, parcours[z].Y - camera.Y), Color.White);
                    else
                        spriteBatch.Draw(pointDePassage, 28 * new Vector2(parcours[z].X - camera.X + 1, parcours[z].Y - camera.Y), Color.CadetBlue);
                }

            foreach (List<Vector2> parcours in _originesPatrouilleursAChevaux)
                for (int v = 0; v < parcours.Count; v++)
                {
                    if (v == 0)
                        spriteBatch.Draw(menu.ListeTexturesGauche[4], 28 * new Vector2(parcours[v].X - camera.X + 1, parcours[v].Y - camera.Y), Color.White);
                    else
                        spriteBatch.Draw(pointDePassage, 28 * new Vector2(parcours[v].X - camera.X + 1, parcours[v].Y - camera.Y), Color.Yellow);
                }

            foreach (Vector2 position in _originesBoss)
                spriteBatch.Draw(menu.ListeTexturesGauche[6], 28 * new Vector2(position.X - camera.X + 1, position.Y - camera.Y), Color.White);

            for (int tamere = 0; tamere < rotationsDesStatues.Count; tamere++)
            {
                switch (rotationsDesStatues[tamere])
                {
                    case 0:
                        spriteBatch.Draw(textureStatue, 28 * new Vector2(_originesStatues[tamere].X - camera.X + 1, _originesStatues[tamere].Y - camera.Y), new Rectangle(0, 0, 112, 94), Color.White);
                        break;
                    case 1:
                        spriteBatch.Draw(textureStatue, 28 * new Vector2(_originesStatues[tamere].X - camera.X + 1, _originesStatues[tamere].Y - camera.Y), new Rectangle(0, 123, 112, 94), Color.White);
                        break;
                    case 2:
                        spriteBatch.Draw(textureStatue, 28 * new Vector2(_originesStatues[tamere].X - camera.X + 1, _originesStatues[tamere].Y - camera.Y), new Rectangle(0, 357, 112, 94), Color.White);
                        break;
                    case 3:
                        spriteBatch.Draw(textureStatue, 28 * new Vector2(_originesStatues[tamere].X - camera.X + 1, _originesStatues[tamere].Y - camera.Y), new Rectangle(0, 243, 112, 94), Color.White);
                        break;
                }
            }

            foreach (Bonus extra in bonus)
            {
                switch (extra.TypeBonus)
                {
                    case TypeBonus.shuriken:
                        spriteBatch.Draw(menu.ListeTexturesGauche[8], 28 * new Vector2(extra.X - camera.X + 1, extra.Y - camera.Y), Color.White);
                        break;
                    case TypeBonus.hadoken:
                        spriteBatch.Draw(menu.ListeTexturesGauche[9], 28 * new Vector2(extra.X - camera.X + 1, extra.Y - camera.Y), Color.White);
                        break;
                    case TypeBonus.checkPoint:
                        spriteBatch.Draw(menu.ListeTexturesGauche[10], 28 * new Vector2(extra.X - camera.X + 1, extra.Y - camera.Y), Color.White);
                        break;
                }
            }

            foreach (Interrupteur bouton in interrupteurs)
                bouton.DrawInMapEditor(spriteBatch, camera);

            if (ServiceHelper.Get<IMouseService>().DansLaCarte())
                curseur.Draw(spriteBatch);

            spriteBatch.Draw(fond, Vector2.Zero, null, Color.White, 0, Vector2.Zero, new Vector2(1, 30), SpriteEffects.None, 1);
            spriteBatch.Draw(fond, new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - 56, 0), null, Color.White, 0, Vector2.Zero, new Vector2(2, 30), SpriteEffects.None, 1);
            menu.Draw(spriteBatch, ascenseur, ScreenManager.GraphicsDevice.Viewport.Width);
            ascenseur.Draw(spriteBatch);
            spriteBatch.Draw(textureBasFond, new Vector2(0, Taille_Ecran.HAUTEUR_ECRAN - 84), null, Color.White, 0, Vector2.Zero, new Vector2((float)ScreenManager.GraphicsDevice.Viewport.Width / (float)textureBasFond.Width, (float)(ScreenManager.GraphicsDevice.Viewport.Height - Taille_Ecran.HAUTEUR_ECRAN + 84) / (float)textureBasFond.Height), SpriteEffects.FlipVertically, 1);
            infos.Draw(spriteBatch, ScreenManager.Font, !enableOrigine1, !enableOrigine2);

            if (chronometre > 3)
            {
                afficherMessageErreur = false;
                afficherMessageSauvegarde = false;
                chronometre = 0;
            }
            else if (afficherMessageErreur)
                spriteBatch.DrawString(ScreenManager.font, Langue.tr("EditorExCharacters"), new Vector2(10), Color.White);
            else if (afficherMessageSauvegarde)
                spriteBatch.DrawString(ScreenManager.font, Langue.tr("EditorSave1") + nomSauvegarde + extension + Langue.tr("EditorSave2"), new Vector2(10), Color.White);

            if (fileExist)
                spriteBatch.DrawString(ScreenManager.font, Langue.tr("FileExists1") + nomSauvegarde + extension + Langue.tr("FileExists2"), new Vector2(10), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void SauvegardeMap()
        {
            string[] existinglevels = LoadMapMenuScreen.ConcatenerTableaux(Directory.GetFiles(System.Windows.Forms.Application.StartupPath + "\\Story", "*.solo"), Directory.GetFiles(System.Windows.Forms.Application.StartupPath + "\\Story", "*.coop"));
            try
            {
                existinglevels = LoadMapMenuScreen.ConcatenerTableaux(existinglevels, Directory.GetFiles(System.Windows.Forms.Application.StartupPath + "\\Levels", "*.solo"));
                existinglevels = LoadMapMenuScreen.ConcatenerTableaux(existinglevels, Directory.GetFiles(System.Windows.Forms.Application.StartupPath + "\\Levels", "*.coop"));
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory("Levels");
            }

            if (enableOrigine1 && enableOrigine2 || _originesBoss.Count == 0)
                afficherMessageErreur = true;
            else
            {
                if (enableSave && nomCarte == "")
                {
                    if (origine1 == -Vector2.One || origine2 == -Vector2.One)
                        extension = ".solo";
                    else
                        extension = ".coop";
                }

                if (!enableSave || nomCarte != "")
                {
                    if (extension == ".solo" && (!enableOrigine1 && !enableOrigine2))
                        extension = ".coop";
                    else if (extension == ".coop" && ((enableOrigine1 && !enableOrigine2) || (!enableOrigine1 && enableOrigine2)))
                        extension = ".solo";
                }

                if (nomCarte == "" || nomCarte.Remove(nomCarte.Length - 5) != nomSauvegarde)
                {
                    foreach (string str in existinglevels)
                    {
                        fileExist = (str == (System.Windows.Forms.Application.StartupPath + "\\Levels\\" + nomSauvegarde + extension));
                        if (fileExist)
                            break;
                    }
                }

                if (!fileExist)
                {
                    sauvegarde = new StreamWriter(System.Windows.Forms.Application.StartupPath + "\\Levels\\" + nomSauvegarde + extension);

                    for (int y = 0; y < Taille_Map.HAUTEUR_MAP; y++)
                    {
                        for (int x = 0; x < Taille_Map.LARGEUR_MAP; x++)
                        {
                            switch (carte.Cases[y, x].Type)
                            {
                                case TypeCase.arbre:
                                    ligne += 'a';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.commode:
                                    ligne += 'b';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.lit:
                                    ligne += 'c';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.mur:
                                    ligne += 'd';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.murBlanc:
                                    ligne += 'e';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.murBlancDrap:
                                    ligne += 'f';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.murBlancEpee:
                                    ligne += 'g';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.murBlancTableau:
                                    ligne += 'h';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.murEpee:
                                    ligne += 'i';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.murTableau:
                                    ligne += 'j';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.tableauMurBlanc:
                                    ligne += 'k';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.tableMoyenne:
                                    ligne += 'l';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.grandeTable:
                                    ligne += 'm';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.grandeTableDeco:
                                    ligne += 'n';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.nvlHerbe:
                                    ligne += 'o';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.parquet:
                                    ligne += 'p';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.parquetArbre:
                                    ligne += 'q';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.parquetBuisson:
                                    ligne += 'r';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.pont1:
                                    ligne += 's';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.pont2:
                                    ligne += 't';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.bibliotheque:
                                    ligne += 'C';
                                    ligne += carte.Cases[y, x].Index;
                                    break;

                                case TypeCase.canape:
                                    ligne += 'u';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.canapeRalonge:
                                    ligne += 'v';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.fenetre:
                                    ligne += 'w';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.porteFenetre:
                                    ligne += 'x';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.grdSiege:
                                    ligne += 'y';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.pillier:
                                    ligne += 'z';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.porte:
                                    ligne += 'A';
                                    ligne += carte.Cases[y, x].Index;
                                    break;
                                case TypeCase.rocher:
                                    ligne += 'B';
                                    ligne += carte.Cases[y, x].Index;
                                    break;




                                case TypeCase.buissonSurHerbe:
                                    ligne += "a7";
                                    break;
                                case TypeCase.coinbotdroit:
                                    ligne += "a8";
                                    break;
                                case TypeCase.coinbotgauche:
                                    ligne += "a9";
                                    break;
                                case TypeCase.cointopdroit:
                                    ligne += "a0";
                                    break;
                                case TypeCase.cointopgauche:
                                    ligne += "b0";
                                    break;
                                case TypeCase.finMurDroit:
                                    ligne += "b5";
                                    break;
                                case TypeCase.finMurGauche:
                                    ligne += "b6";
                                    break;
                                case TypeCase.fondNoir:
                                    ligne += "b7";
                                    break;
                                case TypeCase.piedMurBois:
                                    ligne += "b8";
                                    break;
                                case TypeCase.bois:
                                    ligne += "b9";
                                    break;
                                case TypeCase.boisCarre:
                                    ligne += "c0";
                                    break;
                                case TypeCase.boisDeco:
                                    ligne += "c5";
                                    break;
                                case TypeCase.carlageNoir:
                                    ligne += "c6";
                                    break;
                                case TypeCase.carlageNoirDeco:
                                    ligne += "c7";
                                    break;
                                case TypeCase.herbe:
                                    ligne += "c8";
                                    break;
                                case TypeCase.herbeDeco:
                                    ligne += "c9";
                                    break;
                                case TypeCase.herbeFoncee:
                                    ligne += "d0";
                                    break;
                                case TypeCase.herbeH:
                                    ligne += "d5";
                                    break;
                                case TypeCase.tapisRougeBC:
                                    ligne += "d6";
                                    break;
                                case TypeCase.terre:
                                    ligne += "d7";
                                    break;
                                case TypeCase.finMurBas:
                                    ligne += "d8";
                                    break;
                                case TypeCase.finMurHaut:
                                    ligne += "d9";
                                    break;
                                case TypeCase.eau:
                                    ligne += "e5";
                                    break;
                                case TypeCase.caisse:
                                    ligne += "e6";
                                    break;
                                case TypeCase.chaiseGauche:
                                    ligne += "e7";
                                    break;
                                case TypeCase.chaiseDroite:
                                    ligne += "e8";
                                    break;
                            }
                        }
                        sauvegarde.WriteLine(ligne);
                        ligne = "";
                    }

                    sauvegarde.WriteLine("Joueurs");
                    if (origine1 != -Vector2.One)
                        sauvegarde.WriteLine(origine1.X + "," + origine1.Y + "," + infos.Munitions[0] + "," + infos.Munitions[1] + "," + infos.Munitions[2] + "," + infos.Munitions[3]);
                    if (origine2 != -Vector2.One)
                        sauvegarde.WriteLine(origine2.X + "," + origine2.Y + "," + infos.Munitions[4] + "," + infos.Munitions[5] + "," + infos.Munitions[6] + "," + infos.Munitions[7]);
                    sauvegarde.WriteLine("Gardes");
                    foreach (Vector2 position in _originesGardes)
                        sauvegarde.WriteLine(position.X + "," + position.Y);

                    sauvegarde.WriteLine("Patrouilleurs");
                    foreach (List<Vector2> parcours in _originesPatrouilleurs)
                    {
                        sauvegarde.WriteLine("New");
                        foreach (Vector2 position in parcours)
                            sauvegarde.WriteLine(position.X + "," + position.Y);
                    }

                    sauvegarde.WriteLine("Patrouilleurs A Cheval");
                    foreach (List<Vector2> parcours in _originesPatrouilleursAChevaux)
                    {
                        sauvegarde.WriteLine("New");
                        foreach (Vector2 position in parcours)
                            sauvegarde.WriteLine(position.X + "," + position.Y);
                    }

                    sauvegarde.WriteLine("Dark Hero");
                    if (_origineDark_Hero != -Vector2.One)
                        sauvegarde.WriteLine(_origineDark_Hero.X + "," + _origineDark_Hero.Y);

                    sauvegarde.WriteLine("Boss");
                    foreach (Vector2 position in _originesBoss)
                        sauvegarde.WriteLine(position.X + "," + position.Y);

                    sauvegarde.WriteLine("Statue dragon");
                    for (int k = 0; k < _originesStatues.Count; k++)
                        sauvegarde.WriteLine(_originesStatues[k].X + "," + _originesStatues[k].Y + "," + rotationsDesStatues[k]);

                    sauvegarde.WriteLine("Bonus");
                    foreach (Bonus extra in bonus)
                        sauvegarde.WriteLine(extra.X + "," + extra.Y + "," + (int)extra.TypeBonus);

                    sauvegarde.WriteLine("Interrupteurs");
                    foreach (Interrupteur bouton in interrupteurs)
                        sauvegarde.WriteLine(bouton.position.X + "," + bouton.position.Y + "," + bouton.PortePosition.X + "," + bouton.PortePosition.Y + "," + bouton.rotation + "," + bouton.PorteOuverte);

                    sauvegarde.WriteLine("Salaire");
                    sauvegarde.WriteLine(infos.Salaire);

                    sauvegarde.Close();
                    enableSave = false;

                    nomCarte = nomSauvegarde + extension;

                    afficherMessageSauvegarde = true;
                }

            }
        }

        private void Supprimer_Ennemi_Ou_Bonus()
        {
            for (int t = 0; t < _originesGardes.Count; t++)
                if (_originesGardes[t].X == curseur.Position.X + camera.X && _originesGardes[t].Y == curseur.Position.Y + camera.Y)
                    _originesGardes.RemoveAt(t);

            for (int t = 0; t < _originesPatrouilleurs.Count; t++)
                if (_originesPatrouilleurs[t][0].X == curseur.Position.X + camera.X && _originesPatrouilleurs[t][0].Y == curseur.Position.Y + camera.Y)
                    _originesPatrouilleurs.RemoveAt(t);

            for (int t = 0; t < _originesPatrouilleursAChevaux.Count; t++)
                if (_originesPatrouilleursAChevaux[t][0].X == curseur.Position.X + camera.X && _originesPatrouilleursAChevaux[t][0].Y == curseur.Position.Y + camera.Y)
                    _originesPatrouilleursAChevaux.RemoveAt(t);

            if (_origineDark_Hero.X == curseur.Position.X + camera.X && _origineDark_Hero.Y == curseur.Position.Y + camera.Y)
                _origineDark_Hero = -Vector2.One;

            for (int t = 0; t < _originesBoss.Count; t++)
                if (_originesBoss[t].X == curseur.Position.X + camera.X && _originesBoss[t].Y == curseur.Position.Y + camera.Y)
                    _originesBoss.RemoveAt(t);

            for (int t = 0; t < _originesStatues.Count; t++)
                if (_originesStatues[t].X == curseur.Position.X + camera.X && _originesStatues[t].Y == curseur.Position.Y + camera.Y)
                {
                    for (int y = 0; y < 4; y++)
                        for (int x = 1; x < 3; x++)
                            carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X + x].EstFranchissable = true;

                    _originesStatues.RemoveAt(t);
                    rotationsDesStatues.RemoveAt(t);
                }

            for (int i = 0; i < bonus.Count; i++)
                if (bonus[i].X == curseur.Position.X + camera.X && bonus[i].Y == curseur.Position.Y + camera.Y)
                    bonus.RemoveAt(i);

            for (int i = 0; i < interrupteurs.Count; i++)
                if (interrupteurs[i].position.X == curseur.Position.X + camera.X && interrupteurs[i].position.Y == curseur.Position.Y + camera.Y)
                {
                    switch (interrupteurs[i].rotation)
                    {
                        case 0:
                            carte.Cases[(int)interrupteurs[i].PortePosition.Y, (int)interrupteurs[i].PortePosition.X].EstFranchissable = true;
                            carte.Cases[(int)interrupteurs[i].PortePosition.Y, (int)interrupteurs[i].PortePosition.X + 1].EstFranchissable = true;
                            break;
                        case 1:
                            carte.Cases[(int)interrupteurs[i].PortePosition.Y, (int)interrupteurs[i].PortePosition.X - 1].EstFranchissable = true;
                            carte.Cases[(int)interrupteurs[i].PortePosition.Y + 1, (int)interrupteurs[i].PortePosition.X - 1].EstFranchissable = true;
                            break;
                        case 2:
                            carte.Cases[(int)interrupteurs[i].PortePosition.Y - 1, (int)interrupteurs[i].PortePosition.X - 1].EstFranchissable = true;
                            carte.Cases[(int)interrupteurs[i].PortePosition.Y - 1, (int)interrupteurs[i].PortePosition.X - 2].EstFranchissable = true;
                            break;
                        case 3:
                            carte.Cases[(int)interrupteurs[i].PortePosition.Y - 1, (int)interrupteurs[i].PortePosition.X].EstFranchissable = true;
                            carte.Cases[(int)interrupteurs[i].PortePosition.Y - 2, (int)interrupteurs[i].PortePosition.X].EstFranchissable = true;
                            break;
                    }
                    interrupteurs.RemoveAt(i);
                }
        }

        private void Placer_Personnage_Ou_Bonus()
        {
            if (curseur.Type == TypeCase.Joueur1 && CasePossible)
            {
                if (!enableOrigine1)
                    carte.Cases[(int)origine1.Y, (int)origine1.X].Type = TypeCase.herbe;
                else
                    enableOrigine1 = false;

                origine1 = new Vector2((int)curseur.Position.X + camera.X, (int)curseur.Position.Y + camera.Y);
            }

            else if (curseur.Type == TypeCase.Joueur2 && CasePossible)
            {
                if (!enableOrigine2)
                    carte.Cases[(int)origine2.Y, (int)origine2.X].Type = TypeCase.herbe;
                else
                    enableOrigine2 = false;

                origine2 = new Vector2((int)curseur.Position.X + camera.X, (int)curseur.Position.Y + camera.Y);
            }

            else if (curseur.Type == TypeCase.Dark_Hero && CasePossible)
            {
                enableDH = false;
                _origineDark_Hero = new Vector2(curseur.Position.X + camera.X, curseur.Position.Y + camera.Y);
            }

            else if (curseur.Type == TypeCase.Garde)
                _originesGardes.Add(new Vector2(curseur.Position.X + camera.X, curseur.Position.Y + camera.Y));

            else if (curseur.Type == TypeCase.Patrouilleur)
            {
                _originesPatrouilleurs.Add(new List<Vector2>());
                _originesPatrouilleurs[_originesPatrouilleurs.Count - 1].Add(new Vector2(curseur.Position.X + camera.X, curseur.Position.Y + camera.Y));
            }

            else if (curseur.Type == TypeCase.Patrouilleur_a_cheval)
            {
                _originesPatrouilleursAChevaux.Add(new List<Vector2>());
                _originesPatrouilleursAChevaux[_originesPatrouilleursAChevaux.Count - 1].Add(new Vector2(curseur.Position.X + camera.X, curseur.Position.Y + camera.Y));
            }


            else if (curseur.Type == TypeCase.Boss)
                _originesBoss.Add(new Vector2(curseur.Position.X + camera.X, curseur.Position.Y + camera.Y));

            else if (curseur.Type == TypeCase.Statues && curseur.Position.X + camera.X < Taille_Map.LARGEUR_MAP - 3 && curseur.Position.Y + camera.Y < Taille_Map.HAUTEUR_MAP - 3)
            {
                _originesStatues.Add(new Vector2(curseur.Position.X + camera.X, curseur.Position.Y + camera.Y));
                rotationsDesStatues.Add(0);

                for (int y = 0; y < 4; y++)
                    for (int x = 1; x < 3; x++)
                        carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X + x].EstFranchissable = false;
            }

            else if (curseur.Type == TypeCase.BonusShurikens)
                bonus.Add(new Bonus(28 * new Vector2(curseur.Position.X + camera.X, curseur.Position.Y + camera.Y), TypeBonus.shuriken));
            else if (curseur.Type == TypeCase.BonusHadokens)
                bonus.Add(new Bonus(28 * new Vector2(curseur.Position.X + camera.X, curseur.Position.Y + camera.Y), TypeBonus.hadoken));
            else if (curseur.Type == TypeCase.BonusCheckPoint)
                bonus.Add(new Bonus(28 * new Vector2(curseur.Position.X + camera.X, curseur.Position.Y + camera.Y), TypeBonus.checkPoint));
            else if (curseur.Type == TypeCase.Interrupteur)
            {
                interrupteurs.Add(new Interrupteur(new Vector2(curseur.Position.X + camera.X, curseur.Position.Y + camera.Y), carte));
                interrupteurs[interrupteurs.Count - 1].LoadContent(content);
            }
        }

        private void Placer_Case()
        {
            if (Math.Abs((int)curseur.Type) >= 50 && Math.Abs((int)curseur.Type) < 75)
            {
                if (curseur.Position.Y + camera.Y + 1 < Taille_Map.HAUTEUR_MAP && curseur.Position.X + camera.X + 1 < Taille_Map.LARGEUR_MAP && EmplacementPossible(2, 2))
                {
                    for (int x = 0; x < 2; x++)
                        for (int y = 0; y < 2; y++)
                        {
                            carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X + x].Type = curseur.Type;
                            carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X + x].Index = x + 2 * y + 1;
                            carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X + x].EstFranchissable = curseur.Type > 0;
                        }
                }
            }

            else if (Math.Abs((int)curseur.Type) >= 75 && Math.Abs((int)curseur.Type) < 100)
            {
                if (curseur.Position.Y + camera.Y + 2 < Taille_Map.HAUTEUR_MAP && curseur.Position.X + camera.X + 2 < Taille_Map.LARGEUR_MAP && EmplacementPossible(3, 3))
                {
                    for (int x = 0; x < 3; x++)
                        for (int y = 0; y < 3; y++)
                        {
                            carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X + x].Type = curseur.Type;
                            carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X + x].Index = x + 3 * y + 1;
                            carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X + x].EstFranchissable = curseur.Type > 0;
                        }
                }
            }

            else if ((int)curseur.Type == 18)
            {
                if (curseur.Position.X + camera.X + 1 < Taille_Map.LARGEUR_MAP && EmplacementPossible(2, 1))
                {
                    for (int x = 0; x < 2; x++)
                    {
                        carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X + x].Type = curseur.Type;
                        carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X + x].Index = x + 1;
                        carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X + x].EstFranchissable = curseur.Type > 0;
                    }
                }
            }

            else if (curseur.Type == TypeCase.bibliotheque)
            {
                if (curseur.Position.X + camera.X + 1 < Taille_Map.LARGEUR_MAP && EmplacementPossible(1, 2))
                {
                    for (int y = 0; y < 2; y++)
                    {
                        carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X].Type = curseur.Type;
                        carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X].Index = y + 1;
                        carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X].EstFranchissable = curseur.Type > 0;
                    }
                }
            }

            else if (curseur.Type == TypeCase.grdSiege || curseur.Type == TypeCase.porteFenetre)
            {
                if (curseur.Position.X + camera.X + 1 < Taille_Map.LARGEUR_MAP && EmplacementPossible(3, 2))
                {
                    for (int x = 0; x < 3; x++)
                        for (int y = 0; y < 2; y++)
                        {
                            carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X + x].Type = curseur.Type;
                            carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X + x].Index = x + 3 * y + 1;
                            carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X + x].EstFranchissable = curseur.Type > 0;
                        }
                }
            }

            else if ((int)curseur.Type == 19)
            {
                if (curseur.Position.X + camera.X + 1 < Taille_Map.LARGEUR_MAP && EmplacementPossible(4, 2))
                {
                    for (int x = 0; x < 4; x++)
                        for (int y = 0; y < 2; y++)
                        {
                            carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X + x].Type = curseur.Type;
                            carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X + x].Index = x + 4 * y + 1;
                            carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X + x].EstFranchissable = curseur.Type > 0;
                        }
                }
            }

            else if ((int)curseur.Type == -1 || curseur.Type == TypeCase.canape || curseur.Type == TypeCase.porteFenetre || curseur.Type == TypeCase.grdSiege)
            {
                if (curseur.Position.Y + camera.Y + 2 < Taille_Map.HAUTEUR_MAP && curseur.Position.X + camera.X + 1 < Taille_Map.LARGEUR_MAP && EmplacementPossible(2, 3))
                {
                    for (int x = 0; x < 2; x++)
                        for (int y = 0; y < 3; y++)
                        {
                            carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X + x].Type = curseur.Type;
                            carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X + x].Index = x + 2 * y + 1;
                            carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X + x].EstFranchissable = curseur.Type > 0;
                        }
                }
            }

            else if ((int)curseur.Type < 100)
            {
                carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X].Type = curseur.Type;
                carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X].Index = 1;
                carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X].EstFranchissable = curseur.Type > 0;
            }
        }

        private bool EmplacementPossible(int largeur, int hauteur)
        {
            for (int x = 0; x < largeur; x++)
                for (int y = 0; y < hauteur; y++)
                    if (carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X + x].Type == curseur.Type)
                        return false;

            return true;
        }

        private bool CasePossible
        {
            get { return carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X].EstFranchissable; }
        }

        private void ChangerStyle(TypeCase type)
        {
            for (int x = 0; x < Taille_Map.LARGEUR_MAP; x++)
                for (int y = 0; y < Taille_Map.HAUTEUR_MAP; y++)
                    carte.Cases[y, x].Type = type;
        }
    }
}