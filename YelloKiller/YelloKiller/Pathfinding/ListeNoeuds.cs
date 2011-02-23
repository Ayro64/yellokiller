using System.Collections.Generic;

namespace YelloKiller
{
    class ListeNoeuds<T> : List<T> where T : Noeud
    {
        public new bool Contains(T noeud)
        {
            return this[noeud] != null;
        }

        public T this[T noeud]
        {
            get
            {
                int count = this.Count;
                for (int i = 0; i < count; i++)
                {
                    if (this[i].Position.X == noeud.Position.X && this[i].Position.Y == noeud.Position.Y)
                        return this[i];
                }
                return default(T);
            }
        }

        public void DichotomicInsertion(T noeud)
        {
            int left = 0;
            int right = this.Count - 1;
            int center = 0;
            while (left <= right)
            {
                center = (left + right) / 2;
                if (noeud.EstimatedMovement < this[center].EstimatedMovement)
                    right = center - 1;
                else if (noeud.EstimatedMovement > this[center].EstimatedMovement)
                    left = center + 1;
                else
                {
                    left = center;
                    break;
                }
            }
            this.Insert(left, noeud);
        }
    }
}