using System;
using System.Text;

namespace Qlibrary
{
    public class DualSegmentTree<T> where T : IEquatable<T>
    {
        private int count;
        private T firstValue;
        private Func<T, T, T> updateMethod;
        private int sz;
        private int height;
        private T[] lazy;

        public void Init(int n, T firstValue, Func<T, T, T> updateMethod)
        {
            this.count = n;
            this.firstValue = firstValue;
            this.updateMethod = updateMethod;
            sz = 1;
            height = 0;
            while (sz < n)
            {
                sz <<= 1;
                height++;
            }
            lazy = new T[2 * sz];
            Array.Fill(lazy, firstValue);
        }

        public void Build(Func<int, T> update)
        {
            for (int i = 0; i < count; i++)
            {
                lazy[sz + i] = update(i);
            }
        }

        private void Thrust(int k)
        {
            for (int i = height; i > 0; i--)
            {
                if (lazy[k].Equals(firstValue)) continue;
                lazy[(k << 1) + 0] = updateMethod(lazy[(k << 1) + 0], lazy[k]);
                lazy[(k << 1) + 1] = updateMethod(lazy[(k << 1) + 1], lazy[k]);
                lazy[k] = firstValue;
            }
        }

        public T Get(int k)
        {
            Thrust(k += sz);
            return lazy[k];
        }

        public T this[int k] => Get(k);

        public void Update(int a, int b, T f)
        {
            if (a > b) return;
            if (a < 0) a = 0;
            if (b > count) b = count - 1;
            b++;
            Thrust(a += sz);
            Thrust(b += sz - 1);
            for (int l = a, r = b + 1; l < r; l >>= 1, r >>= 1)
            {
                if ((l & 1) == 1)
                {
                    lazy[l] = updateMethod(lazy[l], f);
                    l++;
                }
                if ((r & 1) == 1)
                {
                    --r;
                    lazy[r] = updateMethod(lazy[r], f);
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                sb.Append($"Index {i} ... ");
                sb.AppendLine(Get(i).ToString());
            }
            return sb.ToString();
        }
    }
}