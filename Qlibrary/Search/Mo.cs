using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace Qlibrary
{
    public class Mo
    {
        private static int width;
        private readonly List<int> left = new List<int>();
        private readonly List<int> right = new List<int>();
        private readonly int[] order;

        public Mo(int n, int q)
        {
            width = Max(1, (int)(1.0 * n / Max(1.0, Sqrt(q * 2.0 / 3.0))));
            order = Enumerable.Range(0, q).ToArray();
        }

        public void Insert(int l, int r)
        {
            left.Add(l);
            right.Add(r+1);
        }

        public void Run(Action<int> add, Action<int> delete, Action<int> build)
        {
            Run(add, add, delete, delete, build);
        }
        
        public void Run(Action<int> addLeft, Action<int> addRight, Action<int> deleteLeft, Action<int> deleteRight,
            Action<int> build)
        {
            int nl = 0, nr = 0;
            Array.Sort(order, (a, b) =>
            {
                int aBlock = left[a] / width;
                int bBlock = left[b] / width;
                if (aBlock != bBlock) return aBlock - bBlock;
                return (aBlock & 1) == 1 ? right[a] - right[b] : right[b] - right[a];
            });
            foreach (var i in order)
            {
                while (nl > left[i]) addLeft(--nl);
                while (nr < right[i]) addRight(nr++);
                while (nl < left[i]) deleteLeft(nl++);
                while (nr > right[i]) deleteRight(--nr);
                build(i);
            }
        }
    }
}