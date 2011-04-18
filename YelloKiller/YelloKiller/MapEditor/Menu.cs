using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

/* Cette classe gere le menu de selection de droite dans l'editeur de map. */

namespace YelloKiller
{
    class Menu
    {
        List<Rectangle> listeRectanglesDroite = new List<Rectangle>(), listeRectanglesGauche = new List<Rectangle>();
        List<Texture2D> listeTexturesDroite = new List<Texture2D>(), listeTexturesGauche = new List<Texture2D>();
        Texture2D fond;

        public int nbTexturesDroite, nbTexturesGauche;

        public Menu(ContentManager content, int nbTexturesDroite, int nbTexturesGauche)
        {
            this.nbTexturesDroite = nbTexturesDroite;
            this.nbTexturesGauche = nbTexturesGauche;

            for (int i = 0; i < nbTexturesDroite; i++)
                listeRectanglesDroite.Add(new Rectangle(0, 0, 28, 28));

            for (int i = 0; i < nbTexturesGauche; i++)
                listeRectanglesGauche.Add(new Rectangle(0, 0, 28, 28));

            listeTexturesGauche.Add(content.Load<Texture2D>("origine_hero1"));
            listeTexturesGauche.Add(content.Load<Texture2D>("origine_hero2"));
            listeTexturesGauche.Add(content.Load<Texture2D>("origine_garde"));
            listeTexturesGauche.Add(content.Load<Texture2D>("origine_patrouilleur"));
            listeTexturesGauche.Add(content.Load<Texture2D>("origine_patrouilleur_a_cheval"));
            listeTexturesGauche.Add(content.Load<Texture2D>("origine_boss"));
            listeTexturesGauche.Add(content.Load<Texture2D>("origine_statue"));

            listeTexturesDroite.Add(content.Load<Texture2D>(@"Textures\arbre"));
            listeTexturesDroite.Add(content.Load<Texture2D>(@"Textures\arbre2"));
            listeTexturesDroite.Add(content.Load<Texture2D>(@"Textures\buissonSurHerbe"));
            listeTexturesDroite.Add(content.Load<Texture2D>(@"Textures\murBlanc"));
            listeTexturesDroite.Add(content.Load<Texture2D>(@"Textures\tableauMurBlanc"));
            listeTexturesDroite.Add(content.Load<Texture2D>(@"Textures\bois"));
            listeTexturesDroite.Add(content.Load<Texture2D>(@"Textures\boisCarre"));
            listeTexturesDroite.Add(content.Load<Texture2D>(@"Textures\tapisRougeBC"));
            listeTexturesDroite.Add(content.Load<Texture2D>(@"Textures\herbe"));
            listeTexturesDroite.Add(content.Load<Texture2D>(@"Textures\herbeFoncee"));
            listeTexturesDroite.Add(content.Load<Texture2D>(@"Textures\piedDeMurBois"));
            listeTexturesDroite.Add(content.Load<Texture2D>(@"Textures\terre"));
            listeTexturesDroite.Add(content.Load<Texture2D>(@"Textures\carlageNoir"));
            listeTexturesDroite.Add(content.Load<Texture2D>(@"Textures\fondNoir"));
            listeTexturesDroite.Add(content.Load<Texture2D>(@"Textures\FinMurFN"));
            listeTexturesDroite.Add(content.Load<Texture2D>(@"Textures\FinMurGauche"));
            listeTexturesDroite.Add(content.Load<Texture2D>(@"Textures\FinMurDroite"));
            listeTexturesDroite.Add(content.Load<Texture2D>(@"Textures\lit"));
            listeTexturesDroite.Add(content.Load<Texture2D>(@"Textures\Commode"));
            listeTexturesDroite.Add(content.Load<Texture2D>(@"Textures\tableMoyenne"));
            listeTexturesDroite.Add(content.Load<Texture2D>(@"Textures\grandeTable"));

            fond = content.Load<Texture2D>(@"Textures\fond");
        }

        public List<Rectangle> ListeRectanglesDroite
        {
            get { return listeRectanglesDroite; }
        }

        public List<Texture2D> ListeTexturesDroite
        {
            get { return listeTexturesDroite; }
        }

        public List<Rectangle> ListeRectanglesGauche
        {
            get { return listeRectanglesGauche; }
        }

        public List<Texture2D> ListeTexturesGauche
        {
            get { return listeTexturesGauche; }
        }

        public void Update(Ascenseur ascenseurDroit, Ascenseur ascenseurGauche)
        {
            for (int i = 0; i < nbTexturesDroite; i++)
                listeRectanglesDroite[i] = new Rectangle(Taille_Ecran.LARGEUR_ECRAN - 56, (int)-ascenseurDroit.Position.Y + i * 40, 28, 28);

            for (int i = 0; i < nbTexturesGauche; i++)
                listeRectanglesGauche[i] = new Rectangle(28, (int)-ascenseurGauche.Position.Y + i * 40, 28, 28);
        }

        public void Draw(SpriteBatch spriteBatch, Ascenseur ascenseurDroit, Ascenseur ascenseurGauche)
        {
            for (int u = 0; u < nbTexturesDroite; u++)
            {
                if (ServiceHelper.Get<IMouseService>().Rectangle().Intersects(listeRectanglesDroite[u]))
                {
                    spriteBatch.Draw(fond, new Vector2(listeRectanglesDroite[u].X + 28 * (1 - listeTexturesDroite[u].Width / 28) - 2, listeRectanglesDroite[u].Y + 28 * (1 - listeTexturesDroite[u].Height / 28) - 2), null, Color.White, 0, Vector2.Zero, 1 + 0.88f * (listeTexturesDroite[u].Width / 28 - 1), SpriteEffects.None, 0);

                    spriteBatch.Draw(listeTexturesDroite[u], new Vector2(listeRectanglesDroite[u].X + 28 * (1 - listeTexturesDroite[u].Width / 28), listeRectanglesDroite[u].Y + 28 * (1 - listeTexturesDroite[u].Height / 28)), Color.White);
                }
                else
                    spriteBatch.Draw(listeTexturesDroite[u], new Vector2(Taille_Ecran.LARGEUR_ECRAN - 56, -ascenseurDroit.Position.Y + u * 40), null, Color.White, 0, Vector2.Zero, (float)28 / listeTexturesDroite[u].Height, SpriteEffects.None, 0);
            }

            for (int c = 0; c < nbTexturesGauche; c++)
            {
                if (ServiceHelper.Get<IMouseService>().Rectangle().Intersects(listeRectanglesGauche[c]))
                {
                    spriteBatch.Draw(fond, new Vector2(listeRectanglesGauche[c].X + 28 * (1 - listeTexturesGauche[c].Width / 28) - 2, listeRectanglesGauche[c].Y + 28 * (1 - listeTexturesGauche[c].Height / 28) - 2), null, Color.White, 0, Vector2.Zero, 1 + 0.88f * (listeTexturesGauche[c].Width / 28 - 1), SpriteEffects.None, 0);

                    spriteBatch.Draw(listeTexturesGauche[c], new Vector2(listeRectanglesGauche[c].X + 28 * (1 - listeTexturesGauche[c].Width / 28), listeRectanglesGauche[c].Y + 28 * (1 - listeTexturesGauche[c].Height / 28)), Color.White);
                }
                else
                    spriteBatch.Draw(listeTexturesGauche[c], new Vector2(28, -ascenseurGauche.Position.Y + c * 40), null, Color.White, 0, Vector2.Zero, (float)28 / listeTexturesGauche[c].Height, SpriteEffects.None, 0);
            }
        }
    }
}