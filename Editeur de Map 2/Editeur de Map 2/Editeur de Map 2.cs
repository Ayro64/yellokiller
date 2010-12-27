using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;
using System.IO;
using System;

namespace Editeur_de_Map_2
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Map carte;
        Cursor curseur;
        StreamWriter sauvegarde;
        string ligne = "", save = "";
        KeyboardState keyboardState, lastKeyboardState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.Components.Add(new GamerServicesComponent(this));
            graphics.PreferredBackBufferHeight = 476;
            graphics.PreferredBackBufferWidth = 616;
        }

        protected override void Initialize()
        {
            base.Initialize();

            carte = new Map();
            curseur = new Cursor(Content);
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

       /* public void EndShowKeyboardInput( )
        {
            string userInput = ;
        }*/

        protected override void Update(GameTime gameTime)
        {
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            curseur.Update(gameTime, carte.largeurMap, carte.hauteurMap);

            if (Keyboard.GetState().IsKeyDown(Keys.F1))
                carte.map[(int)curseur.Position.Y, (int)curseur.Position.X] = 1;
            if (Keyboard.GetState().IsKeyDown(Keys.F2))
                carte.map[(int)curseur.Position.Y, (int)curseur.Position.X] = 2;
            if (Keyboard.GetState().IsKeyDown(Keys.F3))
                carte.map[(int)curseur.Position.Y, (int)curseur.Position.X] = 3;

            if (keyboardState.IsKeyDown(Keys.LeftControl) && keyboardState.IsKeyDown(Keys.S) && lastKeyboardState.IsKeyUp(Keys.S))
            {
                //Guide.BeginShowKeyboardInput(PlayerIndex.One, "Editeur de Map", "Entrez le nom de sauvegarde", "", EndShowKeyboardInput, null);
                save += ".txt";
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
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            carte.Draw(spriteBatch, Content);
            curseur.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
