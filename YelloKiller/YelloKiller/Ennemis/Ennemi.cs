using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace YelloKiller
{
    class Ennemi : Sprite
    {
        Rectangle rectangle;
        int maxIndex, vitesseSprite;
        float index, vitesseAnimation;
        bool monter, descendre, droite, gauche;
        bool regarde_droite, regarde_gauche, regarde_haut, regarde_bas;
        List<Case> parcours, trajet;
        int etape;
        Carte carte;

        public Vector2 positionDesiree;

        public Ennemi(Vector2 position, Carte carte)
            : base(position)
        {
            maxIndex = 0;
            index = 0;
            positionDesiree = position;
            monter = true;
            descendre = true;
            droite = true;
            gauche = true;
            regarde_droite = true;
            regarde_gauche = true;
            regarde_haut = true;
            regarde_bas = true;
            vitesseSprite = 2;
            vitesseAnimation = 0.008f;
            this.carte = carte;
            etape = 0;

            parcours = new List<Case>();
        }

        public void CreerTrajet()
        {
            Trajet = Pathfinding.CalculChemin(carte, Parcours[Etape % Parcours.Count], Parcours[(Etape + 1) % Parcours.Count]);
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

            if (parcours != null && parcours.Count > 1)
            {
                if (trajet != null && trajet.Count != 0)
                {
                    if (Monter && Descendre && Droite && Gauche)
                    {
                        if ((int)trajet[trajet.Count - 1].X < X)
                        {
                            positionDesiree.X -= 28;
                            Gauche = false;
                            trajet.RemoveAt(trajet.Count - 1);
                        }
                        else if ((int)trajet[trajet.Count - 1].X > X)
                        {
                            positionDesiree.X += 28;
                            Droite = false;
                            trajet.RemoveAt(trajet.Count - 1);
                        }
                        else if ((int)trajet[trajet.Count - 1].Y < Y)
                        {
                            positionDesiree.Y -= 28;
                            Monter = false;
                            trajet.RemoveAt(trajet.Count - 1);
                        }
                        else if ((int)trajet[trajet.Count - 1].Y > Y)
                        {
                            positionDesiree.Y += 28;
                            Descendre = false;
                            trajet.RemoveAt(trajet.Count - 1);
                        }
                    }
                }
                else
                {
                    etape++;
                    trajet = Pathfinding.CalculChemin(carte, parcours[etape % parcours.Count], parcours[(etape + 1) % parcours.Count]);
                }
            }

            if (SourceRectangle.Value.Y == sourceRectangle1.Y)
            {
                SourceRectangle = sourceRectangle1;
                regarde_haut = true;
            }
            else
                regarde_haut = false;

            if (SourceRectangle.Value.Y == sourceRectangle3.Y)
            {
                SourceRectangle = sourceRectangle3;
                    regarde_gauche = true;
            }
            else
                regarde_gauche = false;
                

            if (SourceRectangle.Value.Y == sourceRectangle2.Y)
            {
                SourceRectangle = sourceRectangle2;
                Regarder_Bas = true;
            }
            else
                Regarder_Bas = false;            

            if (SourceRectangle.Value.Y == sourceRectangle4.Y)
            {
                SourceRectangle = sourceRectangle4;
                regarde_droite = true;
            }
            else
                regarde_droite = false;

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

        public List<Case> Parcours
        {
            get { return parcours; }
            set { parcours = value; }
        }

        public List<Case> Trajet
        {
            get { return trajet; }
            set { trajet = value; }
        }

        public int Etape
        {
            get { return etape; }
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

        public bool Regarder_Haut
        {
            get { return regarde_haut; }
            set { regarde_haut = value; }
        }

        public bool Regarder_Bas
        {
            get { return regarde_bas; }
            set { regarde_bas = value; }
        }

        public bool Regarder_Droite
        {
            get { return regarde_droite; }
            set { regarde_droite = value; }
        }

        public bool Regarder_Gauche
        {
            get { return regarde_gauche; }
            set { regarde_gauche = value; }
        }
    }
}