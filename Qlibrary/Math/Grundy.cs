using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    public class Grundy
    {
        private readonly TopologicalSorter ts;
        private readonly int size;

        public Grundy(int size)
        {
            ts = new TopologicalSorter(size);
            this.size = size;
        }

        [MethodImpl(256)]
        public void AddPath(int i, int j)
        {
            ts.Connect(i, j);
        }
        
        [MethodImpl(256)]
        public IEnumerable<int> Get()
        {
            int[] grundy = new int[size];
            int[] memo = new int[size + 1];
            foreach (var path in ts.Get().Reverse())
            {
                if (path.To.Count == 0)
                {
                    continue;
                }
                foreach (var i in path.To)
                {
                    memo[grundy[i]]++;
                }
                while (memo[grundy[path.Index]] > 0)
                {
                    grundy[path.Index]++;
                }
                foreach (var i in path.To)
                {
                    memo[grundy[i]]--;
                }
            }
            return grundy;
        }
    }
}