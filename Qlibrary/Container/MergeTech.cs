using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Qlibrary
{
    public class MergeTechOperator<T>
    {
        public Func<int, T> Init { get; set; }
        public Func<T, int> Count { get; set; }
        public Action<(T From, T To)> Merge { get; set; }
    }
    
    public class MergeTech<T>
    {
        private readonly int n;
        private readonly MergeTechOperator<T> op;
        private readonly T[] arrays;
        private readonly UnionFindTree uft;

        public MergeTech(int n, MergeTechOperator<T> op)
        {
            this.n = n;
            this.op = op;
            arrays = new T[n + 1];
            for (int i = 1; i <= n; i++)
            {
                arrays[i] = op.Init(i);
            }
            uft = new UnionFindTree(n + 1);
        }

        public void Merge(int i0, int i1)
        {
            int a = uft.Find(i0);
            int b = uft.Find(i1);
            uft.Unite(i0, i1);
            int x = uft.Find(i1);
            var inQue = op.Count(arrays[a]) > op.Count(arrays[b]) ? arrays[a] : arrays[b];
            var outQue = op.Count(arrays[a]) > op.Count(arrays[b]) ? arrays[b] : arrays[a];
            op.Merge((outQue, inQue));
            arrays[x] = inQue;
        }
    
        public T GetMerged(int index = 1) => arrays[uft.Find(index)];
    }
}
