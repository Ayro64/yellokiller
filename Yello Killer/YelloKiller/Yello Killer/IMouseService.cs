using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Yellokiller.Yello_Killer
{
    interface IMouseService
    {
        bool LeftButtonHasBeenPressed();
        Vector2 GetCoordinates();
    }
}
