using System;
using System.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

/* Si quelqu'un touche a cette classe, je lui demonte sa mere. */

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
        Cursor curseur;
        Menu menu;
        Ascenseur ascenseur;

        StreamWriter sauvegarde;
        string ligne, nomSauvegarde, nomCarte;
        Rectangle camera;
        Vector2 origine1, origine2;
        List<Vector2> _originesGardes, _originesPatrouilleurs, _originesPatrouilleursAChevaux, _originesBoss;
        bool fileExist;
        int compteur;

        bool enableOrigine1, enableOrigine2, enableSave, afficheMessageErreur;
        double chronometre = 0;

        public EditorScreen(string nomCarte)
        {
            if (nomCarte == "")
            {
                nomSauvegarde = nomCarte;
                compteur = 0;
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
            camera = new Rectangle(0, 0, 30, 24);
            carte = new Carte(new Vector2(Taille_Map.LARGEUR_MAP, Taille_Map.HAUTEUR_MAP));

            if (nomCarte == "")
                carte.Initialisation(new Vector2(Taille_Map.LARGEUR_MAP, Taille_Map.HAUTEUR_MAP));
            else
                carte.OuvrirCarte(nomCarte);

            _originesGardes = carte.OriginesGardes;
            _originesPatrouilleurs = carte.OriginesPatrouilleurs;
            _originesPatrouilleursAChevaux = carte.OriginesPatrouilleursAChevaux;
            _originesBoss = carte.OriginesBoss;
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
            menu = new Menu(content, 26);
            curseur = new Cursor(content);
            ascenseur = new Ascenseur(content);

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

            ascenseur.Update();
            menu.Update(ascenseur);
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
                ScreenManager.AddScreen(new PauseMenuScreen(0, 2), ControllingPlayer, true);
            }

            if (camera.X > 0 && ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.Left))
                camera.X--;

            else if (camera.X < Taille_Map.LARGEUR_MAP - camera.Width && ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.Right))
                camera.X++;

            else if (camera.Y > 0 && ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.Up))
                camera.Y--;

            else if (camera.Y < Taille_Map.HAUTEUR_MAP - camera.Height && ServiceHelper.Get<IKeyboardService>().TouchePressee(Keys.Down))
                camera.Y++;

            if (ServiceHelper.Get<IMouseService>().ClicBoutonGauche() && ServiceHelper.Get<IMouseService>().DansLaCarte())
            {
                if (curseur.Type == TypeCase.Garde)
                    _originesGardes.Add(new Vector2(curseur.Position.X + camera.X, curseur.Position.Y + camera.Y));

                else if (curseur.Type == TypeCase.Patrouilleur)
                    _originesPatrouilleurs.Add(new Vector2(curseur.Position.X + camera.X, curseur.Position.Y + camera.Y));

                else if (curseur.Type == TypeCase.Patrouilleur_a_cheval)
                    _originesPatrouilleursAChevaux.Add(new Vector2(curseur.Position.X + camera.X, curseur.Position.Y + camera.Y));

                else if (curseur.Type == TypeCase.Boss)
                    _originesBoss.Add(new Vector2(curseur.Position.X + camera.X, curseur.Position.Y + camera.Y));
            }

            if (ServiceHelper.Get<IMouseService>().BoutonGauchePresse() && ServiceHelper.Get<IMouseService>().DansLaCarte())
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
                else if (curseur.Type != TypeCase.Garde && curseur.Type != TypeCase.Patrouilleur_a_cheval && curseur.Type != TypeCase.Patrouilleur && curseur.Type != TypeCase.Boss && (int)carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X].Type != -6 /*-6 = fond*/)
                {
                    if (curseur.Type == TypeCase.arbre2)
                    {
                        if (curseur.Position.Y + camera.Y - 1 >= 0 && curseur.Position.X + camera.X - 1 >= 0 && curseur.Position.Y + camera.Y + 1 < Taille_Map.HAUTEUR_MAP && curseur.Position.X + camera.X + 1 < Taille_Map.LARGEUR_MAP &&
                            carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X + 1].Type != TypeCase.arbre2 &&
                            carte.Cases[(int)curseur.Position.Y + camera.Y - 1, (int)curseur.Position.X + camera.X + 1].Type != TypeCase.arbre2 &&
                            carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X + 1].Type != TypeCase.arbre2 &&
                            carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X].Type != TypeCase.arbre2 &&
                            carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X - 1].Type != TypeCase.arbre2)
                        {
                            carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X].Type = curseur.Type;

                            if ((int)curseur.Position.Y + camera.Y + 1 < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X < Taille_Map.LARGEUR_MAP)
                                carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X].Type = TypeCase.fond;

                            if ((int)curseur.Position.Y + camera.Y < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X + 1 < Taille_Map.LARGEUR_MAP)
                                carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X + 1].Type = TypeCase.fond;

                            if ((int)curseur.Position.Y + camera.Y + 1 < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X + 1 < Taille_Map.LARGEUR_MAP)
                                carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X + 1].Type = TypeCase.fond;
                        }
                    }
                    else if (curseur.Type == TypeCase.murBlanc)
                    {
                        if (curseur.Position.Y + camera.Y - 1 >= 0 && curseur.Position.X + camera.X - 1 >= 0 && curseur.Position.Y + camera.Y + 1 < Taille_Map.HAUTEUR_MAP && curseur.Position.X + camera.X + 1 < Taille_Map.LARGEUR_MAP &&
                            carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X + 1].Type != TypeCase.murBlanc &&
                            carte.Cases[(int)curseur.Position.Y + camera.Y - 1, (int)curseur.Position.X + camera.X + 1].Type != TypeCase.murBlanc &&
                            carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X + 1].Type != TypeCase.murBlanc &&
                            carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X].Type != TypeCase.murBlanc &&
                            carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X - 1].Type != TypeCase.murBlanc)
                        {
                            carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X].Type = curseur.Type;

                            if ((int)curseur.Position.Y + camera.Y + 1 < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X < Taille_Map.LARGEUR_MAP)
                                carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X].Type = TypeCase.fond;

                            if ((int)curseur.Position.Y + camera.Y < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X + 1 < Taille_Map.LARGEUR_MAP)
                                carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X + 1].Type = TypeCase.fond;

                            if ((int)curseur.Position.Y + camera.Y + 1 < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X + 1 < Taille_Map.LARGEUR_MAP)
                                carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X + 1].Type = TypeCase.fond;
                        }
                    }
                    else if (curseur.Type == TypeCase.tableauMurBlanc)
                    {
                        if (curseur.Position.Y + camera.Y - 1 >= 0 && curseur.Position.X + camera.X - 1 >= 0 && curseur.Position.Y + camera.Y + 1 < Taille_Map.HAUTEUR_MAP && curseur.Position.X + camera.X + 1 < Taille_Map.LARGEUR_MAP &&
                            carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X + 1].Type != TypeCase.tableauMurBlanc &&
                            carte.Cases[(int)curseur.Position.Y + camera.Y - 1, (int)curseur.Position.X + camera.X + 1].Type != TypeCase.tableauMurBlanc &&
                            carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X + 1].Type != TypeCase.tableauMurBlanc &&
                            carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X].Type != TypeCase.tableauMurBlanc &&
                            carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X - 1].Type != TypeCase.tableauMurBlanc)
                        {
                            carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X].Type = curseur.Type;

                            if ((int)curseur.Position.Y + camera.Y + 1 < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X < Taille_Map.LARGEUR_MAP)
                                carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X].Type = TypeCase.fond;

                            if ((int)curseur.Position.Y + camera.Y < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X + 1 < Taille_Map.LARGEUR_MAP)
                                carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X + 1].Type = TypeCase.fond;

                            if ((int)curseur.Position.Y + camera.Y + 1 < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X + 1 < Taille_Map.LARGEUR_MAP)
                                carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X + 1].Type = TypeCase.fond;
                        }
                    }
                    else if (curseur.Type == TypeCase.Lit)
                    {
                        if (curseur.Position.Y + camera.Y - 1 >= 0 && curseur.Position.X + camera.X - 1 >= 0 && curseur.Position.Y + camera.Y + 1 < Taille_Map.HAUTEUR_MAP && curseur.Position.X + camera.X + 1 < Taille_Map.LARGEUR_MAP &&
                            carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X + 1].Type != TypeCase.Lit &&
                            carte.Cases[(int)curseur.Position.Y + camera.Y - 1, (int)curseur.Position.X + camera.X + 1].Type != TypeCase.Lit &&
                            carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X + 1].Type != TypeCase.Lit &&
                            carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X].Type != TypeCase.Lit &&
                            carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X - 1].Type != TypeCase.Lit)
                        {
                            carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X].Type = curseur.Type;

                            if ((int)curseur.Position.Y + camera.Y < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X + 1 < Taille_Map.LARGEUR_MAP)
                                carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X + 1].Type = TypeCase.fond;

                            if ((int)curseur.Position.Y + camera.Y < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X + 2 < Taille_Map.LARGEUR_MAP)
                                carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X + 2].Type = TypeCase.fond;

                            if ((int)curseur.Position.Y + camera.Y + 1 < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X < Taille_Map.LARGEUR_MAP)
                                carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X].Type = TypeCase.fond;

                            if ((int)curseur.Position.Y + camera.Y + 1 < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X + 1 < Taille_Map.LARGEUR_MAP)
                                carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X + 1].Type = TypeCase.fond;

                            if ((int)curseur.Position.Y + camera.Y + 1 < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X + 2 < Taille_Map.LARGEUR_MAP)
                                carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X + 2].Type = TypeCase.fond;

                            if ((int)curseur.Position.Y + camera.Y + 2 < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X < Taille_Map.LARGEUR_MAP)
                                carte.Cases[(int)curseur.Position.Y + camera.Y + 2, (int)curseur.Position.X + camera.X].Type = TypeCase.fond;

                            if ((int)curseur.Position.Y + camera.Y + 2 < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X + 1 < Taille_Map.LARGEUR_MAP)
                                carte.Cases[(int)curseur.Position.Y + camera.Y + 2, (int)curseur.Position.X + camera.X + 1].Type = TypeCase.fond;

                            if ((int)curseur.Position.Y + camera.Y + 2 < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X + 2 < Taille_Map.LARGEUR_MAP)
                                carte.Cases[(int)curseur.Position.Y + camera.Y + 2, (int)curseur.Position.X + camera.X + 2].Type = TypeCase.fond;
                        }
                    }
                    else if (curseur.Type == TypeCase.GrandeTable)
                    {
                        if (curseur.Position.Y + camera.Y - 1 >= 0 && curseur.Position.X + camera.X - 1 >= 0 && curseur.Position.Y + camera.Y + 1 < Taille_Map.HAUTEUR_MAP && curseur.Position.X + camera.X + 1 < Taille_Map.LARGEUR_MAP &&
                            carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X + 1].Type != TypeCase.GrandeTable &&
                            carte.Cases[(int)curseur.Position.Y + camera.Y - 1, (int)curseur.Position.X + camera.X + 1].Type != TypeCase.GrandeTable &&
                            carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X + 1].Type != TypeCase.GrandeTable &&
                            carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X].Type != TypeCase.GrandeTable &&
                            carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X - 1].Type != TypeCase.GrandeTable)
                        {
                            carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X].Type = curseur.Type;

                            if ((int)curseur.Position.Y + camera.Y < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X + 1 < Taille_Map.LARGEUR_MAP)
                                carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X + 1].Type = TypeCase.fond;

                            if ((int)curseur.Position.Y + camera.Y < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X + 2 < Taille_Map.LARGEUR_MAP)
                                carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X + 2].Type = TypeCase.fond;

                            if ((int)curseur.Position.Y + camera.Y + 1 < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X < Taille_Map.LARGEUR_MAP)
                                carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X].Type = TypeCase.fond;

                            if ((int)curseur.Position.Y + camera.Y + 1 < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X + 1 < Taille_Map.LARGEUR_MAP)
                                carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X + 1].Type = TypeCase.fond;

                            if ((int)curseur.Position.Y + camera.Y + 1 < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X + 2 < Taille_Map.LARGEUR_MAP)
                                carte.Cases[(int)curseur.Position.Y + camera.Y + 1, (int)curseur.Position.X + camera.X + 2].Type = TypeCase.fond;

                            if ((int)curseur.Position.Y + camera.Y + 2 < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X < Taille_Map.LARGEUR_MAP)
                                carte.Cases[(int)curseur.Position.Y + camera.Y + 2, (int)curseur.Position.X + camera.X].Type = TypeCase.fond;

                            if ((int)curseur.Position.Y + camera.Y + 2 < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X + 1 < Taille_Map.LARGEUR_MAP)
                                carte.Cases[(int)curseur.Position.Y + camera.Y + 2, (int)curseur.Position.X + camera.X + 1].Type = TypeCase.fond;

                            if ((int)curseur.Position.Y + camera.Y + 2 < Taille_Map.HAUTEUR_MAP && (int)curseur.Position.X + camera.X + 2 < Taille_Map.LARGEUR_MAP)
                                carte.Cases[(int)curseur.Position.Y + camera.Y + 2, (int)curseur.Position.X + camera.X + 2].Type = TypeCase.fond;
                        }
                    }
                    else
                        carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X].Type = curseur.Type;
                }
            }

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
                spriteBatch.Draw(menu.ListeTextures[0], 28 * new Vector2(origine1.X - camera.X, origine1.Y - camera.Y), Color.White);
            if (!enableOrigine2)
                spriteBatch.Draw(menu.ListeTextures[1], 28 * new Vector2(origine2.X - camera.X, origine2.Y - camera.Y), Color.White);

            foreach (Vector2 position in _originesGardes)
                spriteBatch.Draw(menu.ListeTextures[2], 28 * new Vector2(position.X - camera.X, position.Y - camera.Y), Color.White);

            foreach (Vector2 position in _originesPatrouilleurs)
                spriteBatch.Draw(menu.ListeTextures[3], 28 * new Vector2(position.X - camera.X, position.Y - camera.Y), Color.White);

            foreach (Vector2 position in _originesPatrouilleursAChevaux)
                spriteBatch.Draw(menu.ListeTextures[4], 28 * new Vector2(position.X - camera.X, position.Y - camera.Y), Color.White);

            foreach (Vector2 position in _originesBoss)
                spriteBatch.Draw(menu.ListeTextures[5], 28 * new Vector2(position.X - camera.X, position.Y - camera.Y), Color.White);

            if (ServiceHelper.Get<IMouseService>().DansLaCarte())
                curseur.Draw(spriteBatch);

            menu.Draw(spriteBatch, ascenseur);
            ascenseur.Draw(spriteBatch);

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
                        nomSauvegarde = "Ssave0";
                    else
                        nomSauvegarde = "Csave0";
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
                foreach (Vector2 position in _originesPatrouilleurs)
                {
                    sauvegarde.WriteLine(position.X);
                    sauvegarde.WriteLine(position.Y);
                }

                sauvegarde.WriteLine("Patrouilleurs A Cheval");
                foreach (Vector2 position in _originesPatrouilleursAChevaux)
                {
                    sauvegarde.WriteLine(position.X);
                    sauvegarde.WriteLine(position.Y);
                }

                sauvegarde.WriteLine("Boss");
                foreach (Vector2 position in _originesBoss)
                {
                    sauvegarde.WriteLine(position.X);
                    sauvegarde.WriteLine(position.Y);
                }

                sauvegarde.Close();
                enableSave = false;
            }
        }
    }
}