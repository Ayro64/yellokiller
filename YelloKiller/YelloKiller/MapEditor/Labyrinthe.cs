using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace YelloKiller
{
    static class Labyrinthe
    {
        static Cellule[,] cellules = new Cellule[Taille_Map.LARGEUR_MAP / 2, Taille_Map.HAUTEUR_MAP / 2];

        static int hauteur = 30, largeur = 40;

        public static void CreerLabyrinthe(Carte carte)
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

            for (int y = 0; y < Taille_Map.HAUTEUR_MAP / 2; y++)
            {
                for (int x = 0; x < Taille_Map.LARGEUR_MAP / 2; x++)
                {
                    carte.Cases[2 * y + 1, 2 * x + 1].Type = TypeCase.eau;

                    if (y + 1 >= Taille_Map.HAUTEUR_MAP / 2 || !cellules[x, y].isLinked(cellules[x, y + 1]))
                        carte.Cases[2 * y + 1, 2 * x].Type = TypeCase.eau;

                    if (x + 1 >= Taille_Map.LARGEUR_MAP / 2 || !cellules[x, y].isLinked(cellules[x + 1, y]))
                        carte.Cases[2 * y, 2 * x + 1].Type = TypeCase.eau;
                }
            }
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
    }
}