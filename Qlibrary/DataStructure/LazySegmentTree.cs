using System;
using System.Linq;

namespace Qlibrary
{
    public enum LazySegmentTreeMode
    {
        UpdateMin,
        UpdateMax,
        Addition,
        ModAddition,
    }

    public class LazySegmentTree : LazySegmentTreeExtend<long>
    {
        public LazySegmentTree(int count, LazySegmentTreeMode mode) : base(count, GetInvalidValue(mode))
        {
            SetMode(mode);
        }

        private void SetMode(LazySegmentTreeMode mode)
        {
            UpdateMethod = mode switch
            {
                LazySegmentTreeMode.UpdateMin => Math.Min,
                LazySegmentTreeMode.UpdateMax => Math.Max,
                LazySegmentTreeMode.ModAddition => (l1, l2) => (l1 + l2) % ModInt.ModValue,
                _ => (l1, l2) => (l1 + l2),
            };
            UpdateLazyToActualMethod = mode switch
            {
                LazySegmentTreeMode.Addition => (l1, l2, count) => l1 + l2 * count ,
                LazySegmentTreeMode.ModAddition => (l1, l2, count) => (l1 + (l2 * count) % ModInt.ModValue) % ModInt.ModValue,
                _ => (x, m, _) => m
            };
            SetLazyDataMethod = mode switch
            {
                LazySegmentTreeMode.Addition => (l1, l2) => (l1 + l2),
                LazySegmentTreeMode.ModAddition => (l1, l2) => (l1 + l2) % ModInt.ModValue,
                _ => (m1, m2) => m2
            };
        }

        private static long GetInvalidValue(LazySegmentTreeMode mode) =>
            mode switch
            {
                LazySegmentTreeMode.UpdateMin => Common.Infinity,
                LazySegmentTreeMode.UpdateMax => -Common.Infinity,
                _ => 0
            };
    }
    
    public class LazySegmentTreeExtend<T> where T : IEquatable<T>
    {
        private T[] data;
        private T[] lazyData;
        public Func<T, T, T> UpdateMethod { get; set; }
        public Func<T, T, int, T> UpdateLazyToActualMethod { get; set; }
        public Func<T, T, T> SetLazyDataMethod { get; set; }
        public T InvalidValue { get; set; }
        private int n;
        private int count;

        public LazySegmentTreeExtend(int count, T invalidValue = default)
        {
            this.count = count;
            InvalidValue = invalidValue;

            n = 1;
            while (n < count)
            {
                n *= 2;
            }

            data = Enumerable.Repeat(InvalidValue, 2 * n - 1).ToArray();
            lazyData = Enumerable.Repeat(InvalidValue, 2 * n - 1).ToArray();
        }
        
        public void Build(Func<int, T> update)
        {
            for (int i = 0; i < count; i++)
            {
                data[n + i - 1] = update(i);
            }
            for (int i = n - 2; i >= 0; i--)
            {
                data[i] = UpdateMethod(data[2 * i + 1], data[2 * i + 2]);
            }
        }

        private void Evaluate(int k)
        {
            if (lazyData[k].Equals(InvalidValue)) {return;}

            if (k < n - 1)
            {
                lazyData[2 * k + 1] = SetLazyDataMethod(lazyData[2 * k + 1], lazyData[k]);
                lazyData[2 * k + 2] = SetLazyDataMethod(lazyData[2 * k + 2], lazyData[k]);
            }

            int cc = k switch
            {
                0 => n,
                _ => 1 << (MathPlus.Digit(n, 2) - MathPlus.Digit(k+1, 2))
            };
            data[k] = UpdateLazyToActualMethod(data[k], lazyData[k], cc);
            lazyData[k] = InvalidValue;
        }

        public void Update(int left, int right, T value)
        {
            Update(left, right + 1, value, 0, 0, n);
        }

        private void Update(int a, int b, T x, int k, int l, int r)
        {
            Evaluate(k);
            if (r <= a || b <= l) {return;}
            if (a <= l && r <= b)
            {
                lazyData[k] = SetLazyDataMethod(lazyData[k], x);
                Evaluate(k);
            }
            else
            {
                Update(a, b, x, 2 * k + 1, l, (l + r) / 2);
                Update(a, b, x, 2 * k + 2, (l + r) / 2, r);
                data[k] = UpdateMethod(data[2 * k + 1], data[2 * k + 2]);
            }
        }

        public T Query(int left, int right)
        {
            return Query(left, right + 1, 0, 0, n);
        }

        private T Query(int a, int b, int k, int l, int r)
        {
            Evaluate(k);
            if (r <= a || b <= l) return InvalidValue;
            if (a <= l && r <= b) return data[k];
            T vl = Query(a, b, 2 * k + 1, l, (l + r) / 2);
            T vr = Query(a, b, 2 * k + 2, (l + r) / 2, r);
            return UpdateMethod(vl, vr);
        }
    }
}