using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using Microsoft.Xna.Framework.Input;

namespace Space_Invaders
{
    class Player1 : Sprite
    {
        public Player1(Vector2 position)
            : base(position)
        {
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("joueur");
        }

        public void Update(GameTime gameTime)
        {
            if (ServiceHelper.Get<IKeyboardService>().IsKeyDown(Keys.Up))
                Position = new Vector2(Position.X, Position.Y - 0.8f * gameTime.ElapsedGameTime.Milliseconds);
            if (ServiceHelper.Get<IKeyboardService>().IsKeyDown(Keys.Down))
                Position = new Vector2(Position.X, Position.Y + 0.8f * gameTime.ElapsedGameTime.Milliseconds);
            if (ServiceHelper.Get<IKeyboardService>().IsKeyDown(Keys.Left))
                Position = new Vector2(Position.X - 0.8f * gameTime.ElapsedGameTime.Milliseconds, Position.Y);
            if (ServiceHelper.Get<IKeyboardService>().IsKeyDown(Keys.Right))
                Position = new Vector2(Position.X + 0.8f * gameTime.ElapsedGameTime.Milliseconds, Position.Y);
        }
    }
}
