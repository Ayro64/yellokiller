using System;
using System.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

/* Si vous souhaitez rajouter une texture au jeu, soit vous me le demandez, soit vous tentez de le faire vous meme em completant les 4 switchs
 * dans Carte.cs (deux fois), Case.cs, Cursor.cs, la rajouter dans la liste de textures dans Menu.cs, et de rajouter sa valeur dans l'enumeration
 dans GameplayScreenSolo.cs */

namespace YelloKiller
{
    public class EditorScreen : GameScreen
    {
        SpriteBatch spriteBatch;
        ContentManager content;

        Carte carte;
        Curseur curseur;
        Menu menu;
        Ascenseur ascenseur1, ascenseur2;
        YellokillerGame game;
        StreamWriter sauvegarde;
        string ligne, nomSauvegarde, nomCarte, extension;
        Rectangle camera;
        Vector2 origine1, origine2;
        List<Vector2> _originesGardes, _originesBoss, _originesStatues, bonusShurikens, bonusHadokens;
        List<byte> rotationsDesStatues;
        List<List<Vector2>> _originesPatrouilleurs, _originesPatrouilleursAChevaux;
        Texture2D pointDePassage, fond, textureStatue;
        bool fileExist;
        int compteur, salaire;

        bool enableOrigine1, enableOrigine2, enableSave, afficheMessageErreur;
        double chronometre = 0;

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
            afficheMessageErreur = false;
            camera = new Rectangle(0, 0, 32, 27);
            carte = new Carte(new Vector2(Taille_Map.LARGEUR_MAP, Taille_Map.HAUTEUR_MAP));

            if (nomCarte == "")
                carte.Initialisation(new Vector2(Taille_Map.LARGEUR_MAP, Taille_Map.HAUTEUR_MAP));
            else
                carte.OuvrirCarte(nomCarte);

            _originesGardes = carte.OriginesGardes;
            _originesPatrouilleurs = carte.OriginesPatrouilleurs;
            _originesPatrouilleursAChevaux = carte.OriginesPatrouilleursAChevaux;
            _originesBoss = carte.OriginesBoss;
            _originesStatues = carte.OriginesStatues;
            rotationsDesStatues = carte.RotationsDesStatues;
            bonusShurikens = carte.BonusShurikens;
            bonusHadokens = carte.BonusHadokens;

            origine1 = carte.OrigineJoueur1;
            origine2 = carte.OrigineJoueur2;
            salaire = 100000;

            enableOrigine1 = (carte.OrigineJoueur1 == -Vector2.One);
            enableOrigine2 = (carte.OrigineJoueur2 == -Vector2.One);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            // Si vous ajoutez une texture, oubliez pas de changer le nombre de textures en parametres dans le constructeur du menu ci-dessous.
            menu = new Menu(content, 52, 9/*<-- ici*/);
            curseur = new Curseur(content);
            ascenseur1 = new Ascenseur(content, Taille_Ecran.LARGEUR_ECRAN - 28);
            ascenseur2 = new Ascenseur(content, 0);
            fond = content.Load<Texture2D>(@"Textures\Invisible");
            pointDePassage = content.Load<Texture2D>(@"Menu Editeur de Maps\pied");
            textureStatue = content.Load<Texture2D>(@"Feuilles de sprites\statue_dragon");

            spriteBatch = ScreenManager.SpriteBatch;
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (afficheMessageErreur)
                chronometre += gameTime.ElapsedGameTime.TotalSeconds;

            ascenseur1.Update();
            ascenseur2.Update();
            menu.Update(ascenseur1, ascenseur2);
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
                ScreenManager.AddScreen(new PauseMenuScreen(0, 2, game), ControllingPlayer, true);
            }

