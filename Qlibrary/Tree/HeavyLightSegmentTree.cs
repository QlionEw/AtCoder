using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using static Qlibrary.Common;

namespace Qlibrary
{
    public class HeavyLightSegmentTree<T> : HeavyLightDecomposition<T> where T : INumber<T>
    {
        // Edge
        private bool isInitForEdge;
        private T edgeFirstValue;
        private SegmentTreeExtend<T> segTree;
        private readonly Dictionary<(int,int), int> edgeToIndex = new();
        private Func<T, T, T> update;
        // Vertex
        private bool isInitForVertex;
        private T vertexFirstValue;
        private SegmentTreeExtend<T> vertexSegTree;
        private int[] vertexToIndex;
        private Func<T, T, T> updateVertex;

        public HeavyLightSegmentTree(Graph<T> graph, int root = 1) : base(graph, root)
        {
        }

        private void InitEdgeInner(T firstValue)
        {
            isInitForEdge = true;
            edgeFirstValue = firstValue;
            for (int i = 0; i < Tour.Count; i++)
            {
                edgeToIndex.Add(SorTuple(Tour[i]), i);
            }
        }

        [MethodImpl(256)]
        public void InitEdge(T firstValue, Func<T, T, T> updateMethod)
        {
            InitEdgeInner(firstValue);
            update = updateMethod;
            segTree = new SegmentTreeExtend<T>();
            segTree.Init(Tour.Count, firstValue, updateMethod);
        }

        [MethodImpl(256)]
        public void InitEdgeFromGraph(T firstValue, Func<T, T, T> updateMethod)
        {
            InitEdgeInner(firstValue);
            update = updateMethod;
            segTree = new SegmentTreeExtend<T>();
            segTree.Init(Tour.Count, firstValue, updateMethod);
            for (int i = 0; i < Graph.Count; i++)
            {
                foreach (var to in Graph[i])
                {
                    if (edgeToIndex.TryGetValue((i, to.To), out var value))
                    {
                        segTree.SetValue(value, to.Cost);
                    }
                }
            }
        }

        [MethodImpl(256)]
        public void InitVertex(T firstValue, Func<T, T, T> updateMethod)
        {
            isInitForVertex = true;
            vertexFirstValue = firstValue;
            vertexToIndex = new int[Graph.Count];
            for (int i = 0; i < Graph.Count; i++)
            {
                var index = Index(i);
                if (index.In == -1) continue;
                vertexToIndex[i] = index.In;
            }
            updateVertex = updateMethod;
            vertexSegTree = new SegmentTreeExtend<T>();
            vertexSegTree.Init(Graph.Count, firstValue, updateMethod);
        }

        [MethodImpl(256)]
        public void InitVertex(T[] values, T firstValue, Func<T, T, T> updateMethod)
        {
            InitVertex(firstValue, updateMethod);
            var ar = new int[Graph.Count];
            for (int i = 0; i < Graph.Count - 1; i++)
            {
                ar[vertexToIndex[i]] = i;
            }
            vertexSegTree.Build(i => values[ar[i]]);
        }

        [MethodImpl(256)]
        public void SetValueOfEdge(int u, int v, T value)
        {
            Debug.Assert(isInitForEdge);
            segTree.SetValue(edgeToIndex[SorTuple(u, v)], value);
        }

        [MethodImpl(256)]
        public T QueryEdgesOfPath(int u, int v)
        {
            Debug.Assert(isInitForEdge);
            T value = edgeFirstValue;
            QueryPath(u, v, false, (i1, i2) => value = update(value, segTree.Query(i1, i2 - 1)));
            return value;
        }

        [MethodImpl(256)]
        public T QueryEdgesOfSubTree(int u)
        {
            Debug.Assert(isInitForEdge);
            T value = edgeFirstValue;
            QuerySubtree(u, true, (i1, i2) => value = update(value, segTree.Query(i1, i2 - 1)));
            return value;
        }

        [MethodImpl(256)]
        public void SetValueOfVertex(int v, T value)
        {
            Debug.Assert(isInitForVertex);
            vertexSegTree.SetValue(vertexToIndex[v], value);
        }

        [MethodImpl(256)]
        public T QueryVertexesOfPath(int u, int v)
        {
            Debug.Assert(isInitForVertex);
            T value = vertexFirstValue;
            QueryPath(u, v, true, (i1, i2) => value = updateVertex(value, vertexSegTree.Query(i1, i2 - 1)));
            return value;
        }

        [MethodImpl(256)]
        public T QueryVertexesOfSubTree(int u)
        {
            Debug.Assert(isInitForVertex);
            T value = vertexFirstValue;
            QuerySubtree(u, true, (i1, i2) => value = updateVertex(value, vertexSegTree.Query(i1, i2 - 1)));
            return value;
        }
    }
}