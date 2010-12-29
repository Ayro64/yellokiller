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
        StreamWriter sauvegarde;
        string ligne = "";
        KeyboardState keyboardState, lastKeyboardState;
        
        /// <summary>
        /// Cursor
        /// </summary>
        Texture2D cursor;
        Vector2 position;

        /// <summary>
        /// Menu
        /// </summary>
        SpriteFont font;
        Texture2D arbre, maison, mur, arbre2;

        public EditorScreen()
        {
            position = new Vector2(0, 0);
            carte = new MapEdit();
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            spriteBatch = ScreenManager.SpriteBatch;

            cursor = content.Load<Texture2D>("cursor");
            font = content.Load<SpriteFont>("courier");
            arbre = content.Load<Texture2D>("arbre");
            maison = content.Load<Texture2D>("maison");
            mur = content.Load<Texture2D>("mur");
            arbre2 = content.Load<Texture2D>("arbre2");
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            lastKeyboardState = keyboardState;
            keyboardState = input.CurrentKeyboardStates[playerIndex];

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
                ScreenManager.AddScreen(new PauseMenuScreen(0, 1), ControllingPlayer, true);
            }

            if (keyboardState.IsKeyDown(Keys.Left) && lastKeyboardState.IsKeyUp(Keys.Left) && position.X > 0)
                position.X--;
            if (keyboardState.IsKeyDown(Keys.Right) && lastKeyboardState.IsKeyUp(Keys.Right) && position.X < carte.largeurMap - 1)
                position.X++;
            if (keyboardState.IsKeyDown(Keys.Up) && lastKeyboardState.IsKeyUp(Keys.Up) && position.Y > 0)
                position.Y--;
            if (keyboardState.IsKeyDown(Keys.Down) && lastKeyboardState.IsKeyUp(Keys.Down) && position.Y < carte.hauteurMap - 1)
                position.Y++;

            if (keyboardState.IsKeyDown(Keys.F1))
                carte.map[(int)position.Y, (int)position.X] = 1;
            if (keyboardState.IsKeyDown(Keys.F2))
                carte.map[(int)position.Y, (int)position.X] = 2;
            if (keyboardState.IsKeyDown(Keys.F3))
                carte.map[(int)position.Y, (int)position.X] = 3;
            if (keyboardState.IsKeyDown(Keys.F4))
                carte.map[(int)position.Y, (int)position.X] = 4;

            if (keyboardState.IsKeyDown(Keys.LeftControl) && keyboardState.IsKeyDown(Keys.S))
            {
                sauvegarde = new StreamWriter("save.txt");
                for (int y = 0; y < carte.hauteurMap; y++)
                {
                    for (int x = 0; x < carte.largeurMap; x++)
                    {
                        ligne += carte.map[y, x].ToString();
                    }
                    sauvegarde.WriteLine(ligne);
                    ligne = "";
                }
                sauvegarde.Close();
                //Window.Title = "Fichier sauvegardé";
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.DarkOrchid, 0, 0); ;

            spriteBatch.Begin();

            
            carte.Draw(spriteBatch, content);
            
            spriteBatch.DrawString(font, " Touche F1", new Vector2(Taille_Map.LARGEURMAP * 28 + 10, 000), Color.Red);
            spriteBatch.DrawString(font, " Touche F2", new Vector2(Taille_Map.LARGEURMAP * 28 + 10, 100), Color.Red);
            spriteBatch.DrawString(font, " Touche F3", new Vector2(Taille_Map.LARGEURMAP * 28 + 10, 200), Color.Red);
            spriteBatch.DrawString(font, " Touche F4", new Vector2(Taille_Map.LARGEURMAP * 28 + 10, 300), Color.Red);
            spriteBatch.Draw(arbre, new Vector2(Taille_Map.LARGEURMAP * 28 + 61, 50), Color.White);
            spriteBatch.Draw(mur, new Vector2(Taille_Map.LARGEURMAP * 28 + 61, 150), Color.White);
            spriteBatch.Draw(maison, new Vector2(Taille_Map.LARGEURMAP * 28 + 61, 250), Color.White);
            spriteBatch.Draw(arbre2, new Vector2(Taille_Map.LARGEURMAP * 28 + 61, 350), Color.White);

            spriteBatch.Draw(cursor, new Vector2(position.X * cursor.Width, position.Y * cursor.Height), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
