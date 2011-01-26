using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

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
        int chronometre = 0;

        public EditorScreen()
        {
            camera = new Rectangle(0, 0, 28, 22);
            carte = new Carte(new Vector2(Taille_Map.LARGEUR_MAP, Taille_Map.HAUTEUR_MAP));
            carte.Initialisation(new Vector2(Taille_Map.LARGEUR_MAP, Taille_Map.HAUTEUR_MAP));
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            menu = new Menu(content, 8);
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
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            
            int playerIndex = (int)ControllingPlayer.Value;

            if (afficheMessageErreur)
                chronometre++;
            else
                chronometre = 0;

            ascenseur.Update();
            menu.Update(ascenseur);
            curseur.Update(content, menu);

            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

          
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new Pausebckground(), ControllingPlayer, true);
                ScreenManager.AddScreen(new PauseMenuScreen(0, 2), ControllingPlayer, true);
            }

            if (camera.X > 0 && (ServiceHelper.Get<IMouseService>().Coordonnees().X < 28 && ServiceHelper.Get<IMouseService>().Coordonnees().X > 0 || ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.Left)))
                camera.X--;

            else if (camera.X < Taille_Map.LARGEUR_MAP - camera.Width && (ServiceHelper.Get<IMouseService>().Coordonnees().X > Taille_Ecran.LARGEUR_ECRAN - 84 && ServiceHelper.Get<IMouseService>().Coordonnees().X < Taille_Ecran.LARGEUR_ECRAN - 56 || ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.Right)))
                camera.X++;

            else if (camera.Y > 0 && (ServiceHelper.Get<IMouseService>().Coordonnees().Y < 28 && ServiceHelper.Get<IMouseService>().Coordonnees().Y > 0 || ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.Up)))
                camera.Y--;

            else if (camera.Y < Taille_Map.HAUTEUR_MAP - camera.Height && (ServiceHelper.Get<IMouseService>().Coordonnees().Y > Taille_Ecran.HAUTEUR_ECRAN - 28 && ServiceHelper.Get<IMouseService>().Coordonnees().Y < Taille_Ecran.HAUTEUR_ECRAN || ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.Down)))
                camera.Y++;

            if (ServiceHelper.Get<IMouseService>().BoutonGauchePresse() && ServiceHelper.Get<IMouseService>().DansLaCarte())
            {
                if (curseur.Type != TypeCase.origineJoueur1 && curseur.Type != TypeCase.origineJoueur2)
                    carte.Cases[(int)curseur.Position.Y + camera.Y - 1, (int)curseur.Position.X + camera.X - 1].Type = curseur.Type;

                else if (curseur.Type == TypeCase.origineJoueur1)
                {
                    if(!enableOrigine1)
                        carte.Cases[(int)origine1.Y, (int)origine1.X].Type = TypeCase.herbe;
                    else
                        enableOrigine1 = false;

                    origine1 = new Vector2((int)curseur.Position.X + camera.X - 1, (int)curseur.Position.Y + camera.Y - 1);
                    carte.Cases[(int)origine1.Y, (int)origine1.X].Type = TypeCase.origineJoueur1;

                }
                else if (curseur.Type == TypeCase.origineJoueur2)
                {
                    if(!enableOrigine2)
                        carte.Cases[(int)origine2.Y, (int)origine2.X].Type = TypeCase.herbe;
                    else
                        enableOrigine2 = false;

                    origine2 = new Vector2((int)curseur.Position.X + camera.X - 1, (int)curseur.Position.Y + camera.Y - 1);
                    carte.Cases[(int)origine2.Y, (int)origine2.X].Type = TypeCase.origineJoueur2;
                }
            }

            if (ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.LeftControl) && ServiceHelper.Get<IKeyboardService>().TouchePresse(Keys.S) && enableSave)
            {
                if (enableOrigine1 || enableOrigine2)
                {
                    afficheMessageErreur = true;
                    chronometre = 0;
                }
                else if (!(enableOrigine1 || enableOrigine2))
                {

                    /*fileExist = File.Exists(nomSauvegarde + ".txt");
               while (fileExist)
               {
                   compteur += 1;
                   nomSauvegarde = nomSauvegarde.Substring(0, 4) + compteur.ToString();
                   fileExist = File.Exists(nomSauvegarde + ".txt");
               }*/

                    sauvegarde = new StreamWriter(nomSauvegarde + ".txt");

                    for (int y = 0; y < Taille_Map.HAUTEUR_MAP; y++)
                    {
                        for (int x = 0; x < Taille_Map.LARGEUR_MAP; x++)
                        {
                            /*if (carte.Cases[y, x].Type == TypeCase.origineJoueur1)
                                ligne += 'h';
                            else if (carte.Cases[y, x].Type == TypeCase.origineJoueur2)
                                ligne += 'h';
                            else*/
                                ligne += (char)carte.Cases[y, x].Type;
                        }
                        sauvegarde.WriteLine(ligne);
                        ligne = "";
                    }
                    /*sauvegarde.WriteLine(origine1.X);
                    sauvegarde.WriteLine(origine1.Y);
                    sauvegarde.WriteLine(origine2.X);
                    sauvegarde.WriteLine(origine2.Y);*/

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

            if (chronometre > 0)
                spriteBatch.DrawString(ScreenManager.font, "Un ou des personnages n'a / n'ont pas été placé.\n\nVeuillez placer les deux personnages avant de sauvegarder.\n\nMerci", new Vector2(10), Color.White);
            if (chronometre > 300)
                afficheMessageErreur = false;

            if(!enableSave)
                spriteBatch.DrawString(ScreenManager.font, "Fichier sauvegardé sous " + nomSauvegarde.ToString() + ".txt" + "\n\nAppuyez sur ECHAP pour quitter.", new Vector2(10), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}