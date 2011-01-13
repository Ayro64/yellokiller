using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Yellokiller
{
    class Textures_Choice
    {
        Texture2D arbre, maison, mur, arbre2, origine1, origine2, fond;

        public Textures_Choice()
        {
        }

        public void LoadContent(ContentManager content)
        {
            arbre = content.Load<Texture2D>("arbre");
            maison = content.Load<Texture2D>("maison");
            mur = content.Load<Texture2D>("mur");
            arbre2 = content.Load<Texture2D>("arbre2");
            origine1 = content.Load<Texture2D>("origine1");
            origine2 = content.Load<Texture2D>("origine2");
            fond = content.Load<Texture2D>("fond");

        }

        public void Draw(SpriteBatch spriteBatch, Cursor curseur)
        {
            if (curseur.arbre)
                spriteBatch.Draw(fond, new Vector2(Taille_Map.LARGEURMAP * 28 - 71, 48), Color.White);

            if (curseur.mur)
                spriteBatch.Draw(fond, new Vector2(Taille_Map.LARGEURMAP * 28 - 71, 148), Color.White);

            if (curseur.maison)
                spriteBatch.Draw(fond, new Vector2(Taille_Map.LARGEURMAP * 28 - 71, 248), Color.White);

            if (curseur.arbre2)
                spriteBatch.Draw(fond, new Vector2(Taille_Map.LARGEURMAP * 28 - 71, 348), Color.White);

            if (curseur.origine1)
                spriteBatch.Draw(fond, new Vector2(Taille_Map.LARGEURMAP * 28 - 71, 448), Color.White);

            if (curseur.origine2)
                spriteBatch.Draw(fond, new Vector2(Taille_Map.LARGEURMAP * 28 - 71, 548), Color.White);

            spriteBatch.Draw(arbre, new Vector2(Taille_Map.LARGEURMAP * 28 - 69, 50), Color.White);
            spriteBatch.Draw(mur, new Vector2(Taille_Map.LARGEURMAP * 28 - 69, 150), Color.White);
            spriteBatch.Draw(maison, new Vector2(Taille_Map.LARGEURMAP * 28 - 69, 250), Color.White);
            spriteBatch.Draw(arbre2, new Vector2(Taille_Map.LARGEURMAP * 28 - 69, 350), Color.White);
            spriteBatch.Draw(origine1, new Vector2(Taille_Map.LARGEURMAP * 28 - 69, 450), Color.White);
            spriteBatch.Draw(origine2, new Vector2(Taille_Map.LARGEURMAP * 28 - 69, 550), Color.White);
        }
    }
}
