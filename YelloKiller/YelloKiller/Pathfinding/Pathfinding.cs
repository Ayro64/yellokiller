﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace YelloKiller
{
    class Pathfinding
    {
        public static List<Case> CalculChemin(Carte carte, Vector2 depart, Vector2 arrivee, Rectangle camera)
        {
            List<Case> resultat = new List<Case>();
            ListeNoeuds<Noeud> listeOuverte = new ListeNoeuds<Noeud>();
            ListeNoeuds<Noeud> listeFermee = new ListeNoeuds<Noeud>();
            List<Noeud> NoeudsPossibles;
            int nombreNoeudsPossibles;

            Noeud noeudDepart = new Noeud(depart, null, arrivee, camera);

            listeOuverte.Add(noeudDepart);

            while (listeOuverte.Count > 0)
            {
                Noeud courant = listeOuverte[0];
                listeOuverte.RemoveAt(0);
                listeFermee.Add(courant);

                if (courant.Position == arrivee)
                {
                    List<Case> solution = new List<Case>();
                    while (courant.Parent != null)
                    {
                        solution.Add(carte.Cases[(int)courant.Position.Y, (int)courant.Position.X]);
                        courant = courant.Parent;
                    }
                    return solution;
                }

                NoeudsPossibles = courant.NoeudPossibles(carte, arrivee, camera);
                nombreNoeudsPossibles = NoeudsPossibles.Count;

                for (int i = 0; i < nombreNoeudsPossibles; i++)
                {
                    if (!listeFermee.Contains(NoeudsPossibles[i]))
                    {
                        if (listeOuverte.Contains(NoeudsPossibles[i]))
                        {
                            if (NoeudsPossibles[i].EstimatedMovement < listeOuverte[NoeudsPossibles[i]].EstimatedMovement)
                                listeOuverte[NoeudsPossibles[i]].Parent = courant;
                        }
                        else
                            listeOuverte.DichotomicInsertion(NoeudsPossibles[i]);
                    }
                }
            }

            return null;
        }
    }
}