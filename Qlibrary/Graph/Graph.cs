using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static Qlibrary.Common;
using Cost = System.Int64;

namespace Qlibrary
{
    public struct Edge : IComparable<Edge>, IEquatable<Edge>
    {
        public int From { get; set; }
        public int To { get; set; }
        public Cost Cost { get; set; }
        public int Index { get; set; }

        public static bool operator ==(Edge a, Edge b) => a.Equals(b);
        public static bool operator !=(Edge a, Edge b) => !(a == b);
        public int CompareTo(Edge other) => Cost - other.Cost > 0 ? 1 : (Cost - other.Cost) < 0 ? -1 : 0;
        public bool Equals(Edge other) => From == other.From && To == other.To;
        public override bool Equals(object obj) => obj is Edge other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(From, To);
    }
    
    public class Graph : IEnumerable<List<Edge>>
    {
        private readonly long fromIndex = MathPlus.BigPow(10, 9);
        private readonly List<Edge>[] graph;
        private Dictionary<long, long>[] additionalInfo;
        public int Count { get; }
        public int PathCount { get; private set; }
        public bool IsDirected { get;}

        public Graph(int n, bool isDirected)
        {
            Count = n + 1;
            graph = Make(Count, () => new List<Edge>());
            IsDirected = isDirected;
        }

        public Graph(int n, int[][] paths, bool isDirected) : this(n, isDirected)
        {
            SetPath(paths);
        }
        
        private void SetPath(int[][] paths)
        {
            for (int i = 0; i < paths.Length; i++)
            {
                var path = paths[i];
                int cost = path.Length == 2 ? 1 : path[2];
                AddPath(path[0], path[1], cost);
            }
        }

        private void SetAdditionalInfo(params int[] v)
        {
            for (int i = 0; i < v.Length - 2; i++)
            {
                additionalInfo[i].Add(fromIndex * v[0] + v[1], v[i + 2]);
                if (!IsDirected) additionalInfo[i].Add(fromIndex * v[1] + v[0], v[i + 2]);
            }
        }

        public void SetMatrix(int[][] paths, int indexed = 1)
        {
            for (int i = 0; i < Count; i++)
            {
                for (int j = 0; j < Count; j++)
                {
                    if (paths[i][j] == 0) continue;
                    graph[i + indexed].Add(new Edge{ From = i + indexed, To = j + indexed, Cost = paths[i][j] });
                }
            }
        }
        
        public List<Edge> this[int edge] => graph[edge];
        public IEnumerator<List<Edge>> GetEnumerator() => graph.AsEnumerable().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public void AddPath(int p, int q, Cost cost)
        {
            PathCount++;
            graph[p].Add(new Edge{ From = p, To = q, Cost = cost, Index = PathCount });
            if (!IsDirected) graph[q].Add(new Edge{ From = q, To = p, Cost = cost, Index = PathCount });
        } 
    }
}