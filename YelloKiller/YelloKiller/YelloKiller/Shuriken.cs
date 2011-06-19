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
        public bool ShurikenExists { get; private set; }
        float elapsed, circle;
        float tmpshuriken = 0;

        public Shuriken(Vector2 position, Heros heros, ContentManager content)
            : base(position)
        {
            base.LoadContent(content, "shuriken");
            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            this.position.X = position.X + 8;
            this.position.Y = position.Y + 4;
            elapsed = 0;
            circle = MathHelper.Pi * 2;
            ShurikenExists = false;

            rectangle = new Rectangle((int)position.X, (int)position.Y, 12, 12);

            if (heros.isheros == true)
            { 
                if (heros.Regarde_Gauche)
                    direction = -Vector2.UnitX;
                else if (heros.Regarde_Bas)
                    direction = Vector2.UnitY;
                else if (heros.Regarde_Droite)
                    direction = Vector2.UnitX;
                else if (heros.Regarde_Haut)
                    direction = -Vector2.UnitY;
            }
        }

        public void Update(GameTime gameTime, Carte carte)
        {
            rectangle.X = (int)position.X + 1;
            rectangle.Y = (int)position.Y + 1;

            tmpshuriken += gameTime.ElapsedGameTime.Milliseconds * 0.001f;

            if (position.X > 0 && position.X < 28 * Taille_Map.LARGEUR_MAP && position.Y > 0 && position.Y < 28 * Taille_Map.HAUTEUR_MAP &&
                carte.Cases[Y, X].EstFranchissable)
            {
                ShurikenExists = true;
                position += 7 * direction;

                elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Rotation += elapsed + 50;
                Rotation = Rotation % circle;
            }
            else
                ShurikenExists = false;

            if (tmpshuriken > 1)
            {
                ShurikenExists = false;
                tmpshuriken = 0;
            }
        }

        public Rectangle Rectangle
        {
            get { return rectangle; }
        }

        public Vector2 Direction
        {
            get { return direction; }
        }

        public void LoadContent(ContentManager content)
        {
            base.LoadContent(content, "shuriken");
        }
    }
}