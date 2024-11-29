using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using static Qlibrary.Common;

namespace Qlibrary
{
    public class WaveletMatrix
    {
        private readonly int n;
        private int lg;
        private readonly long[] a;
        private List<BitVector> bv;

        public WaveletMatrix(int n)
        {
            this.n = n;
            a = new long[n];
            Build();
        }

        public WaveletMatrix(IEnumerable<int> a) : this(a.Select(x => (long)x))
        {
        }

        public WaveletMatrix(IEnumerable<long> a)
        {
            this.a = a.ToArray();
            n = this.a.Length;
            Build();
        }

        private void Build()
        {
            lg = BitOperations.Log2((ulong)Math.Max(a.Max(), 1)) + 1;
            bv = Make(lg, () => new BitVector(n)).ToList();
            List<long> cur = new List<long>(a);
            List<long> nxt = Enumerable.Repeat(0L, n).ToList();
            for (int h = lg - 1; h >= 0; --h)
            {
                for (int i = 0; i < n; ++i)
                    if (((cur[i] >> h) & 1) != 0)
                        bv[h].Set((uint)i);
                bv[h].Build();
                int index0 = 0;
                int index1 = (int)bv[h].Zeros;
                for (int i = 0; i < n; ++i)
                {
                    if (bv[h].Get((uint)i) == 0)
                    {
                        nxt[index0++] = cur[i];
                    }
                    else
                    {
                        nxt[index1++] = cur[i];
                    }
                }

                (cur, nxt) = (nxt, cur);
            }
        }

        [MethodImpl(256)]
        public void Set(int i, long x) => a[i] = x;

        [MethodImpl(256)]
        (uint, uint) Succ0(int l, int r, int h) => (bv[h].Rank0((uint)l), bv[h].Rank0((uint)r));

        [MethodImpl(256)]
        (uint, uint) Succ1(int l, int r, int h)
        {
            uint l0 = bv[h].Rank0((uint)l);
            uint r0 = bv[h].Rank0((uint)r);
            uint zeros = bv[h].Zeros;
            return ((uint)l + zeros - l0, (uint)r + zeros - r0);
        }

        /// k番目の要素の値を得る。
        [MethodImpl(256)]
        public long Access(int k)
        {
            long ret = 0;
            uint kk = (uint)k;
            for (int h = lg - 1; h >= 0; --h)
            {
                var f = bv[h].Get(kk);
                if (f == 1)
                    ret |= (1L << h);
                if (f == 1)
                    kk = bv[h].Rank1(kk) + bv[h].Zeros;
                else
                    kk = bv[h].Rank0(kk);
            }

            return ret;
        }

        /// [l, r]の範囲でk(1-indexed)番目に小さい値を返す。
        [MethodImpl(256)] public long GetKthSmallest(int l, int r, int k) => KthSmallest(l, r + 1, k - 1);
        /// [l, r]の範囲でk(1-indexed)番目に大きい値を返す。
        [MethodImpl(256)] public long GetKthLargest(int l, int r, int k) => KthLargest(l, r + 1, k - 1);
        /// [l, r]の範囲でupper以下の要素の個数を返す。
        [MethodImpl(256)] public int GetRangeCount(int l, int r, long upper) => RangeFreq(l, r + 1, upper + 1);
        /// [l, r]の範囲でlower以上upper以下の要素の個数を返す。
        [MethodImpl(256)] public int GetRangeCount(int l, int r, long lower, long upper) => RangeFreq(l, r + 1, lower, upper + 1);
        /// [l, r]の範囲でupper以下の最後の値を返す。
        [MethodImpl(256)] public long GetPreviousValue(int l, int r, long upper) => PrevValue(l, r + 1, upper + 1);
        /// [l, r]の範囲でlower以上の最後の値を返す。
        [MethodImpl(256)] public long GetNextValue(int l, int r, long lower) => NextValue(l, r + 1, lower);

        [MethodImpl(256)]
        private long KthSmallest(int l, int r, int k)
        {
            long res = 0;
            uint ll = (uint)l, rr = (uint)r, kk = (uint)k;
            for (int h = lg - 1; h >= 0; --h)
            {
                var l0 = bv[h].Rank0(ll);
                var r0 = bv[h].Rank0(rr);
                if (kk < r0 - l0)
                {
                    ll = l0;
                    rr = r0;
                }
                else
                {
                    kk -= r0 - l0;
                    res |= (1L << h);
                    ll += bv[h].Zeros - l0;
                    rr += bv[h].Zeros - r0;
                }
            }

            return res;
        }


        [MethodImpl(256)]
        private long KthLargest(int l, int r, int k) => KthSmallest(l, r, r - l - k - 1);


        [MethodImpl(256)]
        private int RangeFreq(int l, int r, long upper)
        {
            if (upper >= (1L << lg)) return r - l;
            int ret = 0;
            for (int h = lg - 1; h >= 0; --h)
            {
                bool f = ((upper >> h) & 1) == 1;
                var l0 = bv[h].Rank0((uint)l);
                var r0 = bv[h].Rank0((uint)r);
                if (f)
                {
                    ret += (int)(r0 - l0);
                    l += (int)(bv[h].Zeros - l0);
                    r += (int)(bv[h].Zeros - r0);
                }
                else
                {
                    l = (int)l0;
                    r = (int)r0;
                }
            }

            return ret;
        }

        [MethodImpl(256)]
        public int RangeFreq(int l, int r, long lower, long upper)
        {
            return RangeFreq(l, r, upper) - RangeFreq(l, r, lower);
        }

        [MethodImpl(256)]
        private long PrevValue(int l, int r, long upper)
        {
            int cnt = RangeFreq(l, r, upper);
            return cnt == 0 ? 0 : KthSmallest(l, r, cnt - 1);
        }

        [MethodImpl(256)]
        public long NextValue(int l, int r, long lower)
        {
            int cnt = RangeFreq(l, r, lower);
            return cnt == r - l ? 0 : KthSmallest(l, r, cnt);
        }
    }
}