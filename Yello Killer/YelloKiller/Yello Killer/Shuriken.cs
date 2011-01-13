using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Yellokiller.Yello_Killer

{
    public class Shuriken
    {

        Texture2D _shuriken;
        Vector2 position;

        public Shuriken(GameplayScreen yk, Vector2 position_ini, int largeur)
        {
            _shuriken = yk.Content.Load<Texture2D>("shuriken");

            position.X = position_ini.X + largeur / 2;
            position.Y = position_ini.Y;
        }

        public void draw(SpriteBatch sb)
        {
            sb.Draw(_shuriken, position, Color.White);
        }

        public void Update()
        {
            position.X++;
        }

        public float Get_Y()
        {
            return position.Y;
        }

    }
}

