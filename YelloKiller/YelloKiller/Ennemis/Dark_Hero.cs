using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace YelloKiller
{
    class Dark_Hero : Ennemi
    {
        public Dark_Hero(Vector2 position, Carte carte)
            : base(position, carte)
        {
            Position = position;
            SourceRectangle = new Rectangle(24, 64, 16, 24);
            Rectangle = new Rectangle((int)position.X + 1, (int)position.Y + 1, 16, 24);
            positionDesiree = position;
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            base.LoadContent(content, 0, @"Feuilles de sprites\Dark_Hero");
            MaxIndex = maxIndex;
        }

        public void Update(GameTime gameTime, Carte carte, Hero hero1, Hero hero2, Rectangle camera, List<EnnemiMort> ennemisMorts, Rectangle fumee)
        {
            base.Update(gameTime, new Rectangle((int)Index * 24, 0, 16, 24), new Rectangle((int)Index * 24, 64, 16, 24), new Rectangle((int)Index * 24, 97, 16, 24), new Rectangle((int)Index * 24, 33, 16, 24), hero1, hero2, ennemisMorts, fumee);
        }
    }
}