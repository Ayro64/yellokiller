using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

/* Cette classe gere le curseur de selection des textures dans l'editeur de map. */

namespace YelloKiller
{
    class Curseur
    {
        Texture2D texture, fond;
        Vector2 position;
        TypeCase type;
        float tailleFond;

        public Curseur(ContentManager content)
        {
            position = new Vector2(0, 0);
            texture = content.Load<Texture2D>(@"Textures\herbeFoncee");
            fond = content.Load<Texture2D>(@"Textures\fond");
            type = TypeCase.herbeFoncee;
            tailleFond = 1;
        }

        public Vector2 Position
        {
            get { return new Vector2(position.X - 2, position.Y); }
            set { position = value; }
        }

        public TypeCase Type
        {
            get { return type; }
        }

        public void Update(ContentManager content, Menu menu)
        {
            if (ServiceHelper.Get<IMouseService>().DansLEcran())
                position = new Vector2((int)ServiceHelper.Get<IMouseService>().Coordonnees().X / 28, (int)ServiceHelper.Get<IMouseService>().Coordonnees().Y / 28);

            for (int i = 0; i < menu.nbTexturesDroite; i++)
            {
                if (ServiceHelper.Get<IMouseService>().ClicBoutonGauche() && ServiceHelper.Get<IMouseService>().Rectangle().Intersects(menu.ListeRectanglesDroite[i]))
                {
                    texture = menu.ListeTexturesDroite[i];
                    switch (i)
                    {
                        case (0):
                            type = TypeCase.arbre;
                            break;
                        case (1):
                            type = TypeCase.arbre2;
                            break;
                        case (2):
                            type = TypeCase.buissonSurHerbe;
                            break;
                        case (3):
                            type = TypeCase.murBlanc;
                            break;
                        case (4):
                            type = TypeCase.tableauMurBlanc;
                            break;
                        case (5):
                            type = TypeCase.bois;
                            break;
                        case (6):
                            type = TypeCase.boisCarre;
                            break;
                        case (7):
                            type = TypeCase.tapisRougeBC;
                            break;
                        case (8):
                            type = TypeCase.herbe;
                            break;
                        case (9):
                            type = TypeCase.herbeFoncee;
                            break;
                        case (10):
                            type = TypeCase.piedDeMurBois;
                            break;
                        case (11):
                            type = TypeCase.terre;
                            break;
                        case (12):
                            type = TypeCase.carlageNoir;
                            break;
                        case (13):
                            type = TypeCase.fondNoir;
                            break;
                        case (14):
                            type = TypeCase.finMurFN;
                            break;
                        case (15):
                            type = TypeCase.finMurGauche;
                            break;
                        case (16):
                            type = TypeCase.finMurDroite;
                            break;
                        case (17):
                            type = TypeCase.Lit;
                            break;
                        case (18):
                            type = TypeCase.commode;
                            break;
                        case (19):
                            type = TypeCase.TableMoyenne;
                            break;
                        case (20):
                            type = TypeCase.GrandeTable;
                            break;
                    }

                    tailleFond = 1 + 0.88f * (texture.Width / 28 - 1);
                }
            } 
            
            for (int b = 0; b < menu.nbTexturesGauche; b++)
            {
                if (ServiceHelper.Get<IMouseService>().ClicBoutonGauche() && ServiceHelper.Get<IMouseService>().Rectangle().Intersects(menu.ListeRectanglesGauche[b]))
                {
                    texture = menu.ListeTexturesGauche[b];
                    switch (b)
                    {
                        case (0):
                            type = TypeCase.Joueur1;
                            break;
                        case (1):
                            type = TypeCase.Joueur2;
                            break;
                        case (2):
                            type = TypeCase.Garde;
                            break;
                        case (3):
                            type = TypeCase.Patrouilleur;
                            break;
                        case (4):
                            type = TypeCase.Patrouilleur_a_cheval;
                            break;
                        case (5):
                            type = TypeCase.Boss;
                            break;
                        case (6):
                            type = TypeCase.Statues;
                            break;
                    }

                    tailleFond = 1 + 0.88f * (texture.Width / 28 - 1);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(fond, new Vector2(position.X * 28 - 2, position.Y * 28 - 2), null, Color.White, 0, Vector2.Zero, tailleFond, SpriteEffects.None, 0);            
            spriteBatch.Draw(texture, new Vector2(position.X * 28, position.Y * 28), Color.White);
        }
    }
}
