using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace YelloKiller
{
    static class Labyrinthe
    {
        static Cellule[,] cellules;

        static int hauteur, largeur;

        public static void CreerLabyrintheSimple(Carte carte)
        {
            hauteur = 30;
            largeur = 40;
            cellules = new Cellule[largeur, hauteur];

            InitialiserLabyrinthe(carte);

            for (int y = 0; y < Taille_Map.HAUTEUR_MAP / 2; y++)
            {
                for (int x = 0; x < Taille_Map.LARGEUR_MAP / 2; x++)
                {
                    carte.Cases[2 * y + 1, 2 * x + 1].Type = TypeCase.eau;

                    if (y + 1 >= hauteur || !cellules[x, y].isLinked(cellules[x, y + 1]))
                        carte.Cases[2 * y + 1, 2 * x].Type = TypeCase.eau;

                    if (x + 1 >= largeur || !cellules[x, y].isLinked(cellules[x + 1, y]))
                        carte.Cases[2 * y, 2 * x + 1].Type = TypeCase.eau;
                }
            }
        }

        public static void CreerLabyrintheDouble(Carte carte)
        {
            hauteur = 15;
            largeur = 20;
            cellules = new Cellule[largeur, hauteur];
            InitialiserLabyrinthe(carte);

            for (int y = 0; y < Taille_Map.HAUTEUR_MAP / 4; y++)
            {
                for (int x = 0; x < Taille_Map.LARGEUR_MAP / 4; x++)
                {
                    carte.Cases[4 * y + 2, 4 * x + 2].Type = TypeCase.mur;
                    carte.Cases[4 * y + 2, 4 * x + 3].Type = TypeCase.mur;
                    carte.Cases[4 * y + 3, 4 * x + 2].Type = TypeCase.mur;
                    carte.Cases[4 * y + 3, 4 * x + 3].Type = TypeCase.mur;

                    if (y + 1 >= hauteur || !cellules[x, y].isLinked(cellules[x, y + 1]))
                    {
                        carte.Cases[4 * y + 2, 4 * x].Type = TypeCase.mur;
                        carte.Cases[4 * y + 3, 4 * x].Type = TypeCase.mur;
                        carte.Cases[4 * y + 2, 4 * x + 1].Type = TypeCase.mur;
                        carte.Cases[4 * y + 3, 4 * x + 1].Type = TypeCase.mur;
                    }

                    if (x + 1 >= largeur || !cellules[x, y].isLinked(cellules[x + 1, y]))
                    {
                        carte.Cases[4 * y, 4 * x + 2].Type = TypeCase.mur;
                        carte.Cases[4 * y, 4 * x + 3].Type = TypeCase.mur;
                        carte.Cases[4 * y + 1, 4 * x + 2].Type = TypeCase.mur;
                        carte.Cases[4 * y + 1, 4 * x + 3].Type = TypeCase.mur;
                    }
                }
            }
        }

        private static void InitialiserLabyrinthe(Carte carte)
        {
            carte.Initialisation(new Vector2(Taille_Map.LARGEUR_MAP, Taille_Map.HAUTEUR_MAP));

            for (int y = 0; y < hauteur; y++)
                for (int x = 0; x < largeur; x++)
                    cellules[x, y] = new Cellule();

            for (int y = 0; y < hauteur; y++)
            {
                for (int x = 0; x < largeur; x++)
                {
                    if (x - 1 >= 0)
                        cellules[x, y].addNeighbor(cellules[x - 1, y]);
                    if (y - 1 >= 0)
                        cellules[x, y].addNeighbor(cellules[x, y - 1]);
                    if (x + 1 < largeur)
                        cellules[x, y].addNeighbor(cellules[x + 1, y]);
                    if (y + 1 < hauteur)
                        cellules[x, y].addNeighbor(cellules[x, y + 1]);
                }
            }

            generate();
            //logLaby();
        }

        private static void generate()
        {
            foreach (Cellule cellule in cellules)
                cellule.IsVisited = false;

            Random rand = new Random();
            int x = rand.Next(0, largeur), y = rand.Next(0, hauteur);

            cellules[x, y].IsVisited = true;
            generate(cellules[x, y]);
        }

        private static void generate(Cellule c)
        {
            c.IsVisited = true;
            List<Cellule> CList = new List<Cellule>();
            Random random = new Random();

            c.randomizeNeighbors(random);

            foreach (Cellule cellule in c.getNeighbors())
                if (!cellule.IsVisited)
                {
                    c.addLink(cellule);
                    cellule.addLink(c);

                    generate(cellule);
                }
        }

        /*private static void logLaby() Creer un fichier dans lequel est enregistré le labyrinthe, m'a servi a mettre en place les fonctions, peut toujours servir
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter("laby.txt");

            for (int x = 0; x < largeur; x++)
                file.Write("._");

            file.Write(".");
            file.WriteLine();

            for (int y = 0; y < hauteur; y++)
            {
                file.Write("|");

                for (int x = 0; x < largeur; x++)
                {
                    if (y + 1 < hauteur && cellules[x, y].isLinked(cellules[x, y + 1]))
                        file.Write(" ");
                    else
                        file.Write("_");

                    if (x + 1 < largeur)
                    {
                        if (cellules[x, y].isLinked(cellules[x + 1, y]))
                            file.Write("_");
                        else
                            file.Write("|");
                    }
                }

                file.Write("|");
                file.WriteLine();
            }

            file.Close();
        }*/
    }
}