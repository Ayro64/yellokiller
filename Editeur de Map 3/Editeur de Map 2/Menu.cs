using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Editeur_de_Map_2
{
    class Menu
    {
        SpriteFont font;
        Texture2D arbre, maison, mur, arbre2;

        public Menu(ContentManager content)
        {
            LoadContent(content);
        }

        private void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("Text");
            arbre = content.Load<Texture2D>("arbre");
            maison = content.Load<Texture2D>("maison");
            mur = content.Load<Texture2D>("mur");
            arbre2 = content.Load<Texture2D>("arbre2");

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "Touche F1", new Vector2(Taille_Map.LARGEURMAP * 28 + 10, 000), Color.Red);
            spriteBatch.DrawString(font, "Touche F2", new Vector2(Taille_Map.LARGEURMAP * 28 + 10, 100), Color.Red);
            spriteBatch.DrawString(font, "Touche F3", new Vector2(Taille_Map.LARGEURMAP * 28 + 10, 200), Color.Red);
            spriteBatch.DrawString(font, "Touche F4", new Vector2(Taille_Map.LARGEURMAP * 28 + 10, 300), Color.Red);
            spriteBatch.Draw(arbre, new Vector2(Taille_Map.LARGEURMAP * 28 + 61, 50), Color.White);
            spriteBatch.Draw(mur, new Vector2(Taille_Map.LARGEURMAP * 28 + 61, 150), Color.White);
            spriteBatch.Draw(maison, new Vector2(Taille_Map.LARGEURMAP * 28 + 61, 250), Color.White);
            spriteBatch.Draw(arbre2, new Vector2(Taille_Map.LARGEURMAP * 28 + 61, 350), Color.White);
        }
    }
}