using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Space_Invaders
{
    public class Hero : Microsoft.Xna.Framework.Game
    {

        Texture2D hero1;
        Vector2 position;
        List<Missile> missile1;

        public Hero(SpaceInvaders sp)
        {

            int x = 400;
            int y = 550;
            missile1 = new List<Missile>();
            position = new Vector2(x, y);

            hero1 = sp.Content.Load<Texture2D>("sprites/classic/player");

        }

        public void Draw(SpriteBatch sb)
        {

            sb.Draw(hero1, position, Color.Black);

            for (int i = 0; i < missile1.Count; i++)
            {
                Missile m = missile1[i];
                m.Update();
                m.draw(sb);

                if (m.Get_Y() < -14)
                {
                    missile1.Remove(m);
                }
            }


        }

        public void Update(int largeur, int longeur, SpaceInvaders sp)
        {


            if ((Keyboard.GetState().IsKeyDown(Keys.Up)) && (position.Y > ((largeur / 2) + 10)))
            {
                position.Y -= 10;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && (position.Y < (largeur - 10 - hero1.Height)))
            {
                position.Y += 10;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && (position.X < longeur - 10 - hero1.Width))
            {
                position.X += 10;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && (position.X > 10))
            {
                position.X -= 10;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {

                missile1.Add(new Missile(sp, position, this.hero1.Width));

            }


        }
    }
}
