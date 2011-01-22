using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;
using System.IO;
using System;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;

namespace Yellokiller
{
   public class EditorScreen : GameScreen
    {
        SpriteBatch spriteBatch;
        ContentManager content;
       
        Carte carte;
        Cursor curseur;
        Menu menu;
        Souris souris;
        Ascenseur ascenseur;

        StreamWriter sauvegarde;
        string ligne = "", nomSauvegarde = "save0";
        KeyboardState keyboardState, lastKeyboardState;
        Rectangle camera;
        Vector2 origine1 = new Vector2(-1, -1), origine2 = new Vector2(-1, -1);
        
        bool enableOrigine1 = true, enableOrigine2 = true, enableSave = true, afficheMessageErreur = false;
        int chronometre = 0;

        public EditorScreen()
        {
            souris = new Souris();
            camera = new Rectangle(0, 0, 28, 22);
            carte = new Carte(new Vector2(Taille_Map.LARGEUR_MAP, Taille_Map.HAUTEUR_MAP));
            carte.Initialisation(new Vector2(Taille_Map.LARGEUR_MAP, Taille_Map.HAUTEUR_MAP));
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            menu = new Menu(content, 6);
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

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            if (afficheMessageErreur)
                chronometre++;
            else
                chronometre = 0;

            lastKeyboardState = keyboardState;
            keyboardState = input.CurrentKeyboardStates[playerIndex];

            souris.Update();
            ascenseur.Update(souris);
            menu.Update(ascenseur);
            curseur.Update(content, souris, menu);

            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new Pausebckground(), ControllingPlayer, true);
                ScreenManager.AddScreen(new PauseMenuScreen(0, 2), ControllingPlayer, true);
            }

            if (camera.X > 0 && (souris.Rectangle.X < 28 && souris.Rectangle.X > 0 || keyboardState.IsKeyDown(Keys.Left)))
                camera.X--;

            if (camera.X < Taille_Map.LARGEUR_MAP - camera.Width && (souris.Rectangle.X > Taille_Ecran.LARGEUR_ECRAN - 84 && souris.Rectangle.X < Taille_Ecran.LARGEUR_ECRAN - 56 || keyboardState.IsKeyDown(Keys.Right)))
                camera.X++;

            if (camera.Y > 0 && (souris.Rectangle.Y < 28 && souris.Rectangle.Y > 0 || keyboardState.IsKeyDown(Keys.Up)))
                camera.Y--;

            if (camera.Y < Taille_Map.HAUTEUR_MAP - camera.Height && (souris.Rectangle.Y > Taille_Ecran.HAUTEUR_ECRAN - 28 && souris.Rectangle.Y < Taille_Ecran.HAUTEUR_ECRAN || keyboardState.IsKeyDown(Keys.Down)))
                camera.Y++;
                        
            if (souris.MState.LeftButton == ButtonState.Pressed && souris.DansLaCarte)
            {
                if (curseur.Type != TypeCase.origineJoueur1 && curseur.Type != TypeCase.origineJoueur2)
                    carte.Cases[(int)curseur.Position.Y + camera.Y - 1, (int)curseur.Position.X + camera.X - 1].Type = curseur.Type;
                else if (curseur.Type == TypeCase.origineJoueur1 && enableOrigine1)
                {
                    enableOrigine1 = false;
                    carte.Cases[(int)curseur.Position.Y + camera.Y - 1, (int)curseur.Position.X + camera.X - 1].Type = TypeCase.origineJoueur1;
                }
                else if (curseur.Type == TypeCase.origineJoueur2 && enableOrigine2)
                {
                    enableOrigine2 = false;
                    carte.Cases[(int)curseur.Position.Y + camera.Y - 1, (int)curseur.Position.X + camera.X - 1].Type = TypeCase.origineJoueur2;
                }
            }

            if (keyboardState.IsKeyDown(Keys.LeftControl) && keyboardState.IsKeyDown(Keys.S) && enableSave)
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
                            if (carte.Cases[y, x].Type == TypeCase.origineJoueur1)
                            {
                                ligne += 'h';
                                origine1 = new Vector2(x, y);
                            }
                            else if (carte.Cases[y, x].Type == TypeCase.origineJoueur2)
                            {
                                ligne += 'h';
                                origine2 = new Vector2(x, y);
                            }

                            else
                                ligne += (char)carte.Cases[y, x].Type;
                        }
                        sauvegarde.WriteLine(ligne);
                        ligne = "";
                    }
                    sauvegarde.WriteLine(origine1.X);
                    sauvegarde.WriteLine(origine1.Y);
                    sauvegarde.WriteLine(origine2.X);
                    sauvegarde.WriteLine(origine2.Y);

                    sauvegarde.Close();
                    enableSave = false;
                }
            }

            ScreenManager.Game.IsMouseVisible = !souris.DansLaCarte;
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.DarkOrchid, 0, 0); ;

            spriteBatch.Begin();

            carte.DrawInMapEditor(spriteBatch, content, camera);

            if (souris.DansLaCarte)
                curseur.Draw(spriteBatch);

            menu.Draw(spriteBatch, ascenseur, souris);
            ascenseur.Draw(spriteBatch);

            if (chronometre > 0)
                spriteBatch.DrawString(ScreenManager.font, "Un ou des personnages n'a / n'ont pas été placé.\n\nVeuillez placer les deux personnages avant de sauvegarder.\n\nMerci", new Vector2(100), Color.Red);
            if (chronometre > 300)
                afficheMessageErreur = false;

            if(!enableSave)
                spriteBatch.DrawString(ScreenManager.font, "Fichier sauvegardé sous " + nomSauvegarde.ToString() + ".txt" + "\n\nAppuyez sur ECHAP pour quitter.", new Vector2(100), Color.Red);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}