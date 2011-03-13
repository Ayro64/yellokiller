using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace YelloKiller
{
    class Pathfinding
    {
        public static List<Case> CalculChemin(Carte carte, Case depart, Case arrivee)
        {
            System.Console.WriteLine("Dans pathfinding : Depart X = " + depart.X + " Y = " + depart.Y + " Arrivee X = " + arrivee.X + " Y = " + arrivee.Y);
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

                System.Console.WriteLine("Current X = " + current.Case.X + " Y = " + current.Case.Y);
                if (current.Case.X == arrivee.X && current.Case.Y == arrivee.Y)
                {
                    List<Case> solution = new List<Case>();
                    while (current.Parent != null)
                    {
                        System.Console.WriteLine("Dans boucle de retour Current X = " + current.Case.X + " Y = " + current.Case.Y);
                        solution.Add(current.Case);
                        current = current.Parent;
                    }
                    System.Console.WriteLine("Pathfinding REUSSI AVEC SUCCES");
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