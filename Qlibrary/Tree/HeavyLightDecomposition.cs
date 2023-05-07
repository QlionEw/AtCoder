using System;
using System.Collections.Generic;
using System.Linq;

namespace Qlibrary
{
    public class HeavyLightDecomposition
    {
        private void DfsForSize(int cur)
        {
            Size[cur] = 1;
            for (int i = 0; i < g[cur].Count; i++)
            {
                var dst = g[cur][i];
                if (dst.To == parent[cur]) {
                    if (g[cur].Count >= 2 && dst.To == g[cur][0].To)
                    {
                        (g[cur][0], g[cur][1]) = (g[cur][1], g[cur][0]);
                        dst = g[cur][0];
                    }
                    else
                    {
                        continue;
                    }
                }
                Depth[dst.To] = Depth[cur] + 1;
                parent[dst.To] = cur;
                DfsForSize(dst.To);
                Size[cur] += Size[dst.To];
                if (Size[dst.To] > Size[g[cur][0].To])
                {
                    (g[cur][0], g[cur][i]) = (g[cur][i], g[cur][0]);
                }
            }
        }

        private void DfsForHld(int cur)
        {
            down[cur] = id++;
            foreach (var dst in g[cur].Where(dst => dst.To != parent[cur]))
            {
                nxt[dst.To] = dst.To == g[cur][0].To ? nxt[cur] : dst.To;
                DfsForHld(dst.To);
            }

            up[cur] = id;
        }

        private List<(int, int)> Ascend(int u, int v)
        {
            List<(int, int)> res = new List<(int, int)>();
            while (nxt[u] != nxt[v])
            {
                res.Add((down[u], down[nxt[u]]));
                u = parent[nxt[u]];
            }

            if (u != v) res.Add((down[u], down[v] + 1));
            return res;
        }

        private List<(int, int)> Descend(int u, int v)
        {
            if (u == v) return new List<(int, int)>();
            if (nxt[u] == nxt[v]) return new List<(int, int)>() { (down[u] + 1, down[v]) };
            var res = Descend(u, parent[nxt[v]]);
            res.Add((down[nxt[v]], down[v]));
            return res;
        }

        private readonly Graph g;
        private int id;
        public int[] Depth { get; }
        public int[] Size { get; }
        private readonly int[] down;
        private readonly int[] up;
        private readonly int[] nxt;
        private readonly int[] parent;

        public HeavyLightDecomposition(Graph g, int root = 1)
        {
            this.g = g;
            id = 0;
            Size = new int[g.Count];
            Depth = new int[g.Count];
            down = new int[g.Count];
            up = new int[g.Count];
            nxt = new int[g.Count];
            parent = new int[g.Count];

            down.Init(-1);
            up.Init(-1);
            nxt.Init(root);
            parent.Init(root);

            DfsForSize(root);
            DfsForHld(root);
        }

        public void Build(int root)
        {
            DfsForSize(root);
            DfsForHld(root);
        }

        public (int, int) Index(int i)
        {
            return (down[i], up[i]);
        }

        public void QueryPath(int u, int v, bool vertex, Action<int, int> f)
        {
            int l = GetLca(u, v);
            foreach (var (a, b) in Ascend(u, l))
            {
                int s = a + 1;
                if (s > b)
                {
                    f(b, s);
                }
                else
                {
                    f(s, b);
                }
            }

            if (vertex) f(down[l], down[l] + 1);
            foreach (var (a, b) in Descend(l, v))
            {
                int t = b + 1;
                if (a > t)
                {
                    f(t, a);
                }
                else
                {
                    f(a, t);
                }
            }
        }

        public void QueryPathNonCommutative(int u, int v, bool vertex, Action<int, int> f)
        {
            int l = GetLca(u, v);
            foreach (var (a, b) in Ascend(u, l)) f(a + 1, b);
            if (vertex) f(down[l], down[l] + 1);
            foreach (var (a, b) in Descend(l, v)) f(a, b + 1);
        }

        public void QuerySubtree(int u, bool vertex, Action<int, int> f)
        {
            f(down[u] + (vertex ? 0 : 1), up[u]);
        }

        public int GetLca(int a, int b)
        {
            while (nxt[a] != nxt[b])
            {
                if (down[a] < down[b]) (a, b) = (b, a);
                a = parent[nxt[a]];
            }

            return Depth[a] < Depth[b] ? a : b;
        }

        public int Distance(int a, int b)
        {
            return Depth[a] + Depth[b] - Depth[GetLca(a, b)] * 2;
        }
    };
}