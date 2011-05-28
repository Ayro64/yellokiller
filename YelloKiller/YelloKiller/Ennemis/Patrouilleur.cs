using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace YelloKiller
{
    class Patrouilleur : Ennemi
    {
        List<Case> parcours;
        int etape;

        public Patrouilleur(Vector2 position, Carte carte)
            : base(position, carte)
        {
            this.position = position;
            SourceRectangle = new Rectangle(24, 0, 19, 26);
            etape = 0;
            parcours = new List<Case>();
            Rectangle = new Rectangle((int)position.X + 1, (int)position.Y + 1, 19, 26);
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            LoadContent(content, 0, @"Feuilles de sprites\Patrouilleur");
            MaxIndex = maxIndex;
        }

        public void Update(GameTime gameTime, Carte carte, Hero hero1, Hero hero2, Rectangle camera)
        {
            base.Update(gameTime, new Rectangle((int)Index * 24, 0, 19, 26), new Rectangle((int)Index * 24, 63, 19, 26), new Rectangle((int)Index * 24, 96, 19, 26), new Rectangle((int)Index * 24, 32, 19, 26), hero1, hero2);

            if (!Alerte && !Collision(hero1.Rectangle) && parcours.Count > 1)
            {
                if (Chemin == null || Chemin.Count == 0)
                {
                    etape++;
                    Chemin = Pathfinding.CalculChemin(carte, parcours[etape % parcours.Count], parcours[(etape + 1) % parcours.Count]);
                }
            }
        }

        public void CreerTrajet(Carte carte)
        {
            Chemin = Pathfinding.CalculChemin(carte, Parcours[Etape % Parcours.Count], Parcours[(Etape + 1) % Parcours.Count]);
        }

        public List<Case> Parcours
        {
            get { return parcours; }
            set { parcours = value; }
        }

        public int Etape
        {
            get { return etape; }
        }
    }
}