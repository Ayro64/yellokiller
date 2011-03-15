using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    class Ennemi : Sprite
    {
        Rectangle rectangle;
        int maxIndex, vitesseSprite;
        float index, vitesseAnimation;
        bool monter, descendre, droite, gauche;

        public Vector2 positionDesiree;

        public Ennemi(Vector2 position)
            : base(position)
        {
            maxIndex = 0;
            index = 0;
            positionDesiree = position;
            monter = true;
            descendre = true;
            droite = true;
            gauche = true;
            vitesseSprite = 2;
            vitesseAnimation = 0.008f;
        }

        public Rectangle Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
        }

        public int VitesseSprite
        {
            get { return vitesseSprite; }
            set { vitesseSprite = value; }
        }

        public float VitesseAnimation
        {
            get { return vitesseAnimation; }
            set { vitesseAnimation = value; }
        }

        public int MaxIndex
        {
            get { return maxIndex; }
            set { maxIndex = value; }
        }

        public float Index
        {
            get { return index; }
            set { index = value; }
        }

        public bool Monter
        {
            get { return monter; }
            set { monter = value; }
        }

        public bool Descendre
        {
            get { return descendre; }
            set { descendre = value; }
        }

        public bool Droite
        {
            get { return droite; }
            set { droite = value; }
        }

        public bool Gauche
        {
            get { return gauche; }
            set { gauche = value; }
        }

        public void LoadContent(ContentManager content, int maxIndex, string Assetname)
        {
            LoadContent(content, Assetname);
            this.maxIndex = maxIndex;
        }

        public void Update(GameTime gameTime, Rectangle sourceRectangle1, Rectangle sourceRectangle2, Rectangle sourceRectangle3, Rectangle sourceRectangle4)
        {
            rectangle.X = (int)position.X + 1;
            rectangle.Y = (int)position.Y + 1;

            if (SourceRectangle.Value.Y == sourceRectangle1.Y)
                SourceRectangle = sourceRectangle1;
            if (SourceRectangle.Value.Y == sourceRectangle3.Y)
                SourceRectangle = sourceRectangle3;
            if (SourceRectangle.Value.Y == sourceRectangle2.Y)
                SourceRectangle = sourceRectangle2;
            if (SourceRectangle.Value.Y == sourceRectangle4.Y)
                SourceRectangle = sourceRectangle4;

            if (!monter)
            {
                if (position != positionDesiree)
                {
                    position.Y -= vitesseSprite;
                    SourceRectangle = sourceRectangle1;
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesseAnimation;

                    if (index >= maxIndex)
                        index = 0f;
                }

                else
                {
                    monter = true;
                    position = positionDesiree;
                    index = 0f;
                }
            }

            if (!descendre)
            {
                if (position != positionDesiree)
                {
                    position.Y += vitesseSprite;
                    SourceRectangle = sourceRectangle2;
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesseAnimation;

                    if (index >= maxIndex)
                        index = 0f;
                }
                else
                {
                    descendre = true;
                    position = positionDesiree;
                    index = 0f;
                }
            }

            if (!gauche)
            {
                if (position != positionDesiree)
                {
                    position.X -= vitesseSprite;
                    SourceRectangle = sourceRectangle3;
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesseAnimation;

                    if (index >= maxIndex)
                        index = 0f;
                }

                else
                {
                    gauche = true;
                    position = positionDesiree;
                    index = 0f;
                }
            }

            if (!droite)
            {
                if (position != positionDesiree)
                {
                    position.X += vitesseSprite;
                    SourceRectangle = sourceRectangle4;
                    index += gameTime.ElapsedGameTime.Milliseconds * vitesseAnimation;

                    if (index >= maxIndex)
                        index = 0f;
                }

                else
                {
                    droite = true;
                    position = positionDesiree;
                    index = 0f;
                }
            }
        }
    }
}