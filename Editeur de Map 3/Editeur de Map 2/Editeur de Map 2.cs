using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;
using System.IO;
using System;
using Microsoft.Xna.Framework.Storage;

namespace Editeur_de_Map_2
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Map carte;
        Cursor curseur;
        StreamWriter sauvegarde;
        string ligne = "";
        KeyboardState keyboardState, lastKeyboardState;
        Menu menu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.Components.Add(new GamerServicesComponent(this));
            graphics.PreferredBackBufferHeight = Taille_Map.HAUTEURMAP * 28;
            graphics.PreferredBackBufferWidth = Taille_Map.LARGEURMAP * 28 + 150;
        }

        protected override void Initialize()
        {
            base.Initialize();
            menu = new Menu(Content);
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

        protected override void Update(GameTime gameTime)
        {
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                this.Exit();

            curseur.Update(gameTime, carte.largeurMap, carte.hauteurMap);

            if (Keyboard.GetState().IsKeyDown(Keys.F1))
                carte.map[(int)curseur.Position.Y, (int)curseur.Position.X] = 1;
            if (Keyboard.GetState().IsKeyDown(Keys.F2))
                carte.map[(int)curseur.Position.Y, (int)curseur.Position.X] = 2;
            if (Keyboard.GetState().IsKeyDown(Keys.F3))
                carte.map[(int)curseur.Position.Y, (int)curseur.Position.X] = 3;
            if (Keyboard.GetState().IsKeyDown(Keys.F4))
                carte.map[(int)curseur.Position.Y, (int)curseur.Position.X] = 4;

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
                Window.Title = "Fichier sauvegardé";
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            carte.Draw(spriteBatch, Content);
            curseur.Draw(spriteBatch);
            menu.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
