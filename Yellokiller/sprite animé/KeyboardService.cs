using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace sprite_animé
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