using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using Microsoft.Xna.Framework.Input;

namespace Space_Invaders
{
    class Player : Sprite
    {
        public Player(Vector2 position)
            : base(position)
        {
        }

        public void Update(GameTime gameTime)
        {
            if (ServiceHelper.Get<IKeyboardService>().IsKeyDown(Keys.Up))
                Position = new Vector2(Position.X, Position.Y - gameTime.ElapsedGameTime.Milliseconds);
            if (ServiceHelper.Get<IKeyboardService>().IsKeyDown(Keys.Down))
                Position = new Vector2(Position.X, Position.Y + gameTime.ElapsedGameTime.Milliseconds);
        }
    }
}
