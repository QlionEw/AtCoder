using System;
using System.Collections.Generic;
using System.Linq;

namespace Qlibrary
{
    public class GraphSolver
    {
        public struct PathInfo : IComparable<PathInfo>
        {
            public int From { get; set; }
            public int To { get; set; }
            public long Cost { get; set; }

            public int CompareTo(PathInfo other)
            {
                return (int) (Cost - other.Cost);
            }
        }

        private readonly List<PathInfo>[] pathInfos;
        public long[] Distances { get; private set; }
        private int pathCount = 0;
        private readonly int nodeCount;

        public GraphSolver(int nodeCount)
        {
            this.nodeCount = nodeCount;
            Distances = Enumerable.Repeat(Common.Infinity, nodeCount + 1).ToArray();
            pathInfos = Enumerable.Repeat(0, nodeCount + 1).Select(_ => new List<PathInfo>()).ToArray();
        }

        public void Init()
        {
            Distances = Enumerable.Repeat(Common.Infinity, Distances.Length).ToArray();
        }

        public void AddDirectedPath(int from, int to, long cost, params long[] additionalInfo)
        {
            AddPath(from, to, cost, additionalInfo);
        }
        
        public void AddUndirectedPath(int from, int to, long cost, params long[] additionalInfo)
        {
            AddPath(from, to, cost, additionalInfo);
            AddPath(to, from, cost, additionalInfo);
        }

        private void AddPath(int from, int to, long cost, params long[] additionalInfo)
        {
            pathCount++;
            pathInfos[from].Add(new PathInfo {From = from, To = to, Cost = cost});
        }

        public void Dijkstra(int point)
        {
            PriorityQueue<PathInfo> queue = new PriorityQueue<PathInfo>(pathCount + 1);
            Distances[point] = 0;
            queue.Enqueue(new PathInfo {To = point, Cost = 0});

            while (queue.Count != 0)
            {
                PathInfo pop = queue.Dequeue();
                if (Distances[pop.To] < pop.Cost) { continue; }

                foreach (PathInfo path in pathInfos[pop.To])
                {
                    long nextValue = Distances[pop.To] + path.Cost;
                    if (Distances[path.To] > nextValue)
                    {
                        Distances[path.To] = nextValue;
                        queue.Enqueue(new PathInfo {From = path.From, To = path.To, Cost = Distances[path.To]});
                    }
                }
            }
        }

        private bool[] isVisited; 
        public long _01Bfs(int start, int end)
        {
            Deque<int> deque = new Deque<int>();
            isVisited = new bool[nodeCount];
            Distances[start] = 0;
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
                foreach (PathInfo path in pathInfos[index])
                {
                    long d = Distances[index] + path.Cost;
                    if (d < Distances[path.To])
                    {
                        Distances[path.To] = d;
                        if (path.Cost != 0)
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

            return -1;
        }

        public long Kruskal()
        {
            var edges = pathInfos.SelectMany(pathInfo => pathInfo).OrderBy(x => x.Cost).ToList();
            long totalCost = 0;
            var uft = new UnionFindTree(nodeCount + 1);
            foreach (PathInfo edge in edges.Where(edge => !uft.Same(edge.From, edge.To)))
            {
                uft.Union(edge.From, edge.To);
                totalCost += edge.Cost;
            }
            return totalCost;
        }

        private PathInfo[] bellmanFordList;
        public bool[] IsLoop { get; private set; }

        public void BellmanFord(int point, bool isDetectLoop = false)
        {
            bellmanFordList = pathInfos.SelectMany(x => x).ToArray();
            IsLoop = Enumerable.Repeat(false, nodeCount + 1).ToArray();
            Distances[point] = 0;
            int count;
            for (count = 0; count < Distances.Length; count++)
            {
                bool isUpdated = false;
                foreach (PathInfo path in bellmanFordList)
                {
                    if (Distances[path.From] == Common.Infinity) { continue; }

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
                foreach (PathInfo path in bellmanFordList)
                {
                    if (Distances[path.From] == Common.Infinity) { continue; }

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
        private List<PathInfo>[] reversePathInfos;
        private int[] sccGroups;

        public IEnumerable<int> StronglyConnectedComponent()
        {
            sccUsed = new bool[nodeCount + 1];
            sccOrder = new List<int>();
            reversePathInfos = Enumerable.Repeat(0, nodeCount + 1).Select(_ => new List<PathInfo>()).ToArray();
            foreach (var path in pathInfos.SelectMany(x => x))
            {
                reversePathInfos[path.To].Add(new PathInfo {From = path.To, To = path.From, Cost = path.Cost});
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
            foreach (var to in pathInfos[index].Select(x => x.To))
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

        private long[][] warshallFloydDp;
        private int[][] warshallFloydPathPrev;

        public long[][] WarshallFloyd(int nIndexed)
        {
            var loopCount = nodeCount + nIndexed;
            warshallFloydDp = Enumerable.Repeat(0, loopCount)
                .Select(_ => Enumerable.Repeat(Common.Infinity, loopCount).ToArray()).ToArray();
            for (int i = 0; i < loopCount; i++)
            {
                warshallFloydDp[i][i] = 0;
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

            foreach (var path in pathInfos.SelectMany(x => x))
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
                            warshallFloydDp[i][j] = Math.Min(warshallFloydDp[i][j],
                                warshallFloydDp[i][k] + warshallFloydDp[k][j]);
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