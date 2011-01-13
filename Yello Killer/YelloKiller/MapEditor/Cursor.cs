using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Yellokiller
{
    class Cursor
    {
        Texture2D texture;
        Vector2 position;

        char dessin = 'a';
        bool enable = true;
        public bool arbre = false, mur = false, maison = false, arbre2 = false, origine1 = false, origine2 = false;

        public Cursor(ContentManager content)
        {
            position = new Vector2(0, 0);
            LoadContent(content, "arbre");
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public char Dessin
        {
            get { return dessin; }
            set { dessin = value; }
        }

        public bool Enable
        {
            get { return enable; }
            set { enable = value; }
        }

        private void LoadContent(ContentManager Content, string nom)
        {
            texture = Content.Load<Texture2D>(nom);
        }

        public void Update(ContentManager content, int largeurMap, int hauteurMap, MouseState MState, MouseState lastMState)
        {
            arbre = false;
            mur = false;
            maison = false;
            arbre2 = false;
            origine1 = false;
            origine2 = false;

            enable = true;

            if (MState.X > 0 && MState.X < Taille_Map.LARGEURMAP * 28 && MState.Y > 0 && MState.Y < Taille_Map.HAUTEURMAP * 28)
                position = new Vector2(MState.X / 28, MState.Y / 28);

            if (MState.X > 827 && MState.X < 855 && MState.Y > 50 && MState.Y < 78)
            {
                enable = false;
                arbre = true;
                if (MState.LeftButton == ButtonState.Pressed && lastMState.LeftButton == ButtonState.Released)
                {
                    dessin = 'a';
                    LoadContent(content, "arbre");
                }
            }

            if (MState.X > 827 && MState.X < 855 && MState.Y > 150 && MState.Y < 178)
            {
                enable = false;
                mur = true;
                if (MState.LeftButton == ButtonState.Pressed && lastMState.LeftButton == ButtonState.Released)
                {
                    dessin = 'm';
                    LoadContent(content, "mur");
                }
            }

            if (MState.X > 827 && MState.X < 855 && MState.Y > 250 && MState.Y < 278)
            {
                enable = false;
                maison = true;
                if (MState.LeftButton == ButtonState.Pressed && lastMState.LeftButton == ButtonState.Released)
                {
                    dessin = 'M';
                    LoadContent(content, "maison");
                }
            }

            if (MState.X > 827 && MState.X < 855 && MState.Y > 350 && MState.Y < 378)
            {
                enable = false;
                arbre2 = true;
                if (MState.LeftButton == ButtonState.Pressed && lastMState.LeftButton == ButtonState.Released)
                {
                    dessin = 'A';
                    LoadContent(content, "arbre2");
                }
            }

            if (MState.X > 827 && MState.X < 855 && MState.Y > 450 && MState.Y < 478)
            {
                enable = false;
                origine1 = true;
                if (MState.LeftButton == ButtonState.Pressed && lastMState.LeftButton == ButtonState.Released)
                {
                    dessin = 'o';
                    LoadContent(content, "origine1");
                }
            }

            if (MState.X > 827 && MState.X < 855 && MState.Y > 550 && MState.Y < 578)
            {
                enable = false;
                origine2 = true;
                if (MState.LeftButton == ButtonState.Pressed && lastMState.LeftButton == ButtonState.Released)
                {
                    dessin = 'O';
                    LoadContent(content, "origine2");
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(position.X * texture.Width, position.Y * texture.Height), Color.White);
        }
    }
}
