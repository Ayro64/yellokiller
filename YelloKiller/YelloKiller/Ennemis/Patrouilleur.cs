using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    class Patrouilleur : Ennemi
    {
        float autochemin;

        public Patrouilleur(Vector2 position)
            : base(position)
        {
            this.position = position;
            SourceRectangle = new Rectangle(24, 0, 19, 26); 
            Rectangle = new Rectangle((int)position.X + 1, (int)position.Y + 1, 19, 26);
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            LoadContent(content, "Patrouilleur");
            MaxIndex = maxIndex;
        }

        public void Update(GameTime gameTime, Carte carte, Hero hero, Rectangle camera)
        {
            base.Update(gameTime, new Rectangle((int)Index * 24, 0, 19, 26), new Rectangle((int)Index * 24, 63, 19, 26), new Rectangle((int)Index * 24, 96, 19, 26), new Rectangle((int)Index * 24, 32, 19, 26));

            autochemin += gameTime.ElapsedGameTime.Milliseconds * 0.001f;

            if (Monter && Descendre && Droite && Gauche)
            {
                if (autochemin < 5 && position.X < 28 * Taille_Map.LARGEUR_MAP - 18 && (int)carte.Cases[Y, X + 1].Type > 0)
                {
                    positionDesiree.X = position.X + 28;
                    positionDesiree.Y = position.Y;
                    Droite = false;
                }
                else if (autochemin < 10 && position.Y < 28 * (Taille_Map.HAUTEUR_MAP - 1) && (int)carte.Cases[Y + 1, X].Type > 0)
                {
                    positionDesiree.Y = position.Y + 28;
                    positionDesiree.X = position.X;
                    Descendre = false;
                }
                else if (autochemin < 15 && position.X > 0 && (int)carte.Cases[Y, Y - 1].Type > 0)
                {
                    positionDesiree.X = position.X - 28;
                    positionDesiree.Y = position.Y;
                    Gauche = false;
                }
                else if (autochemin < 20 && position.Y > 0 && (int)carte.Cases[Y - 1, X].Type > 0)
                {
                    positionDesiree.Y = position.Y - 28;
                    positionDesiree.X = position.X;
                    Monter = false;
                }
                else
                    autochemin = 0;
            }
        }
    }
}