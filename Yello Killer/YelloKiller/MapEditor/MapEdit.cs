using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;


namespace Yellokiller
{
    class MapEdit
    {
        public char[,] map = new char[Taille_Map.HAUTEURMAP + 2, Taille_Map.LARGEURMAP + 2];
        public int largeurMap = Taille_Map.LARGEURMAP, hauteurMap = Taille_Map.HAUTEURMAP;

        public MapEdit()
        {
            for (int y = 0; y < hauteurMap; y++)
            {
                for (int x = 0; x < largeurMap; x++)
                {
                    map[y, x] = 'h';
                }
            }
        }

        private Texture2D LoadContent(ContentManager content, string assetName)
        {
            return content.Load<Texture2D>(assetName);
        }

        public void Draw(SpriteBatch spriteBatch, ContentManager content)
        {
            for (int y = 0; y < hauteurMap; y++)
            {
                for (int x = 0; x < largeurMap; x++)
                {
                    switch (map[y, x])
                    {
                        case 'h':
                            spriteBatch.Draw(LoadContent(content, "herbe"), new Vector2(x * 28, y * 28), Color.White);
                            break;
                        case 'a':
                            spriteBatch.Draw(LoadContent(content, "arbre"), new Vector2(x * 28, y * 28), Color.White);
                            break;
                        case 'm':
                            spriteBatch.Draw(LoadContent(content, "mur"), new Vector2(x * 28, y * 28), Color.White);
                            break;
                        case 'M':
                            spriteBatch.Draw(LoadContent(content, "maison"), new Vector2(x * 28, y * 28), Color.White);
                            break;
                        case 'A':
                            spriteBatch.Draw(LoadContent(content, "arbre2"), new Vector2(x * 28, y * 28), Color.White);
                            break;
                        case 'o':
                            spriteBatch.Draw(LoadContent(content, "origine1"), new Vector2(x * 28, y * 28), Color.White);
                            break;
                        case 'O':
                            spriteBatch.Draw(LoadContent(content, "origine2"), new Vector2(x * 28, y * 28), Color.White);
                            break;
                    }
                }
            }
        }
    }
}
