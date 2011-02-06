using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Yellokiller
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
        string ligne = "", nomSauvegarde = "save0";
        Rectangle camera;
        Vector2 origine1 = new Vector2(-1, -1), origine2 = new Vector2(-1, -1);

        bool enableOrigine1 = true, enableOrigine2 = true, enableSave = true, afficheMessageErreur = false;
        double chronometre = 0;

        public EditorScreen()
        {
            camera = new Rectangle(0, 0, 30, 24);
            carte = new Carte(new Vector2(Taille_Map.LARGEUR_MAP, Taille_Map.HAUTEUR_MAP));
            carte.Initialisation(new Vector2(Taille_Map.LARGEUR_MAP, Taille_Map.HAUTEUR_MAP));
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            menu = new Menu(content, 9);
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


            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            int playerIndex = (int)ControllingPlayer.Value;

            ascenseur.Update();
            menu.Update(ascenseur);
            curseur.Update(content, menu);

            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new Pausebckground(), ControllingPlayer, true);
                ScreenManager.AddScreen(new PauseMenuScreen(0, 2), ControllingPlayer, true);
            }

            if (camera.X > 0 && ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.Left))
                camera.X--;

            else if (camera.X < Taille_Map.LARGEUR_MAP - camera.Width && ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.Right))
                camera.X++;

            else if (camera.Y > 0 && ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.Up))
                camera.Y--;

            else if (camera.Y < Taille_Map.HAUTEUR_MAP - camera.Height && ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.Down))
                camera.Y++;

            if (ServiceHelper.Get<IMouseService>().BoutonGauchePresse() && ServiceHelper.Get<IMouseService>().DansLaCarte())
            {
                if (curseur.Type != TypeCase.Joueur1 && curseur.Type != TypeCase.Joueur2)
                    carte.Cases[(int)curseur.Position.Y + camera.Y, (int)curseur.Position.X + camera.X].Type = curseur.Type;

                else if (curseur.Type == TypeCase.Joueur1)
                {
                    if (!enableOrigine1)
                        carte.Cases[(int)origine1.Y, (int)origine1.X].Type = TypeCase.herbe;
                    else
                        enableOrigine1 = false;

                    origine1 = new Vector2((int)curseur.Position.X + camera.X, (int)curseur.Position.Y + camera.Y);
                    carte.Cases[(int)origine1.Y, (int)origine1.X].Type = TypeCase.Joueur1;

                }
                else if (curseur.Type == TypeCase.Joueur2)
                {
                    if (!enableOrigine2)
                        carte.Cases[(int)origine2.Y, (int)origine2.X].Type = TypeCase.herbe;
                    else
                        enableOrigine2 = false;

                    origine2 = new Vector2((int)curseur.Position.X + camera.X, (int)curseur.Position.Y + camera.Y);
                    carte.Cases[(int)origine2.Y, (int)origine2.X].Type = TypeCase.Joueur2;
                }
            }

            if (ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.LeftControl) && ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.S) && enableSave)
            {
                if (enableOrigine1 && enableOrigine2)
                    afficheMessageErreur = true;
                else
                {
                    /*fileExist = File.Exists(nomSauvegarde + ".txt");
               while (fileExist)
               {
                   compteur += 1;
                   nomSauvegarde = nomSauvegarde.Substring(0, 4) + compteur.ToString();
                   fileExist = File.Exists(nomSauvegarde + ".txt");
               }*/

                    if (origine1 == -Vector2.One || origine2 == -Vector2.One)
                        nomSauvegarde = 'S' + nomSauvegarde;
                    else
                        nomSauvegarde = 'C' + nomSauvegarde;
                    
                    sauvegarde = new StreamWriter(nomSauvegarde + ".txt");

                    for (int y = 0; y < Taille_Map.HAUTEUR_MAP; y++)
                    {
                        for (int x = 0; x < Taille_Map.LARGEUR_MAP; x++)
                        {
                            switch (carte.Cases[y, x].Type)
                            {
                                case (TypeCase.herbe):
                                    ligne += 'h';
                                    break;
                                case (TypeCase.herbeFoncee):
                                    ligne += 'H';
                                    break;
                                case (TypeCase.arbre):
                                    ligne += 'a';
                                    break;
                                case (TypeCase.mur):
                                    ligne += 'm';
                                    break;
                                case (TypeCase.maison):
                                    ligne += 'M';
                                    break;
                                case (TypeCase.Ennemi):
                                    ligne += 'E';
                                    break;
                                case (TypeCase.Joueur1):
                                    ligne += 'o';
                                    break;
                                case (TypeCase.Joueur2):
                                    ligne += 'O';
                                    break;
                                case (TypeCase.arbre2):
                                    ligne += 'A';
                                    break;
                            }
                        }
                        sauvegarde.WriteLine(ligne);
                        ligne = "";
                    }
                    
                    sauvegarde.Close();
                    enableSave = false;
                }
            }

            ScreenManager.Game.IsMouseVisible = !ServiceHelper.Get<IMouseService>().DansLaCarte();
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.DarkOrchid, 0, 0); ;

            spriteBatch.Begin();

            carte.DrawInMapEditor(spriteBatch, content, camera);

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
                spriteBatch.DrawString(ScreenManager.font, "Le ou les personnages n'a / n'ont pas été placé.\n\nVeuillez placer un ou deux personnages avant de sauvegarder.\n\nMerci", new Vector2(10), Color.White);


            if (!enableSave)
                spriteBatch.DrawString(ScreenManager.font, "Fichier sauvegardé sous " + nomSauvegarde.ToString() + ".txt" + "\n\nAppuyez sur ECHAP pour quitter.", new Vector2(10), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}