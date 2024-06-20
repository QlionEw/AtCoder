using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using static Qlibrary.Common;

namespace Qlibrary
{
    public class UnionFindTree
    {
        private readonly int[] data;

        public UnionFindTree(int n)
        {
            data = Enumerable.Repeat(-1, n).ToArray();
        }

        public int Find(int x)
        {
            if (data[x] < 0) return x;
            data[x] = Find(data[x]);
            return data[x];
        }

        public int Size(int x) => -data[Find(x)];
        public bool Same(int x, int y) => Find(x) == Find(y);

        public bool Unite(int x, int y)
        {
            x = Find(x);
            y = Find(y);
            
            if (x == y) return false;

            if (Size(x) < Size(y))
            {
                (x, y) = (y, x);
            }

            data[x] += data[y];
            data[y] = x;

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