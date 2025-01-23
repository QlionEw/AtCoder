using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Qlibrary
{
    public class Sweep
    {
        private const int Max = 63; // MAX bit
        private readonly long[] v = new long[Max];
        public int Size { get; private set; }

        public Sweep()
        {
            Size = 0;
        }

        public Sweep(IEnumerable<long> a) : this()
        {
            foreach (var x in a)
            {
                Add(x);
            }
        }

        public long this[int i] => v[i];

        // 追加成功かどうかを返す
        public bool Add(long x)
        {
            int t = Msb(x);
            if (t >= Max) throw new ArgumentOutOfRangeException(nameof(x));
            while (t != -1 && v[t] != 0)
            {
                t = Msb(x ^= v[t]);
            }
            if (t == -1) return false;
            v[t] = x;
            Size++;
            return true;
        }

        // rhs との和を求める
        public void Merge(Sweep rhs)
        {
            for (int t = Max - 1; t >= 0; t--)
            {
                if (rhs.v[t] == 0) continue;
                int _t = t;
                long x = rhs.v[_t];
                while (_t != -1 && v[_t] != 0)
                {
                    _t = Msb(x ^= v[_t]);
                }
                if (x != 0)
                {
                    v[_t] = x;
                    Size++;
                }
            }
        }

        // 正規化された基底を得る (O(MAX^2))
        public List<long> GetBasis()
        {
            Normalize();
            List<long> res = new List<long>();
            for (int t = 0; t < Max; t++)
            {
                if (v[t] != 0)
                {
                    res.Add(v[t]);
                }
            }
            res.Reverse();
            return res;
        }

        // x を作れるか？
        public bool Test(long x)
        {
            if (x == 0) return true;
            int t = Msb(x);
            if (t >= Max) return false;
            while (t != -1 && v[t] != 0)
            {
                t = Msb(x ^= v[t]);
            }
            return x == 0;
        }

        // 作れる x の最大値
        public long GetMax()
        {
            long res = 0;
            for (int t = Max - 1; t >= 0; t--)
            {
                res = Math.Max(res, res ^ v[t]);
            }
            return res;
        }

        public (long maxValue, List<long> contributingElements) GetMaxWithOriginalElements()
        {
            long res = 0;
            List<long> contributingElements = new List<long>();

            for (int t = Max - 1; t >= 0; t--)
            {
                if ((res ^ v[t]) > res)
                {
                    res ^= v[t];
                    contributingElements.Add(v[t]);
                }
            }

            return (res, contributingElements);
        }

        // 行列を標準化する
        private void Normalize()
        {
            for (int t = Max - 1; t >= 0; t--)
            {
                if (v[t] != 0)
                {
                    for (int u = Max - 1; u > t; u--)
                    {
                        v[u] = Math.Min(v[u], v[u] ^ v[t]);
                    }
                }
            }
        }

        public List<long> OrthogonalComplement(int N = Max)
        {
            Normalize();
            List<int> b = new List<int>();
            for (int t = N - 1; t >= 0; t--)
            {
                if (v[t] != 0) b.Add(t);
            }
            int rank = b.Count;
            for (int t = N - 1; t >= 0; t--)
            {
                if (v[t] == 0) b.Add(t);
            }
            List<long> res = new List<long>(new long[N - rank]);
            for (int i = rank; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if ((j < rank && ((v[b[j]] >> b[i]) & 1) != 0) || i == j)
                    {
                        res[i - rank] |= 1L << b[j];
                    }
                }
            }
            return res;
        }


        public override string ToString()
        {
            return "{ " + string.Join(", ", v.Where(x => x != 0)) + " }";
        }

        private static int Msb(long x)
        {
            return x != 0 ? 63 - BitOperations.LeadingZeroCount((ulong)x) : -1;
        }
    }

}