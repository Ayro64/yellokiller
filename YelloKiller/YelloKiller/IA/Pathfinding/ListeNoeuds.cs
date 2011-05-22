using System.Collections.Generic;

namespace YelloKiller
{
    class NodeList<T> : List<T> where T : Noeud
    {
        public new bool Contains(T node)
        {
            return this[node] != null;
        }

        public T this[T noeud]
        {
            get
            {
                foreach (T node in this)
                    if (node.Case == noeud.Case)
                        return node;

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
                if (noeud.Manhattan < this[center].Manhattan)
                    right = center - 1;
                else if (noeud.Manhattan > this[center].Manhattan)
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