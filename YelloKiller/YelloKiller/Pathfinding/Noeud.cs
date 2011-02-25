using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace YelloKiller
{
    class Noeud
    {
        Case _case;
        Noeud parent;
        int manhattan;

        public Noeud(Case _case, Noeud parent, Case destination)
        {
            this._case = _case;
            this.parent = parent;
            this.manhattan = Math.Abs(_case.X - destination.X) + Math.Abs(_case.Y - destination.Y);
        }

        public Case Case
        {
            get { return _case; }
        }

        public Noeud Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public int Manhattan
        {
            get { return manhattan; }
        }

        public List<Noeud> NoeudsPossibles(Carte carte, Case destination)
        {
            List<Noeud> result = new List<Noeud>();
            // Bas
            if (carte.CaseValide(_case.X, _case.Y + 1) && carte.Cases[_case.Y + 1, _case.X].Type > 0)
                result.Add(new Noeud(carte.Cases[_case.Y + 1, _case.X], this, destination));
            // Droite
            if (carte.CaseValide(_case.X + 1, _case.Y) && carte.Cases[_case.Y, _case.X + 1].Type > 0)
                result.Add(new Noeud(carte.Cases[_case.Y, _case.X + 1], this, destination));
            // Haut
            if (carte.CaseValide(_case.X, _case.Y - 1) && carte.Cases[_case.Y - 1, _case.X].Type > 0)
                result.Add(new Noeud(carte.Cases[_case.Y - 1, _case.X], this, destination));
            // Gauche
            if (carte.CaseValide(_case.X - 1, _case.Y) && carte.Cases[_case.Y, _case.X - 1].Type > 0)
                result.Add(new Noeud(carte.Cases[_case.Y, _case.X - 1], this, destination));

            return result;
        }
    }
}
