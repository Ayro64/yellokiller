using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace YelloKiller.YelloKiller
{
    class Sprite : Case
    {
        Vector2 position;
        Texture2D texture;
        Rectangle? sourceRectangle = null;
        Color color = Color.White;
        float rotation = 0;
        Vector2 origin = Vector2.Zero;
        Vector2 scale = Vector2.One;
        SpriteEffects effect = SpriteEffects.None;
        float layerDepth = 0;
        TypeCase type;

        public Vector2 PositionDesire
        {
            get { return position; }
            set { position = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
        }

        public Sprite(Vector2 position)
            : base(position)
        {
            this.position = position;
        }

        public Sprite(Vector2 position, Rectangle? sourceRectangle)
            : base(position, sourceRectangle)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
        }

        public Sprite(Vector2 position, Rectangle? sourceRectangle, TypeCase type)
            : base(position, sourceRectangle, type)
        {
            this.sourceRectangle = sourceRectangle;
            this.position = position;
            this.type = type;
        }

        public Sprite(Vector2 position, Rectangle? sourceRectangle, Color color)
            : base(position, sourceRectangle, color)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            this.color = color;
        }

        public Sprite(Vector2 position, Rectangle? sourceRectangle, Color color, float rotation)
            : base(position, sourceRectangle, color, rotation)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            this.color = color;
            this.rotation = rotation;
        }

        public Sprite(Vector2 position, Rectangle? sourceRectangle, Color color, float
        rotation, Vector2 origin)
            : base(position, sourceRectangle, color, rotation, origin)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            this.color = color;
            this.rotation = rotation;
            this.origin = origin;
        }

        public Sprite(Vector2 position, Rectangle? sourceRectangle, Color color, float
        rotation, Vector2 origin, Vector2 scale)
            : base(position, sourceRectangle, color, rotation, origin, scale)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            this.color = color;
            this.rotation = rotation;
            this.origin = origin;
            this.scale = scale;
        }

        public Sprite(Vector2 position, Rectangle? sourceRectangle, Color color, float
        rotation, Vector2 origin, Vector2 scale, SpriteEffects effect)
            : base(position, sourceRectangle, color, rotation, origin, scale, effect)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            this.color = color;
            this.rotation = rotation;
            this.origin = origin;
            this.scale = scale;
            this.effect = effect;
        }

        public Sprite(Vector2 position, Rectangle? sourceRectangle, Color color, float
        rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float layerDepth)
            : base(position, sourceRectangle, color, rotation, origin, scale, effect, layerDepth)
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

        public void LoadContent(ContentManager content, string assetName)
        {
            texture = content.Load<Texture2D>(assetName);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, rotation,
            origin, scale, effect, layerDepth);
        }
    }
}
