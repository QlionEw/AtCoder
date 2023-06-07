using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static Qlibrary.Common;

namespace Qlibrary
{
    public class BipartiteMatching
    {
        private readonly int n1;
        private readonly List<int>[] preMap;
        private int[] match;
        private readonly bool[] isUsed;
        public int[][] Map { get; private set; }

        public BipartiteMatching(int n1, int n2)
        {
            this.n1 = n1;
            var n = n1 + n2;
            preMap = Make(n, () => new List<int>());
            isUsed = new bool[n];
        }

        [MethodImpl(256)]
        public void AddEdge(int from, int to)
        {
            preMap[from].Add(n1 + to);
            preMap[n1 + to].Add(from);
        }

        public void AddEdges(int[][] des)
        {
            foreach (var e in des) AddEdge(e[0], e[1]);
        }

        [MethodImpl(256)]
        private bool Dfs(int v1)
        {
            isUsed[v1] = true;
            foreach (var v2 in Map[v1])
            {
                var u1 = match[v2];
                if (u1 == -1 || !isUsed[u1] && Dfs(u1))
                {
                    match[v1] = v2;
                    match[v2] = v1;
                    return true;
                }
            }

            return false;
        }

        public List<(int,int)> Dinic()
        {
            Map = Array.ConvertAll(preMap, l => l.ToArray());
            match = Array.ConvertAll(preMap, _ => -1);
            
            for (int v1 = 0; v1 < n1; ++v1)
            {
                if (match[v1] != -1) continue;
                Array.Clear(isUsed, 0, isUsed.Length);
                Dfs(v1);
            }

            var r = new List<(int,int)>();
            for (int v1 = 0; v1 < n1; ++v1)
                if (match[v1] != -1)
                    r.Add((v1, match[v1] - n1));
            
            return r;
        }
    }
}