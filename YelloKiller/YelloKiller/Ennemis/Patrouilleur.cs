using System.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace YelloKiller
{
    class Patrouilleur : Ennemi
    {
        List<Case> parcours;
        public int Etape { get; set; }
        public int Identifiant { get; private set; }

        public Patrouilleur(Vector2 position, Carte carte, int id)
            : base(position, carte)
        {
            this.position = position;
            SourceRectangle = new Rectangle(24, 0, 19, 26);
            Etape = 0;
            parcours = new List<Case>();
            Rectangle = new Rectangle((int)position.X + 1, (int)position.Y + 1, 19, 26);
            Identifiant = id;
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            LoadContent(content, 0, @"Feuilles de sprites\Patrouilleur");
            MaxIndex = maxIndex;
        }

        public void Update(GameTime gameTime, Carte carte, Heros heros1, Heros heros2, Rectangle camera, List<EnnemiMort> morts, Rectangle fumeeHeros1, Rectangle fumeeHeros2)
        {
            base.Update(gameTime, new Rectangle((int)Index * 24, 0, 19, 26), new Rectangle((int)Index * 24, 63, 19, 26), new Rectangle((int)Index * 24, 96, 19, 26), new Rectangle((int)Index * 24, 32, 19, 26), heros1, heros2, morts, fumeeHeros1, fumeeHeros2);

            if (parcours.Count > 1 && RetourneCheminNormal)
                if (Chemin == null || Chemin.Count == 0)
                {
                    Etape++;
                    Depart = carte.Cases[(int)positionDesiree.Y / 28, (int)positionDesiree.X / 28];
                    Arrivee = parcours[(Etape + 1) % parcours.Count];
                    Chemin = Pathfinding.CalculChemin(carte, Depart, Arrivee);
                }

            if (Collision(heros1.Rectangle, fumeeHeros1, fumeeHeros2))
            {
                Depart = carte.Cases[(int)positionDesiree.Y / 28, (int)positionDesiree.X / 28];
                Arrivee = carte.Cases[heros1.Y, heros1.X];
                Chemin = Pathfinding.CalculChemin(carte, Depart, Arrivee);
                RetourneCheminNormal = false;
            }
            else if (heros2 != null && Collision(heros2.Rectangle, fumeeHeros1, fumeeHeros2))
            {
                Depart = carte.Cases[(int)positionDesiree.Y / 28, (int)positionDesiree.X / 28];
                Arrivee = carte.Cases[heros2.Y, heros2.X];
                Chemin = Pathfinding.CalculChemin(carte, Depart, Arrivee);
                RetourneCheminNormal = false;
            }
            else if (this.Alerte && !RetourneCheminNormal && Chemin.Count == 0)
            {
                Depart = carte.Cases[(int)positionDesiree.Y / 28, (int)positionDesiree.X / 28];
                Arrivee = parcours[Etape % parcours.Count];
                Chemin = Pathfinding.CalculChemin(carte, Depart, Arrivee);
                RetourneCheminNormal = true;
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