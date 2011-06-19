﻿using System.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace YelloKiller
{
    class Patrouilleur_a_cheval : Ennemi
    {
        List<Case> parcours;
        public int Etape { get; set; }
        public int Identifiant { get; private set; }

        public Patrouilleur_a_cheval(Vector2 position, Carte carte, int id)
            : base(position, carte)
        {
            this.position = position;
            SourceRectangle = new Rectangle(24, 0, 23, 30);
            Etape = 0;
            parcours = new List<Case>();
            Rectangle = new Rectangle((int)position.X + 1, (int)position.Y + 1, 22, 30);
            VitesseSprite = 4;
            VitesseAnimation = 0.016f;
            Identifiant = id;
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            LoadContent(content, 0, @"Feuilles de sprites\Patrouilleur_a_cheval");
            MaxIndex = maxIndex;
        }

        public void Update(GameTime gameTime, Carte carte, Hero hero1, Hero hero2, Rectangle camera, List<EnnemiMort> morts, Rectangle fumeeHeros1, Rectangle fumeeHeros2)
        {
            base.Update(gameTime, new Rectangle((int)Index * 24, 0, 23, 30), new Rectangle((int)Index * 24, 65, 23, 30), new Rectangle((int)Index * 24, 98, 23, 30), new Rectangle((int)Index * 24, 34, 23, 30), hero1, hero2, morts, fumeeHeros1, fumeeHeros2);

            if (!this.Alerte && (!Collision(hero1.Rectangle, fumeeHeros1, fumeeHeros2) || hero2 != null && !Collision(hero2.Rectangle, fumeeHeros1, fumeeHeros2)) && parcours.Count > 1)
                if (Chemin == null || Chemin.Count == 0)
                {
                    Etape++;
                    Chemin = Pathfinding.CalculChemin(carte, parcours[Etape % parcours.Count], parcours[(Etape + 1) % parcours.Count]);
                }
        }

        public void CreerTrajet(Carte carte)
        {
            Chemin = Pathfinding.CalculChemin(carte, Parcours[Etape % Parcours.Count], Parcours[(Etape + 1) % Parcours.Count]);
        }

        public void SauvegarderCheckPointPat(ref StreamWriter file)
        {
            file.WriteLine(X.ToString() + "," + Y.ToString() + "," + Identifiant.ToString() + "," + Etape.ToString());
        }

        public List<Case> Parcours
        {
            get { return parcours; }
            set { parcours = value; }
        }
    }
}