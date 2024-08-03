using System.Collections.Generic;
using System.Numerics;

namespace Qlibrary
{
    public class CentroidDecomposition<T> where T : INumber<T>
    {
        private readonly Graph<T> g;
        public int[] Sub { get; }
        private readonly bool[] v;
        public Graph<T> Tree { get; }
        public int Root { get; private set; }

        public CentroidDecomposition(Graph<T> g, int cur = 1)
        {
            this.g = g;
            Sub = new int[g.Count];
            v = new bool[g.Count];
            Tree = new Graph<T>(g.Count, g.IsDirected);

            Build(cur);
        }

        private void Build(int cur)
        {
            Root = BuildDfs(cur);
        }

        private int GetSize(int cur, int par)
        {
            Sub[cur] = 1;
            foreach (var dst in g[cur])
            {
                if (dst.To == par || v[dst.To]) continue;
                Sub[cur] += GetSize(dst.To, cur);
            }
            return Sub[cur];
        }

        private int GetCentroid(int cur, int par, int mid)
        {
            foreach (var dst in g[cur])
            {
                if (dst.To == par || v[dst.To]) continue;
                if (Sub[dst.To] > mid) return GetCentroid(dst.To, cur, mid);
            }
            return cur;
        }

        private int BuildDfs(int cur)
        {
            int centroid = GetCentroid(cur, -1, GetSize(cur, -1) / 2);
            v[centroid] = true;
            foreach (var dst in g[centroid])
            {
                if (!v[dst.To])
                {
                    int nxt = BuildDfs(dst.To);
                    if (centroid != nxt) Tree.AddPath(centroid, nxt, dst.Cost);
                }
            }
            v[centroid] = false;
            return centroid;
        }
    }
}