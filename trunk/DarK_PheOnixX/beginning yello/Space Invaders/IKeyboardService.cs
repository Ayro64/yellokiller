using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Space_Invaders
{
    interface IKeyboardService
    {
        bool IsKeyDown(Keys key);
    }
}
