﻿using Microsoft.Xna.Framework;
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

            if (Math.Abs((int)(hero1.position.X / 28) - (int)(position.X / 28)) < 4 && Math.Abs((int)(hero1.position.Y / 28) - (int)(position.Y / 28)) < 4)
            {
                depart = carte.Cases[((int)position.Y) / 28, ((int)position.X) / 28];
                arrivee = carte.Cases[((int)hero1.position.Y) / 28, ((int)hero1.position.X) / 28];
                chemin = Pathfinding.CalculChemin(carte, depart, arrivee);
            }
            else if (hero2 != null && Math.Abs((int)(hero2.position.X / 28) - (int)(position.X / 28)) < 4 && Math.Abs((int)(hero2.position.Y / 28) - (int)(position.Y / 28)) < 4)
            {
                depart = carte.Cases[((int)position.Y) / 28, ((int)position.X) / 28];
                arrivee = carte.Cases[((int)hero2.position.Y) / 28, ((int)hero2.position.X) / 28];
                chemin = Pathfinding.CalculChemin(carte, depart, arrivee);
            }

            if (chemin != null && chemin.Count != 0)
            {
                if (Monter && Descendre && Droite && Gauche)
                {
                    if ((int)chemin[chemin.Count - 1].X < (int)Position.X / 28)
                    {
                        positionDesiree.X -= 28;
                        Gauche = false;
                        chemin.RemoveAt(chemin.Count - 1);
                    }
                    else if ((int)chemin[chemin.Count - 1].X > (int)Position.X / 28)
                    {
                        positionDesiree.X += 28;
                        Droite = false;
                        chemin.RemoveAt(chemin.Count - 1);
                    }
                    else if ((int)chemin[chemin.Count - 1].Y < (int)Position.Y / 28)
                    {
                        positionDesiree.Y -= 28;
                        Monter = false;
                        chemin.RemoveAt(chemin.Count - 1);
                    }
                    else if ((int)chemin[chemin.Count - 1].Y > (int)Position.Y / 28)
                    {
                        positionDesiree.Y += 28;
                        Descendre = false;
                        chemin.RemoveAt(chemin.Count - 1);
                    }
                }
            }

            IA.Esquive_Shuriken.Boss_Esquive_Shuriken(hero1, this, shuriken, carte, camera);
        }
    }
}