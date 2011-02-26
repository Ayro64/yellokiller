using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    class Boss : Ennemi
    {
        int vie;

        public Boss(Vector2 position)
            : base(position)
        {
            this.position = position;
            SourceRectangle = new Rectangle(26, 64, 18, 26);
            Rectangle = new Rectangle((int)position.X + 1, (int)position.Y + 1, 18, 26);
            MaxIndex = 0;
            positionDesiree = position;
            vie = 5;
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            base.LoadContent(content, "Boss");
            MaxIndex = maxIndex;
        }

        public int Vie
        {
            get { return vie; }
            set { vie = value; }
        }

        /*
        public void Update(GameTime gameTime)
        {
            Update(gameTime);
        }*/
    }
}