            if (camera.X > 0 && ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.Left))
                camera.X--;

            else if (camera.X < Taille_Map.LARGEUR_MAP - camera.Width && ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.Right))
                camera.X++;

            else if (camera.Y > 0 && ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.Up))
                camera.Y--;

            else if (camera.Y < Taille_Map.HAUTEUR_MAP - camera.Height && ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.Down))
                camera.Y++;

            if (ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.Delete))
                Supprimer_Ennemi_Ou_Bonus();

            if (ServiceHelper.Get<IMouseService>().ClicBoutonGauche() && ServiceHelper.Get<IMouseService>().DansLaCarte())
                Placer_Personnage_Ou_Bonus();

            if (ServiceHelper.Get<IMouseService>().ClicBoutonDroit() && ServiceHelper.Get<IMouseService>().DansLaCarte())
            {
                if (curseur.Type == TypeCase.Patrouilleur)
                    _originesPatrouilleurs[_originesPatrouilleurs.Count - 1].Add(new Vector2(curseur.Position.X + camera.X, curseur.Position.Y + camera.Y));
                else if (curseur.Type == TypeCase.Patrouilleur_a_cheval)
                    _originesPatrouilleursAChevaux[_originesPatrouilleursAChevaux.Count - 1].Add(new Vector2(curseur.Position.X + camera.X, curseur.Position.Y + camera.Y));
                else if (_originesStatues.Count > 0 && curseur.Type == TypeCase.Statues)
                    rotationsDesStatues[rotationsDesStatues.Count - 1] = (byte)((rotationsDesStatues[rotationsDesStatues.Count - 1] + 1) % 4);
            }

            if (ServiceHelper.Get<IMouseService>().BoutonGauchePresse() && ServiceHelper.Get<IMouseService>().DansLaCarte())
                Placer_Case();

            if (ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.LeftControl) && ServiceHelper.Get<IKeyboardService>().ToucheAEtePressee(Keys.S))
                SauvegardeMap();

            ScreenManager.Game.IsMouseVisible = !ServiceHelper.Get<IMouseService>().DansLaCarte();
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Gray, 0, 0); ;

            spriteBatch.Begin();

            carte.DrawInMapEditor(spriteBatch, content, camera);

            if (!enableOrigine1)
                spriteBatch.Draw(menu.ListeTexturesGauche[0], 28 * new Vector2(origine1.X - camera.X + 2, origine1.Y - camera.Y), Color.White);
            if (!enableOrigine2)
                spriteBatch.Draw(menu.ListeTexturesGauche[1], 28 * new Vector2(origine2.X - camera.X + 2, origine2.Y - camera.Y), Color.White);

            foreach (Vector2 position in _originesGardes)
                spriteBatch.Draw(menu.ListeTexturesGauche[2], 28 * new Vector2(position.X - camera.X + 2, position.Y - camera.Y), Color.White);

            foreach (List<Vector2> parcours in _originesPatrouilleurs)
                for (int z = 0; z < parcours.Count; z++)
                {
                    if (z == 0)
                        spriteBatch.Draw(menu.ListeTexturesGauche[3], 28 * new Vector2(parcours[z].X - camera.X + 2, parcours[z].Y - camera.Y), Color.White);
                    else
                        spriteBatch.Draw(pointDePassage, 28 * new Vector2(parcours[z].X - camera.X + 2, parcours[z].Y - camera.Y), Color.Red);
                }

            foreach (List<Vector2> parcours in _originesPatrouilleursAChevaux)
                for (int v = 0; v < parcours.Count; v++)
                {
                    if (v == 0)
                        spriteBatch.Draw(menu.ListeTexturesGauche[4], 28 * new Vector2(parcours[v].X - camera.X + 2, parcours[v].Y - camera.Y), Color.White);
                    else
                        spriteBatch.Draw(pointDePassage, 28 * new Vector2(parcours[v].X - camera.X + 2, parcours[v].Y - camera.Y), Color.Yellow);
                }

            foreach (Vector2 position in _originesBoss)
                spriteBatch.Draw(menu.ListeTexturesGauche[5], 28 * new Vector2(position.X - camera.X + 2, position.Y - camera.Y), Color.White);

            for (int tamere = 0; tamere < rotationsDesStatues.Count; tamere++)
            {
                switch (rotationsDesStatues[tamere])
                {
                    case 0:
                        spriteBatch.Draw(textureStatue, 28 * new Vector2(_originesStatues[tamere].X - camera.X + 2, _originesStatues[tamere].Y - camera.Y), new Rectangle(0, 0, 112, 94), Color.White);
                        break;
                    case 1:
                        spriteBatch.Draw(textureStatue, 28 * new Vector2(_originesStatues[tamere].X - camera.X + 2, _originesStatues[tamere].Y - camera.Y), new Rectangle(0, 123, 112, 94), Color.White);
                        break;
                    case 2:
                        spriteBatch.Draw(textureStatue, 28 * new Vector2(_originesStatues[tamere].X - camera.X + 2, _originesStatues[tamere].Y - camera.Y), new Rectangle(0, 357, 112, 94), Color.White);
                        break;
                    case 3:
                        spriteBatch.Draw(textureStatue, 28 * new Vector2(_originesStatues[tamere].X - camera.X + 2, _originesStatues[tamere].Y - camera.Y), new Rectangle(0, 243, 112, 94), Color.White);
                        break;
                }
            }

            foreach (Vector2 bonus in bonusShurikens)
                spriteBatch.Draw(menu.ListeTexturesGauche[7], 28 * new Vector2(bonus.X - camera.X + 2, bonus.Y - camera.Y), Color.White);

            foreach (Vector2 bonus in bonusHadokens)
                spriteBatch.Draw(menu.ListeTexturesGauche[8], 28 * new Vector2(bonus.X - camera.X + 2, bonus.Y - camera.Y), Color.White);

            if (ServiceHelper.Get<IMouseService>().DansLaCarte())
                curseur.Draw(spriteBatch);

            spriteBatch.Draw(fond, Vector2.Zero, null, Color.White, 0, Vector2.Zero, new Vector2(2, 27), SpriteEffects.None, 1);
            spriteBatch.Draw(fond, new Vector2(Taille_Ecran.LARGEUR_ECRAN - 56, 0), null, Color.White, 0, Vector2.Zero, new Vector2(2, 27), SpriteEffects.None, 1);

            menu.Draw(spriteBatch, ascenseur1, ascenseur2);
            ascenseur1.Draw(spriteBatch);
            ascenseur2.Draw(spriteBatch);

            if (chronometre > 3)
            {
                afficheMessageErreur = false;
                chronometre = 0;
            }
            else if (chronometre > 0)
                spriteBatch.DrawString(ScreenManager.font, Langue.tr("EditorExCharacters"), new Vector2(10), Color.White);

            if (!enableSave)
                spriteBatch.DrawString(ScreenManager.font, Langue.tr("EditorSave1") + nomSauvegarde.ToString() + extension + Langue.tr("EditorSave2"), new Vector2(10), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void SauvegardeMap()
        {
            if (enableOrigine1 && enableOrigine2 || _originesBoss.Count == 0)
                afficheMessageErreur = true;
            else
            {
                if (enableSave && nomCarte == "")
                {
                    nomSauvegarde = "save1";
                    if (origine1 == -Vector2.One || origine2 == -Vector2.One)
                        extension = ".solo";
                    else
                        extension = ".coop";
                }

                if (!enableSave || nomCarte != "")
                {
                    if (extension == ".solo" && (!enableOrigine1 && !enableOrigine2))
                    {
                        nomSauvegarde = "save" + compteur.ToString();
                        extension = ".coop";
                    }
                    else if (extension == ".coop" && ((enableOrigine1 && !enableOrigine2) || (!enableOrigine1 && enableOrigine2)))
                    {
                        nomSauvegarde = "save" + compteur.ToString();
                        extension = ".solo";
                    }
                }

                fileExist = File.Exists(nomSauvegarde + extension);
                while (fileExist && enableSave && nomCarte == "")
                {
                    compteur++;
                    nomSauvegarde = nomSauvegarde.Substring(0, nomSauvegarde.Length - 1) + compteur.ToString();
                    fileExist = File.Exists(nomSauvegarde + extension);
                }

                sauvegarde = new StreamWriter(nomSauvegarde + extension);

                for (int y = 0; y < Taille_Map.HAUTEUR_MAP; y++)
                {
                    for (int x = 0; x < Taille_Map.LARGEUR_MAP; x++)
                    {
                        switch (carte.Cases[y, x].Type)
                        {
                            case TypeCase.arbre:
                                ligne += 'a';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.commode:
                                ligne += 'b';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.lit:
                                ligne += 'c';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.mur:
                                ligne += 'd';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.murBlanc:
                                ligne += 'e';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.murBlancDrap:
                                ligne += 'f';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.murBlancEpee:
                                ligne += 'g';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.murBlancTableau:
                                ligne += 'h';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.murEpee:
                                ligne += 'i';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.murTableau:
                                ligne += 'j';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.tableauMurBlanc:
                                ligne += 'k';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.tableMoyenne:
                                ligne += 'l';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.grandeTable:
                                ligne += 'm';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.grandeTableDeco:
                                ligne += 'n';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.nvlHerbe:
                                ligne += 'o';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.parquet:
                                ligne += 'p';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.parquetArbre:
                                ligne += 'q';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.parquetBuisson:
                                ligne += 'r';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.pont1:
                                ligne += 's';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.pont2:
                                ligne += 't';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.bibliotheque:
                                ligne += 'C';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            
                            case TypeCase.canape:
                                ligne += 'u';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.canapeRalonge:
                                ligne += 'v';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.fenetre:
                                ligne += 'w';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.porteFenetre:
                                ligne += 'x';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.grdSiege:
                                ligne += 'y';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.pillier:
                                ligne += 'z';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.porte:
                                ligne += 'A';
                                ligne += carte.Cases[y, x].Etienne.Z;
                                break;
                            case TypeCase.rocher:
                                ligne += 'B';
                                ligne += carte.Cases[y, x].Etienne.Z;
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
                        }
                    }
                    sauvegarde.WriteLine(ligne);
                    ligne = "";
                }

                sauvegarde.WriteLine("Joueurs");
                if (origine1 != -Vector2.One)
                {
                    sauvegarde.WriteLine(origine1.X);
                    sauvegarde.WriteLine(origine1.Y);
                }
                if (origine2 != -Vector2.One)
                {
                    sauvegarde.WriteLine(origine2.X);
                    sauvegarde.WriteLine(origine2.Y);
                }
                sauvegarde.WriteLine("Gardes");
                foreach (Vector2 position in _originesGardes)
                {
                    sauvegarde.WriteLine(position.X);
                    sauvegarde.WriteLine(position.Y);
                }

                sauvegarde.WriteLine("Patrouilleurs");
                foreach (List<Vector2> parcours in _originesPatrouilleurs)
                {
                    sauvegarde.WriteLine("New");
                    foreach (Vector2 position in parcours)
                    {
                        sauvegarde.WriteLine(position.X);
                        sauvegarde.WriteLine(position.Y);
                    }
                }

                sauvegarde.WriteLine("Patrouilleurs A Cheval");
                foreach (List<Vector2> parcours in _originesPatrouilleursAChevaux)
                {
                    sauvegarde.WriteLine("New");
                    foreach (Vector2 position in parcours)
                    {
                        sauvegarde.WriteLine(position.X);
                        sauvegarde.WriteLine(position.Y);
                    }
                }

                sauvegarde.WriteLine("Boss");
                foreach (Vector2 position in _originesBoss)
                {
                    sauvegarde.WriteLine(position.X);
                    sauvegarde.WriteLine(position.Y);
                }

                sauvegarde.WriteLine("Statue dragon");
                for (int k = 0; k < _originesStatues.Count; k++)
                {
                    sauvegarde.WriteLine(_originesStatues[k].X);
                    sauvegarde.WriteLine(_originesStatues[k].Y);
                    sauvegarde.WriteLine(rotationsDesStatues[k]);
                }

                sauvegarde.WriteLine("Bonus Shurikens");
                foreach (Vector2 bonus in bonusShurikens)
                {
                    sauvegarde.WriteLine(bonus.X);
                    sauvegarde.WriteLine(bonus.Y);
                }

                sauvegarde.WriteLine("Bonus Hadokens");
                foreach (Vector2 bonus in bonusHadokens)
                {
                    sauvegarde.WriteLine(bonus.X);
                    sauvegarde.WriteLine(bonus.Y);
                }

                sauvegarde.WriteLine("Salaire");
                sauvegarde.WriteLine(salaire);

                sauvegarde.Close();
                enableSave = false;
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

            for (int t = 0; t < _originesBoss.Count; t++)
                if (_originesBoss[t].X == curseur.Position.X + camera.X && _originesBoss[t].Y == curseur.Position.Y + camera.Y)
                    _originesBoss.RemoveAt(t);

            for (int t = 0; t < _originesStatues.Count; t++)
                if (_originesStatues[t].X == curseur.Position.X + camera.X && _originesStatues[t].Y == curseur.Position.Y + camera.Y)
                {
                    _originesStatues.RemoveAt(t);
                    rotationsDesStatues.RemoveAt(t);
                }

            for (int pt = 0; pt < bonusShurikens.Count; pt++)
                if (bonusShurikens[pt].X == curseur.Position.X + camera.X && bonusShurikens[pt].Y == curseur.Position.Y + camera.Y)
                    bonusShurikens.RemoveAt(pt);

            for (int miaou = 0; miaou < bonusHadokens.Count; miaou++)
                if (bonusHadokens[miaou].X == curseur.Position.X + camera.X && bonusHadokens[miaou].Y == curseur.Position.Y + camera.Y)
                    bonusHadokens.RemoveAt(miaou);
        }

        private void Placer_Personnage_Ou_Bonus()
        {
            if (curseur.Type == TypeCase.Joueur1)
            {
                if (!enableOrigine1)
                    carte.Cases[(int)origine1.Y, (int)origine1.X].Type = TypeCase.herbe;
                else
                    enableOrigine1 = false;

                origine1 = new Vector2((int)curseur.Position.X + camera.X, (int)curseur.Position.Y + camera.Y);
            }

            else if (curseur.Type == TypeCase.Joueur2)
            {
                if (!enableOrigine2)
                    carte.Cases[(int)origine2.Y, (int)origine2.X].Type = TypeCase.herbe;
                else
                    enableOrigine2 = false;

                origine2 = new Vector2((int)curseur.Position.X + camera.X, (int)curseur.Position.Y + camera.Y);
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

            else if (curseur.Type == TypeCase.Statues)
            {
                _originesStatues.Add(new Vector2(curseur.Position.X + camera.X, curseur.Position.Y + camera.Y));
                rotationsDesStatues.Add(0);
            }

            else if (curseur.Type == TypeCase.BonusShurikens)
                bonusShurikens.Add(new Vector2(curseur.Position.X + camera.X, curseur.Position.Y + camera.Y));

            else if (curseur.Type == TypeCase.BonusHadokens)
                bonusHadokens.Add(new Vector2(curseur.Position.X + camera.X, curseur.Position.Y + camera.Y));
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
                            carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X + x].Etienne_Z = x + 2 * y + 1;
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
                            carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X + x].Etienne_Z = x + 3 * y + 1;
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
                        carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X + x].Etienne_Z = x + 1;
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
                        carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X].Etienne_Z = y + 1;
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
                            carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X + x].Etienne_Z = x + 3 * y + 1;
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
                            carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X + x].Etienne_Z = x + 4 * y + 1;
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
                            carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X + x].Etienne_Z = x + 2 * y + 1;
                        }
                }
            }

            else if ((int)curseur.Type < 100)
            {
                carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X].Type = curseur.Type;
                carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X].Etienne_Z = 1;
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
    }
}