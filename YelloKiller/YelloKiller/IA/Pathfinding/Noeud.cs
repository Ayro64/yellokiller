using System;
using System.Collections.Generic;

namespace YelloKiller
{
    class Noeud
    {
        public Case Case { get; private set; }
        public Noeud Parent { get; set; }
        public int Manhattan { get; private set; }

        public Noeud(Case _case, Noeud parent, Case destination)
        {
            this.Case = _case;
            this.Parent = parent;
            this.Manhattan = Math.Abs(_case.X - destination.X) + Math.Abs(_case.Y - destination.Y);
        }

        public List<Noeud> NoeudsPossibles(Carte carte, Case destination)
        {
            List<Noeud> result = new List<Noeud>();
            // Bas
            if (CaseValide(Case.X, Case.Y + 1) && carte.Cases[Case.Y + 1, Case.X].Type > 0)
                result.Add(new Noeud(carte.Cases[Case.Y + 1, Case.X], this, destination));
            // Droite
            if (CaseValide(Case.X + 1, Case.Y) && carte.Cases[Case.Y, Case.X + 1].Type > 0)
                result.Add(new Noeud(carte.Cases[Case.Y, Case.X + 1], this, destination));
            // Haut
            if (CaseValide(Case.X, Case.Y - 1) && carte.Cases[Case.Y - 1, Case.X].Type > 0)
                result.Add(new Noeud(carte.Cases[Case.Y - 1, Case.X], this, destination));
            // Gauche
            if (CaseValide(Case.X - 1, Case.Y) && carte.Cases[Case.Y, Case.X - 1].Type > 0)
                result.Add(new Noeud(carte.Cases[Case.Y, Case.X - 1], this, destination));

            return result;
        }

        private bool CaseValide(int x, int y)
        {
            return (x >= 0 && x < Taille_Map.LARGEUR_MAP && y >= 0 && y < Taille_Map.HAUTEUR_MAP);
        }
    }
}
