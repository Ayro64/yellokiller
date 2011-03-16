using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace YelloKiller
{
    class Pathfinding
    {
        public static List<Case> CalculChemin(Carte carte, Case depart, Case arrivee)
        {
            List<Case> resultat = new List<Case>();
            NodeList<Noeud> listeOuverte = new NodeList<Noeud>();
            NodeList<Noeud> listeFermee = new NodeList<Noeud>();
            List<Noeud> noeudsPossibles;
            int nombreNoeudsPossibles;

            Noeud premier = new Noeud(depart, null, arrivee);
            
            listeOuverte.Add(premier);

            while (listeOuverte.Count > 0)
            {
                Noeud current = listeOuverte[0];
                listeOuverte.RemoveAt(0);
                listeFermee.Add(current);

                if (current.Case == arrivee)
                {
                    List<Case> solution = new List<Case>();
                    while (current.Parent != null)
                    {
                        solution.Add(current.Case);
                        current = current.Parent;
                    }
                    return solution;
                }

                noeudsPossibles = current.NoeudsPossibles(carte, arrivee);
                nombreNoeudsPossibles = noeudsPossibles.Count;

                for (int i = 0; i < nombreNoeudsPossibles; i++)
                {
                    if (!listeFermee.Contains(noeudsPossibles[i]))
                    {
                        if (listeOuverte.Contains(noeudsPossibles[i]))
                        {
                            if (noeudsPossibles[i].Manhattan < listeOuverte[noeudsPossibles[i]].Manhattan)
                                listeOuverte[noeudsPossibles[i]].Parent = current;
                        }
                        else
                            listeOuverte.DichotomicInsertion(noeudsPossibles[i]);
                    }
                }
            }
            return null;
        }
    }
}