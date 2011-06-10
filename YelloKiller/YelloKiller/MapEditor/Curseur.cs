using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    class Curseur
    {
        Texture2D texture, fond;
        Vector2 position;
        TypeCase type;
        Vector2 tailleFond;

        public Curseur(ContentManager content)
        {
            position = new Vector2(0, 0);
            texture = content.Load<Texture2D>(@"Textures\petites\herbeFoncee");
            fond = content.Load<Texture2D>(@"Textures\fond");
            type = TypeCase.herbeFoncee;
            tailleFond = Vector2.One;
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
                        case 0:
                            type = TypeCase.arbre;
                            break;
                        case 1:
                            type = TypeCase.commode;
                            break;
                        case 2:
                            type = TypeCase.grandeTable;
                            break;
                        case 3:
                            type = TypeCase.grandeTableDeco;
                            break;
                        case 4:
                            type = TypeCase.nvlHerbe;
                            break;
                        case 5:
                            type = TypeCase.lit;
                            break;
                        case 6:
                            type = TypeCase.mur;
                            break;
                        case 7:
                            type = TypeCase.murBlanc;
                            break;
                        case 8:
                            type = TypeCase.murBlancDrap;
                            break;
                        case 9:
                            type = TypeCase.murBlancEpee;
                            break;
                        case 10:
                            type = TypeCase.murBlancTableau;
                            break;
                        case 11:
                            type = TypeCase.murEpee;
                            break;
                        case 12:
                            type = TypeCase.murTableau;
                            break;
                        case 13:
                            type = TypeCase.tableauMurBlanc;
                            break;
                        case 14:
                            type = TypeCase.parquet;
                            break;
                        case 15:
                            type = TypeCase.parquetArbre;
                            break;
                        case 16:
                            type = TypeCase.parquetBuisson;
                            break;
                        case 17:
                            type = TypeCase.bois;
                            break;
                        case 18:
                            type = TypeCase.boisCarre;
                            break;
                        case 19:
                            type = TypeCase.boisDeco;
                            break;
                        case 20:
                            type = TypeCase.buissonSurHerbe;
                            break;
                        case 21:
                            type = TypeCase.carlageNoir;
                            break;
                        case 22:
                            type = TypeCase.carlageNoirDeco;
                            break;
                        case 23:
                            type = TypeCase.coinbotdroit;
                            break;
                        case 24:
                            type = TypeCase.coinbotgauche;
                            break;
                        case 25:
                            type = TypeCase.cointopdroit;
                            break;
                        case 26:
                            type = TypeCase.cointopgauche;
                            break;
                        case 27:
                            type = TypeCase.finMurBas;
                            break;
                        case 28:
                            type = TypeCase.finMurDroit;
                            break;
                        case 29:
                            type = TypeCase.finMurHaut;
                            break;
                        case 30:
                            type = TypeCase.finMurGauche;
                            break;
                        case 31:
                            type = TypeCase.fondNoir;
                            break;
                        case 32:
                            type = TypeCase.herbe;
                            break;
                        case 33:
                            type = TypeCase.herbeDeco;
                            break;
                        case 34:
                            type = TypeCase.herbeFoncee;
                            break;
                        case 35:
                            type = TypeCase.herbeH;
                            break;
                        case 36:
                            type = TypeCase.piedMurBois;
                            break;
                        case 37:
                            type = TypeCase.tapisRougeBC;
                            break;
                        case 38:
                            type = TypeCase.terre;
                            break;
                        case 39:
                            type = TypeCase.eau;
                            break;
                        case 40:
                            type = TypeCase.caisse;
                            break;
                        case 41:
                            type = TypeCase.chaiseGauche;
                            break;
                        case 42:
                            type = TypeCase.chaiseDroite;
                            break;
                        case 43:
                            type = TypeCase.pont1;
                            break;
                        case 44:
                            type = TypeCase.pont2;
                            break;
                        case 45:
                            type = TypeCase.tableMoyenne;
                            break;
                        case 46:
                            type = TypeCase.bibliotheque;
                            break;
                        case 47:
                            type = TypeCase.canape;
                            break;
                        case 48:
                            type = TypeCase.canapeRalonge;
                            break;
                        case 49:
                            type = TypeCase.fenetre;
                            break;
                        case 50:
                            type = TypeCase.porteFenetre;
                            break;
                        case 51:
                            type = TypeCase.grdSiege;
                            break;
                        case 52:
                            type = TypeCase.pillier;
                            break;
                        case 53:
                            type = TypeCase.porte;
                            break;
                        case 54:
                            type = TypeCase.rocher;
                            break;
                    }

                    tailleFond.X = 1 + 0.88f * (texture.Width / 28 - 1);
                    tailleFond.Y = 1 + 0.88f * (texture.Height / 28 - 1);
                }
            }

            for (int b = 0; b < menu.nbTexturesGauche; b++)
            {
                if (ServiceHelper.Get<IMouseService>().ClicBoutonGauche() && ServiceHelper.Get<IMouseService>().Rectangle().Intersects(menu.ListeRectanglesGauche[b]))
                {
                    texture = menu.ListeTexturesGauche[b];
                    switch (b)
                    {
                        case 0:
                            type = TypeCase.Joueur1;
                            break;
                        case 1:
                            type = TypeCase.Joueur2;
                            break;
                        case 2:
                            type = TypeCase.Garde;
                            break;
                        case 3:
                            type = TypeCase.Patrouilleur;
                            break;
                        case 4:
                            type = TypeCase.Patrouilleur_a_cheval;
                            break;
                        case 5:
                            type = TypeCase.Boss;
                            break;
                        case 6:
                            type = TypeCase.Statues;
                            break;
                        case 7:
                            type = TypeCase.BonusShurikens;
                            break;
                        case 8:
                            type = TypeCase.BonusHadokens;
                            break;
                        case 9:
                            type = TypeCase.BonusCheckPoint;
                            break;
                    }

                    tailleFond.X = 1 + 0.88f * (texture.Width / 28 - 1);
                    tailleFond.Y = 1 + 0.88f * (texture.Height / 28 - 1);
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
