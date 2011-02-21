using System.Collections.Generic;

namespace YelloKiller
{
    class NodeList<T> : List<T> where T : Node
    {
        public new bool Contains(T node)
        {
            return this[node] != null;
        }

        public T this[T node]
        {
            get
            {
                int count = this.Count;
                for (int i = 0; i < count; i++)
                {
                    if (this[i].Case == node.Case)
                        return this[i];
                }
                return default(T);
            }
        }

        public void DichotomicInsertion(T node)
        {
            int left = 0;
            int right = this.Count - 1;
            int center = 0;
            while (left <= right)
            {
                center = (left + right) / 2;
                if (node.EstimatedMovement < this[center].EstimatedMovement)
                    right = center - 1;
                else if (node.EstimatedMovement > this[center].EstimatedMovement)
                    left = center + 1;
                else
                {
                    left = center;
                    break;
                }
            }
            this.Insert(left, node);
        }
    }
}


