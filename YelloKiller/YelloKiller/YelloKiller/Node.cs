using System;
using System.Collections.Generic;

namespace YelloKiller
{
    class Node
    {
        Case _case;

        public Case Case
        {
            get { return _case; }
        }
        Node parent;

        public Node Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        int estimatedMovement;

        public int EstimatedMovement
        {
            get { return estimatedMovement; }
        }

        public Node(Case _case, Node parent, Case destination)
        {
            this._case = _case;
            this.parent = parent;
            this.estimatedMovement = Convert.ToInt32(Math.Abs(_case.Position.X - destination.Position.X) + Math.Abs(_case.Position.Y - destination.Position.Y)) + (int)_case.Type;
        }

        public List<Node> GetPossibleNode(Carte carte, Case destination)
        {
            List<Node> result = new List<Node>();
            // Bottom
            if (carte.ValidCoordinates(_case.Position.X, _case.Position.Y + 1) && carte.Cases[(int)_case.Position.X + 1, (int)_case.Position.X].Type != TypeCase.mur)
                result.Add(new Node(carte.Cases[(int)_case.Position.Y + 1, (int)_case.Position.X], this, destination));

            // Right
            if (carte.ValidCoordinates(_case.Position.X + 1, _case.Position.Y) && carte.Cases[(int)_case.Position.Y, (int)_case.Position.X + 1].Type != TypeCase.mur)
                result.Add(new Node(carte.Cases[(int)_case.Position.Y, (int)_case.Position.X + 1], this, destination));

            // Top
            if (carte.ValidCoordinates((int)_case.Position.X, _case.Position.Y - 1) && carte.Cases[(int)_case.Position.Y - 1, (int)_case.Position.X].Type != TypeCase.mur)
                result.Add(new Node(carte.Cases[(int)_case.Position.Y - 1, (int)_case.Position.X], this, destination));

            // Left
            if (carte.ValidCoordinates((int)_case.Position.X - 1, (int)_case.Position.Y) && carte.Cases[(int)_case.Position.Y, (int)_case.Position.X - 1].Type != TypeCase.mur)
                result.Add(new Node(carte.Cases[(int)_case.Position.Y, (int)_case.Position.X - 1], this, destination));

            return result;
        }
    }
}
