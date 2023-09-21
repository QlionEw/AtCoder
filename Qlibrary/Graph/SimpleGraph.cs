using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static Qlibrary.Common;

namespace Qlibrary
{
    public class SimpleGraph : IEnumerable<int[]>
    {
        private int[][] graph;
        private readonly HashSet<int>[] tempGraph;
        public int Count { get; }
        public int PathCount { get; private set; }
        public bool IsDirected { get; }

        public SimpleGraph(int n, bool isDirected)
        {
            Count = n + 1;
            tempGraph = Make(Count, () => new HashSet<int>());
            IsDirected = isDirected;
        }

        public SimpleGraph(int n, int[][] paths, bool isDirected) : this(n, isDirected)
        {
            SetPath(paths);
        }

        private void SetPath(int[][] paths)
        {
            foreach (var path in paths)
            {
                AddPath(path[0], path[1]);
            }
        }

        public void AddPath(int p, int q)
        {
            PathCount++;
            tempGraph[p].Add(q);
            if (!IsDirected) tempGraph[q].Add(p);
        }

        public void SetMatrix(int[][] paths, int indexed = 1)
        {
            for (int i = 0; i < Count; i++)
            {
                for (int j = 0; j < Count; j++)
                {
                    if (paths[i][j] == 0) continue;
                    tempGraph[i + indexed].Add(j + indexed);
                }
            }
        }

        public int[][] GetGraph()
        {
            if (graph == null) graph = Array.ConvertAll(tempGraph, x => x.ToArray());
            return graph;
        }

        public bool HasPath(int from, int to) => tempGraph[from].Contains(to);
        public int[] this[int edge] => GetGraph()[edge];
        public IEnumerator<int[]> GetEnumerator() => GetGraph().AsEnumerable().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}