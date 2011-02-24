using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    class Ennemi : Sprite
    {
        Vector2 position, positionDesiree;
        int maxIndex;
        Rectangle? sourceRectangle;
        Rectangle rectangle;
        Texture2D texture;

        public Ennemi(Vector2 position, Rectangle? sourceRectangle, TypeCase type)
            : base(position, sourceRectangle, type)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            rectangle = new Rectangle((int)position.X + 1, (int)position.Y + 1, 16, 26);
            maxIndex = 0;
            positionDesiree = position;
        }

        public Rectangle Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
        }

        public Vector2 PositionDesiree
        {
            get { return positionDesiree; }
        }

        public void LoadContent(ContentManager content, int maxIndex, string Assetname)
        {
            texture = content.Load<Texture2D>(Assetname);
            this.maxIndex = maxIndex;
        }
    }
}