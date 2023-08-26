using System;
using System.Collections.Generic;

namespace Qlibrary
{
    public class FibonacciSearch
    {
        private static readonly List<long> Fib = new List<long> { 1, 2 };
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

        public (long Index, long Value) SolveMax(Func<long, long> func)
        {
            long index = -1;
            long val = -Common.Infinity;
            if (range <= 5)
            {
                for (long i = 1; i <= range; i++)
                {
                    long c = CheckMax(i + min, func);
                    if (c <= val) continue;
                    val = c;
                    index = i + min;
                }
                return (index, val);
            }
            long cl = 0, cr = Fib[^1], dl = Fib[^3], dr = Fib[^2];
            long el = CheckMax(dl + min, func);
            long er = CheckMax(dr + min, func);
            if (el < er)
            {
                cl = dl;
                dl = dr;
                dr = -1;
                el = er;
                er = -1;
            }
            else
            {
                cr = dr;
                dr = dl;
                dl = -1;
                er = el;
                el = -1;
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
                    er = -1;
                }
                else
                {
                    cr = dr;
                    dr = dl;
                    dl = -1;
                    er = el;
                    el = -1;
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

        private long CheckMax(long value, Func<long, long> func)
        {
            if (value > max || value <= min) return -Common.Infinity;
            return func(value);
        }
        
        public (long Index, long Value) SolveMin(Func<long, long> func)
        {
            long index = -1;
            long val = Common.Infinity;
            if (range <= 5)
            {
                for (long i = 1; i <= range; i++)
                {
                    var c = CheckMin(i + min, func);
                    if (c >= val) continue;
                    val = c;
                    index = i + min;
                }
                return (index, val);
            }
            long cl = 0, cr = Fib[^1], dl = Fib[^3], dr = Fib[^2];
            long el = CheckMin(dl + min, func);
            long er = CheckMin(dr + min, func);
            if (el > er)
            {
                cl = dl;
                dl = dr;
                dr = -1;
                el = er;
                er = -1;
            }
            else
            {
                cr = dr;
                dr = dl;
                dl = -1;
                er = el;
                el = -1;
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
                    er = -1;
                }
                else
                {
                    cr = dr;
                    dr = dl;
                    dl = -1;
                    er = el;
                    el = -1;
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

        private long CheckMin(long value, Func<long, long> func)
        {
            if (value > max || value <= min) return Common.Infinity;
            return func(value);
        }
        
    }
}