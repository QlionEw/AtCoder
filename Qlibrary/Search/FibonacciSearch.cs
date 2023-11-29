using System;
using System.Collections.Generic;
using System.Numerics;

namespace Qlibrary
{
    public class FibonacciSearch : FibonacciSearch<long>
    {
        public FibonacciSearch(long min, long max) : base(min, max)
        {
        }
    }
    
    public class FibonacciSearch<T> where T : INumber<T>
    {
        private static readonly List<long> Fib = new() { 1, 2 };
        private long max;
        private long min;
        private long range;
        private int n;

        public FibonacciSearch()
        {
        }

        public FibonacciSearch(long min, long max)
        {
            SetRange(min, max);
        }

        public void SetRange(long min, long max)
        {
            this.min = min - 1;
            this.max = max;
            range = max - min + 1;
            while (Fib[^1] < range)
            {
                Fib.Add(Fib[^1] + Fib[^2]);
            }
            n = Fib.Count;
        }

        public (long Index, T Value) SolveMax(Func<long, T> func)
        {
            long index = -1;
            T val = -Common.GetInfinity<T>();
            if (range <= 5)
            {
                for (long i = 1; i <= range; i++)
                {
                    T c = CheckMax(i + min, func);
                    if (c <= val) continue;
                    val = c;
                    index = i + min;
                }
                return (index, val);
            }
            long cl = 0, cr = Fib[^1], dl = Fib[^3], dr = Fib[^2];
            T el = CheckMax(dl + min, func);
            T er = CheckMax(dr + min, func);
            if (el < er)
            {
                cl = dl;
                dl = dr;
                dr = -1;
                el = er;
                er = T.CreateChecked(-1);
            }
            else
            {
                cr = dr;
                dr = dl;
                dl = -1;
                er = el;
                el = T.CreateChecked(-1);
            }

            for (int i = n - 4; i >= 0; i--)
            {
                if (dl == -1)
                {
                    dl = cl + Fib[i];
                    el = CheckMax(dl + min, func);
                }
                else if (dr == -1)
                {
                    dr = cr - Fib[i];
                    er = CheckMax(dr + min, func);
                }
                if (el < er)
                {
                    cl = dl;
                    dl = dr;
                    dr = -1;
                    el = er;
                    er = T.CreateChecked(-1);
                }
                else
                {
                    cr = dr;
                    dr = dl;
                    dl = -1;
                    er = el;
                    el = T.CreateChecked(-1);
                }
            }

            long ans = cl + 1 + min;
            index = ans;
            for (long i = ans - 1; i <= ans + 1; i++)
            {
                var c = CheckMax(i, func);
                if (c <= val) continue;
                val = c;
                index = i;
            }
            return (index, val);
        }

        private T CheckMax(long value, Func<long, T> func)
        {
            if (value > max || value <= min) return -Common.GetInfinity<T>();
            return func(value);
        }
        
        public (long Index, T Value) SolveMin(Func<long, T> func)
        {
            long index = -1;
            T val = Common.GetInfinity<T>();
            if (range <= 5)
            {
                for (long i = 1; i <= range; i++)
                {
                    T c = CheckMin(i + min, func);
                    if (c >= val) continue;
                    val = c;
                    index = i + min;
                }
                return (index, val);
            }
            long cl = 0, cr = Fib[^1], dl = Fib[^3], dr = Fib[^2];
            T el = CheckMin(dl + min, func);
            T er = CheckMin(dr + min, func);
            if (el > er)
            {
                cl = dl;
                dl = dr;
                dr = -1;
                el = er;
                er = T.CreateChecked(-1);
            }
            else
            {
                cr = dr;
                dr = dl;
                dl = -1;
                er = el;
                el = T.CreateChecked(-1);
            }

            for (int i = n - 4; i >= 0; i--)
            {
                if (dl == -1)
                {
                    dl = cl + Fib[i];
                    el = CheckMin(dl + min, func);
                }
                else if (dr == -1)
                {
                    dr = cr - Fib[i];
                    er = CheckMin(dr + min, func);
                }
                if (el > er)
                {
                    cl = dl;
                    dl = dr;
                    dr = -1;
                    el = er;
                    er = T.CreateChecked(-1);
                }
                else
                {
                    cr = dr;
                    dr = dl;
                    dl = -1;
                    er = el;
                    el = T.CreateChecked(-1);
                }
            }

            long ans = cl + 1 + min;
            index = ans;
            for (long i = ans - 1; i <= ans + 1; i++)
            {
                var c = CheckMin(i, func);
                if (c >= val) continue;
                val = c;
                index = i;
            }
            return (index, val);
        }

        private T CheckMin(long value, Func<long, T> func)
        {
            if (value > max || value <= min) return Common.GetInfinity<T>();
            return func(value);
        }
        
    }
}