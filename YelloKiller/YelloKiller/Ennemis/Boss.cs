using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    class Boss : Ennemi
    {
        Vector2 position, positionDesiree;
        int maxIndex;
        Rectangle? sourceRectangle;
        public Rectangle rectangle;
        Texture2D texture;
        List<Case> chemin;
        public int vie = 5;

        public Boss(Vector2 position, Rectangle? sourceRectangle, TypeCase type)
            : base(position, sourceRectangle, type)
        {
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            rectangle = new Rectangle((int)position.X, (int)position.Y, 18, 26);
            maxIndex = 0;
            positionDesiree = position;
            chemin = new List<Case>();
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            texture = content.Load<Texture2D>("Boss");
            this.maxIndex = maxIndex;
        }

        public void UpdateInSolo(GameTime gameTime, Carte carte, Hero hero, Rectangle camera)
        {
            Position = position;
            rectangle.X = (int)position.X + 1;
            rectangle.Y = (int)position.Y + 1;
            Rectangle = rectangle;
            
        }

        public void UpdateInCoop(GameTime gameTime, Carte carte, Hero1 hero1, Hero2 hero2)
        {
            Position = position;
            rectangle.X = (int)position.X + 1;
            rectangle.Y = (int)position.Y + 1;
            Rectangle = rectangle;
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle camera)
        {
            spriteBatch.Draw(texture, new Vector2(position.X - camera.X, position.Y - camera.Y), sourceRectangle, Color.White);
        }
    }
}