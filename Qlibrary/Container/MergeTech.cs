using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Qlibrary
{
    public class MergeTech
    {
        private readonly int n;
        private readonly List<List<int>> arrays = new();
        private readonly UnionFindTree uft;

        public MergeTech(int n)
        {
            this.n = n;
            for (int i = 0; i <= n; i++)
            {
                arrays.Add(new List<int> { i });
            }
            uft = new UnionFindTree(n + 1);
        }

        public void Merge(int i0, int i1)
        {
            int a = uft.Find(i0);
            int b = uft.Find(i1);
            uft.Unite(i0, i1);
            int x = uft.Find(i1);
            var inQue = arrays[a].Count > arrays[b].Count ? arrays[a] : arrays[b];
            var outQue = arrays[a].Count > arrays[b].Count ? arrays[b] : arrays[a];
            inQue.AddRange(outQue);
            arrays[x] = inQue;
        }
    
        public List<int> GetMerged(int index = 1) => arrays[uft.Find(index)];
    }
}
