using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;


namespace Yellokiller
{
    class MapEdit
    {
        public int[,] map = new int[Taille_Map.HAUTEURMAP, Taille_Map.LARGEURMAP];
        public int largeurMap = Taille_Map.LARGEURMAP, hauteurMap = Taille_Map.HAUTEURMAP;

        public MapEdit()
        {
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
                        case 0:
                            spriteBatch.Draw(LoadContent(content, "herbe"), new Vector2(x * 28, y * 28), Color.White);
                            break;
                        case 1:
                            spriteBatch.Draw(LoadContent(content, "arbre"), new Vector2(x * 28, y * 28), Color.White);
                            break;
                        case 2:
                            spriteBatch.Draw(LoadContent(content, "mur"), new Vector2(x * 28, y * 28), Color.White);
                            break;
                        case 3:
                            spriteBatch.Draw(LoadContent(content, "maison"), new Vector2(x * 28, y * 28), Color.White);
                            break;
                        case 4:
                            spriteBatch.Draw(LoadContent(content, "arbre2"), new Vector2(x * 28, y * 28), Color.White);
                            break;
                    }
                }
            }
        }
    }
}
