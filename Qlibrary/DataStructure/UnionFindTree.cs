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
        public bool IsHistoryEnable { get; set; }
        private readonly int[] data;
        private readonly Stack<(int, int)> history = new Stack<(int, int)>();
        private int innerSnap;

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

            if (IsHistoryEnable)
            {
                history.Push((x, data[x]));
                history.Push((y, data[y]));
            }
            
            if (x == y) return false;

            if (Size(x) < Size(y))
            {
                (x, y) = (y, x);
            }

            data[x] += data[y];
            data[y] = x;

            return true;
        }

        public void Undo()
        {
            Debug.Assert(IsHistoryEnable, "IsHistoryEnable is false.");
            Loop(2, () =>
            {
                var lastItem = history.Pop();
                data[lastItem.Item1] = lastItem.Item2;
            });
        }

        public void TakeSnapShot()
        {
            Debug.Assert(IsHistoryEnable, "IsHistoryEnable is false.");
            innerSnap = (history.Count >> 1);
        }
        public int HistoryCount => history.Count >> 1;

        public void Rollback(int state = -1) 
        {
            Debug.Assert(IsHistoryEnable, "IsHistoryEnable is false.");
            if (state == -1) { state = innerSnap; }
            state <<= 1;
            while (state < history.Count) { Undo(); }
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