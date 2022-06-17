using System;
using System.Collections.Generic;
using System.Linq;

namespace Qlibrary
{
    public class Flow
    {
        private class FlowEdge
        {
            public int To { get; set; }
            public long Capacity { get; set; }
            public long Cost { get; set; }
            public int Reverse { get; set; }
        }

        private List<FlowEdge>[] graphs;
        private int[] level;
        private int[] iter;
        private int size;

        public Flow(int size)
        {
            this.size = size;
            graphs = Enumerable.Range(0, size+1).Select(_ => new List<FlowEdge>()).ToArray();
        }

        public void AddEdge(int from, int to, long capacity, long cost = 0)
        {
            graphs[from].Add(new FlowEdge{ To = to, Capacity = capacity, Cost = cost, Reverse = graphs[to].Count});
            graphs[to].Add(new FlowEdge{ To = from, Capacity = 0, Cost = -cost, Reverse = graphs[from].Count - 1});
        }

        public long GetMaxFlow(int from, int to)
        {
            long flow = 0;
            while (true)
            {
                FlowBfs(from);
                if (level[to] < 0)
                {
                    return flow;
                }
                iter = new int[size+1];
                long f;
                while ((f = FlowDfs(from, to, Common.Infinity)) > 0)
                {
                    flow += f;
                }
            }
        }

        public class PathInfo : IComparable<PathInfo>
        {
            public long Distance { get; set; }
            public int Number { get; set; }

            public int CompareTo(PathInfo other)
            {
                if (ReferenceEquals(this, other)) return 0;
                if (ReferenceEquals(null, other)) return 1;
                return Distance.CompareTo(other.Distance);
            }
        }
        
        private long[] distance;
        private long[] h;
        private long[] prevv;
        private long[] preve;
        public long GetMinCostFlow(int s, int t, long f)
        {
            long res = 0;
            h = new long[size+1];
            distance = new long[size+1];
            prevv = new long[size+1];
            preve = new long[size+1];
            while (f > 0)
            {
                PriorityQueue<PathInfo> queue = new PriorityQueue<PathInfo>(2 * size + 1);
                Array.Fill(distance, Common.Infinity);
                distance[s] = 0;
                queue.Enqueue(new PathInfo {Distance = 0, Number = s});

                while (queue.Count != 0)
                {
                    PathInfo pop = queue.Dequeue();
                    int v = pop.Number;
                    if (distance[v] < pop.Distance) { continue; }

                    for (int i = 0; i < graphs[v].Count; i++)
                    {
                        FlowEdge e = graphs[v][i];
                        if (e.Capacity > 0 && distance[e.To] > distance[v] + e.Cost + h[v] - h[e.To])
                        {
                            distance[e.To] = distance[v] + e.Cost + h[v] - h[e.To];
                            prevv[e.To] = v;
                            preve[e.To] = i;
                            queue.Enqueue(new PathInfo{ Distance = distance[e.To], Number = e.To});
                        }
                    }
                }

                if (distance[t] == Common.Infinity)
                {
                    return -1;
                }

                for (int i = 0; i < size+1; i++)
                {
                    h[i] += distance[i];
                }

                long d = f;
                for (long i = t; i != s; i = prevv[i])
                {
                    d = Math.Min(d, graphs[prevv[i]][(int)preve[i]].Capacity);
                }

                f -= d;
                res += d * h[t];
                for (long i = t; i != s; i = prevv[i])
                {
                    var e = graphs[prevv[i]][(int)preve[i]];
                    e.Capacity -= d;
                    graphs[i][e.Reverse].Capacity += d;
                }
            }

            return res;
        }

        private long FlowDfs(int from, int to, long flow)
        {
            if (from == to)
            {
                return flow;
            }
            for (; iter[from] < graphs[from].Count; iter[from]++)
            {
                int i = iter[from];
                var e = graphs[from][i];
                if (e.Capacity <= 0 || level[from] >= level[e.To]) {continue;}
                long d = FlowDfs(e.To, to, Math.Min(flow, e.Capacity));
                if (d <= 0) {continue;}
                e.Capacity -= d;
                graphs[e.To][e.Reverse].Capacity += d;
                return d;
            }
            return 0;
        }

        private void FlowBfs(int from)
        {
            level = Enumerable.Repeat(-1, size + 1).ToArray();
            var q = new Queue<int>();
            level[from] = 0;
            q.Enqueue(from);
            while (q.Count != 0)
            {
                int v = q.Dequeue();
                for (int i = 0; i < graphs[v].Count; i++)
                {
                    var e = graphs[v][i];
                    if (e.Capacity <= 0 || level[e.To] >= 0) {continue;}
                    level[e.To] = level[v] + 1;
                    q.Enqueue(e.To);
                }
            }
        }

        public long BipartiteMatching(int indexStart, int part1Size, int part2Size)
        {
            int start = size - 2;
            int end = size - 1;
            for (int i = indexStart; i < indexStart + part1Size; i++)
            {
                AddEdge(start, i, 1);
            }
            for (int i = indexStart + part1Size; i < indexStart + part1Size + part2Size; i++)
            {
                AddEdge(i, end, 1);
            }

            return GetMaxFlow(start, end);
        }
    }
}