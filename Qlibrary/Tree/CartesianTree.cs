using System.Collections.Generic;
using System.Numerics;

namespace Qlibrary
{
    public class CartesianTree : Graph<int>
    {
        public int Root { get; set; }
        public int[] Parents { get; set; }

        public CartesianTree(int[] array) : base(array.Length, false)
        {
            var n = array.Length;
            Parents = new int[n];
            Parents.Init(-1);
            var st = new Stack<int>();
            for (int i = 0; i < n; i++)
            {
                int prv = -1;
                while (st.Count != 0 && array[i] < array[st.Peek()])
                {
                    prv = st.Pop();
                }
                if (prv != -1)
                {
                    Parents[prv] = i;
                }
                if (st.Count != 0)
                {
                    Parents[i] = st.Peek();
                }
                st.Push(i);
            }
            for (int i = 0; i < n; i++)
            {
                if (Parents[i] != -1)
                {
                    AddPath(Parents[i], i, 1);
                }
                else
                {
                    Root = i;
                }
            }
        }
    }
}