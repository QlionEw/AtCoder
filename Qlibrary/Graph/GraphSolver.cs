using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    public class GraphSolver<T> where T : INumber<T>
    {
        public readonly T Invalid = Common.GetInfinity<T>();
        private readonly Graph<T> graph;
        public T[] Distances { get; private set; }
        private int pathCount = 0;
        private readonly int nodeCount;

        public GraphSolver(Graph<T> g)
        {
            graph = g;
            nodeCount = g.Count;
            pathCount = g.PathCount;
            Distances = Common.Make(nodeCount + 1, () => Invalid);
        }

        [MethodImpl(256)]
        public void Init()
        {
            Array.Fill(Distances, Invalid);
        }

        public void Dijkstra(int point)
        {
            Distances[point] = T.Zero;
            var queue = new PriorityQueue<(T Cost,int To)>(pathCount + 1);
            queue.Enqueue((T.Zero, point));

            while (queue.Count != 0)
            {
                var pop = queue.Dequeue();
                if (Distances[pop.To] < pop.Cost) { continue; }

                foreach (Edge<T> path in graph[pop.To])
                {
                    var nextValue = Distances[pop.To] + path.Cost;
                    if (Distances[path.To] > nextValue)
                    {
                        Distances[path.To] = nextValue;
                        queue.Enqueue((Distances[path.To], path.To));
                    }
                }
            }
        }

        private bool[] isVisited; 
        public T _01Bfs(int start, int end = -1)
        {
            Deque<int> deque = new Deque<int>();
            isVisited = new bool[nodeCount + 1];
            Distances[start] = T.Zero;
            deque.PushBack(start);

            while (deque.Length != 0)
            {
                int index = deque.PopFront();
                if (index == end)
                {
                    return Distances[index];
                }
                if (isVisited[index])
                {
                    continue;
                }
                isVisited[index] = true;
                foreach (Edge<T> path in graph[index])
                {
                    T d = Distances[index] + path.Cost;
                    if (d < Distances[path.To])
                    {
                        Distances[path.To] = d;
                        if (path.Cost != T.Zero)
                        {
                            deque.PushBack(path.To);
                        }
                        else
                        {
                            deque.PushFront(path.To);
                        }
                    }
                }
            }

            return T.CreateChecked(-1);
        }

        public T Kruskal()
        {
            var edges = graph.SelectMany(pathInfo => pathInfo).OrderBy(x => x.Cost).ToList();
            T totalCost = T.Zero;
            var uft = new UnionFindTree(nodeCount + 1);
            foreach (Edge<T> edge in edges.Where(edge => !uft.Same(edge.From, edge.To)))
            {
                uft.Unite(edge.From, edge.To);
                totalCost += edge.Cost;
            }
            return totalCost;
        }

        private Edge<T>[] bellmanFordList;
        public bool[] IsLoop { get; private set; }

        public void BellmanFord(int point, bool isDetectLoop = false)
        {
            bellmanFordList = graph.SelectMany(x => x).ToArray();
            IsLoop = Enumerable.Repeat(false, nodeCount + 1).ToArray();
            Distances[point] = T.Zero;
            int count;
            for (count = 0; count < Distances.Length; count++)
            {
                bool isUpdated = false;
                foreach (Edge<T> path in bellmanFordList)
                {
                    if (Distances[path.From] == Invalid) { continue; }

                    if (Distances[path.To] <= Distances[path.From] + path.Cost) { continue; }

                    Distances[path.To] = Distances[path.From] + path.Cost;
                    isUpdated = true;
                }

                if (!isUpdated) { break; }
            }

            if (isDetectLoop)
            {
                DetectBellmanFordLoop();
            }
        }

        private void DetectBellmanFordLoop()
        {
            for (int i = 0; i <= Distances.Length; i++)
            {
                foreach (Edge<T> path in bellmanFordList)
                {
                    if (Distances[path.From] == Invalid) { continue; }

                    if (Distances[path.To] > Distances[path.From] + path.Cost)
                    {
                        Distances[path.To] = Distances[path.From] + path.Cost;
                        IsLoop[path.To] = true;
                    }

                    if (IsLoop[path.From])
                    {
                        IsLoop[path.To] = true;
                    }
                }
            }
        }

        private bool[] sccUsed;
        private List<int> sccOrder;
        private List<Edge<T>>[] reversePathInfos;
        private int[] sccGroups;

        public IEnumerable<int> StronglyConnectedComponent()
        {
            sccUsed = new bool[nodeCount + 1];
            sccOrder = new List<int>();
            reversePathInfos = Enumerable.Repeat(0, nodeCount + 1).Select(_ => new List<Edge<T>>()).ToArray();
            foreach (var path in graph.SelectMany(x => x))
            {
                reversePathInfos[path.To].Add(new Edge<T>(path.To, path.From, path.Cost));
            }

            for (int i = 1; i <= nodeCount; i++)
            {
                if (!sccUsed[i])
                {
                    SccDfs(i);
                }
            }

            sccUsed = new bool[nodeCount + 1];
            sccGroups = new int[nodeCount];
            int groupNumber = 0;
            for (int i = sccOrder.Count - 1; i >= 0; i--)
            {
                if (!sccUsed[sccOrder[i]])
                {
                    ReverseScc(sccOrder[i], groupNumber++);
                }
            }

            return sccGroups;
        }

        private void SccDfs(int index)
        {
            sccUsed[index] = true;
            foreach (var to in graph[index].Select(x => x.To))
            {
                if (!sccUsed[to])
                {
                    SccDfs(to);
                }
            }

            sccOrder.Add(index);
        }

        private void ReverseScc(int index, int groupNumber)
        {
            sccUsed[index] = true;
            sccGroups[index - 1] = groupNumber;
            foreach (var to in reversePathInfos[index].Select(x => x.To))
            {
                if (!sccUsed[to])
                {
                    ReverseScc(to, groupNumber);
                }
            }
        }

        private T[][] warshallFloydDp;
        private int[][] warshallFloydPathPrev;

        public T[][] WarshallFloyd(int nIndexed)
        {
            var loopCount = nodeCount + nIndexed;
            warshallFloydDp = Enumerable.Repeat(0, loopCount)
                .Select(_ => Enumerable.Repeat(Invalid, loopCount).ToArray()).ToArray();
            for (int i = 0; i < loopCount; i++)
            {
                warshallFloydDp[i][i] = T.Zero;
            }
            warshallFloydPathPrev = Enumerable.Repeat(0, loopCount)
                .Select(_ => Enumerable.Repeat(0, loopCount).ToArray()).ToArray();
            for (int i = nIndexed; i < loopCount; i++)
            {
                for (int j = nIndexed; j < loopCount; j++)
                {
                    warshallFloydPathPrev[i][j] = i;
                }
            }

            foreach (var path in graph.SelectMany(x => x))
            {
                warshallFloydDp[path.From][path.To] = path.Cost;
            }

            for (int k = nIndexed; k < loopCount; k++)
            {
                for (int i = nIndexed; i < loopCount; i++)
                {
                    for (int j = nIndexed; j < loopCount; j++)
                    {
                        if (warshallFloydDp[i][k] + warshallFloydDp[k][j] < warshallFloydDp[i][j])
                        {
                            warshallFloydPathPrev[i][j] = warshallFloydPathPrev[k][j];
                            warshallFloydDp[i][j] = warshallFloydDp[i][k] + warshallFloydDp[k][j];
                        }
                    }
                }
            }

            return warshallFloydDp;
        }
        
        public IEnumerable<(int,int)> GetWarshallFloydPaths(int start, int goal)
        {
            var stack = new Stack<int>();
            for (int cur = goal; cur != start; cur = warshallFloydPathPrev[start][cur])
            {
                stack.Push(cur);
            }

            int a = start;
            while (stack.Count != 0)
            {
                int c = stack.Pop();
                yield return (a, c);
                a = c;
            }
        }
    }
}