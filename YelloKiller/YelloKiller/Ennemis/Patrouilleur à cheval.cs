using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace YelloKiller
{
    class Patrouilleur_a_cheval : Ennemi
    {
        List<Case> parcours;
        private int Etape { get; set; }

        public Patrouilleur_a_cheval(Vector2 position, Carte carte)
            : base(position, carte)
        {
            this.position = position;
            SourceRectangle = new Rectangle(24, 0, 23, 30);
            Etape = 0;
            parcours = new List<Case>();
            Rectangle = new Rectangle((int)position.X + 1, (int)position.Y + 1, 22, 30);
            VitesseSprite = 2;
            VitesseAnimation = 0.016f;
        }

        public void LoadContent(ContentManager content, int maxIndex)
        {
            LoadContent(content, 0, @"Feuilles de sprites\Patrouilleur_a_cheval");
            MaxIndex = maxIndex;
        }

        public void Update(GameTime gameTime, Carte carte, Hero hero1, Hero hero2, Rectangle camera)
        {
            base.Update(gameTime, new Rectangle((int)Index * 24, 0, 23, 30), new Rectangle((int)Index * 24, 65, 23, 30), new Rectangle((int)Index * 24, 98, 23, 30), new Rectangle((int)Index * 24, 34, 23, 30), hero1, hero2);

            if (!this.Alerte && (!Collision(hero1.Rectangle) || hero2 != null && !Collision(hero2.Rectangle)) && parcours.Count > 1)
                if (Chemin == null || Chemin.Count == 0)
                {
                    Etape++;
                    Chemin = Pathfinding.CalculChemin(carte, carte.Cases[Y, X], parcours[(Etape + 1) % parcours.Count]);
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
    }
}