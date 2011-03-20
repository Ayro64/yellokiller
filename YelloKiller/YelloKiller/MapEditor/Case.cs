using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
   public class Case
    {
        Vector2 position;
        Texture2D texture;
        TypeCase type;
        string nomTexture;
        int x, y;

        public Case(Vector2 position, TypeCase type)
        {
            this.position = position;
            this.type = type;
            x = (int)position.X / 28;
            y = (int)position.Y / 28;
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public int X
        {
            get { return x; }
        }

        public int Y
        {
            get { return y; }
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
                case TypeCase.commode:
                    nomTexture = @"Textures\Commode";
                    break;
                case TypeCase.TableMoyenne:
                    nomTexture = @"Textures\tableMoyenne";
                    break;
                case TypeCase.GrandeTable:
                    nomTexture = @"Textures\grandeTable";
                    break;
                case TypeCase.Lit:
                    nomTexture = @"Textures\lit";
                    break;
                case TypeCase.fond:
                    nomTexture = @"Textures\fondNoir";
                    break;

                case TypeCase.Joueur1:
                    nomTexture = "origine_hero1";
                    break;
                case TypeCase.Joueur2:
                    nomTexture = "origine_hero2";
                    break;
                case TypeCase.Garde:
                    nomTexture = "origine_garde";
                    break;
                case TypeCase.Patrouilleur:
                    nomTexture = "origine_patrouilleur";
                    break;
                case TypeCase.Patrouilleur_a_cheval:
                    nomTexture = "origine_patrouilleur_a_cheval";
                    break;
                case TypeCase.Boss:
                    nomTexture = "origine_patrouilleur_a_cheval";
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

        public void DrawInMenu(SpriteBatch spriteBatch, ContentManager content, Vector2 origine)
        {
            LoadContent(content);
            spriteBatch.Draw(texture, 0.07f * 28 *  new Vector2(position.X, position.Y) + new Vector2(origine.X, origine.Y), null, Color.White, 0, Vector2.Zero, 0.07f, SpriteEffects.None, 0);
        }
    }
}