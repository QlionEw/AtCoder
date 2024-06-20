using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using static System.Math;

namespace Qlibrary
{
    public class OfflineDynamicConnectivity<TValue, TQuery> where TQuery : IComparable<TQuery>, INumber<TQuery>
    {
        private int time = 0;
        private bool isExistEdge = true;
        private readonly Dictionary<(int, int), (int, TQuery)> containEdge = new();
        private readonly Dictionary<(int, int), TQuery> lazyEdge = new();
        private readonly List<(int, int, int, int)> edgeRange = new();
        private readonly List<(int, int)> query = new();
        private readonly List<(int, TValue, int)> queryUpdate = new();
        private readonly List<int> untilQuery = new();
        private readonly TValue[] val;
        private readonly Func<TValue, TValue, TValue> op = (a, b) => default;
        private readonly TValue e;

        public OfflineDynamicConnectivity(int n, TValue e = default)
        {
            val = new TValue[n];
            this.e = e;
        }

        public OfflineDynamicConnectivity(int n, Func<TValue, TValue, TValue> onUnite, TValue e = default)
        {
            op = onUnite;
            val = new TValue[n];
            this.e = e;
        }

        public OfflineDynamicConnectivity(TValue[] v, Func<TValue, TValue, TValue> onUnite, TValue e = default)
        {
            val = v;
            this.e = e;
        }

        [MethodImpl(256)]
        private void TraversalEdge()
        {
            if (!isExistEdge) return;
            isExistEdge = false;
            foreach (var p in lazyEdge)
            {
                if (!containEdge.ContainsKey(p.Key))
                {
                    if (p.Value.CompareTo(default) > 0)
                    {
                        containEdge[p.Key] = (time, p.Value);
                    }
                }
                else
                {
                    var x = containEdge[p.Key];
                    var newValue = x.Item2 + p.Value;
                    containEdge[p.Key] = (x.Item1, newValue);
                    if (newValue.CompareTo(default) > 0) continue;
                    edgeRange.Add((p.Key.Item1, p.Key.Item2, x.Item1, time));
                    containEdge.Remove(p.Key);
                }
            }
            lazyEdge.Clear();
        }

        [MethodImpl(256)]
        private void TraversalQuery()
        {
            if (isExistEdge) return;
            isExistEdge = true;
            untilQuery.Add(query.Count);
            time++;
        }

        [MethodImpl(256)]
        private void EdgeSub(int u, int v, TQuery x)
        {
            if (e.Equals(default)) return;
            TraversalQuery();
            var key = (Math.Min(u, v), Math.Max(u, v));
            if (!lazyEdge.TryAdd(key, x))
            {
                lazyEdge[key] += x;
            }
        }

        [MethodImpl(256)]
        public void AddEdge(int u, int v) => EdgeSub(u, v, TQuery.One);

        [MethodImpl(256)]
        public void EraseEdge(int u, int v) => EdgeSub(u, v, -TQuery.One);

        [MethodImpl(256)]
        public void Update(int v, TValue x)
        {
            if (x.Equals(default)) return;
            TraversalQuery();
            queryUpdate.Add((v, x, time));
        }

        [MethodImpl(256)]
        public void QuerySame(int u, int v)
        {
            TraversalEdge();
            query.Add((u, v));
        }

        [MethodImpl(256)]
        public void QueryCount(int v)
        {
            TraversalEdge();
            query.Add((-1, v));
        }

        [MethodImpl(256)]
        public void QueryValue(int v)
        {
            TraversalEdge();
            query.Add((-2, v));
        }

        public List<(int, TValue)> Run()
        {
            time++;
            untilQuery.Add(query.Count);
            foreach (var p in containEdge)
            {
                edgeRange.Add((p.Key.Item1, p.Key.Item2, p.Value.Item1, time));
            }
            int size = 1;
            while (size < time) size <<= 1;
            var edges = new List<(int, int)>[2 * size];
            var updates = new List<(int, TValue)>[2 * size];
            for (int i = 0; i < 2 * size; i++)
            {
                edges[i] = new List<(int, int)>();
                updates[i] = new List<(int, TValue)>();
            }
            foreach (var edge in edgeRange)
            {
                int l = edge.Item3 + size, r = edge.Item4 + size;
                while (l < r)
                {
                    if ((l & 1) != 0) edges[l++].Add((edge.Item1, edge.Item2));
                    if ((r & 1) != 0) edges[--r].Add((edge.Item1, edge.Item2));
                    l >>=  1;
                    r >>=  1;
                }
            }
            foreach (var update in queryUpdate)
            {
                int l = update.Item3 + size, r = size + size;
                while (l < r)
                {
                    if ((l & 1) != 0) updates[l++].Add((update.Item1, update.Item2));
                    if ((r & 1) != 0) updates[--r].Add((update.Item1, update.Item2));
                    l >>=  1;
                    r >>=  1;
                }
            }
            var uf = new UndoableUnionFind<TValue>(val, op);
            var res = new List<(int, TValue)>();
            int now = 0;
            Dfs(1);
            return res;
            void Dfs(int k)
            {
                foreach (var edge in edges[k])
                {
                    uf.Unite(edge.Item1, edge.Item2);
                }
                foreach (var update in updates[k])
                {
                    uf.Update(update.Item1, update.Item2);
                }
                if (k < size)
                {
                    Dfs(2 * k);
                    Dfs(2 * k + 1);
                }
                else
                {
                    int m = k - size;
                    if (m < untilQuery.Count)
                    {
                        for (; now < untilQuery[m]; now++)
                        {
                            var q = query[now];
                            switch (q.Item1)
                            {
                                case -1:
                                    res.Add((uf.Size(q.Item2), default));
                                    break;
                                case -2:
                                    res.Add((-1, uf.Get(q.Item2)));
                                    break;
                                default:
                                    res.Add((uf.Same(q.Item1, q.Item2) ? 1 : 0, default));
                                    break;
                            }
                        }
                    }
                }
                int count = edges[k].Count + updates[k].Count + 1;
                while (--count > 0)
                {
                    uf.Undo();
                }
            }
        }
    }
}