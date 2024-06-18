using System;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    public class LinkCutTree<T>
    {
        private readonly ReversibleSplayTree<T> splay;

        public LinkCutTree(Func<T, T, T> f, Func<T, T> t)
        {
            splay = new ReversibleSplayTree<T>(f, t);
        }

        [MethodImpl(256)]
        public LinkCutNode<T> Expose(LinkCutNode<T> t)
        {
            LinkCutNode<T> rp = null;
            for (LinkCutNode<T> cur = t; cur != null; cur = cur.Parent)
            {
                splay.Splay(cur);
                cur.Right = rp;
                splay.Update(cur);
                rp = cur;
            }
            splay.Splay(t);
            return rp;
        }

        /// <summary>頂点u,vをつなぐ </summary>
        [MethodImpl(256)]
        public virtual void Link(LinkCutNode<T> u, LinkCutNode<T> v)
        {
            Evert(u);
            Expose(v);
            u.Parent = v;
        }

        /// <summary>頂点u,vを切り離す </summary>
        [MethodImpl(256)]
        public void Cut(LinkCutNode<T> u, LinkCutNode<T> v)
        {
            Evert(u);
            Expose(v);
            if (u.Parent != v)
                throw new Exception($"u is not a child of v, {u.Parent.GetHashCode()}, {v.GetHashCode()}");
            v.Left = u.Parent = null;
            splay.Update(v);
        }

        /// <summary>頂点uを木全体の根とする </summary>
        [MethodImpl(256)]
        public void Evert(LinkCutNode<T> t)
        {
            Expose(t);
            splay.Toggle(t);
            splay.Push(t);
        }

        /// <summary> u と v のLCAを返す </summary>
        [MethodImpl(256)]
        public LinkCutNode<T> Lca(LinkCutNode<T> u, LinkCutNode<T> v)
        {
            if (GetRoot(u) != GetRoot(v)) return null;
            Expose(u);
            return Expose(v);
        }

        /// <summary> 根方向にK進んだノードを取得 </summary>
        [MethodImpl(256)]
        public LinkCutNode<T> GetKth(LinkCutNode<T> x, int k)
        {
            Expose(x);
            while (x != null)
            {
                splay.Push(x);
                if (x.Right != null && x.Right.Count > k)
                {
                    x = x.Right;
                }
                else
                {
                    if (x.Right != null) k -= x.Right.Count;
                    if (k == 0) return x;
                    k -= 1;
                    x = x.Left;
                }
            }
            return null;
        }

        /// <summary> uの根を返す </summary>
        [MethodImpl(256)]
        public LinkCutNode<T> GetRoot(LinkCutNode<T> x)
        {
            Expose(x);
            while (x.Left != null)
            {
                splay.Push(x);
                x = x.Left;
            }
            return x;
        }

        /// <summary> 根方向にK進んだノードを取得 </summary>
        [MethodImpl(256)]
        public LinkCutNode<T> GetParent(LinkCutNode<T> x)
        {
            Expose(x);
            LinkCutNode<T> p = x.Left;
            if (p == null) return null;
            while (true)
            {
                splay.Push(p);
                if (p.Right == null) return p;
                p = p.Right;
            }
        }

        /// <summary> 頂点uのkeyを書き換える </summary>
        [MethodImpl(256)]
        public virtual void SetKey(LinkCutNode<T> t, T key)
        {
            splay.Splay(t);
            t.Key = key;
            splay.Update(t);
        }

        /// <summary> 頂点uのkeyの値を得る </summary>
        [MethodImpl(256)]
        public virtual T GetKey(LinkCutNode<T> t)
        {
            return t.Key;
        }

        /// <summary> u, v間のパスのkeyの和を得る </summary>
        [MethodImpl(256)]
        public T Fold(LinkCutNode<T> u, LinkCutNode<T> v)
        {
            Evert(u);
            Expose(v);
            return v.Sum;
        }
    }
}