using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace YelloKiller
{
    class Shuriken
    {
        Texture2D _shuriken;
        Vector2 position, origin, direction;
        Rectangle rectangle;
        float RotationAngle;
        public bool existshuriken = false;

        public Shuriken(GameplayScreenCoop yk, Vector2 position_ini, int largeur, Hero1 hero1, Hero2 hero2)
        {
            _shuriken = yk.Content.Load<Texture2D>("shuriken");
            origin.X = _shuriken.Width / 2;
            origin.Y = _shuriken.Height / 2;

            position.X = position_ini.X + 8;
            position.Y = position_ini.Y + 4;

            rectangle = new Rectangle((int)position.X, (int)position.Y, 12, 12);

            if (hero1.ishero1 == true)
            {
                if (hero1.sourceRectangle.Value.Y == 230)
                    direction = -Vector2.UnitX;
                else if (hero1.sourceRectangle.Value.Y == 198)
                    direction = Vector2.UnitY;
                else if (hero1.sourceRectangle.Value.Y == 166)
                    direction = Vector2.UnitX;
                else if (hero1.sourceRectangle.Value.Y == 133)
                    direction = -Vector2.UnitY;
            }
            if (hero2.ishero2 == true)
            {
                if (hero2.sourceRectangle.Value.Y == 230)
                    direction = -Vector2.UnitX;
                else if (hero2.sourceRectangle.Value.Y == 198)
                    direction = Vector2.UnitY;
                else if (hero2.sourceRectangle.Value.Y == 166)
                    direction = Vector2.UnitX;
                else if (hero2.sourceRectangle.Value.Y == 133)
                    direction = -Vector2.UnitY;
            }           
        }

        public Shuriken(GameplayScreenSolo yk, Vector2 position_ini, int largeur, Hero hero)
        {
            _shuriken = yk.Content.Load<Texture2D>("shuriken");
            origin.X = _shuriken.Width / 2;
            origin.Y = _shuriken.Height / 2;

            position.X = position_ini.X + 8;
            position.Y = position_ini.Y + 4;

            rectangle = new Rectangle((int)position.X, (int)position.Y, 12, 12);

            if (hero.ishero == true)
            {
                Console.WriteLine("sourcerectangle hero = " + hero.sourceRectangle.Value.Y);
                if (hero.sourceRectangle.Value.Y == 230)
                    direction = -Vector2.UnitX;
                else if (hero.sourceRectangle.Value.Y == 198)
                    direction = Vector2.UnitY;
                else if (hero.sourceRectangle.Value.Y == 166)
                    direction = Vector2.UnitX;
                else if (hero.sourceRectangle.Value.Y == 133)
                    direction = -Vector2.UnitY;
            }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public Rectangle Rectangle
        {
            get { return rectangle; }
        }

        public void Update(GameTime gameTime, Carte carte)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, 12, 12);

            if (position.X > 0 && position.X < 28 * (Taille_Map.LARGEUR_MAP - 1) && position.Y > 0 && position.Y < 28 * (Taille_Map.HAUTEUR_MAP - 1) &&
                carte.Cases[(int)position.Y / 28, (int)position.X / 28].Type > 0)
            {
                existshuriken = true;
                position += 2 * direction;

                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                RotationAngle += elapsed + 50;
                float circle = MathHelper.Pi * 2;
                RotationAngle = RotationAngle % circle;
            }
            else
                existshuriken = false;
        }

        public void Draw(SpriteBatch sb, Rectangle camera)
        {
            sb.Draw(_shuriken, new Vector2(position.X - camera.X, position.Y - camera.Y), null, Color.White, RotationAngle, origin, 1.0f,
                SpriteEffects.None, 0f);
        }
    }
}