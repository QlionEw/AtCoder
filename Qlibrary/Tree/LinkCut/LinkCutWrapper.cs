using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using static Qlibrary.Common;

namespace Qlibrary
{
    /// <summary>
    ///  Link-Cut Treeをインデックスで操作できるようにしたラッパークラス
    /// Dictionaryを作る分若干遅いので注意
    /// </summary>
    public class LinkCutWrapper : LinkCutWrapper<int>
    {
        public LinkCutWrapper(int n) : 
            base(Enumerable.Range(1, n).ToArray())
        {
        }
    }
    
    public class LinkCutWrapper<T>
    {
        private LinkCutTree<T> tree;
        private LinkCutNode<T>[] nodes;
        private Dictionary<LinkCutNode<T>, int> indexDict = new();

        public LinkCutWrapper(T[] array) : this(array, (_, _) => default, _ => default)
        {
        }

        public LinkCutWrapper(T[] array, Func<T, T, T> mergeFunc, Func<T, T> toggleFunc)
        {
            nodes = new LinkCutNode<T>[array.Length + 1];
            nodes[0] = new LinkCutNode<T>(default);
            for (int i = 1; i <= array.Length; i++)
            {
                nodes[i] = new LinkCutNode<T>(array[i-1]);
            }
            Build(array.Length, mergeFunc, toggleFunc);
        }

        private void Build(int n, Func<T, T, T> mergeFunc, Func<T, T> toggleFunc)
        {
            tree = new LinkCutTree<T>(mergeFunc, toggleFunc);
            for (int i = 1; i <= n; i++)
            {
                indexDict.Add(nodes[i], i);
            }
        }
        
        public LinkCutNode<T> this[int index]
        {
            [MethodImpl(256)] get => nodes[index];
            [MethodImpl(256)] set => nodes[index] = value;
        }

        [MethodImpl(256)]
        public int Expose(int index) => indexDict.GetValueOrDefault(tree.Expose(this[index]), -1);

        /// <summary>頂点u,vをつなぐ </summary>
        [MethodImpl(256)]
        public virtual void Link(int u, int v) => tree.Link(this[u], this[v]);

        /// <summary>頂点u,vを切り離す </summary>
        [MethodImpl(256)]
        public void Cut(int u, int v) => tree.Cut(this[u], this[v]);

        /// <summary>頂点uを木全体の根とする </summary>
        [MethodImpl(256)]
        public void Evert(int index) => tree.Evert(this[index]);

        /// <summary> u と v のLCAを返す </summary>
        [MethodImpl(256)]
        public int Lca(int u, int v) => GetIndex(tree.Lca(this[u], this[v]));

        /// <summary> 根方向にK進んだノードを取得 </summary>
        [MethodImpl(256)]
        public int GetKth(int index, int k) => GetIndex(tree.GetKth(this[index], k));

        /// <summary> uの根を返す </summary>
        [MethodImpl(256)]
        public int GetRoot(int index) => GetIndex(tree.GetRoot(this[index]));

        /// <summary> 親を取得 </summary>
        [MethodImpl(256)]
        public int GetParent(int index) => GetIndex(tree.GetParent(this[index]));

        /// <summary> 頂点uのkeyを書き換える </summary>
        [MethodImpl(256)]
        public virtual void SetKey(int index, T key) => tree.SetKey(this[index], key);

        /// <summary> 頂点uのkeyの値を得る </summary>
        [MethodImpl(256)]
        public virtual T GetKey(int index) => tree.GetKey(this[index]);

        /// <summary> u, v間のパスのkeyの和を得る </summary>
        [MethodImpl(256)]
        public T Fold(int u, int v) => tree.Fold(this[u], this[v]);

        [MethodImpl(256)]
        private int GetIndex(LinkCutNode<T> node) => node != null ? indexDict[node] : -1;
    }
}