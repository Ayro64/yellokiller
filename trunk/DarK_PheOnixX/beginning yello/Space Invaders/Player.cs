using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using Microsoft.Xna.Framework.Input;

namespace Space_Invaders
{
    class Player : Sprite
    {
        public Player()
            : base(Vector2.Zero)
        {
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("joueur");
        }

        public void Update(GameTime gameTime)
        {
            if (ServiceHelper.Get<IKeyboardService>().IsKeyDown(Keys.Up))
                Position = new position
        }
    }
}
