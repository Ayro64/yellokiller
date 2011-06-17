using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
   public class Case
    {
        Vector2 position;
        Texture2D texture;
        TypeCase type;
        string nomTexture;
        int x, y;
        double temps;

        public Case(Vector2 position, TypeCase type, int index)
        {
            this.position = position;
            this.type = type;
            this.Index = index;
            temps = 0;
            x = (int)position.X / 28;
            y = (int)position.Y / 28;
        }

        public int Index { get; set; }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public int X
        {
            get { return x; }
        }

        public int Y
        {
            get { return y; }
        }

        public TypeCase Type
        {
            get { return type; }
            set { type = value; }
        }

        private void LoadContent(ContentManager content)
        {
            switch (type)
            {
                case TypeCase.arbre:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\Arbre\arbre1";
                            break;
                        case 2:
                            nomTexture = @"Textures\Arbre\arbre2";
                            break;
                        case 3:
                            nomTexture = @"Textures\Arbre\arbre3";
                            break;
                        case 4:
                            nomTexture = @"Textures\Arbre\arbre4";
                            break;
                        case 5:
                            nomTexture = @"Textures\Arbre\arbre5";
                            break;
                        case 6:
                            nomTexture = @"Textures\Arbre\arbre6";
                            break;
                    }
                    break;
                case TypeCase.canape:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\canape\canape1";
                            break;
                        case 2:
                            nomTexture = @"Textures\canape\canape2";
                            break;
                        case 3:
                            nomTexture = @"Textures\canape\canape3";
                            break;
                        case 4:
                            nomTexture = @"Textures\canape\canape4";
                            break;
                        case 5:
                            nomTexture = @"Textures\canape\canape5";
                            break;
                        case 6:
                            nomTexture = @"Textures\canape\canape6";
                            break;
                    }
                    break;
                case TypeCase.porteFenetre:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\fenetre\portefenetre1";
                            break;
                        case 2:
                            nomTexture = @"Textures\fenetre\portefenetre2";
                            break;
                        case 3:
                            nomTexture = @"Textures\fenetre\portefenetre3";
                            break;
                        case 4:
                            nomTexture = @"Textures\fenetre\portefenetre4";
                            break;
                        case 5:
                            nomTexture = @"Textures\fenetre\portefenetre5";
                            break;
                        case 6:
                            nomTexture = @"Textures\fenetre\portefenetre6";
                            break;
                    }
                    break;
                case TypeCase.grdSiege:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\grdsiege\grdsiege1";
                            break;
                        case 2:
                            nomTexture = @"Textures\grdsiege\grdsiege2";
                            break;
                        case 3:
                            nomTexture = @"Textures\grdsiege\grdsiege3";
                            break;
                        case 4:
                            nomTexture = @"Textures\grdsiege\grdsiege4";
                            break;
                        case 5:
                            nomTexture = @"Textures\grdsiege\grdsiege5";
                            break;
                        case 6:
                            nomTexture = @"Textures\grdsiege\grdsiege6";
                            break;
                    }
                    break;
                case TypeCase.pont1:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\ponts\pont11";
                            break;
                        case 2:
                            nomTexture = @"Textures\ponts\pont12";
                            break;
                    }
                    break;
                case TypeCase.bibliotheque:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\bibli\bibli1";
                            break;
                        case 2:
                            nomTexture = @"Textures\bibli\bibli2";
                            break;
                    }
                    break;
                case TypeCase.pont2:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\ponts\pont21";
                            break;
                        case 2:
                            nomTexture = @"Textures\ponts\pont22";
                            break;
                        case 3:
                            nomTexture = @"Textures\ponts\pont23";
                            break;
                        case 4:
                            nomTexture = @"Textures\ponts\pont24";
                            break;
                        case 5:
                            nomTexture = @"Textures\ponts\pont25";
                            break;
                        case 6:
                            nomTexture = @"Textures\ponts\pont26";
                            break;
                        case 7:
                            nomTexture = @"Textures\ponts\pont27";
                            break;
                        case 8:
                            nomTexture = @"Textures\ponts\pont28";
                            break;
                    }
                    break;

                case TypeCase.commode:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\commode\commode1";
                            break;
                        case 2:
                            nomTexture = @"Textures\commode\commode2";
                            break;
                        case 3:
                            nomTexture = @"Textures\commode\commode3";
                            break;
                        case 4:
                            nomTexture = @"Textures\commode\commode4";
                            break;
                    }
                    break;

                case TypeCase.lit:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\lit\lit1";
                            break;
                        case 2:
                            nomTexture = @"Textures\lit\lit2";
                            break;
                        case 3:
                            nomTexture = @"Textures\lit\lit3";
                            break;
                        case 4:
                            nomTexture = @"Textures\lit\lit4";
                            break;
                    }
                    break;

                case TypeCase.mur:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\mur\mur\mur1";
                            break;
                        case 2:
                            nomTexture = @"Textures\mur\mur\mur2";
                            break;
                        case 3:
                            nomTexture = @"Textures\mur\mur\mur3";
                            break;
                        case 4:
                            nomTexture = @"Textures\mur\mur\mur4";
                            break;
                    }
                    break;

                case TypeCase.murBlanc:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\mur\murBlanc\murBlanc1";
                            break;
                        case 2:
                            nomTexture = @"Textures\mur\murBlanc\murBlanc2";
                            break;
                        case 3:
                            nomTexture = @"Textures\mur\murBlanc\murBlanc3";
                            break;
                        case 4:
                            nomTexture = @"Textures\mur\murBlanc\murBlanc4";
                            break;
                    }
                    break;

                case TypeCase.murBlancDrap:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\mur\MurBlancdrap\MurBlancdrap1";
                            break;
                        case 2:
                            nomTexture = @"Textures\mur\MurBlancdrap\MurBlancdrap2";
                            break;
                        case 3:
                            nomTexture = @"Textures\mur\MurBlancdrap\MurBlancdrap3";
                            break;
                        case 4:
                            nomTexture = @"Textures\mur\MurBlancdrap\MurBlancdrap4";
                            break;
                    }
                    break;

                case TypeCase.murBlancEpee:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\mur\murblancepee\MurBlancEpee1";
                            break;
                        case 2:
                            nomTexture = @"Textures\mur\murblancepee\MurBlancEpee2";
                            break;
                        case 3:
                            nomTexture = @"Textures\mur\murblancepee\MurBlancEpee3";
                            break;
                        case 4:
                            nomTexture = @"Textures\mur\murblancepee\MurBlancEpee4";
                            break;
                    }
                    break;

                case TypeCase.murBlancTableau:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\mur\murblanctableau\MurBlanctableau1";
                            break;
                        case 2:
                            nomTexture = @"Textures\mur\murblanctableau\MurBlanctableau2";
                            break;
                        case 3:
                            nomTexture = @"Textures\mur\murblanctableau\MurBlanctableau3";
                            break;
                        case 4:
                            nomTexture = @"Textures\mur\murblanctableau\MurBlanctableau4";
                            break;
                    }
                    break;

                case TypeCase.murEpee:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\mur\murepee\murepee1";
                            break;
                        case 2:
                            nomTexture = @"Textures\mur\murepee\murepee2";
                            break;
                        case 3:
                            nomTexture = @"Textures\mur\murepee\murepee3";
                            break;
                        case 4:
                            nomTexture = @"Textures\mur\murepee\murepee4";
                            break;
                    }
                    break;

                case TypeCase.murTableau:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\mur\murtableau\murtableau1";
                            break;
                        case 2:
                            nomTexture = @"Textures\mur\murtableau\murtableau2";
                            break;
                        case 3:
                            nomTexture = @"Textures\mur\murtableau\murtableau3";
                            break;
                        case 4:
                            nomTexture = @"Textures\mur\murtableau\murtableau4";
                            break;
                    }
                    break;

                case TypeCase.tableauMurBlanc:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\mur\tableauMurBlanc\tableauMurBlanc1";
                            break;
                        case 2:
                            nomTexture = @"Textures\mur\tableauMurBlanc\tableauMurBlanc2";
                            break;
                        case 3:
                            nomTexture = @"Textures\mur\tableauMurBlanc\tableauMurBlanc3";
                            break;
                        case 4:
                            nomTexture = @"Textures\mur\tableauMurBlanc\tableauMurBlanc4";
                            break;
                    }
                    break;

                case TypeCase.tableMoyenne:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\tableMoyenne\tableMoyenne1";
                            break;
                        case 2:
                            nomTexture = @"Textures\tableMoyenne\tableMoyenne2";
                            break;
                        case 3:
                            nomTexture = @"Textures\tableMoyenne\tableMoyenne3";
                            break;
                        case 4:
                            nomTexture = @"Textures\tableMoyenne\tableMoyenne4";
                            break;
                    }
                    break;

                case TypeCase.nvlHerbe:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\herbe\nvlherbe1";
                            break;
                        case 2:
                            nomTexture = @"Textures\herbe\nvlherbe2";
                            break;
                        case 3:
                            nomTexture = @"Textures\herbe\nvlherbe3";
                            break;
                        case 4:
                            nomTexture = @"Textures\herbe\nvlherbe4";
                            break;
                    }
                    break;

                case TypeCase.parquet:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\parquet\parquet\parquet1";
                            break;
                        case 2:
                            nomTexture = @"Textures\parquet\parquet\parquet2";
                            break;
                        case 3:
                            nomTexture = @"Textures\parquet\parquet\parquet3";
                            break;
                        case 4:
                            nomTexture = @"Textures\parquet\parquet\parquet4";
                            break;
                    }
                    break;

                case TypeCase.parquetArbre:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\parquet\parquetarbre\parquetarbre1";
                            break;
                        case 2:
                            nomTexture = @"Textures\parquet\parquetarbre\parquetarbre2";
                            break;
                        case 3:
                            nomTexture = @"Textures\parquet\parquetarbre\parquetarbre3";
                            break;
                        case 4:
                            nomTexture = @"Textures\parquet\parquetarbre\parquetarbre4";
                            break;
                    }
                    break;

                case TypeCase.parquetBuisson:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\parquet\parquetbuisson\parquetbuisson1";
                            break;
                        case 2:
                            nomTexture = @"Textures\parquet\parquetbuisson\parquetbuisson2";
                            break;
                        case 3:
                            nomTexture = @"Textures\parquet\parquetbuisson\parquetbuisson3";
                            break;
                        case 4:
                            nomTexture = @"Textures\parquet\parquetbuisson\parquetbuisson4";
                            break;
                    }
                    break;

                case TypeCase.canapeRalonge:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\canape\canapeRalonge1";
                            break;
                        case 2:
                            nomTexture = @"Textures\canape\canapeRalonge2";
                            break;
                        case 3:
                            nomTexture = @"Textures\canape\canapeRalonge3";
                            break;
                        case 4:
                            nomTexture = @"Textures\canape\canapeRalonge4";
                            break;
                    }
                    break;

                case TypeCase.fenetre:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\fenetre\fenetre1";
                            break;
                        case 2:
                            nomTexture = @"Textures\fenetre\fenetre2";
                            break;
                        case 3:
                            nomTexture = @"Textures\fenetre\fenetre3";
                            break;
                        case 4:
                            nomTexture = @"Textures\fenetre\fenetre4";
                            break;
                    }
                    break;

                case TypeCase.pillier:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\pillier\pillier1";
                            break;
                        case 2:
                            nomTexture = @"Textures\pillier\pillier2";
                            break;
                        case 3:
                            nomTexture = @"Textures\pillier\pillier3";
                            break;
                        case 4:
                            nomTexture = @"Textures\pillier\pillier4";
                            break;
                    }
                    break;

                case TypeCase.porte:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\porte\porte1";
                            break;
                        case 2:
                            nomTexture = @"Textures\porte\porte2";
                            break;
                        case 3:
                            nomTexture = @"Textures\porte\porte3";
                            break;
                        case 4:
                            nomTexture = @"Textures\porte\porte4";
                            break;
                    }
                    break;

                case TypeCase.rocher:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\rocher\rocher1";
                            break;
                        case 2:
                            nomTexture = @"Textures\rocher\rocher2";
                            break;
                        case 3:
                            nomTexture = @"Textures\rocher\rocher3";
                            break;
                        case 4:
                            nomTexture = @"Textures\rocher\rocher4";
                            break;
                    }
                    break;


                case TypeCase.grandeTable:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\grandeTable\grandeTable1";
                            break;
                        case 2:
                            nomTexture = @"Textures\grandeTable\grandeTable2";
                            break;
                        case 3:
                            nomTexture = @"Textures\grandeTable\grandeTable3";
                            break;
                        case 4:
                            nomTexture = @"Textures\grandeTable\grandeTable4";
                            break;
                        case 5:
                            nomTexture = @"Textures\grandeTable\grandeTable5";
                            break;
                        case 6:
                            nomTexture = @"Textures\grandeTable\grandeTable6";
                            break;
                        case 7:
                            nomTexture = @"Textures\grandeTable\grandeTable7";
                            break;
                        case 8:
                            nomTexture = @"Textures\grandeTable\grandeTable8";
                            break;
                        case 9:
                            nomTexture = @"Textures\grandeTable\grandeTable9";
                            break;
                    }
                    break;

                case TypeCase.grandeTableDeco:
                    switch (Index)
                    {
                        case 1:
                            nomTexture = @"Textures\grandeTableDeco\grandeTableDeco1";
                            break;
                        case 2:
                            nomTexture = @"Textures\grandeTableDeco\grandeTableDeco2";
                            break;
                        case 3:
                            nomTexture = @"Textures\grandeTableDeco\grandeTableDeco3";
                            break;
                        case 4:
                            nomTexture = @"Textures\grandeTableDeco\grandeTableDeco4";
                            break;
                        case 5:
                            nomTexture = @"Textures\grandeTableDeco\grandeTableDeco5";
                            break;
                        case 6:
                            nomTexture = @"Textures\grandeTableDeco\grandeTableDeco6";
                            break;
                        case 7:
                            nomTexture = @"Textures\grandeTableDeco\grandeTableDeco7";
                            break;
                        case 8:
                            nomTexture = @"Textures\grandeTableDeco\grandeTableDeco8";
                            break;
                        case 9:
                            nomTexture = @"Textures\grandeTableDeco\grandeTableDeco9";
                            break;
                    }
                    break;



                case TypeCase.buissonSurHerbe:
                    nomTexture = @"Textures\petites\buissonSurHerbe";
                    break;
                case TypeCase.coinbotdroit:
                    nomTexture = @"Textures\petites\coinbotdroit";
                    break;
                case TypeCase.coinbotgauche:
                    nomTexture = @"Textures\petites\coinbotgauche";
                    break;
                case TypeCase.cointopdroit:
                    nomTexture = @"Textures\petites\cointopdroit";
                    break;
                case TypeCase.cointopgauche:
                    nomTexture = @"Textures\petites\cointopgauche";
                    break;
                case TypeCase.finMurDroit:
                    nomTexture = @"Textures\petites\finMurDroit";
                    break;
                case TypeCase.finMurGauche:
                    nomTexture = @"Textures\petites\finMurGauche";
                    break;
                case TypeCase.fondNoir:
                    nomTexture = @"Textures\petites\fondNoir";
                    break;
                case TypeCase.piedMurBois:
                    nomTexture = @"Textures\petites\piedMurBois";
                    break;
                case TypeCase.bois:
                    nomTexture = @"Textures\petites\bois";
                    break;
                case TypeCase.boisCarre:
                    nomTexture = @"Textures\petites\boisCarre";
                    break;
                case TypeCase.boisDeco:
                    nomTexture = @"Textures\petites\boisDeco";
                    break;
                case TypeCase.carlageNoir:
                    nomTexture = @"Textures\petites\carlageNoir";
                    break;
                case TypeCase.carlageNoirDeco:
                    nomTexture = @"Textures\petites\carlageNoirDeco";
                    break;
                case TypeCase.herbe:
                    nomTexture = @"Textures\petites\herbe";
                    break;
                case TypeCase.herbeDeco:
                    nomTexture = @"Textures\petites\herbeDeco";
                    break;
                case TypeCase.herbeFoncee:
                    nomTexture = @"Textures\petites\herbeFoncee";
                    break;
                case TypeCase.herbeH:
                    nomTexture = @"Textures\petites\herbeH";
                    break;
                case TypeCase.tapisRougeBC:
                    nomTexture = @"Textures\petites\tapisRougeBC";
                    break;
                case TypeCase.terre:
                    nomTexture = @"Textures\petites\terre";
                    break;
                case TypeCase.finMurBas:
                    nomTexture = @"Textures\petites\finMurBas";
                    break;
                case TypeCase.finMurHaut:
                    nomTexture = @"Textures\petites\finMurHaut";
                    break;
                case TypeCase.eau:
                    nomTexture = @"Textures\petites\eau";
                    break;
                case TypeCase.caisse:
                    nomTexture = @"Textures\petites\caisse";
                    break;
                case TypeCase.chaiseGauche:
                    nomTexture = @"Textures\petites\chaiseGauche";
                    break;
                case TypeCase.chaiseDroite:
                    nomTexture = @"Textures\petites\chaiseDroite";
                    break;







                case TypeCase.Joueur1:
                    nomTexture = "origine_hero1";
                    break;
                case TypeCase.Joueur2:
                    nomTexture = "origine_hero2";
                    break;
                case TypeCase.Garde:
                    nomTexture = "origine_garde";
                    break;
                case TypeCase.Patrouilleur:
                    nomTexture = "origine_patrouilleur";
                    break;
                case TypeCase.Patrouilleur_a_cheval:
                    nomTexture = "origine_patrouilleur_a_cheval";
                    break;
                case TypeCase.Boss:
                    nomTexture = "origine_boss";
                    break;
                case TypeCase.Statues:
                    nomTexture = "origine_statue";
                    break;
            }
            texture = content.Load<Texture2D>(nomTexture);
        }

        public void DrawInGame(GameTime gameTime, SpriteBatch spriteBatch, ContentManager content)
        {
            LoadContent(content);

            if (type == TypeCase.eau)
            {
                if (temps < 800)
                    spriteBatch.Draw(texture, position, null, Color.White, 0, Vector2.Zero,1, SpriteEffects.None, 1);
                else if (temps < 1500)
                    spriteBatch.Draw(texture, position + new Vector2(28, 0), null, Color.White, MathHelper.Pi / 2, Vector2.Zero,1, SpriteEffects.None, 1);
                else if (temps < 2500)
                    spriteBatch.Draw(texture, position + new Vector2(28, 28), null, Color.White, MathHelper.Pi, Vector2.Zero, 1, SpriteEffects.None, 1);
                else if (temps < 4000)
                    spriteBatch.Draw(texture, position + new Vector2(0, 28), null, Color.White, -MathHelper.Pi / 2, Vector2.Zero, 1, SpriteEffects.None, 1);
                else
                {
                    spriteBatch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    temps = 0;
                }

                temps += gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else
                spriteBatch.Draw(texture, position, Color.White);
        }

        public void DrawInMapEditor(SpriteBatch spriteBatch, ContentManager content)
        {
            LoadContent(content);
            spriteBatch.Draw(texture, 28 * position, Color.White);
        }

        public void DrawInMenu(SpriteBatch spriteBatch, ContentManager content, Vector2 origine)
        {
            LoadContent(content);
            spriteBatch.Draw(texture, 0.1f * 28 *  new Vector2(position.X, position.Y) + new Vector2(origine.X, origine.Y), null, Color.White, 0, Vector2.Zero, 0.2f, SpriteEffects.None, 0);
        }
    }

   public enum TypeCase // Valeur strictement positive pour les cases franchissables, negative sinon.
   {
       arbre = -1,
       canape = -2,
       porteFenetre = -3,
       grdSiege = -4,

       buissonSurHerbe = -14,
       coinbotdroit = -15,
       coinbotgauche = -16,
       cointopdroit = -17,
       cointopgauche = -18,
       finMurDroit = -19,
       finMurGauche = -20,
       fondNoir = -21,
       piedMurBois = -22,
       eau = -23,
       finMurBas = -24,
       caisse = -25,
       chaiseGauche = -26,
       chaiseDroite = -27,

       commode = -50,
       lit = -51,
       mur = -52,
       murBlanc = -53,
       murBlancDrap = -54,
       murBlancEpee = -55,
       murBlancTableau = -56,
       murEpee = -57,
       murTableau = -58,
       tableauMurBlanc = -59,
       tableMoyenne = -60,
       canapeRalonge = -61,
       fenetre = -62,
       porte = -63,
       rocher = -64,

       grandeTable = -75,
       grandeTableDeco = -76,

       nvlHerbe = 50,
       parquet = 51,
       parquetArbre = 52,
       parquetBuisson = 53,
       pillier = 54,

       bois = 5,
       boisCarre = 6,
       boisDeco = 7,
       carlageNoir = 8,
       carlageNoirDeco = 9,
       herbe = 10,
       herbeDeco = 11,
       herbeFoncee = 12,
       herbeH = 13,
       tapisRougeBC = 14,
       terre = 15,
       finMurHaut = 17,

       pont1 = 18,
       pont2 = 19,
       bibliotheque = 20,

       Joueur1 = 100,
       Joueur2 = 101,
       Garde = 102,
       Patrouilleur = 103,
       Patrouilleur_a_cheval = 104,
       Boss = 105,
       Statues = 106,
       Dark_Hero = 107,

       BonusShurikens = 200,
       BonusHadokens = 201,
       BonusCheckPoint = 202,

       Interrupteur = 210,
       Gomme = 211
   }
}