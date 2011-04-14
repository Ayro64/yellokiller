using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    class Patrouilleur : Ennemi
    {
        public Patrouilleur(Vector2 position, Carte carte)
            : base(position, carte)
        {
            this.position = position;
            SourceRectangle = new Rectangle(24, 0, 19, 26);
            Rectangle = new Rectangle((int)position.X + 1, (int)position.Y + 1, 19, 26);
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            LoadContent(content, "Patrouilleur");
            MaxIndex = maxIndex;
        }

        public void Update(GameTime gameTime, Carte carte, Hero hero, Rectangle camera)
        {
            base.Update(gameTime, new Rectangle((int)Index * 24, 0, 19, 26), new Rectangle((int)Index * 24, 63, 19, 26), new Rectangle((int)Index * 24, 96, 19, 26), new Rectangle((int)Index * 24, 32, 19, 26));
        }
    }
}