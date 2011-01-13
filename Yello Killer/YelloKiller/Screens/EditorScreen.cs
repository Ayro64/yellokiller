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
    class EditorScreen : GameScreen
    {
        SpriteBatch spriteBatch;
        ContentManager content;

        MapEdit carte;
        Cursor curseur;
        StreamWriter sauvegarde;
        KeyboardState keyboardState, lastKeyboardState;
        MouseState MState, lastMState;
        Textures_Choice menu;
        Vector2 origine1 = new Vector2(-1, -1), origine2 = new Vector2(-1, -1);
        
        string ligne = "", nomSauvegarde = "save0";
        bool enableOrigine1 = true, enableOrigine2 = true, fileExist = false, enableSave = true, afficheMessageErreur = false, afficheMessageSauvegarde = false;
        int compteur = 0, chronometre = 0;

        public EditorScreen()
        {
            menu = new Textures_Choice();
            carte = new MapEdit();
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            curseur = new Cursor(content);
            menu.LoadContent(content);
            
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

            lastMState = MState;
            MState = Mouse.GetState();

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

            curseur.Update(content, Taille_Map.LARGEURMAP, Taille_Map.HAUTEURMAP, MState, lastMState);
            
            if (MState.LeftButton == ButtonState.Pressed && curseur.Enable)
            {
                if (curseur.Dessin != 'o' && curseur.Dessin != 'O')
                    carte.map[(int)curseur.Position.Y, (int)curseur.Position.X] = curseur.Dessin;
                else if (curseur.Dessin == 'o' && enableOrigine1)
                {
                    enableOrigine1 = false;
                    carte.map[(int)curseur.Position.Y, (int)curseur.Position.X] = 'o';
                }
                else if (curseur.Dessin == 'O' && enableOrigine2)
                {
                    enableOrigine2 = false;
                    carte.map[(int)curseur.Position.Y, (int)curseur.Position.X] = 'O';
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

                    for (int y = 0; y < carte.hauteurMap; y++)
                    {
                        for (int x = 0; x < carte.largeurMap; x++)
                        {
                            if (carte.map[y, x] == 'o')
                            {
                                ligne += 'h';
                                origine1 = new Vector2(x, y);
                            }
                            else if (carte.map[y, x] == 'O')
                            {
                                ligne += 'h';
                                origine2 = new Vector2(x, y);
                            }

                            else
                                ligne += carte.map[y, x];
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
                    afficheMessageSauvegarde = true;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.DarkOrchid, 0, 0); ;

            spriteBatch.Begin();

            carte.Draw(spriteBatch, content);
            curseur.Draw(spriteBatch);
            menu.Draw(spriteBatch, curseur);

            if (chronometre > 0)
                spriteBatch.DrawString(ScreenManager.font, "Un ou des personnages n'a / n'ont pas été placé.\n\nVeuillez placer les deux personnages avant de sauvegarder.\n\nMerci", new Vector2(100), Color.Red);
            if (chronometre > 300)
                afficheMessageErreur = false;

            if(afficheMessageSauvegarde)
                spriteBatch.DrawString(ScreenManager.font, "Fichier sauvegardé sous " + nomSauvegarde.ToString() + ".txt" + "\n\nAppuyez sur ECHAP pour quitter.", new Vector2(100), Color.Red);

                    
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}