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

            listeOuverte.Add(new Noeud(depart, null, arrivee));

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

                foreach (Noeud voisin in current.NoeudsPossibles(carte, arrivee))
                    if (!listeFermee.Contains(voisin))
                    {
                        if (listeOuverte.Contains(voisin))
                        {
                            if (voisin.Manhattan < listeOuverte[voisin].Manhattan)
                                listeOuverte[voisin].Parent = current;
                        }
                        else
                            listeOuverte.DichotomicInsertion(voisin);
                    }
            }

            return null;
        }
    }
}