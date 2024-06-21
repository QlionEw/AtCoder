using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static System.Math;

namespace Qlibrary
{
    public class LowLink<T> where T : INumber<T>
    {
        private readonly Graph<T> graph;
        private readonly int[] ord;
        private readonly int[] low;
        public List<int> ArticulationPoints { get; } = new();
        public List<(int, int)> Bridges { get; } = new();

        public LowLink(Graph<T> graph)
        {
            this.graph = graph;
            var nodeCount = graph.Count;
            ord = new int[nodeCount];
            low = new int[nodeCount];
            
            ord.Init(-1);
            low.Init(-1);

            for (int i = 0, k = 0; i < nodeCount; i++)
            {
                if (ord[i] == -1)
                {
                    k = Dfs(i, k, -1);
                }
            }
        }

        private int Dfs(int idx, int k, int parent)
        {
            low[idx] = ord[idx] = k++;
            int cnt = 0;
            bool isArticulation = false, second = false;

            foreach (var to in graph[idx].Select(x => x.To))
            {
                if (ord[to] == -1)
                {
                    cnt++;
                    k = Dfs(to, k, idx);
                    low[idx] = Min(low[idx], low[to]);

                    if (parent != -1 && low[to] >= ord[idx])
                    {
                        isArticulation = true;
                    }

                    if (ord[idx] < low[to])
                    {
                        Bridges.Add((Min(idx, to), Max(idx, to)));
                    }
                }
                else if (to != parent || second)
                {
                    low[idx] = Min(low[idx], ord[to]);
                }
                else
                {
                    second = true;
                }
            }

            if (parent == -1 && cnt > 1)
            {
                isArticulation = true;
            }

            if (isArticulation)
            {
                ArticulationPoints.Add(idx);
            }

            return k;
        }
    }
}