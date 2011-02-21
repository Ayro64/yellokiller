using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    class Case
    {
        Vector2 position;
        Texture2D texture;
        TypeCase type;

        string nomTexture;

        public Case(Vector2 position, Rectangle? sourceRectangle, TypeCase type)
        {
            this.position = position;
            this.type = type;
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public TypeCase Type
        {
            get { return type; }
            set { type = value; }
        }

        private void LoadContent(ContentManager content)
        {
            switch (type)
            {
                case TypeCase.herbe:
                    nomTexture = "herbe";
                    break;
                case TypeCase.herbeFoncee:
                    nomTexture = "herbeFoncee";
                    break;
                case TypeCase.arbre:
                    nomTexture = "arbre";
                    break;
                case TypeCase.mur:
                    nomTexture = "mur";
                    break;
                case TypeCase.maison:
                    nomTexture = "maison";
                    break;
                case TypeCase.Ennemi:
                    nomTexture = "origineEnnemi1";
                    break;
                case TypeCase.Joueur1:
                    nomTexture = "origine1";
                    break;
                case TypeCase.Joueur2:
                    nomTexture = "origine2";
                    break;
                case TypeCase.arbre2:
                    nomTexture = "arbre2";
                    break;   
            }
            texture = content.Load<Texture2D>(nomTexture);
        }

        public void DrawInGame(SpriteBatch spriteBatch, ContentManager content)
        {
            LoadContent(content);
            spriteBatch.Draw(texture, position, Color.White);
        }

        public void DrawInMapEditor(SpriteBatch spriteBatch, ContentManager content)
        {
            LoadContent(content);
            spriteBatch.Draw(texture, 28 * position, Color.White);
        }
    }
}