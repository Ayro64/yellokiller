using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    class Patrouilleur_a_cheval : Ennemi
    {
        float autoChemin;

        public Patrouilleur_a_cheval(Vector2 position)
            : base(position)
        {
            this.position = position;
            SourceRectangle = new Rectangle(24, 0, 23, 30);
            Rectangle = new Rectangle((int)position.X + 1, (int)position.Y + 1, 22, 30);
            VitesseSprite = 2;
            VitesseAnimation = 0.016f;
            autoChemin = 0;
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            LoadContent(content, "Patrouilleur_a_cheval");
            MaxIndex = maxIndex;
        }

        public void Update(GameTime gameTime, Carte carte, Hero hero, Rectangle camera)
        {
            base.Update(gameTime, new Rectangle((int)Index * 24, 0, 23, 30), new Rectangle((int)Index * 24, 65, 23, 30), new Rectangle((int)Index * 24, 98, 23, 30), new Rectangle((int)Index * 24, 34, 23, 30));

            autoChemin += gameTime.ElapsedGameTime.Milliseconds * 0.001f;

            if (Monter && Descendre && Droite && Gauche)
            {
                if (autoChemin < 5 && position.X < 28 * Taille_Map.LARGEUR_MAP - 1 &&
                    (int)carte.Cases[(int)(position.Y) / 28, (int)(position.X + 28) / 28].Type > 0)
                {
                    positionDesiree.X = position.X + 28;
                    positionDesiree.Y = position.Y;
                    Droite = false;
                }
                else if (autoChemin < 10 && position.Y < 28 * (Taille_Map.HAUTEUR_MAP - 1) &&
                         (int)carte.Cases[(int)(position.Y + 28) / 28, (int)(position.X) / 28].Type > 0)
                {
                    positionDesiree.Y = position.Y + 28;
                    positionDesiree.X = position.X;
                    Descendre = false;
                }
                else if (autoChemin < 15 && position.X > 0 &&
                         (int)carte.Cases[(int)(position.Y) / 28, (int)(position.X - 28) / 28].Type > 0)
                {
                    positionDesiree.X = position.X - 28;
                    positionDesiree.Y = position.Y;
                    Gauche = false;
                }
                else if (autoChemin < 20 && position.Y > 0 &&
                         (int)carte.Cases[(int)(position.Y - 28) / 28, (int)(position.X) / 28].Type > 0)
                {
                    positionDesiree.Y = position.Y - 28;
                    positionDesiree.X = position.X;
                    Monter = false;
                }
                else
                    autoChemin = 0;
            }
        }
    }
}