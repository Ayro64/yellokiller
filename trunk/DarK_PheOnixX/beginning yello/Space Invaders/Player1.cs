using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using Microsoft.Xna.Framework.Input;

namespace Space_Invaders
{
    class Player1 : Sprite
    {
        public Player1(Vector2 position, ContentManager content)
            : base(position)
        {
            base.LoadContent(content, "joueur");
        }

        public void Update(GameTime gameTime)
        {
            if (ServiceHelper.Get<IKeyboardService>().IsKeyDown(Keys.W))
                Position = new Vector2(Position.X, Position.Y - 0.1f * gameTime.ElapsedGameTime.Milliseconds);
            if (ServiceHelper.Get<IKeyboardService>().IsKeyDown(Keys.S))
                Position = new Vector2(Position.X, Position.Y + 0.1f * gameTime.ElapsedGameTime.Milliseconds);
            if (ServiceHelper.Get<IKeyboardService>().IsKeyDown(Keys.A))
                Position = new Vector2(Position.X - 0.1f * gameTime.ElapsedGameTime.Milliseconds, Position.Y);
            if (ServiceHelper.Get<IKeyboardService>().IsKeyDown(Keys.D))
                Position = new Vector2(Position.X + 0.1f * gameTime.ElapsedGameTime.Milliseconds, Position.Y);
        }
    }
}
