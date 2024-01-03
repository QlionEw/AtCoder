using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Qlibrary
{
    /// <summary>
    /// 遅延評価セグメント木
    /// </summary>
    /// <remarks>
    /// https://atcoder.github.io/ac-library/production/document_ja/lazysegtree.html
    /// https://qiita.com/katsumata_yusuke/items/2081690ae959c06b05a1
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class LazySegmentTree<T>
    {
        private T[] data;
        private T[] lazyData;
        /// <summary> 全ノードの集計方法を表す二項演算 </summary>
        /// <example> 最終的に和を得たいなら (a,b) = a + b のようにする </example>
        public Func<(T A, T B), T> Operation { get; set; }
        /// <summary> 自ノードの更新方法 f(x) 元の値を更新によりどのように保持しておくか  </summary>
        /// <example> 更新が置換なら (cur,laz) => laz, 乗算なら (f,g) => cur * laz など </example>
        public Func<(T Current, T Lazy), T> Mapping { get; set; }
        /// <summary> 更新区間が重なったときの更新の合成方法 f∘g </summary>
        /// <example> 更新が置換なら (f,g) => g, 乗算なら (f,g) => f * g など </example>
        public Func<(T F, T G), T> Composition { get; set; }
        /// <summary> 単位元 </summary>
        public T E { get; set; }
        private int n;
        private int count;
        private int height = 0;

        public LazySegmentTree(int count, T e = default)
        {
            this.count = count;
            E = e;

            n = 1;
            while (n < count)
            {
                n <<= 1;
                height++;
            }

            data = Enumerable.Repeat(E, 2 * n - 1).ToArray();
            lazyData = Enumerable.Repeat(E, 2 * n - 1).ToArray();
        }
        
        [MethodImpl(256)]
        public void Build(Func<int, T> update)
        {
            for (int i = 0; i < count; i++)
            {
                data[n + i - 1] = update(i);
            }
            for (int i = n - 2; i >= 0; i--)
            {
                data[i] = Operation((data[2 * i + 1], data[2 * i + 2]));
            }
        }

        [MethodImpl(256)]
        private void Evaluate(int k)
        {
            if (lazyData[k].Equals(E)) {return;}

            if (k < n - 1)
            {
                lazyData[2 * k + 1] = Composition((lazyData[2 * k + 1], lazyData[k]));
                lazyData[2 * k + 2] = Composition((lazyData[2 * k + 2], lazyData[k]));
            }

            // int cc = k switch
            // {
            //     0 => n,
            //     _ => 1 << (MathPlus.Digit(n, 2) - MathPlus.Digit(k+1, 2))
            // };
            data[k] = Mapping((data[k], lazyData[k]));
            lazyData[k] = E;
        }

        [MethodImpl(256)]
        public void Update(int left, int right, T value)
        {
            Update(left, right + 1, value, 0, 0, n);
        }

        [MethodImpl(256)]
        private void Update(int a, int b, T x, int k, int l, int r)
        {
            Evaluate(k);
            if (r <= a || b <= l) {return;}
            if (a <= l && r <= b)
            {
                lazyData[k] = Composition((lazyData[k], x));
                Evaluate(k);
            }
            else
            {
                Update(a, b, x, 2 * k + 1, l, (l + r) / 2);
                Update(a, b, x, 2 * k + 2, (l + r) / 2, r);
                data[k] = Operation((data[2 * k + 1], data[2 * k + 2]));
            }
        }

        [MethodImpl(256)]
        public T Query(int left, int right)
        {
            return Query(left, right + 1, 0, 0, n);
        }

        [MethodImpl(256)]
        private T Query(int a, int b, int k, int l, int r)
        {
            Evaluate(k);
            if (r <= a || b <= l) return E;
            if (a <= l && r <= b) return data[k];
            T vl = Query(a, b, 2 * k + 1, l, (l + r) / 2);
            T vr = Query(a, b, 2 * k + 2, (l + r) / 2, r);
            return Operation((vl, vr));
        }

        public override string ToString()
        {
            var v = 1 << height;
            int index = 0;
            var sb = new StringBuilder();
            while (v > 0)
            {
                sb.AppendLine($"=== Depth {index} ===");
                for (int i = 0; i < n / v; i++)
                {
                    sb.Append($"{v * i} to {v * i + v - 1} ... ");
                    sb.AppendLine(Query(v * i, v * i + v - 1).ToString());
                }
                v >>= 1;
                index++;
            }
            sb.AppendLine();
            return sb.ToString();
        }
    }
}