using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    public class Sprite
    {
        public Vector2 position;
        Texture2D texture;
        Rectangle? sourceRectangle = null;
        Color color = Color.White;
        float rotation = 0;
        Vector2 origin = Vector2.Zero;
        Vector2 scale = Vector2.One;
        SpriteEffects effect = SpriteEffects.None;
        float layerDepth = 0;

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

        public void LoadContent(ContentManager content, string nom)
        {
            texture = content.Load<Texture2D>(nom);
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle camera)
        {
            spriteBatch.Draw(texture, new Vector2(position.X - camera.X, position.Y - camera.Y), sourceRectangle, color, rotation, origin, scale, effect, layerDepth);
        }
    }
}