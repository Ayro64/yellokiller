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
        TypeCase type;

        public Cursor(ContentManager content)
        {
            position = new Vector2(0, 0);
            LoadContent(content, "herbefoncee");
            type = TypeCase.herbeFoncee;
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public TypeCase Type
        {
            get { return type; }
        }

        private void LoadContent(ContentManager Content, string nom)
        {
            texture = Content.Load<Texture2D>(nom);
        }

        public void Update(ContentManager content, Souris souris, Menu menu)
        {
            if (souris.MState.X > 0 && souris.MState.X < Taille_Map.LARGEUR_MAP * 28 && souris.MState.Y > 0 && souris.MState.Y < Taille_Map.HAUTEUR_MAP * 28)
                position = new Vector2(souris.MState.X / 28, souris.MState.Y / 28);

            for (int i = 0; i < 6; i++)
            {
                if (souris.MState.LeftButton == ButtonState.Pressed && souris.LastMState.LeftButton == ButtonState.Released && souris.Rectangle.Intersects(menu.ListesRectangles[i]))
                {
                    switch (i)
                    {
                        case (0):
                            type = TypeCase.herbeFoncee;
                            LoadContent(content, "herbeFoncee");
                            break;
                        case (1):
                            type = TypeCase.mur;
                            LoadContent(content, "mur");
                            break;
                        case (2):
                            type = TypeCase.maison;
                            LoadContent(content, "maison");
                            break;
                        case (3):
                            type = TypeCase.arbre;
                            LoadContent(content, "arbre");
                            break;
                        case (4):
                            type = TypeCase.origineJoueur1;
                            LoadContent(content, "origine1");
                            break;
                        case (5):
                            type = TypeCase.origineJoueur2;
                            LoadContent(content, "origine2");
                            break;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(position.X * texture.Width, position.Y * texture.Height), Color.White);
        }
    }
}
