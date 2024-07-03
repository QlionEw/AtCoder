using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Qlibrary
{
    public class HeavyLightDecomposition<T> where T : INumber<T>
    {
        private void DfsForSize(int cur)
        {
            Size[cur] = 1;
            for (int i = 0; i < Graph[cur].Length; i++)
            {
                var dst = Graph[cur][i];
                if (dst.To == Parent[cur])
                {
                    if (Graph[cur].Length >= 2 && dst.To == Graph[cur][0].To)
                    {
                        (Graph[cur][0], Graph[cur][1]) = (Graph[cur][1], Graph[cur][0]);
                        dst = Graph[cur][0];
                    }
                    else
                    {
                        continue;
                    }
                }
                Depth[dst.To] = Depth[cur] + 1;
                Parent[dst.To] = cur;
                DfsForSize(dst.To);
                Size[cur] += Size[dst.To];
                if (Size[dst.To] > Size[Graph[cur][0].To])
                {
                    (Graph[cur][0], Graph[cur][i]) = (Graph[cur][i], Graph[cur][0]);
                }
            }
        }

        public List<(int, int)> Tour { get; } = new();
        private void DfsForHld(int cur)
        {
            Down[cur] = id++;
            Tour.Add((Parent[cur], cur));
            foreach (var dst in Graph[cur].Where(dst => dst.To != Parent[cur]))
            {
                Next[dst.To] = dst.To == Graph[cur][0].To ? Next[cur] : dst.To;
                DfsForHld(dst.To);
            }

            Up[cur] = id;
        }

        private List<(int, int)> Ascend(int u, int v)
        {
            List<(int, int)> res = new List<(int, int)>();
            while (Next[u] != Next[v])
            {
                res.Add((Down[u], Down[Next[u]]));
                u = Parent[Next[u]];
            }

            if (u != v) res.Add((Down[u], Down[v] + 1));
            return res;
        }

        private List<(int, int)> Descend(int u, int v)
        {
            if (u == v) return new List<(int, int)>();
            if (Next[u] == Next[v]) return new List<(int, int)>() { (Down[u] + 1, Down[v]) };
            var res = Descend(u, Parent[Next[v]]);
            res.Add((Down[Next[v]], Down[v]));
            return res;
        }

        protected readonly Graph<T> Graph;
        private int id;
        public int[] Depth { get; }
        public int[] Size { get; }
        protected readonly int[] Down;
        protected readonly int[] Up;
        protected readonly int[] Next;
        protected readonly int[] Parent;

        public HeavyLightDecomposition(Graph<T> graph, int root = 1)
        {
            this.Graph = graph;
            id = 0;
            Size = new int[graph.Count];
            Depth = new int[graph.Count];
            Down = new int[graph.Count];
            Up = new int[graph.Count];
            Next = new int[graph.Count];
            Parent = new int[graph.Count];

            Down.Init(-1);
            Up.Init(-1);
            Next.Init(root);
            Parent.Init(root);

            DfsForSize(root);
            DfsForHld(root);
        }

        public void Build(int root)
        {
            DfsForSize(root);
            DfsForHld(root);
        }

        public (int In, int Out) Index(int i)
        {
            return (Down[i], Up[i]);
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

            if (vertex) f(Down[l], Down[l] + 1);
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
            if (vertex) f(Down[l], Down[l] + 1);
            foreach (var (a, b) in Descend(l, v)) f(a, b + 1);
        }

        public void QuerySubtree(int u, bool vertex, Action<int, int> f)
        {
            f(Down[u] + (vertex ? 0 : 1), Up[u]);
        }

        public int GetLca(int a, int b)
        {
            while (Next[a] != Next[b])
            {
                if (Down[a] < Down[b]) (a, b) = (b, a);
                a = Parent[Next[a]];
            }

            return Depth[a] < Depth[b] ? a : b;
        }

        public int Distance(int a, int b)
        {
            return Depth[a] + Depth[b] - Depth[GetLca(a, b)] * 2;
        }
    };
}