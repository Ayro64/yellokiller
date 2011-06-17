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
        Vector2 _origineDark_Hero;
        List<Bonus> bonus;
        List<List<Vector2>> _originesPatrouilleur, _originesPatrouilleur_a_cheval;
        List<Interrupteur> interrupteurs;
        public int Salaire { get; private set; }
        int[] munitions;

        public Carte(Vector2 size)
        {
            _case = new Case[(int)size.Y, (int)size.X];
            origineJoueur1 = -Vector2.One;
            origineJoueur2 = -Vector2.One;
            positionTemporaire = Vector2.Zero;
            _originesGarde = new List<Vector2>();
            _originesPatrouilleur = new List<List<Vector2>>();
            _originesPatrouilleur_a_cheval = new List<List<Vector2>>();
            _origineDark_Hero = -Vector2.One;
            _originesBoss = new List<Vector2>();
            _originesStatues = new List<Vector2>();
            rotationsDesStatues = new List<byte>();
            bonus = new List<Bonus>();
            interrupteurs = new List<Interrupteur>();
            munitions = new int[8];
        }

        public void Initialisation(Vector2 size)
        {
            for (int y = 0; y < size.Y; y++)
                for (int x = 0; x < size.X; x++)
                    _case[y, x] = new Case(new Vector2(x, y), TypeCase.herbe, 1);
        }

        public void OuvrirCartePourMenu(string nomDeFichier)
        {
            StreamReader file;

            if (File.Exists("Story\\" + nomDeFichier))
                file = new StreamReader("Story\\" + nomDeFichier);
            else
                file = new StreamReader("Levels\\" + nomDeFichier);

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

        public void OuvrirCarte(string nomDeFichier, ContentManager content)
        {
            StreamReader file;

            if (File.Exists("Story\\" + nomDeFichier))
                file = new StreamReader("Story\\" + nomDeFichier);
            else
                file = new StreamReader("Levels\\" + nomDeFichier);

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
            munitions[0] = Convert.ToInt32(dessert[2]);
            munitions[1] = Convert.ToInt32(dessert[3]);
            munitions[2] = Convert.ToInt32(dessert[4]);
            munitions[3] = Convert.ToInt32(dessert[5]);


            if (nomDeFichier[nomDeFichier.Length - 1] == 'p') // Si la carte est en cooperation.
            {
                banana = file.ReadLine();
                dessert = banana.Split(',');
                origineJoueur2.X = Convert.ToInt32(dessert[0]);
                origineJoueur2.Y = Convert.ToInt32(dessert[1]);
                munitions[4] = Convert.ToInt32(dessert[2]);
                munitions[5] = Convert.ToInt32(dessert[3]);
                munitions[6] = Convert.ToInt32(dessert[4]);
                munitions[7] = Convert.ToInt32(dessert[5]);
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


            while (banana != "Dark Hero" && banana != null)
            {
                banana = file.ReadLine();
                dessert = banana.Split(',');
                _originesPatrouilleur_a_cheval.Add(new List<Vector2>());

                while (banana != "New" && banana != "Dark Hero")
                {
                    dessert = banana.Split(',');
                    positionTemporaire.X = Convert.ToInt32(dessert[0]);
                    positionTemporaire.Y = Convert.ToInt32(dessert[1]);
                    _originesPatrouilleur_a_cheval[_originesPatrouilleur_a_cheval.Count - 1].Add(positionTemporaire);
                    banana = file.ReadLine();
                }
            }

            banana = file.ReadLine();

            while (banana != "Boss" && banana != null)
            {
                dessert = banana.Split(',');
                positionTemporaire.X = Convert.ToInt32(dessert[0]);
                positionTemporaire.Y = Convert.ToInt32(dessert[1]);
                _origineDark_Hero = positionTemporaire;
                banana = file.ReadLine();
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

            while (banana != "Interrupteurs")
            {
                dessert = banana.Split(',');
                positionTemporaire.X = Convert.ToInt32(dessert[0]);
                positionTemporaire.Y = Convert.ToInt32(dessert[1]);
                bonus.Add(new Bonus(28 * positionTemporaire, (TypeBonus)(Convert.ToInt32(dessert[2]))));
                banana = file.ReadLine();
            }

            banana = file.ReadLine();

            while (banana != "Salaire")
            {
                dessert = banana.Split(',');
                positionTemporaire.X = Convert.ToInt32(dessert[0]);
                positionTemporaire.Y = Convert.ToInt32(dessert[1]);
                interrupteurs.Add(new Interrupteur(positionTemporaire));
                positionTemporaire.X = Convert.ToInt32(dessert[2]);
                positionTemporaire.Y = Convert.ToInt32(dessert[3]);
                interrupteurs[interrupteurs.Count - 1].PortePosition = positionTemporaire;
                interrupteurs[interrupteurs.Count - 1].rotation = Convert.ToByte(dessert[4]);
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
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.arbre, 1);
                    break;
                case "a2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.arbre, 2);
                    break;
                case "a3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.arbre, 3);
                    break;
                case "a4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.arbre, 4);
                    break;
                case "a5":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.arbre, 5);
                    break;
                case "a6":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.arbre, 6);
                    break;

                case "u1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.canape, 1);
                    break;
                case "u2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.canape, 2);
                    break;
                case "u3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.canape, 3);
                    break;
                case "u4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.canape, 4);
                    break;
                case "u5":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.canape, 5);
                    break;
                case "u6":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.canape, 6);
                    break;

                case "x1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.porteFenetre, 1);
                    break;
                case "x2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.porteFenetre, 2);
                    break;
                case "x3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.porteFenetre, 3);
                    break;
                case "x4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.porteFenetre, 4);
                    break;
                case "x5":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.porteFenetre, 5);
                    break;
                case "x6":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.porteFenetre, 6);
                    break;

                case "y1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grdSiege, 1);
                    break;
                case "y2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grdSiege, 2);
                    break;
                case "y3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grdSiege, 3);
                    break;
                case "y4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grdSiege, 4);
                    break;
                case "y5":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grdSiege, 5);
                    break;
                case "y6":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grdSiege, 6);
                    break;








                case "s1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pont1, 1);
                    break;
                case "s2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pont1, 2);
                    break;

                case "C1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.bibliotheque, 1);
                    break;
                case "C2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.bibliotheque, 2);
                    break;







                case "t1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pont2, 1);
                    break;
                case "t2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pont2, 2);
                    break;
                case "t3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pont2, 3);
                    break;
                case "t4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pont2, 4);
                    break;
                case "t5":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pont2, 5);
                    break;
                case "t6":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pont2, 6);
                    break;
                case "t7":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pont2, 7);
                    break;
                case "t8":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pont2, 8);
                    break;









                case "b1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.commode, 1);
                    break;
                case "b2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.commode, 2);
                    break;
                case "b3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.commode, 3);
                    break;
                case "b4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.commode, 4);
                    break;


                case "c1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.lit, 1);
                    break;
                case "c2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.lit, 2);
                    break;
                case "c3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.lit, 3);
                    break;
                case "c4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.lit, 4);
                    break;

                case "d1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.mur, 1);
                    break;
                case "d2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.mur, 2);
                    break;
                case "d3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.mur, 3);
                    break;
                case "d4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.mur, 4);
                    break;

                case "e1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlanc, 1);
                    break;
                case "e2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlanc, 2);
                    break;
                case "e3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlanc, 3);
                    break;
                case "e4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlanc, 4);
                    break;

                case "f1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancDrap, 1);
                    break;
                case "f2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancDrap, 2);
                    break;
                case "f3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancDrap, 3);
                    break;
                case "f4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancDrap, 4);
                    break;

                case "g1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancEpee, 1);
                    break;
                case "g2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancEpee, 2);
                    break;
                case "g3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancEpee, 3);
                    break;
                case "g4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancEpee, 4);
                    break;

                case "h1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancTableau, 1);
                    break;
                case "h2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancTableau, 2);
                    break;
                case "h3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancTableau, 3);
                    break;
                case "h4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murBlancTableau, 4);
                    break;

                case "i1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murEpee, 1);
                    break;
                case "i2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murEpee, 2);
                    break;
                case "i3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murEpee, 3);
                    break;
                case "i4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murEpee, 4);
                    break;

                case "j1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murTableau, 1);
                    break;
                case "j2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murTableau, 2);
                    break;
                case "j3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murTableau, 3);
                    break;
                case "j4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.murTableau, 4);
                    break;

                case "k1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tableauMurBlanc, 1);
                    break;
                case "k2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tableauMurBlanc, 2);
                    break;
                case "k3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tableauMurBlanc, 3);
                    break;
                case "k4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tableauMurBlanc, 4);
                    break;

                case "l1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tableMoyenne, 1);
                    break;
                case "l2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tableMoyenne, 2);
                    break;
                case "l3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tableMoyenne, 3);
                    break;
                case "l4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tableMoyenne, 4);
                    break;

                case "o1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.nvlHerbe, 1);
                    break;
                case "o2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.nvlHerbe, 2);
                    break;
                case "o3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.nvlHerbe, 3);
                    break;
                case "o4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.nvlHerbe, 4);
                    break;

                case "p1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquet, 1);
                    break;
                case "p2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquet, 2);
                    break;
                case "p3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquet, 3);
                    break;
                case "p4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquet, 4);
                    break;

                case "q1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquetArbre, 1);
                    break;
                case "q2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquetArbre, 2);
                    break;
                case "q3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquetArbre, 3);
                    break;
                case "q4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquetArbre, 4);
                    break;

                case "r1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquetBuisson, 1);
                    break;
                case "r2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquetBuisson, 2);
                    break;
                case "r3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquetBuisson, 3);
                    break;
                case "r4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.parquetBuisson, 4);
                    break;

                case "v1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.canapeRalonge, 1);
                    break;
                case "v2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.canapeRalonge, 2);
                    break;
                case "v3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.canapeRalonge, 3);
                    break;
                case "v4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.canapeRalonge, 4);
                    break;

                case "w1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.fenetre, 1);
                    break;
                case "w2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.fenetre, 2);
                    break;
                case "w3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.fenetre, 3);
                    break;
                case "w4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.fenetre, 4);
                    break;

                case "z1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pillier, 1);
                    break;
                case "z2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pillier, 2);
                    break;
                case "z3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pillier, 3);
                    break;
                case "z4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.pillier, 4);
                    break;

                case "A1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.porte, 1);
                    break;
                case "A2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.porte, 2);
                    break;
                case "A3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.porte, 3);
                    break;
                case "A4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.porte, 4);
                    break;

                case "B1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.rocher, 1);
                    break;
                case "B2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.rocher, 2);
                    break;
                case "B3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.rocher, 3);
                    break;
                case "B4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.rocher, 4);
                    break;






                case "m1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, 1);
                    break;
                case "m2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, 2);
                    break;
                case "m3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, 3);
                    break;
                case "m4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, 4);
                    break;
                case "m5":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, 5);
                    break;
                case "m6":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, 6);
                    break;
                case "m7":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, 7);
                    break;
                case "m8":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, 8);
                    break;
                case "m9":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTable, 9);
                    break;

                case "n1":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, 1);
                    break;
                case "n2":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, 2);
                    break;
                case "n3":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, 3);
                    break;
                case "n4":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, 4);
                    break;
                case "n5":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, 5);
                    break;
                case "n6":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, 6);
                    break;
                case "n7":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, 7);
                    break;
                case "n8":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, 8);
                    break;
                case "n9":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.grandeTableDeco, 9);
                    break;













                case ("a7"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.buissonSurHerbe, 1);
                    break;
                case ("a8"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.coinbotdroit, 1);
                    break;
                case ("a9"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.coinbotgauche, 1);
                    break;
                case ("a0"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.cointopdroit, 1);
                    break;
                case ("b0"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.cointopgauche, 1);
                    break;
                case ("b5"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.finMurDroit, 1);
                    break;
                case ("b6"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.finMurGauche, 1);
                    break;
                case ("b7"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.fondNoir, 1);
                    break;
                case ("b8"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.piedMurBois, 1);
                    break;
                case ("b9"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.bois, 1);
                    break;
                case ("c0"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.boisCarre, 1);
                    break;
                case ("c5"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.boisDeco, 1);
                    break;
                case ("c6"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.carlageNoir, 1);
                    break;
                case ("c7"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.carlageNoirDeco, 1);
                    break;
                case ("c8"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.herbe, 1);
                    break;
                case ("c9"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.herbeDeco, 1);
                    break;
                case ("d0"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.herbeFoncee, 1);
                    break;
                case ("d5"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.herbeH, 1);
                    break;
                case ("d6"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.tapisRougeBC, 1);
                    break;
                case ("d7"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.terre, 1);
                    break;
                case ("d8"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.finMurBas, 1);
                    break;
                case ("d9"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.finMurHaut, 1);
                    break;
                case "e5":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.eau, 1);
                    break;
                case ("e6"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.caisse, 1);
                    break;
                case ("e7"):
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.chaiseGauche, 1);
                    break;
                case "e8":
                    _case[y, x] = new Case(28 * new Vector2(x, y), TypeCase.chaiseDroite, 1);
                    break;
            }
        }

        public void DrawInGame(GameTime gameTime, SpriteBatch spriteBatch, ContentManager content, Rectangle camera)
        {
            for (int y = camera.Y / 28 + camera.Height; y >= camera.Y / 28 - 2; y--)
                if (y < 60 && y >= 0)
                    for (int x = camera.X / 28 + camera.Width + 1; x >= camera.X / 28 - 2; x--)
                        if (x < 80 && x >= 0)
                        {
                            _case[y, x].Position = 28 * new Vector2(x, y) - new Vector2(camera.X, camera.Y);
                            _case[y, x].DrawInGame(gameTime, spriteBatch, content);
                        }
        }

        public void DrawInMapEditor(SpriteBatch spriteBatch, ContentManager content, Rectangle camera)
        {
            for (int y = camera.Y + camera.Height - (camera.Y + camera.Height + 1 < 60 ? 0 : 1); y >= camera.Y; y--)
                for (int x = camera.X + camera.Width - (camera.X + camera.Width + 1 < 80 ? 0 : 1); x >= camera.X; x--)
                {
                    _case[y, x].Position = new Vector2(x - camera.X + 1, y - camera.Y);
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

        public Vector2 OrigineDarkHero
        {
            get { return _origineDark_Hero; }
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

        public List<Interrupteur> Interrupteurs
        {
            get { return interrupteurs; }
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

        public int[] Munitions
        {
            get { return munitions; }
        }
    }
}