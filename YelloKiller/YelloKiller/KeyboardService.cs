using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace YelloKiller
{
    class KeyboardService : GameComponent, IKeyboardService
    {
        KeyboardState KBState;

        public KeyboardService(Game game)
            : base(game)
        {
            ServiceHelper.Add<IKeyboardService>(this);
        }

        public bool IsKeyDown(Keys key)
        {
            return KBState.IsKeyDown(key);
        }

        public override void Update(GameTime gameTime)
        {
            KBState = Keyboard.GetState();
            base.Update(gameTime);
        }
    }
}