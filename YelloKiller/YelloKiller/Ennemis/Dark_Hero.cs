using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace YelloKiller
{
    class Dark_Hero : Sprite
    {
        Rectangle rectangle;
        List<Case> chemin;
        Carte carte;
        Case depart, arrivee;
        public Vector2 positionDesiree;

        public Dark_Hero(Vector2 position, Carte carte)
            : base(position)
        {
            VitesseSprite = 1;
            VitesseAnimation = 0.008f;
            Position = position;
            SourceRectangle = new Rectangle(24, 64, 16, 24);
            Rectangle = new Rectangle((int)position.X + 1, (int)position.Y + 1, 16, 24);
            positionDesiree = position;
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            base.LoadContent(content, @"Feuilles de sprites\Dark_Hero");
            MaxIndex = maxIndex;
        }

        public void Update(GameTime gameTime, Carte carte, Hero hero1, Hero hero2, Rectangle camera, List<EnnemiMort> ennemisMorts, Rectangle fumee)
        {
            if (SourceRectangle.Value.Y == 0)
            {
                SourceRectangle = new Rectangle((int)Index * 24, 0, 16, 24);
                Regarde_Haut = true;
            }
            else
                Regarde_Haut = false;

            if (SourceRectangle.Value.Y == 97)
            {
                SourceRectangle = new Rectangle((int)Index * 24, 97, 16, 24);
                Regarde_Gauche = true;
            }
            else
                Regarde_Gauche = false;

            if (SourceRectangle.Value.Y == 64)
            {
                SourceRectangle = new Rectangle((int)Index * 24, 64, 16, 24);
                Regarde_Bas = true;
            }
            else
                Regarde_Bas = false;

            if (SourceRectangle.Value.Y == 33)
            {
                SourceRectangle = new Rectangle((int)Index * 24, 33, 16, 24);
                Regarde_Droite = true;
            }
            else
                Regarde_Droite = false;

            if (!VaEnHaut)
            {
                if (position != positionDesiree)
                {
                    position.Y -= VitesseSprite;
                    SourceRectangle = new Rectangle((int)Index * 24, 0, 16, 24);
                    Index += gameTime.ElapsedGameTime.Milliseconds * VitesseAnimation;

                    if (Index >= MaxIndex)
                        Index = 0f;
                }
                else
                {
                    VaEnHaut = true;
                    position = positionDesiree;
                    Index = 0f;
                }
            }

            if (!VaEnBas)
            {
                if (position != positionDesiree)
                {
                    position.Y += VitesseSprite;
                    SourceRectangle = new Rectangle((int)Index * 24, 64, 16, 24);
                    Index += gameTime.ElapsedGameTime.Milliseconds * VitesseAnimation;

                    if (Index >= MaxIndex)
                        Index = 0f;
                }
                else
                {
                    VaEnBas = true;
                    position = positionDesiree;
                    Index = 0f;
                }
            }

            if (!VaAGauche)
            {
                if (position != positionDesiree)
                {
                    position.X -= VitesseSprite;
                    SourceRectangle = new Rectangle((int)Index * 24, 97, 16, 24);
                    Index += gameTime.ElapsedGameTime.Milliseconds * VitesseAnimation;

                    if (Index >= MaxIndex)
                        Index = 0f;
                }
                else
                {
                    VaAGauche = true;
                    position = positionDesiree;
                    Index = 0f;
                }
            }

            if (!VaADroite)
            {
                if (position != positionDesiree)
                {
                    position.X += VitesseSprite;
                    SourceRectangle = new Rectangle((int)Index * 24, 33, 16, 24);
                    Index += gameTime.ElapsedGameTime.Milliseconds * VitesseAnimation;

                    if (Index >= MaxIndex)
                        Index = 0f;
                }
                else
                {
                    VaADroite = true;
                    position = positionDesiree;
                    Index = 0f;
                }
            }
        }


        public void SauvegarderCheckPoint(ref StreamWriter file)
        {
            file.WriteLine(X.ToString() + "," + Y.ToString());
        }

        public List<Case> Chemin
        {
            get { return chemin; }
            set { chemin = value; }
        }

        public Rectangle Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
        }

        public int VitesseSprite { get; set; }

        public float VitesseAnimation { get; set; }

        public int MaxIndex { get; set; }

        public float Index { get; private set; }

        public bool VaEnHaut { get; set; }

        public bool VaEnBas { get; set; }

        public bool VaADroite { get; set; }

        public bool VaAGauche { get; set; }

        public bool Regarde_Haut { get; private set; }

        public bool Regarde_Bas { get; private set; }

        public bool Regarde_Droite { get; private set; }

        public bool Regarde_Gauche { get; private set; } 
    }
}