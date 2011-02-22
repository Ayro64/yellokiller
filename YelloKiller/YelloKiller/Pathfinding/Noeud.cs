using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace YelloKiller
{
    class Noeud
    {
        Case _case;
        Noeud parent;
        int estimatedMovement;

        public Noeud(Case _case, Noeud parent, Case destination)
        {
            this._case = _case;
            this.parent = parent;
            this.estimatedMovement = Math.Abs((int)_case.Position.X / 28 - (int)destination.Position.X / 28) + Math.Abs((int)_case.Position.Y / 28 - (int)destination.Position.Y / 28);
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

        public int EstimatedMovement
        {
            get { return estimatedMovement; }
        }
        
        public List<Noeud> NoeudPossibles(Carte carte, Case destination, Rectangle camera)
        {
            List<Noeud> result = new List<Noeud>();
            // Bas
            if ((int)_case.Position.Y / 28 + 1 < Taille_Map.HAUTEUR_MAP && (int)_case.Position.Y >= 0 && (int)_case.Position.X / 28 >= 0 && carte.Cases[(int)_case.Position.Y / 28 + 1, (int)_case.Position.X / 28].Type > 0)
                result.Add(new Noeud(carte.Cases[(int)_case.Position.Y / 28 + 1, (int)_case.Position.X / 28], this, destination));
            // Droite
            if ((int)_case.Position.X / 28 + 1 < Taille_Map.LARGEUR_MAP && (int)_case.Position.X / 28 + 1 >= 0 && (int)_case.Position.Y / 28 > 0 && carte.Cases[(int)_case.Position.Y / 28, (int)_case.Position.X / 28 + 1].Type > 0)
                result.Add(new Noeud(carte.Cases[(int)_case.Position.Y / 28, (int)_case.Position.X / 28 + 1], this, destination));
            // Haut
            if ((int)_case.Position.Y / 28 - 1 >= 0 && (int)_case.Position.X / 28 >= 0 && carte.Cases[(int)_case.Position.Y / 28 - 1, (int)_case.Position.X / 28].Type > 0)
                result.Add(new Noeud(carte.Cases[(int)_case.Position.Y / 28 - 1, (int)_case.Position.X / 28], this, destination));
            // Gauche
            if ((int)_case.Position.X / 28 - 1 >= 0 && (int)_case.Position.Y >= 0 && carte.Cases[(int)_case.Position.Y / 28, (int)_case.Position.X / 28 - 1].Type > 0)
                result.Add(new Noeud(carte.Cases[(int)_case.Position.Y / 28, (int)_case.Position.X / 28 - 1], this, destination));
            
            return result;
        }
    }
}