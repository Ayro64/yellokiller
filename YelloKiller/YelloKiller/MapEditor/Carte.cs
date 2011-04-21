using System;
using System.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YelloKiller
{
    public class Carte
    {
        Case[,] _case;
        Vector2 origineJoueur1, origineJoueur2, positionTemporaire;
        List<byte> rotationsDesStatues;
        List<Vector2> _originesGarde, _originesBoss, _originesStatues, bonusShurikens;
        List<List<Vector2>> _originesPatrouilleur, _originesPatrouilleur_a_cheval;
        int salaire;

        public Carte(Vector2 size)
        {
            _case = new Case[(int)size.Y, (int)size.X];
            origineJoueur1 = -Vector2.One;
            origineJoueur2 = -Vector2.One;
            positionTemporaire = Vector2.Zero;
            _originesGarde = new List<Vector2>();
            _originesPatrouilleur = new List<List<Vector2>>();
            _originesPatrouilleur_a_cheval = new List<List<Vector2>>();
            _originesBoss = new List<Vector2>();
            _originesStatues = new List<Vector2>();
            rotationsDesStatues = new List<byte>();
            bonusShurikens = new List<Vector2>();
        }

        public void Initialisation(Vector2 size)
        {
            for (int y = 0; y < size.Y; y++)
                for (int x = 0; x < size.X; x++)
                    _case[y, x] = new Case(new Vector2(x, y), TypeCase.herbe, new Vector3(1,1,1), true);
        }

        public void OuvrirCartePourMenu(string nomDeFichier)
        {
            StreamReader file = new StreamReader(nomDeFichier);
            string line;

            for (int y = 0; y < Taille_Map.HAUTEUR_MAP; y++)
            {
                line = file.ReadLine();
                if (line == "")
                    line = file.ReadLine();
                else if (line == null)
                    break;
                for (int x = 0; x < Taille_Map.LARGEUR_MAP; x++)
                    Switch(string.Concat(line[2 * x].ToString(), line[2 * x + 1].ToString()), x, y);
            }

            file.Close();
        }

        public void OuvrirCarte(string nomDeFichier)
        {
            StreamReader file = new StreamReader(nomDeFichier);
            string line;

            for (int y = 0; y < Taille_Map.HAUTEUR_MAP; y++)
            {
                line = file.ReadLine();
                if (line == "")
                    line = file.ReadLine();
                else if (line == null)
                    break;
                for (int x = 0; x < Taille_Map.LARGEUR_MAP; x++)
                    Switch(string.Concat(line[2 * x].ToString(), line[2 * x + 1].ToString()), x, y);
            }

            line = file.ReadLine();
            line = file.ReadLine();
            origineJoueur1.X = Convert.ToInt32(line);

            line = file.ReadLine();
            origineJoueur1.Y = Convert.ToInt32(line);

            if (nomDeFichier[0] == 'C') // Si la carte est en cooperation.
            {
                line = file.ReadLine();
                origineJoueur2.X = Convert.ToInt32(line);

                line = file.ReadLine();
                origineJoueur2.Y = Convert.ToInt32(line);
            }

            line = file.ReadLine();
            line = file.ReadLine();

            while (line != "Patrouilleurs" && line != null)
            {
                positionTemporaire.X = Convert.ToInt32(line);
                line = file.ReadLine();
                positionTemporaire.Y = Convert.ToInt32(line);
                _originesGarde.Add(positionTemporaire);
                line = file.ReadLine();
            }

            line = file.ReadLine();

            while (line != "Patrouilleurs A Cheval" && line != null)
            {
                line = file.ReadLine();
                _originesPatrouilleur.Add(new List<Vector2>());

                while (line != "New" && line != "Patrouilleurs A Cheval")
                {
                    positionTemporaire.X = Convert.ToInt32(line);
                    line = file.ReadLine();
                    positionTemporaire.Y = Convert.ToInt32(line);
                    _originesPatrouilleur[_originesPatrouilleur.Count - 1].Add(positionTemporaire);
                    line = file.ReadLine();
                }
            }

            line = file.ReadLine();

            while (line != "Boss" && line != null)
            {
                line = file.ReadLine();
                _originesPatrouilleur_a_cheval.Add(new List<Vector2>());

                while (line != "New" && line != "Boss")
                {
                    positionTemporaire.X = Convert.ToInt32(line);
                    line = file.ReadLine();
                    positionTemporaire.Y = Convert.ToInt32(line);
                    _originesPatrouilleur_a_cheval[_originesPatrouilleur_a_cheval.Count - 1].Add(positionTemporaire);
                    line = file.ReadLine();
                }
            }

            line = file.ReadLine();

            while (line != "Statue dragon" && line != null)
            {
                positionTemporaire.X = Convert.ToInt32(line);
                line = file.ReadLine();
                positionTemporaire.Y = Convert.ToInt32(line);
                _originesBoss.Add(positionTemporaire);
                line = file.ReadLine();
            }

            line = file.ReadLine();

            while (line != "Bonus Shurikens" && line != null)
            {
                positionTemporaire.X = Convert.ToInt32(line);
                line = file.ReadLine();
                positionTemporaire.Y = Convert.ToInt32(line);
                _originesStatues.Add(positionTemporaire);
                line = file.ReadLine();
                rotationsDesStatues.Add(Convert.ToByte(line));
                line = file.ReadLine();
            }
            
            line = file.ReadLine();

            while (line != "Salaire")
            {
                positionTemporaire.X = Convert.ToInt32(line);
                line = file.ReadLine();
                positionTemporaire.Y = Convert.ToInt32(line);
                bonusShurikens.Add(positionTemporaire);
                line = file.ReadLine();
            }

            line = file.ReadLine();

            salaire = Convert.ToInt32(line);
            file.Close();
        }

        public void Switch(string s, int x, int y)
        {
            switch (s)
            {
                case "a1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.arbre, new Vector3(2, 3, 1), false);
                    break;
                case "a2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.arbre, new Vector3(2, 3, 2), false);
                    break;
                case "a3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.arbre, new Vector3(2, 3, 3), false);
                    break;
                case "a4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.arbre, new Vector3(2, 3, 4), false);
                    break;
                case "a5":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.arbre, new Vector3(2, 3, 5), false);
                    break;
                case "a6":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.arbre, new Vector3(2, 3, 6), false);
                    break;






                case "s1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pont1, new Vector3(2, 2, 1), false);
                    break;
                case "s2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pont1, new Vector3(2, 2, 2), false);
                    break;

                case "t1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pont2, new Vector3(2, 2, 1), false);
                    break;
                case "t2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pont2, new Vector3(2, 2, 2), false);
                    break;









                case "b1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.commode, new Vector3(2, 2, 1), false);
                    break;
                case "b2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.commode, new Vector3(2, 2, 2), false);
                    break;
                case "b3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.commode, new Vector3(2, 2, 3), false);
                    break;
                case "b4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.commode, new Vector3(2, 2, 4), false);
                    break;


                case "c1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.lit, new Vector3(2, 2, 1), false);
                    break;
                case "c2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.lit, new Vector3(2, 2, 2), false);
                    break;
                case "c3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.lit, new Vector3(2, 2, 3), false);
                    break;
                case "c4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.lit, new Vector3(2, 2, 4), false);
                    break;

                case "d1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.mur, new Vector3(2, 2, 1), false);
                    break;
                case "d2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.mur, new Vector3(2, 2, 2), false);
                    break;
                case "d3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.mur, new Vector3(2, 2, 3), false);
                    break;
                case "d4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.mur, new Vector3(2, 2, 4), false);
                    break;

                case "e1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlanc, new Vector3(2, 2, 1), false);
                    break;
                case "e2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlanc, new Vector3(2, 2, 2), false);
                    break;
                case "e3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlanc, new Vector3(2, 2, 3), false);
                    break;
                case "e4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlanc, new Vector3(2, 2, 4), false);
                    break;

                case "f1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancDrap, new Vector3(2, 2, 1), false);
                    break;
                case "f2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancDrap, new Vector3(2, 2, 2), false);
                    break;
                case "f3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancDrap, new Vector3(2, 2, 3), false);
                    break;
                case "f4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancDrap, new Vector3(2, 2, 4), false);
                    break;

                case "g1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancEpee, new Vector3(2, 2, 1), false);
                    break;
                case "g2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancEpee, new Vector3(2, 2, 2), false);
                    break;
                case "g3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancEpee, new Vector3(2, 2, 3), false);
                    break;
                case "g4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancEpee, new Vector3(2, 2, 4), false);
                    break;

                case "h1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancTableau, new Vector3(2, 2, 1), false);
                    break;
                case "h2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancTableau, new Vector3(2, 2, 2), false);
                    break;
                case "h3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancTableau, new Vector3(2, 2, 3), false);
                    break;
                case "h4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancTableau, new Vector3(2, 2, 4), false);
                    break;

                case "i1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murEpee, new Vector3(2, 2, 1), false);
                    break;
                case "i2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murEpee, new Vector3(2, 2, 2), false);
                    break;
                case "i3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murEpee, new Vector3(2, 2, 3), false);
                    break;
                case "i4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murEpee, new Vector3(2, 2, 4), false);
                    break;

                case "j1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murTableau, new Vector3(2, 2, 1), false);
                    break;
                case "j2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murTableau, new Vector3(2, 2, 2), false);
                    break;
                case "j3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murTableau, new Vector3(2, 2, 3), false);
                    break;
                case "j4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murTableau, new Vector3(2, 2, 4), false);
                    break;

                case "k1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tableauMurBlanc, new Vector3(2, 2, 1), false);
                    break;
                case "k2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tableauMurBlanc, new Vector3(2, 2, 2), false);
                    break;
                case "k3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tableauMurBlanc, new Vector3(2, 2, 3), false);
                    break;
                case "k4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tableauMurBlanc, new Vector3(2, 2, 4), false);
                    break;

                case "l1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tableMoyenne, new Vector3(2, 2, 1), false);
                    break;
                case "l2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tableMoyenne, new Vector3(2, 2, 2), false);
                    break;
                case "l3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tableMoyenne, new Vector3(2, 2, 3), false);
                    break;
                case "l4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tableMoyenne, new Vector3(2, 2, 4), false);
                    break;

                case "o1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.nvlHerbe, new Vector3(2, 2, 1), false);
                    break;
                case "o2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.nvlHerbe, new Vector3(2, 2, 2), false);
                    break;
                case "o3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.nvlHerbe, new Vector3(2, 2, 3), false);
                    break;
                case "o4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.nvlHerbe, new Vector3(2, 2, 4), false);
                    break;

                case "p1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquet, new Vector3(2, 2, 1), false);
                    break;
                case "p2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquet, new Vector3(2, 2, 2), false);
                    break;
                case "p3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquet, new Vector3(2, 2, 3), false);
                    break;
                case "p4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquet, new Vector3(2, 2, 4), false);
                    break;

                case "q1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquetArbre, new Vector3(2, 2, 1), false);
                    break;
                case "q2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquetArbre, new Vector3(2, 2, 2), false);
                    break;
                case "q3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquetArbre, new Vector3(2, 2, 3), false);
                    break;
                case "q4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquetArbre, new Vector3(2, 2, 4), false);
                    break;

                case "r1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquetBuisson, new Vector3(2, 2, 1), false);
                    break;
                case "r2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquetBuisson, new Vector3(2, 2, 2), false);
                    break;
                case "r3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquetBuisson, new Vector3(2, 2, 3), false);
                    break;
                case "r4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquetBuisson, new Vector3(2, 2, 4), false);
                    break;






                case "m1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, new Vector3(3, 3, 1), false);
                    break;
                case "m2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, new Vector3(3, 3, 2), false);
                    break;
                case "m3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, new Vector3(3, 3, 3), false);
                    break;
                case "m4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, new Vector3(3, 3, 4), false);
                    break;
                case "m5":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, new Vector3(3, 3, 5), false);
                    break;
                case "m6":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, new Vector3(3, 3, 6), false);
                    break;
                case "m7":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, new Vector3(3, 3, 7), false);
                    break;
                case "m8":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, new Vector3(3, 3, 8), false);
                    break;
                case "m9":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, new Vector3(3, 3, 9), false);
                    break;

                case "n1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, new Vector3(3, 3, 1), false);
                    break;
                case "n2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, new Vector3(3, 3, 2), false);
                    break;
                case "n3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, new Vector3(3, 3, 3), false);
                    break;
                case "n4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, new Vector3(3, 3, 4), false);
                    break;
                case "n5":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, new Vector3(3, 3, 5), false);
                    break;
                case "n6":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, new Vector3(3, 3, 6), false);
                    break;
                case "n7":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, new Vector3(3, 3, 7), false);
                    break;
                case "n8":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, new Vector3(3, 3, 8), false);
                    break;
                case "n9":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, new Vector3(3, 3, 9), false);
                    break;













                case ("a7"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.buissonSurHerbe, new Vector3(1, 1, 1), false);
                    break;
                case ("a8"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.coinbotdroit, new Vector3(1, 1, 1), false);
                    break;
                case ("a9"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.coinbotgauche, new Vector3(1, 1, 1), false);
                    break;
                case ("a0"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.cointopdroit, new Vector3(1, 1, 1), false);
                    break;
                case ("b0"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.cointopgauche, new Vector3(1, 1, 1), false);
                    break;
                case ("b5"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.finMurDroit, new Vector3(1, 1, 1), false);
                    break;
                case ("b6"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.finMurGauche, new Vector3(1, 1, 1), false);
                    break;
                case ("b7"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.fondNoir, new Vector3(1, 1, 1), false);
                    break;
                case ("b8"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.piedMurBois, new Vector3(1, 1, 1), false);
                    break;
                case ("b9"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.bois, new Vector3(1, 1, 1), false);
                    break;
                case ("c0"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.boisCarre, new Vector3(1, 1, 1), false);
                    break;
                case ("c5"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.boisDeco, new Vector3(1, 1, 1), false);
                    break;
                case ("c6"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.carlageNoir, new Vector3(1, 1, 1), false);
                    break;
                case ("c7"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.carlageNoirDeco, new Vector3(1, 1, 1), false);
                    break;
                case ("c8"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.herbe, new Vector3(1, 1, 1), false);
                    break;
                case ("c9"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.herbeDeco, new Vector3(1, 1, 1), false);
                    break;
                case ("d0"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.herbeFoncee, new Vector3(1, 1, 1), false);
                    break;
                case ("d5"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.herbeH, new Vector3(1, 1, 1), false);
                    break;
                case ("d6"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tapisRougeBC, new Vector3(1, 1, 1), false);
                    break;
                case ("d7"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.terre, new Vector3(1, 1, 1), false);
                    break;
                case ("d8"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.finMurBas, new Vector3(1, 1, 1), false);
                    break;
                case ("d9"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.finMurHaut, new Vector3(1, 1, 1), false);
                    break;
            }
        }

        public void DrawInGame(SpriteBatch spriteBatch, ContentManager content, Rectangle camera)
        {
            for (int y = camera.Y / 28 + camera.Height; y >= camera.Y / 28 - 2; y--)
            {
                if (y < 60 && y >= 0)
                {
                    for (int x = camera.X / 28 + camera.Width; x >= camera.X / 28 - 2; x--)
                    {
                        if (x < 80 && x >= 0)
                        {
                            _case[y, x].Position = 28 * new Vector2(x, y) - new Vector2(camera.X, camera.Y);

                            _case[y, x].DrawInGame(spriteBatch, content);
                        }
                    }
                }
            }
        }

        public void DrawInMapEditor(SpriteBatch spriteBatch, ContentManager content, Rectangle camera)
        {
            for (int y = camera.Y + camera.Height - 1; y >= 0; y--)
            {
                for (int x = camera.X + camera.Width - 1; x >= 0; x--)
                {
                    _case[y, x].Position = new Vector2(x - camera.X + 2, y - camera.Y);

                    _case[y, x].DrawInMapEditor(spriteBatch, content);
                }
            }
        }

        public void DrawInMenu(SpriteBatch spriteBatch, ContentManager content, Vector2 origine)
        {
            for (int y = Taille_Map.HAUTEUR_MAP - 1; y >= 0; y -= 2)
            {
                for (int x = Taille_Map.LARGEUR_MAP - 1; x >= 0; x -= 2)
                {
                    _case[y, x].Position = new Vector2(x, y);

                    _case[y, x].DrawInMenu(spriteBatch, content, origine);
                }
            }
        }

        public int Salaire
        {
            get { return salaire; }
        }

        public List<Vector2> OriginesGardes
        {
            get { return _originesGarde; }
        }

        public List<List<Vector2>> OriginesPatrouilleurs
        {
            get { return _originesPatrouilleur; }
        }

        public List<List<Vector2>> OriginesPatrouilleursAChevaux
        {
            get { return _originesPatrouilleur_a_cheval; }
        }

        public List<Vector2> OriginesBoss
        {
            get { return _originesBoss; }
        }

        public List<Vector2> OriginesStatues
        {
            get { return _originesStatues; }
        }

        public Vector2 OrigineJoueur1
        {
            get { return origineJoueur1; }
        }

        public Vector2 OrigineJoueur2
        {
            get { return origineJoueur2; }
        }

        public List<Vector2> BonusShuriken
        {
            get { return bonusShurikens; }
        }

        public Case[,] Cases
        {
            get { return _case; }
            set { _case = value; }
        }

        public List<byte> RotationsDesStatues
        {
            get { return rotationsDesStatues; }
        }
    }
}