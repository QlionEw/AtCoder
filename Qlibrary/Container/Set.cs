using System;
using System.Collections.Generic;

namespace Qlibrary
{
    public class Set<T>
    {
        private Node root;
        private readonly IComparer<T> comparer;
        private readonly Node nil;
        public bool IsMultiSet { get; set; }

        public Set(IComparer<T> comparer, bool isMultiSet = false)
        {
            nil = new Node(default(T));
            root = nil;
            this.comparer = comparer;
            IsMultiSet = isMultiSet;
        }

        public Set(Comparison<T> comparision, bool isMultiSet = false) : this(Comparer<T>.Create(comparision), isMultiSet)
        {
        }

        public Set(bool isMultiSet) : this(Comparer<T>.Default, isMultiSet)
        {
        }
        
        public Set(IEnumerable<T>items , bool isMultiSet) : this(Comparer<T>.Default, isMultiSet)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }
        
        public Set() : this(false)
        {
        }

        public bool Add(T v)
        {
            return Insert(ref root, v);
        }

        public bool Remove(T v)
        {
            return Remove(ref root, v);
        }

        public T this[int index] => Find(root, index);
        public int Count => root.Count;

        public void RemoveAt(int k)
        {
            if (k < 0 || k >= root.Count) throw new ArgumentOutOfRangeException();
            RemoveAt(ref root, k);
        }

        public T[] Items
        {
            get
            {
                var ret = new T[root.Count];
                var k = 0;
                Walk(root, ret, ref k);
                return ret;
            }
        }

        private static void Walk(Node t, IList<T> a, ref int k)
        {
            while (true)
            {
                if (t.Count == 0) return;
                Walk(t.Lst, a, ref k);
                a[k++] = t.Key;
                t = t.Rst;
            }
        }

        private bool Insert(ref Node t, T key)
        {
            if (t.Count == 0)
            {
                t = new Node(key);
                t.Lst = t.Rst = nil;
                t.Update();
                return true;
            }

            var cmp = comparer.Compare(t.Key, key);
            bool res;
            if (cmp > 0)
                res = Insert(ref t.Lst, key);
            else if (cmp == 0)
            {
                if (IsMultiSet) res = Insert(ref t.Lst, key);
                else return false;
            }
            else res = Insert(ref t.Rst, key);

            Balance(ref t);
            return res;
        }

        private bool Remove(ref Node t, T key)
        {
            if (t.Count == 0) return false;
            var cmp = comparer.Compare(key, t.Key);
            bool ret;
            if (cmp < 0) ret = Remove(ref t.Lst, key);
            else if (cmp > 0) ret = Remove(ref t.Rst, key);
            else
            {
                ret = true;
                var k = t.Lst.Count;
                if (k == 0)
                {
                    t = t.Rst;
                    return true;
                }

                if (t.Rst.Count == 0)
                {
                    t = t.Lst;
                    return true;
                }


                t.Key = Find(t.Lst, k - 1);
                RemoveAt(ref t.Lst, k - 1);
            }

            Balance(ref t);
            return ret;
        }

        private void RemoveAt(ref Node t, int k)
        {
            var cnt = t.Lst.Count;
            if (cnt < k) RemoveAt(ref t.Rst, k - cnt - 1);
            else if (cnt > k) RemoveAt(ref t.Lst, k);
            else
            {
                if (cnt == 0)
                {
                    t = t.Rst;
                    return;
                }

                if (t.Rst.Count == 0)
                {
                    t = t.Lst;
                    return;
                }

                t.Key = Find(t.Lst, k - 1);
                RemoveAt(ref t.Lst, k - 1);
            }

            Balance(ref t);
        }

        private static void Balance(ref Node t)
        {
            var balance = t.Lst.Height - t.Rst.Height;
            if (balance == -2)
            {
                if (t.Rst.Lst.Height - t.Rst.Rst.Height > 0) { RotR(ref t.Rst); }

                RotL(ref t);
            }
            else if (balance == 2)
            {
                if (t.Lst.Lst.Height - t.Lst.Rst.Height < 0) RotL(ref t.Lst);
                RotR(ref t);
            }
            else t.Update();
        }

        private T Find(Node t, int k)
        {
            if (k < 0 || k > root.Count) throw new ArgumentOutOfRangeException();
            for (;;)
            {
                if (k == t.Lst.Count) return t.Key;
                else if (k < t.Lst.Count) t = t.Lst;
                else
                {
                    k -= t.Lst.Count + 1;
                    t = t.Rst;
                }
            }
        }

        public int LowerBound(T v)
        {
            var k = 0;
            var t = root;
            for (;;)
            {
                if (t.Count == 0) return k - 1;
                if (comparer.Compare(v, t.Key) <= 0) t = t.Lst;
                else
                {
                    k += t.Lst.Count + 1;
                    t = t.Rst;
                }
            }
        }

        public int UpperBound(T v)
        {
            var k = 0;
            var t = root;
            for (;;)
            {
                if (t.Count == 0) return k;
                if (comparer.Compare(t.Key, v) <= 0)
                {
                    k += t.Lst.Count + 1;
                    t = t.Rst;
                }
                else t = t.Lst;
            }
        }

        private static void RotR(ref Node t)
        {
            var l = t.Lst;
            t.Lst = l.Rst;
            l.Rst = t;
            t.Update();
            l.Update();
            t = l;
        }

        private static void RotL(ref Node t)
        {
            var r = t.Rst;
            t.Rst = r.Lst;
            r.Lst = t;
            t.Update();
            r.Update();
            t = r;
        }


        private class Node
        {
            public Node(T key)
            {
                Key = key;
            }

            public int Count { get; private set; }
            public sbyte Height { get; private set; }
            public T Key { get; set; }
            public Node Lst, Rst;

            public void Update()
            {
                Count = 1 + Lst.Count + Rst.Count;
                Height = (sbyte) (1 + Math.Max(Lst.Height, Rst.Height));
            }

            public override string ToString()
            {
                return $"Count = {Count}, Key = {Key}";
            }
        }
    }
}