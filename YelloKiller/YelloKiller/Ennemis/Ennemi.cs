using System;
using System.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    class Ennemi : Sprite
    {
        Rectangle rectangle, champDeVision1, champDeVision2, champDeVision3;
        List<Case> chemin;
        Carte carte;
        Case depart, arrivee;
        public bool Alerte { get; set; }
        public bool RetourneCheminNormal { get; set; }

        public Vector2 positionDesiree;

        public Ennemi(Vector2 position, Carte carte)
            : base(position)
        {
            MaxIndex = 0;
            Index = 0;
            positionDesiree = position;
            VaEnHaut = true;
            VaEnBas = true;
            VaADroite = true;
            VaAGauche = true;
            Regarde_Droite = true;
            Regarde_Gauche = true;
            Regarde_Haut = true;
            Regarde_Bas = true;
            VitesseSprite = 2;
            VitesseAnimation = 0.008f;
            this.carte = carte;
            champDeVision1 = new Rectangle(28 * X - 28, 28 * Y + 28, 0, 0);
            champDeVision2 = new Rectangle(28 * X, 28 * Y + 28, 0, 0);
            champDeVision3 = new Rectangle(28 * X + 28, 28 * Y + 28, 0, 0);
            chemin = new List<Case>();
            RetourneCheminNormal = false;
        }

        public void LoadContent(ContentManager content, int maxIndex, string Assetname)
        {
            LoadContent(content, Assetname); 
            this.MaxIndex = maxIndex;
        }

        public void Update(GameTime gameTime, Rectangle sourceRectangle1, Rectangle sourceRectangle2, Rectangle sourceRectangle3, Rectangle sourceRectangle4, Hero hero1, Hero hero2, List<EnnemiMort> ennemisMorts, Rectangle fumee)
        {
            rectangle.X = (int)position.X + 1;
            rectangle.Y = (int)position.Y + 1;
            UpdateChampDeVision(carte);
            MortDansLeChampDeVision(ennemisMorts, fumee);

            if (!RetourneCheminNormal && Collision(hero1.Rectangle, fumee))
            {
                depart = carte.Cases[Y, X];
                arrivee = carte.Cases[hero1.Y, hero1.X];
                chemin = Pathfinding.CalculChemin(carte, depart, arrivee);
            }
            else if (!RetourneCheminNormal && hero2 != null && (Collision(hero2.Rectangle, fumee)))
            {
                depart = carte.Cases[Y, X];
                arrivee = carte.Cases[hero2.Y, hero2.X];
                chemin = Pathfinding.CalculChemin(carte, depart, arrivee);
            }

            if (chemin.Count != 0)
            {
                if (VaEnHaut && VaEnBas && VaADroite && VaAGauche)
                {
                    if ((int)chemin[chemin.Count - 1].X < X)
                    {
                        positionDesiree.X -= 28;
                        VaAGauche = false;
                        chemin.RemoveAt(chemin.Count - 1);
                    }
                    else if ((int)chemin[chemin.Count - 1].X > X)
                    {
                        positionDesiree.X += 28;
                        VaADroite = false;
                        chemin.RemoveAt(chemin.Count - 1);
                    }
                    else if ((int)chemin[chemin.Count - 1].Y < Y)
                    {
                        positionDesiree.Y -= 28;
                        VaEnHaut = false;
                        chemin.RemoveAt(chemin.Count - 1);
                    }
                    else if ((int)chemin[chemin.Count - 1].Y > Y)
                    {
                        positionDesiree.Y += 28;
                        VaEnBas = false;
                        chemin.RemoveAt(chemin.Count - 1);
                    }
                    else
                        chemin.RemoveAt(chemin.Count - 1);
                }
            }
            else
                Alerte = false;

            if (SourceRectangle.Value.Y == sourceRectangle1.Y)
            {
                SourceRectangle = sourceRectangle1;
                Regarde_Haut = true;
            }
            else
                Regarde_Haut = false;

            if (SourceRectangle.Value.Y == sourceRectangle3.Y)
            {
                SourceRectangle = sourceRectangle3;
                Regarde_Gauche = true;
            }
            else
                Regarde_Gauche = false;

            if (SourceRectangle.Value.Y == sourceRectangle2.Y)
            {
                SourceRectangle = sourceRectangle2;
                Regarde_Bas = true;
            }
            else
                Regarde_Bas = false;

            if (SourceRectangle.Value.Y == sourceRectangle4.Y)
            {
                SourceRectangle = sourceRectangle4;
                Regarde_Droite = true;
            }
            else
                Regarde_Droite = false;

            if (!VaEnHaut)
            {
                if (position != positionDesiree)
                {
                    position.Y -= VitesseSprite;
                    SourceRectangle = sourceRectangle1;
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
                    SourceRectangle = sourceRectangle2;
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
                    SourceRectangle = sourceRectangle3;
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
                    SourceRectangle = sourceRectangle4;
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

        private void UpdateChampDeVision(Carte carte)
        {
            if (Regarde_Bas || Regarde_Haut)
            {
                champDeVision1.X = 28 * X - 28;
                champDeVision2.X = 28 * X;
                champDeVision3.X = 28 * X + 28;

                champDeVision1.Width = 28;
                champDeVision2.Width = 28;
                champDeVision3.Width = 28;

                champDeVision1.Height = 28 * DistanceToWall(carte, -1);
                champDeVision2.Height = 28 * DistanceToWall(carte, 0);
                champDeVision3.Height = 28 * DistanceToWall(carte, 1);

                champDeVision1.Y = 28 * Y + (Regarde_Bas ? 28 : -28 * DistanceToWall(carte, -1));
                champDeVision2.Y = 28 * Y + (Regarde_Bas ? 28 : -28 * DistanceToWall(carte, 0));
                champDeVision3.Y = 28 * Y + (Regarde_Bas ? 28 : -28 * DistanceToWall(carte, 1));
            }
            else if (Regarde_Gauche || Regarde_Droite)
            {
                champDeVision1.Y = 28 * Y - 28;
                champDeVision2.Y = 28 * Y;
                champDeVision3.Y = 28 * Y + 28;

                champDeVision1.Height = 28;
                champDeVision2.Height = 28;
                champDeVision3.Height = 28;

                champDeVision1.Width = 28 * DistanceToWall(carte, -1);
                champDeVision2.Width = 28 * DistanceToWall(carte, 0);
                champDeVision3.Width = 28 * DistanceToWall(carte, 1);

                champDeVision1.X = 28 * X + (Regarde_Gauche ? -28 * DistanceToWall(carte, -1) : 28);
                champDeVision2.X = 28 * X + (Regarde_Gauche ? -28 * DistanceToWall(carte, 0) : 28);
                champDeVision3.X = 28 * X + (Regarde_Gauche ? -28 * DistanceToWall(carte, 1) : 28);
            }
        }

        private int DistanceToWall(Carte carte, int pos)
        {
            int distance = 0;

            if (this.X + pos >= 0 && this.X + pos < Taille_Map.LARGEUR_MAP)
            {
                if (Regarde_Haut)
                    for (int i = 0; i < 6 && this.Y - i >= 0 && carte.Cases[this.Y - i, this.X + pos].EstFranchissable; i++)
                        distance++;
                else if (Regarde_Bas)
                    for (int i = 0; i < 6 && this.Y + i < Taille_Map.HAUTEUR_MAP && carte.Cases[this.Y + i, this.X + pos].EstFranchissable; i++)
                        distance++;
            }
            if (this.Y + pos >= 0 && this.Y + pos < Taille_Map.HAUTEUR_MAP)
            {
                if (Regarde_Droite)
                    for (int i = 0; i < 6 && this.X + i < Taille_Map.LARGEUR_MAP && carte.Cases[this.Y + pos, this.X + i].EstFranchissable; i++)
                        distance++;
                else if (Regarde_Gauche)
                    for (int i = 0; i < 6 && this.X - i >= 0 && carte.Cases[this.Y + pos, this.X - i].EstFranchissable; i++)
                        distance++;
            }

            return (distance < 6 ? distance - 1 : 5);
        }

        public bool Collision(Rectangle contour, Rectangle fumee)
        {
            if (fumee != null && fumee.Intersects(contour))
                return false;

            if (contour.Intersects(champDeVision1) || contour.Intersects(champDeVision2) || contour.Intersects(champDeVision3))
            {
                GameplayScreen.Alerte = true;
                this.Alerte = true;
                return true;
            }
            return false;
        }

        private void MortDansLeChampDeVision(List<EnnemiMort> ennemisMorts, Rectangle fumee)
        {
            foreach (EnnemiMort mort in ennemisMorts)
                if (Collision(mort.Rectangle, fumee))
                {
                    GameplayScreen.Alerte = true;
                    this.Alerte = true;
                    break;
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
        /*
        public void tamere(SpriteBatch SP, Rectangle camera)
        {
            Draw(SP, camera);
            SP.Draw(fond, new Vector2(champDeVision1.X - camera.X, champDeVision1.Y - camera.Y), null, Color.White, 0, Vector2.Zero, new Vector2((float)champDeVision1.Width / (float)fond.Width, (float)champDeVision1.Height / (float)fond.Height), SpriteEffects.None, 0);
            SP.Draw(fond, new Vector2(champDeVision2.X - camera.X, champDeVision2.Y - camera.Y), null, Color.White, 0, Vector2.Zero, new Vector2((float)champDeVision2.Width / (float)fond.Width, (float)champDeVision2.Height / (float)fond.Height), SpriteEffects.None, 0);
            SP.Draw(fond, new Vector2(champDeVision3.X - camera.X, champDeVision3.Y - camera.Y), null, Color.White, 0, Vector2.Zero, new Vector2((float)champDeVision3.Width / (float)fond.Width, (float)champDeVision3.Height / (float)fond.Height), SpriteEffects.None, 0);
        }*/
    }
}