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
                    for (int j = 2; j < 4; j++)
                        for (int i = 2; i < 4; i++)
                            carte.Cases[4 * y + j, 4 * x + i].Type = TypeCase.murBlanc;

                    if (y + 1 >= hauteur || !cellules[x, y].isLinked(cellules[x, y + 1]))
                        for (int j = 2; j < 4; j++)
                            for (int i = 0; i < 2; i++)
                                carte.Cases[4 * y + j, 4 * x + i].Type = TypeCase.murBlanc;

                    if (x + 1 >= largeur || !cellules[x, y].isLinked(cellules[x + 1, y]))
                        for (int j = 0; j < 2; j++)
                            for (int i = 2; i < 4; i++)
                                carte.Cases[4 * y + j, 4 * x + i].Type = TypeCase.murBlanc;
                }
            }
        }

        public static void CreerLabyrintheTriple(Carte carte)
        {
            hauteur = 10;
            largeur = 13;
            cellules = new Cellule[largeur, hauteur];
            InitialiserLabyrinthe(carte);

            for (int y = 0; y < Taille_Map.HAUTEUR_MAP / 6; y++)
            {
                for (int x = 0; x < Taille_Map.LARGEUR_MAP / 6; x++)
                {
                    for (int j = 3; j < 6; j++)
                        for (int i = 3; i < 6; i++)
                            carte.Cases[6 * y + j, 6 * x + i].Type = TypeCase.fondNoir;

                    if (y + 1 >= hauteur || !cellules[x, y].isLinked(cellules[x, y + 1]))
                        for (int j = 3; j < 6; j++)
                            for (int i = 0; i < 3; i++)
                                carte.Cases[6 * y + j, 6 * x + i].Type = TypeCase.fondNoir;

                    if (x + 1 >= largeur || !cellules[x, y].isLinked(cellules[x + 1, y]))
                        for (int j = 0; j < 3; j++)
                            for (int i = 3; i < 6; i++)
                                carte.Cases[6 * y + j, 6 * x + i].Type = TypeCase.fondNoir;
                }
            }
        }

        public static void CreerLabyrintheQuadruple(Carte carte)
        {
            hauteur = 7;
            largeur = 10;
            cellules = new Cellule[largeur, hauteur];
            InitialiserLabyrinthe(carte);

            for (int y = 0; y < Taille_Map.HAUTEUR_MAP / 8; y++)
            {
                for (int x = 0; x < Taille_Map.LARGEUR_MAP / 8; x++)
                {
                    for (int j = 4; j < 8; j++)
                        for (int i = 4; i < 8; i++)
                            carte.Cases[8 * y + j, 8 * x + i].Type = TypeCase.buissonSurHerbe;

                    if (y + 1 >= hauteur || !cellules[x, y].isLinked(cellules[x, y + 1]))
                        for (int j = 4; j < 8; j++)
                            for (int i = 0; i < 4; i++)
                                carte.Cases[8 * y + j, 8 * x + i].Type = TypeCase.buissonSurHerbe;

                    if (x + 1 >= largeur || !cellules[x, y].isLinked(cellules[x + 1, y]))
                        for (int j = 0; j < 4; j++)
                            for (int i = 4; i < 8; i++)
                                carte.Cases[8 * y + j, 8 * x + i].Type = TypeCase.buissonSurHerbe;
                }
            }
        }

        private static void InitialiserLabyrinthe(Carte carte)
        {
            //carte.Initialisation(new Vector2(Taille_Map.LARGEUR_MAP, Taille_Map.HAUTEUR_MAP));

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
        }

        private static void generate()
        {
            foreach (Cellule cellule in cellules)
                cellule.IsVisited = false;

            Random rand = new Random();
            int x = rand.Next(0, largeur), y = rand.Next(0, hauteur);

            cellules[x, y].IsVisited = true;
            generate(cellules[x, y], rand);
        }

        private static void generate(Cellule c, Random random)
        {
            c.IsVisited = true;
            List<Cellule> CList = new List<Cellule>();

            c.randomizeNeighbors(random);

            foreach (Cellule cellule in c.getNeighbors())
                if (!cellule.IsVisited)
                {
                    c.addLink(cellule);
                    cellule.addLink(c);

                    generate(cellule, random);
                }
        }
    }
}