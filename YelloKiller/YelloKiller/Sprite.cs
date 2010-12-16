using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace YelloKiller
{
   class Sprite
    {
        Vector2 position, origin = Vector2.Zero, scale = Vector2.One;
        Texture2D texture;
        Rectangle? sourceRectangle = null;
        Color color = Color.White;
        float rotation = 0, layerDepth = 0;
        SpriteEffects effect = SpriteEffects.None;

        public bool CollisionPerPixel(Sprite spriteA, Sprite spriteB)
        {
            int top = Math.Max(spriteA.Rectangle.Top, spriteB.Rectangle.Top);
            int bottom = Math.Min(spriteA.Rectangle.Bottom, spriteB.Rectangle.Bottom);
            int left = Math.Max(spriteA.Rectangle.Left, spriteB.Rectangle.Left);
            int right = Math.Min(spriteA.Rectangle.Right, spriteB.Rectangle.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color colorA = spriteA.TextureData[(x - spriteA.Rectangle.Left) +
                    (y - spriteA.Rectangle.Top) * spriteA.Rectangle.Width];

                    Color colorB = spriteB.TextureData[(x - spriteB.Rectangle.Left) +
                    (y - spriteB.Rectangle.Top) * spriteB.Rectangle.Width];

                    if (colorA.A != 0 && colorB.A != 0)
                        return true;
                }
            }
            return false;
        }

        public Sprite(Vector2 position)
        {
            this.position = position;
        }

        public Sprite(Vector2 position, Rectangle? sourceRectangle)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
        }

        public Sprite(Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            this.color = color;
        }

        public Sprite(Vector2 position, Rectangle? sourceRectangle, Color color, float rotation)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            this.color = color;
            this.rotation = rotation;
        }

        public Sprite(Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            this.color = color;
            this.rotation = rotation;
            this.origin = origin;
        }

        public Sprite(Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            this.color = color;
            this.rotation = rotation;
            this.origin = origin;
            this.scale = scale;
        }

        public Sprite(Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            this.color = color;
            this.rotation = rotation;
            this.origin = origin;
            this.scale = scale;
            this.effect = effect;
        }

        public Sprite(Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float layerDepth)
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

        public Texture2D Texture
        {
            get { return texture; }
        }

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

        public float LayerDepth
        {
            get { return layerDepth; }
            set { layerDepth = value; }
        }

        public void LoadContent(ContentManager content, string assetName)
        {
            texture = content.Load<Texture2D>(assetName);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effect, layerDepth);
        }

        public Rectangle Rectangle
        {
            get
            {
                if (sourceRectangle == null)
                    return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
                else
                    return new Rectangle((int)position.X, (int)position.Y, sourceRectangle.Value.Width, sourceRectangle.Value.Height);
            }
        }

        public Color[] TextureData
        {
            get
            {
                Color[] textureData = new Color[texture.Width * texture.Height];
                texture.GetData(textureData);
                return textureData;
            }
        }
    }
}