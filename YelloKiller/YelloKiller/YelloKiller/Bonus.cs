using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace YelloKiller
{
    public enum Pouvoir
    {
        shuriken = 0,
    }

    class Bonus : Sprite
    {
        Pouvoir pouvoir;

        public Bonus(Vector2 position, Pouvoir pouvoir)
            :base(position)
        {
            this.pouvoir = pouvoir;
        }
    }
}