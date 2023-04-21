using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static Qlibrary.Common;

namespace Qlibrary
{
    public struct Edge : IComparable<Edge>, IEquatable<Edge>
    {
        public int From { get; set; }
        public int To { get; set; }
        public long Cost { get; set; }
        public int Index { get; set; }

        public static bool operator ==(Edge a, Edge b) => a.Equals(b);
        public static bool operator !=(Edge a, Edge b) => !(a == b);
        public int CompareTo(Edge other) => (int) (Cost - other.Cost);
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
        
        public void SetPath(int[][] paths)
        {
            for (int i = 0; i < paths.Length; i++)
            {
                var path = paths[i];
                int cost = path.Length == 2 ? 1 : path[2];
                graph[path[0]].Add(new Edge{ From = path[0], To = path[1], Cost = cost, Index = i });
                if (!IsDirected) graph[path[1]].Add(new Edge{ From = path[1], To = path[0], Cost = cost, Index = i });
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
    }
}