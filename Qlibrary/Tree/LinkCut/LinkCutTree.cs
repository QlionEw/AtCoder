using System;

namespace Qlibrary
{
    public class LinkCutTree<T>
    {
        public ReversibleSplayTree<T> Splay;

        public LinkCutTree(Func<T, T, T> f, Func<T, T> t)
        {
            Splay = new ReversibleSplayTree<T>(f, t);
        }

        public LinkCutNode<T> Expose(LinkCutNode<T> t)
        {
            LinkCutNode<T> rp = null;
            for (LinkCutNode<T> cur = t; cur != null; cur = cur.Parent)
            {
                Splay.Splay(cur);
                cur.Right = rp;
                Splay.Update(cur);
                rp = cur;
            }
            Splay.Splay(t);
            return rp;
        }

        public virtual void Link(LinkCutNode<T> u, LinkCutNode<T> v)
        {
            Evert(u);
            Expose(v);
            u.Parent = v;
        }

        public void Cut(LinkCutNode<T> u, LinkCutNode<T> v)
        {
            Evert(u);
            Expose(v);
            if (u.Parent != v)
                throw new Exception($"u is not a child of v, {u.Parent.GetHashCode()}, {v.GetHashCode()}");
            v.Left = u.Parent = null;
            Splay.Update(v);
        }

        public void Evert(LinkCutNode<T> t)
        {
            Expose(t);
            Splay.Toggle(t);
            Splay.Push(t);
        }

        public LinkCutNode<T> Lca(LinkCutNode<T> u, LinkCutNode<T> v)
        {
            if (GetRoot(u) != GetRoot(v)) return null;
            Expose(u);
            return Expose(v);
        }

        public LinkCutNode<T> GetKth(LinkCutNode<T> x, int k)
        {
            Expose(x);
            while (x != null)
            {
                Splay.Push(x);
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

        public LinkCutNode<T> GetRoot(LinkCutNode<T> x)
        {
            Expose(x);
            while (x.Left != null)
            {
                Splay.Push(x);
                x = x.Left;
            }
            return x;
        }

        public LinkCutNode<T> GetParent(LinkCutNode<T> x)
        {
            Expose(x);
            LinkCutNode<T> p = x.Left;
            if (p == null) return null;
            while (true)
            {
                Splay.Push(p);
                if (p.Right == null) return p;
                p = p.Right;
            }
        }

        public virtual void SetKey(LinkCutNode<T> t, T key)
        {
            Splay.Splay(t);
            t.Key = key;
            Splay.Update(t);
        }

        public virtual T GetKey(LinkCutNode<T> t)
        {
            return t.Key;
        }

        public T Fold(LinkCutNode<T> u, LinkCutNode<T> v)
        {
            Evert(u);
            Expose(v);
            return v.Sum;
        }
    }
}