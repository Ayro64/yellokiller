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
        Vector2 origin;
        float RotationAngle;
        
        public Shuriken(GameplayScreen yk, Vector2 position_ini, int largeur)
        {
            _shuriken = yk.Content.Load<Texture2D>("shuriken");
            origin.X = _shuriken.Width / 2;
            origin.Y = _shuriken.Height / 2;
            position.X = position_ini.X + 8;
            position.Y = position_ini.Y + 4;
        }

        public void draw(SpriteBatch sb, Rectangle camera)
        {
            sb.Draw(_shuriken, new Vector2(position.X, position.Y - camera.Y), null, Color.White, RotationAngle, origin, 1.0f,
                SpriteEffects.None, 0f);
        }

        public void Update(GameTime gameTime)
        {
            position.X += 2;
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            RotationAngle += elapsed + 50;
            float circle = MathHelper.Pi * 2;
            RotationAngle = RotationAngle % circle;

        }

        public float Get_Y()
        {
            return position.Y;
        }

    }
}

