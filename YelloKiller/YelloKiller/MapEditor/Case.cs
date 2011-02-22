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
                case TypeCase.arbre:
                    nomTexture = @"Textures\arbre";
                    break;
                case TypeCase.arbre2:
                    nomTexture = @"Textures\arbre2";
                    break;
                case TypeCase.buissonSurHerbe:
                    nomTexture = @"Textures\buissonSurHerbe";
                    break;
                case TypeCase.murBlanc:
                    nomTexture = @"Textures\murBlanc";
                    break;
                case TypeCase.tableauMurBlanc:
                    nomTexture = @"Textures\tableauMurBlanc";
                    break;
                case TypeCase.bois:
                    nomTexture = @"Textures\bois";
                    break;
                case TypeCase.boisCarre:
                    nomTexture = @"Textures\boisCarre";
                    break;
                case TypeCase.tapisRougeBC:
                    nomTexture = @"Textures\tapisRougeBC";
                    break;
                case TypeCase.herbe:
                    nomTexture = @"Textures\herbe";
                    break;
                case TypeCase.herbeFoncee:
                    nomTexture = @"Textures\herbeFoncee";
                    break;
                case TypeCase.piedDeMurBois:
                    nomTexture = @"Textures\piedDeMurBois";
                    break;
                case TypeCase.terre:
                    nomTexture = @"Textures\terre";
                    break;
                case TypeCase.carlageNoir:
                    nomTexture = @"Textures\carlageNoir";
                    break;
                case TypeCase.fondNoir:
                    nomTexture = @"Textures\fondNoir";
                    break;
                case TypeCase.finMurFN:
                    nomTexture = @"Textures\FinMurFN";
                    break;
                case TypeCase.finMurGauche:
                    nomTexture = @"Textures\FinMurGauche";
                    break;
                case TypeCase.finMurDroite:
                    nomTexture = @"Textures\FinMurDroite";
                    break;
                case TypeCase.Joueur1:
                    nomTexture = "origine1";
                    break;
                case TypeCase.Joueur2:
                    nomTexture = "origine2";
                    break;
                case TypeCase.Ennemi:
                    nomTexture = "origineEnnemi1";
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