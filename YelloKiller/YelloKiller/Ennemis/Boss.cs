using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Input;

namespace YelloKiller
{
    class Boss : Ennemi
    {
        int vie;
        List<Case> chemin;
        Case depart;
        Case arrivee;

        public Boss(Vector2 position)
            : base(position)
        {
            Position = position;
            SourceRectangle = new Rectangle(26, 64, 18, 26);
            Rectangle = new Rectangle((int)position.X + 1, (int)position.Y + 1, 18, 26);
            MaxIndex = 0;
            positionDesiree = position;
            vie = 5;
            chemin = new List<Case>();

        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            base.LoadContent(content, "Boss");
            MaxIndex = maxIndex;
        }

        public int Vie
        {
            get { return vie; }
            set { vie = value; }
        }

        public void Update(GameTime gameTime, List<Shuriken> shuriken, Carte carte, Hero hero1, Hero hero2, Rectangle camera)
        {
            base.Update(gameTime, new Rectangle((int)Index * 24, 0, 16, 24), new Rectangle((int)Index * 24, 64, 16, 24), new Rectangle((int)Index * 24, 97, 16, 24), new Rectangle((int)Index * 24, 33, 16, 24));

            ServiceHelper.Game.Window.Title = "chemin =" + (chemin.Count) + "   Haut = " + Regarder_Haut + "  Droit = " + Regarder_Droite + "  Bas = " + Regarder_Bas + "  Gauche = " + Regarder_Gauche;

            if (Math.Abs(hero1.X - X) < 4 && Math.Abs(hero1.Y - Y) < 4)
            {
                depart = carte.Cases[Y, X];
                arrivee = carte.Cases[hero1.Y, hero1.X];
                chemin = Pathfinding.CalculChemin(carte, depart, arrivee);
            }
            else if (hero2 != null && Math.Abs(hero2.X - X) < 4 && Math.Abs(hero2.Y - Y) < 4)
            {
                depart = carte.Cases[Y, X];
                arrivee = carte.Cases[hero2.Y, hero2.X];
                chemin = Pathfinding.CalculChemin(carte, depart, arrivee);
            }

            if (chemin != null && chemin.Count != 0)
            {
                if (Monter && Descendre && Droite && Gauche)
                {
                    if ((int)chemin[chemin.Count - 1].X < X)
                    {
                        positionDesiree.X -= 28;
                        Gauche = false;
                        chemin.RemoveAt(chemin.Count - 1);
                    }
                    else if ((int)chemin[chemin.Count - 1].X > X)
                    {
                        positionDesiree.X += 28;
                        Droite = false;
                        chemin.RemoveAt(chemin.Count - 1);
                    }
                    else if ((int)chemin[chemin.Count - 1].Y < Y)
                    {
                        positionDesiree.Y -= 28;
                        Monter = false;
                        chemin.RemoveAt(chemin.Count - 1);
                    }
                    else if ((int)chemin[chemin.Count - 1].Y > Y)
                    {
                        positionDesiree.Y += 28;
                        Descendre = false;
                        chemin.RemoveAt(chemin.Count - 1);
                    }
                }
            }

            IA.Esquive_Shuriken.Boss_Esquive_Shuriken(hero1, this, shuriken, carte, camera);

            if (vie < 5 && chemin.Count == 0)
            {
                if (hero1.Regarder_Bas && position.Y > hero1.position.Y)
                    SourceRectangle = new Rectangle(26, 0, 16, 24);
                else if (hero1.Regarder_Gauche && position.X < hero1.position.X)
                    SourceRectangle = new Rectangle(26, 33, 16, 24);
               else if (hero1.Regarder_Haut && position.Y < hero1.position.Y)
                    SourceRectangle = new Rectangle(26, 64, 16, 24);
                else if (hero1.Regarder_Droite && position.X > hero1.position.X)
                    SourceRectangle = new Rectangle(26, 97, 16, 24);
            }
        }
    }
}