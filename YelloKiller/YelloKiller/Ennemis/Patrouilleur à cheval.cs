using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace YelloKiller
{
    class Patrouilleur_a_cheval : Ennemi
    {
        public Patrouilleur_a_cheval(Vector2 position, Carte carte)
            : base(position, carte)
        {
            this.position = position;
            SourceRectangle = new Rectangle(24, 0, 23, 30);
            Rectangle = new Rectangle((int)position.X + 1, (int)position.Y + 1, 22, 30);
            VitesseSprite = 2;
            VitesseAnimation = 0.016f;
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            LoadContent(content, "Patrouilleur_a_cheval");
            MaxIndex = maxIndex;
        }

        public void Update(GameTime gameTime, Carte carte, Hero hero, Rectangle camera)
        {
            base.Update(gameTime, new Rectangle((int)Index * 24, 0, 23, 30), new Rectangle((int)Index * 24, 65, 23, 30), new Rectangle((int)Index * 24, 98, 23, 30), new Rectangle((int)Index * 24, 34, 23, 30));
        }
    }
}