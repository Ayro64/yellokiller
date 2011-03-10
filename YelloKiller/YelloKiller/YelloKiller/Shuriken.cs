using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace YelloKiller
{
    class Shuriken : Sprite
    {
        Vector2 direction;
        Rectangle rectangle;
        bool shurikenExists;
        float elapsed, circle;

        public Shuriken(Vector2 position, Hero hero, ContentManager content)
            : base(position)
        {
            base.LoadContent(content, "shuriken");
            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            this.position.X = position.X + 8;
            this.position.Y = position.Y + 4;
            elapsed = 0;
            circle = MathHelper.Pi * 2;
            shurikenExists = false;

            rectangle = new Rectangle((int)position.X, (int)position.Y, 12, 12);

            if (hero.ishero == true)
            {
                if (hero.SourceRectangle.Value.Y == 230)
                    direction = -Vector2.UnitX;
                else if (hero.SourceRectangle.Value.Y == 198)
                    direction = Vector2.UnitY;
                else if (hero.SourceRectangle.Value.Y == 166)
                    direction = Vector2.UnitX;
                else if (hero.SourceRectangle.Value.Y == 133)
                    direction = -Vector2.UnitY;
            }
        }

        public void Update(GameTime gameTime, Carte carte)
        {
            rectangle.X = (int)position.X + 1;
            rectangle.Y = (int)position.Y + 1;

            if (position.X > 0 && position.X < 28 * Taille_Map.LARGEUR_MAP && position.Y > 0 && position.Y < 28 * Taille_Map.HAUTEUR_MAP &&
                carte.Cases[(int)position.Y / 28, (int)position.X / 28].Type > 0)
            {
                shurikenExists = true;
                position += 7 * direction;

                elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Rotation += elapsed + 50;
                Rotation = Rotation % circle;
            }
            else
                shurikenExists = false;
        }


        public Rectangle Rectangle
        {
            get { return rectangle; }
        }

        public Vector2 Direction
        {
            get { return direction; }
        }

        public bool ShurikenExists
        {
            get { return shurikenExists; }
        }

        public void LoadContent(ContentManager content)
        {
            base.LoadContent(content, "shuriken");
        }
    }
}