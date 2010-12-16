using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Space_Invaders
{
    public class Missile
    {

        Texture2D missile1 ;
        Vector2 position;

        public Missile(SpaceInvaders sp, Vector2 position_ini, int largeur)
        { 
            missile1 = sp.Content.Load<Texture2D>("sprites/classic/laser");

            position.X = position_ini.X + largeur/2;
            position.Y = position_ini.Y;
        }

        public void draw(SpriteBatch sb)
        {
            sb.Draw(missile1,position,Color.Black);
        }

        public void Update()
        {

            position.Y--;
        
        }

        public float Get_Y()
        {
            return position.Y;
        }

    }
}
