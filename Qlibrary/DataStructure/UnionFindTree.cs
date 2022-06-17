using System.Collections.Generic;
using System.Linq;

namespace Qlibrary
{
    public class UnionFindTree
    {
        public long[] Parents { get; }

        public UnionFindTree(int n)
        {
            Parents = Enumerable.Repeat((long) -1, n).ToArray();
        }

        public long Find(long x)
        {
            if (Parents[x] < 0) return x;
            Parents[x] = Find(Parents[x]);
            return Parents[x];
        }

        public long Size(long x) => -Parents[Find(x)];
        public bool Same(long x, long y) => Find(x) == Find(y);

        public bool Union(long x, long y)
        {
            x = Find(x);
            y = Find(y);

            if (x == y) return false;

            if (Size(x) < Size(y))
            {
                (x, y) = (y, x);
            }

            Parents[x] += Parents[y];
            Parents[y] = x;

            return true;
        }

        public IEnumerable<List<int>> GetUnions(int count, int indexStart = 0)
        {
            Dictionary<long, List<int>> dict = new Dictionary<long, List<int>>();
            for (int i = indexStart; i < count + indexStart; i++)
            {
                var parent = Find(i);
                if (!dict.ContainsKey(parent))
                {
                    dict.Add(parent,new List<int>());
                }
                dict[parent].Add(i);
            }
            return dict.Values;
        }
    }
}