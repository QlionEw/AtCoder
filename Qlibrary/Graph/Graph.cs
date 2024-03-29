using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using static Qlibrary.Common;

namespace Qlibrary
{
    public readonly struct Edge<T> : IComparable<Edge<T>>, IEquatable<Edge<T>> where T : INumber<T>
    {
        public int From { get; }
        public int To { get; }
        public T Cost { get; }

        public Edge(int to, T cost) : this(-1, to, cost)
        {
        }
        
        public Edge(int from, int to, T cost)
        {
            From = from;
            To = to;
            Cost = cost;
        }

        public static bool operator ==(Edge<T> a, Edge<T> b) => a.Equals(b);
        public static bool operator !=(Edge<T> a, Edge<T> b) => !(a == b);
        public int CompareTo(Edge<T> other) => Cost - other.Cost > T.Zero ? 1 : (Cost - other.Cost) < T.Zero ? -1 : 0;
        public bool Equals(Edge<T> other) => From == other.From && To == other.To;
        public override bool Equals(object obj) => obj is Edge<T> other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(From, To);
    }

    public class Graph<T> : IEnumerable<Edge<T>[]> where T : INumber<T>
    {
        private readonly long fromIndex = MathPlus.BigPow(10, 9);
        private Edge<T>[][] graph;
        private readonly List<Edge<T>>[] tempGraph;
        private Dictionary<long, long>[] additionalInfo;
        public int Count { get; }
        public int PathCount { get; private set; }
        public bool IsDirected { get;}

        public Graph(int n, bool isDirected)
        {
            Count = n + 1;
            tempGraph = Make(Count, () => new List<Edge<T>>());
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
                AddPath(path[0], path[1], T.CreateChecked(cost));
            }
        }
        
        public void AddPath(int p, int q, T cost)
        {
            PathCount++;
            tempGraph[p].Add(new Edge<T>(p, q, cost));
            if (!IsDirected) tempGraph[q].Add(new Edge<T>(q, p, cost));
        } 

        private void SetAdditionalInfo(params int[] v)
        {
            additionalInfo = Make(v.Length + 1, () => new Dictionary<long, long>());
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
                    tempGraph[i + indexed].Add(new Edge<T>(i + indexed, j + indexed, T.CreateChecked(paths[i][j])));
                }
            }
        }
        
        public Edge<T>[][] GetGraph()
        {
            if(graph == null) graph = Array.ConvertAll(tempGraph, x => x.ToArray());
            return graph;
        }
        
        public Edge<T>[] this[int edge] => GetGraph()[edge];
        public IEnumerator<Edge<T>[]> GetEnumerator() => GetGraph().AsEnumerable().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}