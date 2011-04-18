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
        string ligne, nomSauvegarde, nomCarte;
        Rectangle camera;
        Vector2 origine1, origine2;
        List<Vector2> _originesGardes, _originesBoss, _originesStatues;
        List<byte> rotationsDesStatues;
        List<List<Vector2>> _originesPatrouilleurs, _originesPatrouilleursAChevaux;
        Texture2D pointDePassagePatrouilleur, pointDePassagePatrouilleurACheval, fond;
        bool fileExist;
        int compteur;

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
                nomSauvegarde = nomCarte.Substring(0, 6);
                compteur = nomCarte[nomCarte.Length - 4];
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
            rotationsDesStatues = carte.RotationsDesStatues; ;
            origine1 = carte.OrigineJoueur1;
            origine2 = carte.OrigineJoueur2;

            enableOrigine1 = (carte.OrigineJoueur1 == -Vector2.One);
            enableOrigine2 = (carte.OrigineJoueur2 == -Vector2.One);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            // Si vous ajoutez une texture, oubliez pas de changer le nombre de textures en parametres dans le constructeur du menu ci-dessous.
            menu = new Menu(content, 21, 7/*<-- ici*/);
            curseur = new Curseur(content);
            ascenseur1 = new Ascenseur(content, Taille_Ecran.LARGEUR_ECRAN - 28);
            ascenseur2 = new Ascenseur(content, 0);
            fond = content.Load<Texture2D>(@"Textures\Invisible");
            pointDePassagePatrouilleur = content.Load<Texture2D>("pied");
            pointDePassagePatrouilleurACheval = content.Load<Texture2D>("pied");

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
                SupprimerEnnemi();

            if (ServiceHelper.Get<IMouseService>().ClicBoutonGauche() && ServiceHelper.Get<IMouseService>().DansLaCarte())
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

                else
                    carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X].Type = curseur.Type;
            }

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
                PlacerUneCaseInfranchissable();

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
                        spriteBatch.Draw(pointDePassagePatrouilleur, 28 * new Vector2(parcours[z].X - camera.X + 2, parcours[z].Y - camera.Y), Color.White);
                }

            foreach (List<Vector2> parcours in _originesPatrouilleursAChevaux)
                for (int v = 0; v < parcours.Count; v++)
                {
                    if (v == 0)
                        spriteBatch.Draw(menu.ListeTexturesGauche[4], 28 * new Vector2(parcours[v].X - camera.X + 2, parcours[v].Y - camera.Y), Color.White);
                    else
                        spriteBatch.Draw(pointDePassagePatrouilleurACheval, 28 * new Vector2(parcours[v].X - camera.X + 2, parcours[v].Y - camera.Y), Color.White);
                }

            foreach (Vector2 position in _originesBoss)
                spriteBatch.Draw(menu.ListeTexturesGauche[5], 28 * new Vector2(position.X - camera.X + 2, position.Y - camera.Y), Color.White);

            for (int tamere = 0; tamere < rotationsDesStatues.Count; tamere++)
            {
                switch (rotationsDesStatues[tamere])
                {
                    case 0:
                        spriteBatch.Draw(menu.ListeTexturesGauche[6], 28 * new Vector2(_originesStatues[tamere].X - camera.X + 3, _originesStatues[tamere].Y - camera.Y + 1), null, Color.White, (float)Math.PI, Vector2.Zero, 1, SpriteEffects.None, 1);
                        break;
                    case 1:
                        spriteBatch.Draw(menu.ListeTexturesGauche[6], 28 * new Vector2(_originesStatues[tamere].X - camera.X + 2, _originesStatues[tamere].Y - camera.Y + 1), null, Color.White, -(float)Math.PI / 2f, Vector2.Zero, 1, SpriteEffects.None, 1);
                        break;
                    case 2:
                        spriteBatch.Draw(menu.ListeTexturesGauche[6], 28 * new Vector2(_originesStatues[tamere].X - camera.X + 2, _originesStatues[tamere].Y - camera.Y), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                        break;
                    case 3:
                        spriteBatch.Draw(menu.ListeTexturesGauche[6], 28 * new Vector2(_originesStatues[tamere].X - camera.X + 3, _originesStatues[tamere].Y - camera.Y), null, Color.White, (float)Math.PI / 2f, Vector2.Zero, 1, SpriteEffects.None, 1);
                        break;
                }
            }

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
                spriteBatch.DrawString(ScreenManager.font, Langue.tr("EditorSave1") + nomSauvegarde.ToString() + Langue.tr("EditorSave2"), new Vector2(10), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void SauvegardeMap()
        {
            if (enableOrigine1 && enableOrigine2)
                afficheMessageErreur = true;
            else
            {
                if (enableSave && nomCarte == "")
                {
                    if (origine1 == -Vector2.One || origine2 == -Vector2.One)
                        nomSauvegarde = "Ssave1";
                    else
                        nomSauvegarde = "Csave1";
                }

                if (!enableSave || nomCarte != "")
                {
                    if (nomSauvegarde[0] == 'S' && (!enableOrigine1 && !enableOrigine2))
                        nomSauvegarde = "Csave" + compteur.ToString();
                    else if (nomSauvegarde[0] == 'C' && ((enableOrigine1 && !enableOrigine2) || (!enableOrigine1 && enableOrigine2)))
                        nomSauvegarde = "Ssave" + compteur.ToString();
                }

                fileExist = File.Exists(nomSauvegarde + ".txt");
                while (fileExist && enableSave && nomCarte == "")
                {
                    compteur++;
                    nomSauvegarde = nomSauvegarde.Substring(0, 5) + compteur.ToString();
                    fileExist = File.Exists(nomSauvegarde + ".txt");
                }

                sauvegarde = new StreamWriter(nomSauvegarde + ".txt");

                for (int y = 0; y < Taille_Map.HAUTEUR_MAP; y++)
                {
                    for (int x = 0; x < Taille_Map.LARGEUR_MAP; x++)
                    {
                        switch (carte.Cases[y, x].Type)
                        {
                            case (TypeCase.arbre):
                                ligne += 'a';
                                break;
                            case (TypeCase.arbre2):
                                ligne += 'A';
                                break;
                            case (TypeCase.buissonSurHerbe):
                                ligne += 's';
                                break;
                            case (TypeCase.murBlanc):
                                ligne += 'm';
                                break;
                            case (TypeCase.tableauMurBlanc):
                                ligne += 't';
                                break;
                            case (TypeCase.bois):
                                ligne += 'b';
                                break;
                            case (TypeCase.boisCarre):
                                ligne += 'B';
                                break;
                            case (TypeCase.tapisRougeBC):
                                ligne += 'T';
                                break;
                            case (TypeCase.herbe):
                                ligne += 'h';
                                break;
                            case (TypeCase.herbeFoncee):
                                ligne += 'H';
                                break;
                            case (TypeCase.piedDeMurBois):
                                ligne += 'p';
                                break;
                            case (TypeCase.terre):
                                ligne += 'r';
                                break;
                            case (TypeCase.carlageNoir):
                                ligne += 'c';
                                break;
                            case (TypeCase.fondNoir):
                                ligne += 'f';
                                break;
                            case (TypeCase.finMurFN):
                                ligne += 'F';
                                break;
                            case (TypeCase.finMurGauche):
                                ligne += 'g';
                                break;
                            case (TypeCase.finMurDroite):
                                ligne += 'd';
                                break;
                            case (TypeCase.fond):
                                ligne += 'a';
                                break;
                            case (TypeCase.Lit):
                                ligne += 'l';
                                break;
                            case (TypeCase.commode):
                                ligne += 'L';
                                break;
                            case (TypeCase.TableMoyenne):
                                ligne += 'y';
                                break;
                            case (TypeCase.GrandeTable):
                                ligne += 'Y';
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

                sauvegarde.Close();
                enableSave = false;
            }
        }

        private void SupprimerEnnemi()
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
        }

        private void PlacerUneCaseInfranchissable()
        {
            if ((int)curseur.Type <= -50 && (int)curseur.Type > -75)
            {
                if (curseur.Position.Y + camera.Y + 1 < Taille_Map.HAUTEUR_MAP && curseur.Position.X + camera.X + 1 < Taille_Map.LARGEUR_MAP && EmplacementPossible(2, 2))
                {
                    carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X].Type = curseur.Type;

                    for (int x = 0; x < 2; x++)
                        for (int y = 0; y < 2; y++)
                            if ((x != 0 || y != 0) && (int)curseur.Position.Y + camera.Y + y < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X + x < Taille_Map.LARGEUR_MAP)
                                if ((int)curseur.Position.Y + camera.Y + 1 < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X < Taille_Map.LARGEUR_MAP)
                                    carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X + x].Type = TypeCase.fond;
                }
            }
            else if ((int)curseur.Type <= -75 && (int)curseur.Type > -100)
            {
                if (curseur.Position.Y + camera.Y + 2 < Taille_Map.HAUTEUR_MAP && curseur.Position.X + camera.X + 2 < Taille_Map.LARGEUR_MAP && EmplacementPossible(3, 3))
                {
                    carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X].Type = curseur.Type;

                    for (int x = 0; x < 3; x++)
                        for (int y = 0; y < 3; y++)
                            if ((x != 0 || y != 0) && (int)curseur.Position.Y + camera.Y + y < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X + x < Taille_Map.LARGEUR_MAP)
                                if ((int)curseur.Position.Y + camera.Y + 1 < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X < Taille_Map.LARGEUR_MAP)
                                    carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X + x].Type = TypeCase.fond;
                }
            }
            else if ((int)curseur.Type < 100)
                carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X].Type = curseur.Type;
        }

        private bool EmplacementPossible(int largeur, int hauteur)
        {
            for (int x = 0; x < largeur; x++)
                for (int y = 0; y < hauteur; y++)
                    if (carte.Cases[(int)curseur.Position.Y + camera.Y + y, (int)curseur.Position.X + camera.X + x].Type == TypeCase.fond)
                        return false;

            if ((int)carte.Cases[(int)curseur.Position.Y + camera.Y + hauteur - 1, (int)curseur.Position.X + camera.X + largeur - 1].Type < 0)
                return false;

            return true;
        }
    }
}