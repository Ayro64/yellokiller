﻿using System;
using System.Collections.Generic;

namespace YelloKiller
{
    class Cellule
    {
        List<Cellule> neighbors, linked;

        public Cellule()
        {
            neighbors = new List<Cellule>();
            linked = new List<Cellule>();
            IsVisited = false;
        }

        public bool IsVisited
        {
            get;
            set;
        }

        public void addNeighbor(Cellule c)
        {
            neighbors.Add(c);
        }

        public void addLink(Cellule c)
        {
            if (neighbors.Contains(c))
                linked.Add(c);
        }

        public bool isNeighbor(Cellule c)
        {
            return neighbors.Contains(c);
        }

        public bool isLinked(Cellule c)
        {
            return linked.Contains(c);
        }

        public List<Cellule> getNeighbors()
        {
            return neighbors;
        }

        public List<Cellule> getLinked()
        {
            return linked;
        }

        public void randomizeNeighbors(Random rng)
        {
            List<Cellule> resultat = new List<Cellule>();
            int random;

            while(neighbors.Count > 0)
            {
                random = rng.Next(neighbors.Count);
                resultat.Add(neighbors[random]);
                neighbors.RemoveAt(random);
            }

            neighbors = resultat;
        }
    }
}