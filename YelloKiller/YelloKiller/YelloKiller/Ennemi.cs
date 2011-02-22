using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using YelloKiller.YelloKiller;

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
            rectangle = new Rectangle((int)position.X, (int)position.Y, 16, 28);
            maxIndex = 0;
            positionDesiree = position;

        }

        public Rectangle Rectangle
        {
            get { return rectangle; }
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