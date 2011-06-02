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
        List<Vector2> _originesGarde, _originesBoss, _originesStatues;
        List<Bonus> bonus;
        List<List<Vector2>> _originesPatrouilleur, _originesPatrouilleur_a_cheval;
        public int Salaire { get; private set; }

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
            bonus = new List<Bonus>();
        }

        public void Initialisation(Vector2 size)
        {
            for (int y = 0; y < size.Y; y++)
                for (int x = 0; x < size.X; x++)
                    _case[y, x] = new Case(new Vector2(x, y), TypeCase.herbe, new Vector3(1,1,1));
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
            string banana = null;
            string[] dessert = null;

            for (int y = 0; y < Taille_Map.HAUTEUR_MAP; y++)
            {
                banana = file.ReadLine();
                if (banana == "")
                    banana = file.ReadLine();
                else if (banana == null)
                    break;
                for (int x = 0; x < Taille_Map.LARGEUR_MAP; x++)
                    Switch(string.Concat(banana[2 * x].ToString(), banana[2 * x + 1].ToString()), x, y);
            }

            banana = file.ReadLine();
            banana = file.ReadLine();
            dessert = banana.Split(',');
            origineJoueur1.X = Convert.ToInt32(dessert[0]);
            origineJoueur1.Y = Convert.ToInt32(dessert[1]);

            if (nomDeFichier[nomDeFichier.Length - 1] == 'p') // Si la carte est en cooperation.
            {
                banana = file.ReadLine();
                dessert = banana.Split(',');
                origineJoueur2.X = Convert.ToInt32(dessert[0]);
                origineJoueur2.Y = Convert.ToInt32(dessert[1]);
            }

            banana = file.ReadLine();
            banana = file.ReadLine();

            while (banana != "Patrouilleurs" && banana != null)
            {
                dessert = banana.Split(',');
                positionTemporaire.X = Convert.ToInt32(dessert[0]);
                positionTemporaire.Y = Convert.ToInt32(dessert[1]);
                _originesGarde.Add(positionTemporaire);
                banana = file.ReadLine();
            }

            banana = file.ReadLine();

            while (banana != "Patrouilleurs A Cheval" && banana != null)
            {
                banana = file.ReadLine();
                dessert = banana.Split(',');
                _originesPatrouilleur.Add(new List<Vector2>());

                while (banana != "New" && banana != "Patrouilleurs A Cheval")
                {
                    dessert = banana.Split(',');
                    positionTemporaire.X = Convert.ToInt32(dessert[0]);
                    positionTemporaire.Y = Convert.ToInt32(dessert[1]);
                    _originesPatrouilleur[_originesPatrouilleur.Count - 1].Add(positionTemporaire);
                    banana = file.ReadLine();
                }
            }

            banana = file.ReadLine();

            while (banana != "Boss" && banana != null)
            {
                banana = file.ReadLine();
                dessert = banana.Split(',');
                _originesPatrouilleur_a_cheval.Add(new List<Vector2>());

                while (banana != "New" && banana != "Boss")
                {
                    dessert = banana.Split(',');
                    positionTemporaire.X = Convert.ToInt32(dessert[0]);
                    positionTemporaire.Y = Convert.ToInt32(dessert[1]);
                    _originesPatrouilleur_a_cheval[_originesPatrouilleur_a_cheval.Count - 1].Add(positionTemporaire);
                    banana = file.ReadLine();
                }
            }

            banana = file.ReadLine();

            while (banana != "Statue dragon" && banana != null)
            {
                dessert = banana.Split(',');
                positionTemporaire.X = Convert.ToInt32(dessert[0]);
                positionTemporaire.Y = Convert.ToInt32(dessert[1]);
                _originesBoss.Add(positionTemporaire);
                banana = file.ReadLine();
            }

            banana = file.ReadLine();

            while (banana != "Bonus" && banana != null)
            {
                dessert = banana.Split(',');
                positionTemporaire.X = Convert.ToInt32(dessert[0]);
                positionTemporaire.Y = Convert.ToInt32(dessert[1]);
                _originesStatues.Add(positionTemporaire);
                rotationsDesStatues.Add(Convert.ToByte(dessert[2]));
                banana = file.ReadLine();
            }
            
            banana = file.ReadLine();

            while (banana != "Salaire")
            {
                dessert = banana.Split(',');
                positionTemporaire.X = Convert.ToInt32(dessert[0]);
                positionTemporaire.Y = Convert.ToInt32(dessert[1]);
                bonus.Add(new Bonus(28 * positionTemporaire, (TypeBonus)(Convert.ToInt32(dessert[2]))));
                banana = file.ReadLine();
            }

            banana = file.ReadLine();

            Salaire = Convert.ToInt32(banana);
            file.Close();
        }

        public void Switch(string s, int x, int y)
        {
            switch (s)
            {
                case "a1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.arbre, new Vector3(2, 3, 1));
                    break;
                case "a2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.arbre, new Vector3(2, 3, 2));
                    break;
                case "a3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.arbre, new Vector3(2, 3, 3));
                    break;
                case "a4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.arbre, new Vector3(2, 3, 4));
                    break;
                case "a5":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.arbre, new Vector3(2, 3, 5));
                    break;
                case "a6":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.arbre, new Vector3(2, 3, 6));
                    break;

                case "u1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.canape, new Vector3(2, 3, 1));
                    break;
                case "u2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.canape, new Vector3(2, 3, 2));
                    break;
                case "u3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.canape, new Vector3(2, 3, 3));
                    break;
                case "u4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.canape, new Vector3(2, 3, 4));
                    break;
                case "u5":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.canape, new Vector3(2, 3, 5));
                    break;
                case "u6":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.canape, new Vector3(2, 3, 6));
                    break;

                case "x1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.porteFenetre, new Vector3(2, 3, 1));
                    break;
                case "x2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.porteFenetre, new Vector3(2, 3, 2));
                    break;
                case "x3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.porteFenetre, new Vector3(2, 3, 3));
                    break;
                case "x4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.porteFenetre, new Vector3(2, 3, 4));
                    break;
                case "x5":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.porteFenetre, new Vector3(2, 3, 5));
                    break;
                case "x6":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.porteFenetre, new Vector3(2, 3, 6));
                    break;

                case "y1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grdSiege, new Vector3(2, 3, 1));
                    break;
                case "y2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grdSiege, new Vector3(2, 3, 2));
                    break;
                case "y3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grdSiege, new Vector3(2, 3, 3));
                    break;
                case "y4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grdSiege, new Vector3(2, 3, 4));
                    break;
                case "y5":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grdSiege, new Vector3(2, 3, 5));
                    break;
                case "y6":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grdSiege, new Vector3(2, 3, 6));
                    break;








                case "s1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pont1, new Vector3(2, 2, 1));
                    break;
                case "s2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pont1, new Vector3(2, 2, 2));
                    break;

                case "C1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.bibliotheque, new Vector3(1, 2, 1));
                    break;
                case "C2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.bibliotheque, new Vector3(1, 2, 2));
                    break;







                case "t1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pont2, new Vector3(4, 2, 1));
                    break;
                case "t2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pont2, new Vector3(4, 2, 2));
                    break;
                case "t3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pont2, new Vector3(4, 2, 3));
                    break;
                case "t4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pont2, new Vector3(4, 2, 4));
                    break;
                case "t5":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pont2, new Vector3(4, 2, 5));
                    break;
                case "t6":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pont2, new Vector3(4, 2, 6));
                    break;
                case "t7":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pont2, new Vector3(4, 2, 7));
                    break;
                case "t8":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pont2, new Vector3(4, 2, 8));
                    break;









                case "b1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.commode, new Vector3(2, 2, 1));
                    break;
                case "b2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.commode, new Vector3(2, 2, 2));
                    break;
                case "b3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.commode, new Vector3(2, 2, 3));
                    break;
                case "b4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.commode, new Vector3(2, 2, 4));
                    break;


                case "c1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.lit, new Vector3(2, 2, 1));
                    break;
                case "c2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.lit, new Vector3(2, 2, 2));
                    break;
                case "c3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.lit, new Vector3(2, 2, 3));
                    break;
                case "c4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.lit, new Vector3(2, 2, 4));
                    break;

                case "d1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.mur, new Vector3(2, 2, 1));
                    break;
                case "d2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.mur, new Vector3(2, 2, 2));
                    break;
                case "d3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.mur, new Vector3(2, 2, 3));
                    break;
                case "d4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.mur, new Vector3(2, 2, 4));
                    break;

                case "e1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlanc, new Vector3(2, 2, 1));
                    break;
                case "e2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlanc, new Vector3(2, 2, 2));
                    break;
                case "e3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlanc, new Vector3(2, 2, 3));
                    break;
                case "e4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlanc, new Vector3(2, 2, 4));
                    break;

                case "f1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancDrap, new Vector3(2, 2, 1));
                    break;
                case "f2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancDrap, new Vector3(2, 2, 2));
                    break;
                case "f3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancDrap, new Vector3(2, 2, 3));
                    break;
                case "f4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancDrap, new Vector3(2, 2, 4));
                    break;

                case "g1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancEpee, new Vector3(2, 2, 1));
                    break;
                case "g2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancEpee, new Vector3(2, 2, 2));
                    break;
                case "g3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancEpee, new Vector3(2, 2, 3));
                    break;
                case "g4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancEpee, new Vector3(2, 2, 4));
                    break;

                case "h1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancTableau, new Vector3(2, 2, 1));
                    break;
                case "h2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancTableau, new Vector3(2, 2, 2));
                    break;
                case "h3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancTableau, new Vector3(2, 2, 3));
                    break;
                case "h4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancTableau, new Vector3(2, 2, 4));
                    break;

                case "i1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murEpee, new Vector3(2, 2, 1));
                    break;
                case "i2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murEpee, new Vector3(2, 2, 2));
                    break;
                case "i3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murEpee, new Vector3(2, 2, 3));
                    break;
                case "i4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murEpee, new Vector3(2, 2, 4));
                    break;

                case "j1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murTableau, new Vector3(2, 2, 1));
                    break;
                case "j2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murTableau, new Vector3(2, 2, 2));
                    break;
                case "j3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murTableau, new Vector3(2, 2, 3));
                    break;
                case "j4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murTableau, new Vector3(2, 2, 4));
                    break;

                case "k1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tableauMurBlanc, new Vector3(2, 2, 1));
                    break;
                case "k2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tableauMurBlanc, new Vector3(2, 2, 2));
                    break;
                case "k3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tableauMurBlanc, new Vector3(2, 2, 3));
                    break;
                case "k4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tableauMurBlanc, new Vector3(2, 2, 4));
                    break;

                case "l1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tableMoyenne, new Vector3(2, 2, 1));
                    break;
                case "l2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tableMoyenne, new Vector3(2, 2, 2));
                    break;
                case "l3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tableMoyenne, new Vector3(2, 2, 3));
                    break;
                case "l4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tableMoyenne, new Vector3(2, 2, 4));
                    break;

                case "o1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.nvlHerbe, new Vector3(2, 2, 1));
                    break;
                case "o2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.nvlHerbe, new Vector3(2, 2, 2));
                    break;
                case "o3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.nvlHerbe, new Vector3(2, 2, 3));
                    break;
                case "o4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.nvlHerbe, new Vector3(2, 2, 4));
                    break;

                case "p1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquet, new Vector3(2, 2, 1));
                    break;
                case "p2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquet, new Vector3(2, 2, 2));
                    break;
                case "p3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquet, new Vector3(2, 2, 3));
                    break;
                case "p4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquet, new Vector3(2, 2, 4));
                    break;

                case "q1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquetArbre, new Vector3(2, 2, 1));
                    break;
                case "q2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquetArbre, new Vector3(2, 2, 2));
                    break;
                case "q3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquetArbre, new Vector3(2, 2, 3));
                    break;
                case "q4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquetArbre, new Vector3(2, 2, 4));
                    break;

                case "r1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquetBuisson, new Vector3(2, 2, 1));
                    break;
                case "r2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquetBuisson, new Vector3(2, 2, 2));
                    break;
                case "r3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquetBuisson, new Vector3(2, 2, 3));
                    break;
                case "r4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquetBuisson, new Vector3(2, 2, 4));
                    break;

                case "v1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.canapeRalonge, new Vector3(2, 2, 1));
                    break;
                case "v2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.canapeRalonge, new Vector3(2, 2, 2));
                    break;
                case "v3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.canapeRalonge, new Vector3(2, 2, 3));
                    break;
                case "v4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.canapeRalonge, new Vector3(2, 2, 4));
                    break;

                case "w1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.fenetre, new Vector3(2, 2, 1));
                    break;
                case "w2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.fenetre, new Vector3(2, 2, 2));
                    break;
                case "w3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.fenetre, new Vector3(2, 2, 3));
                    break;
                case "w4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.fenetre, new Vector3(2, 2, 4));
                    break;

                case "z1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pillier, new Vector3(2, 2, 1));
                    break;
                case "z2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pillier, new Vector3(2, 2, 2));
                    break;
                case "z3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pillier, new Vector3(2, 2, 3));
                    break;
                case "z4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pillier, new Vector3(2, 2, 4));
                    break;

                case "A1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.porte, new Vector3(2, 2, 1));
                    break;
                case "A2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.porte, new Vector3(2, 2, 2));
                    break;
                case "A3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.porte, new Vector3(2, 2, 3));
                    break;
                case "A4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.porte, new Vector3(2, 2, 4));
                    break;

                case "B1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.rocher, new Vector3(2, 2, 1));
                    break;
                case "B2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.rocher, new Vector3(2, 2, 2));
                    break;
                case "B3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.rocher, new Vector3(2, 2, 3));
                    break;
                case "B4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.rocher, new Vector3(2, 2, 4));
                    break;






                case "m1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, new Vector3(3, 3, 1));
                    break;
                case "m2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, new Vector3(3, 3, 2));
                    break;
                case "m3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, new Vector3(3, 3, 3));
                    break;
                case "m4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, new Vector3(3, 3, 4));
                    break;
                case "m5":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, new Vector3(3, 3, 5));
                    break;
                case "m6":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, new Vector3(3, 3, 6));
                    break;
                case "m7":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, new Vector3(3, 3, 7));
                    break;
                case "m8":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, new Vector3(3, 3, 8));
                    break;
                case "m9":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, new Vector3(3, 3, 9));
                    break;

                case "n1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, new Vector3(3, 3, 1));
                    break;
                case "n2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, new Vector3(3, 3, 2));
                    break;
                case "n3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, new Vector3(3, 3, 3));
                    break;
                case "n4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, new Vector3(3, 3, 4));
                    break;
                case "n5":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, new Vector3(3, 3, 5));
                    break;
                case "n6":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, new Vector3(3, 3, 6));
                    break;
                case "n7":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, new Vector3(3, 3, 7));
                    break;
                case "n8":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, new Vector3(3, 3, 8));
                    break;
                case "n9":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, new Vector3(3, 3, 9));
                    break;













                case ("a7"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.buissonSurHerbe, new Vector3(1, 1, 1));
                    break;
                case ("a8"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.coinbotdroit, new Vector3(1, 1, 1));
                    break;
                case ("a9"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.coinbotgauche, new Vector3(1, 1, 1));
                    break;
                case ("a0"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.cointopdroit, new Vector3(1, 1, 1));
                    break;
                case ("b0"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.cointopgauche, new Vector3(1, 1, 1));
                    break;
                case ("b5"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.finMurDroit, new Vector3(1, 1, 1));
                    break;
                case ("b6"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.finMurGauche, new Vector3(1, 1, 1));
                    break;
                case ("b7"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.fondNoir, new Vector3(1, 1, 1));
                    break;
                case ("b8"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.piedMurBois, new Vector3(1, 1, 1));
                    break;
                case ("b9"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.bois, new Vector3(1, 1, 1));
                    break;
                case ("c0"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.boisCarre, new Vector3(1, 1, 1));
                    break;
                case ("c5"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.boisDeco, new Vector3(1, 1, 1));
                    break;
                case ("c6"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.carlageNoir, new Vector3(1, 1, 1));
                    break;
                case ("c7"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.carlageNoirDeco, new Vector3(1, 1, 1));
                    break;
                case ("c8"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.herbe, new Vector3(1, 1, 1));
                    break;
                case ("c9"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.herbeDeco, new Vector3(1, 1, 1));
                    break;
                case ("d0"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.herbeFoncee, new Vector3(1, 1, 1));
                    break;
                case ("d5"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.herbeH, new Vector3(1, 1, 1));
                    break;
                case ("d6"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tapisRougeBC, new Vector3(1, 1, 1));
                    break;
                case ("d7"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.terre, new Vector3(1, 1, 1));
                    break;
                case ("d8"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.finMurBas, new Vector3(1, 1, 1));
                    break;
                case ("d9"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.finMurHaut, new Vector3(1, 1, 1));
                    break;
                case "e5":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.eau, new Vector3(1, 1, 1));
                    break;
            }
        }

        public void DrawInGame(GameTime gameTime, SpriteBatch spriteBatch, ContentManager content, Rectangle camera)
        {
            for (int y = camera.Y / 28 + camera.Height; y >= camera.Y / 28 - 2; y--)
                if (y < 60 && y >= 0)
                    for (int x = camera.X / 28 + camera.Width; x >= camera.X / 28 - 2; x--)
                        if (x < 80 && x >= 0)
                        {
                            _case[y, x].Position = 28 * new Vector2(x, y) - new Vector2(camera.X, camera.Y);
                            _case[y, x].DrawInGame(gameTime, spriteBatch, content);
                        }
        }

        public void DrawInMapEditor(SpriteBatch spriteBatch, ContentManager content, Rectangle camera)
        {
            for (int y = camera.Y + camera.Height - 1; y >= 0; y--)
                for (int x = camera.X + camera.Width - 1; x >= 0; x--)
                {
                    _case[y, x].Position = new Vector2(x - camera.X + 2, y - camera.Y);
                    _case[y, x].DrawInMapEditor(spriteBatch, content);
                }
        }

        public void DrawInMenu(SpriteBatch spriteBatch, ContentManager content, Vector2 origine)
        {
            for (int y = Taille_Map.HAUTEUR_MAP - 1; y >= 0; y -= 2)
                for (int x = Taille_Map.LARGEUR_MAP - 1; x >= 0; x -= 2)
                {
                    _case[y, x].Position = new Vector2(x, y);
                    _case[y, x].DrawInMenu(spriteBatch, content, origine);
                }
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

        public List<Bonus> Bonus
        {
            get { return bonus; }
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