using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Qlibrary
{
    public class MergeTech<T>
    {
        private readonly int n;
        private readonly Func<T, int> count;
        private readonly T[] arrays;
        private readonly UnionFindTree uft;

        public MergeTech(int n, Func<int, T> init, Func<T, int> count)
        {
            this.n = n;
            this.count = count;
            arrays = new T[n + 1];
            for (int i = 1; i <= n; i++)
            {
                arrays[i] = init(i);
            }
            uft = new UnionFindTree(n + 1);
        }

        public void Merge(int i0, int i1, Action<(T From, int FromIndex, T To, int ToIndex)> merge)
        {
            int a = uft.Find(i0);
            int b = uft.Find(i1);
            if (a == b)
            {
                return;
            }
            uft.Unite(i0, i1);
            int x = uft.Find(i1);
            var inQue = count(arrays[a]) > count(arrays[b]) ? arrays[a] : arrays[b];
            var outQue = count(arrays[a]) > count(arrays[b]) ? arrays[b] : arrays[a];
            var inIndex = count(arrays[a]) > count(arrays[b]) ? i0 : i1;
            var outIndex = count(arrays[a]) > count(arrays[b]) ? i1 : i0;
            merge((outQue, outIndex, inQue, inIndex));
            arrays[x] = inQue;
        }
    
        public T GetMerged(int index = 1) => arrays[uft.Find(index)];
    }
}
