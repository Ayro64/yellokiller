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
        Rectangle? sourceRectangle = null;
        Color color = Color.White;
        float rotation = 0;
        Vector2 origin = Vector2.Zero;
        Vector2 scale = Vector2.One;
        SpriteEffects effect = SpriteEffects.None;
        float layerDepth = 0;

        public Rectangle? SourceRectangle
        {
            get { return sourceRectangle; }
            set { sourceRectangle = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public Vector2 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public SpriteEffects Effect
        {
            get { return effect; }
            set { effect = value; }
        }

        public float LayerDepth
        {
            get { return layerDepth; }
            set { layerDepth = value; }
        }

        public Case(Vector2 position, Rectangle? sourceRectangle, TypeCase type)
        {
            this.sourceRectangle = sourceRectangle;
            this.position = position;
            this.type = type;
        }

        public Case(Vector2 position)
        {
            this.position = position;
        }

        public Case(Vector2 position, Rectangle? sourceRectangle)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
        }

        public Case(Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            this.color = color;
        }

        public Case(Vector2 position, Rectangle? sourceRectangle, Color color, float rotation)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            this.color = color;
            this.rotation = rotation;
        }

        public Case(Vector2 position, Rectangle? sourceRectangle, Color color, float
        rotation, Vector2 origin)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            this.color = color;
            this.rotation = rotation;
            this.origin = origin;
        }

        public Case(Vector2 position, Rectangle? sourceRectangle, Color color, float
        rotation, Vector2 origin, Vector2 scale)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            this.color = color;
            this.rotation = rotation;
            this.origin = origin;
            this.scale = scale;
        }

        public Case(Vector2 position, Rectangle? sourceRectangle, Color color, float
        rotation, Vector2 origin, Vector2 scale, SpriteEffects effect)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            this.color = color;
            this.rotation = rotation;
            this.origin = origin;
            this.scale = scale;
            this.effect = effect;
        }

        public Case(Vector2 position, Rectangle? sourceRectangle, Color color, float
        rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float layerDepth)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            this.color = color;
            this.rotation = rotation;
            this.origin = origin;
            this.scale = scale;
            this.effect = effect;
            this.layerDepth = layerDepth;
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