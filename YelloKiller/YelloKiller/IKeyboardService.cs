using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace YelloKiller
{
    interface IKeyboardService
    {
        bool IsKeyDown(Keys key);
    }
}
