using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using static Qlibrary.Common;

namespace Qlibrary
{
    public class LazyHeavyLightTree<T> : HeavyLightDecomposition<T> where T : INumber<T>
    {
        // Edge
        private bool isInitForEdge;
        private T edgeFirstValue;
        private LazySegmentTree<T> segTree;
        private readonly Dictionary<(int,int), int> edgeToIndex = new();
        public Func<(T A, T B), T> OperationEdge
        {
            get => segTree.Operation;
            set => segTree.Operation = value;
        }
        public Func<(T Current, T Lazy, int length), T> MappingEdge { set => segTree.Mapping = value; }
        public Func<(T F, T G), T> CompositionEdge { set => segTree.Composition = value; }
        // Vertex
        private bool isInitForVertex;
        private T vertexFirstValue;
        private LazySegmentTree<T> vertexSegTree;
        private int[] vertexToIndex;
        public Func<(T A, T B), T> OperationVertex
        {
            get => vertexSegTree.Operation;
            set => vertexSegTree.Operation = value;
        }
        public Func<(T Current, T Lazy, int length), T> MappingVertex { set => vertexSegTree.Mapping = value; }
        public Func<(T F, T G), T> CompositionVertex { set => vertexSegTree.Composition = value; }
        
        public LazyHeavyLightTree(Graph<T> graph, int root = 1) : base(graph, root)
        {
        }
        
        [MethodImpl(256)]
        public void InitEdge(T firstValue = default)
        {
            isInitForEdge = true;
            edgeFirstValue = firstValue;
            for (int i = 0; i < Tour.Count; i++)
            {
                edgeToIndex.Add(SorTuple(Tour[i]), i);
            }
            segTree = new LazySegmentTree<T>(Tour.Count, firstValue);
        }
        
        [MethodImpl(256)]
        public void BuildEdge()
        {
            for (int i = 0; i < Graph.Count; i++)
            {
                foreach (var to in Graph[i])
                {
                    if (edgeToIndex.TryGetValue((i, to.To), out var value))
                    {
                        segTree.Update(value, value, to.Cost);
                    }
                }
            }
        }
        
        [MethodImpl(256)]
        public void UpdateEdgesOfPath(int u, int v, T value)
        {
            Debug.Assert(isInitForEdge);
            QueryPath(u, v, false, (i1, i2) => segTree.Update(i1, i2 - 1, value));
        }
        
        [MethodImpl(256)]
        public void UpdateEdgesOfSubTree(int u, int v, T value)
        {
            Debug.Assert(isInitForEdge);
            QuerySubtree(u, false, (i1, i2) => segTree.Update(i1, i2 - 1, value));
        }
        
        [MethodImpl(256)]
        public T QueryEdgesOfPath(int u, int v)
        {
            Debug.Assert(isInitForEdge);
            T value = edgeFirstValue;
            QueryPath(u, v, false, (i1, i2) => value = OperationEdge((value, segTree.Query(i1, i2 - 1))));
            return value;
        }

        [MethodImpl(256)]
        public T QueryEdgesOfSubTree(int u)
        {
            Debug.Assert(isInitForEdge);
            T value = edgeFirstValue;
            QuerySubtree(u, false, (i1, i2) => value = OperationEdge((value, segTree.Query(i1, i2 - 1))));
            return value;
        }
        
        [MethodImpl(256)]
        public void InitVertex(T firstValue = default)
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
            vertexSegTree = new LazySegmentTree<T>(Graph.Count, firstValue);
        }

        [MethodImpl(256)]
        public void BuildVertex(T[] values)
        {
            var ar = new int[Graph.Count];
            for (int i = 0; i < Graph.Count - 1; i++)
            {
                ar[vertexToIndex[i]] = i;
            }
            vertexSegTree.Build(i => values[ar[i]]);
        }
        
        [MethodImpl(256)]
        public void UpdateVertexOfPath(int u, int v, T value)
        {
            Debug.Assert(isInitForVertex);
            QueryPath(u, v, true, (i1, i2) => vertexSegTree.Update(i1, i2 - 1, value));
        }
        
        [MethodImpl(256)]
        public void UpdateVertexOfSubTree(int u, int v, T value)
        {
            Debug.Assert(isInitForVertex);
            QuerySubtree(u, true, (i1, i2) => vertexSegTree.Update(i1, i2 - 1, value));
        }
        
        [MethodImpl(256)]
        public T QueryVertexesOfPath(int u, int v)
        {
            Debug.Assert(isInitForVertex);
            T value = vertexFirstValue;
            QueryPath(u, v, true, (i1, i2) => value = OperationVertex((value, vertexSegTree.Query(i1, i2 - 1))));
            return value;
        }

        [MethodImpl(256)]
        public T QueryVertexesOfSubTree(int u)
        {
            Debug.Assert(isInitForVertex);
            T value = vertexFirstValue;
            QuerySubtree(u, true, (i1, i2) => value = OperationVertex((value, vertexSegTree.Query(i1, i2 - 1))));
            return value;
        }
    }
}