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

        // return a[k]
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

        // k-th (0-indexed) smallest number in a[l, r)
        public long KthSmallest(int l, int r, int k)
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

        // k-th (0-indexed) largest number in a[l, r)
        public long KthLargest(int l, int r, int k)
        {
            return KthSmallest(l, r, r - l - k - 1);
        }

        // count i s.t. (l <= i < r) && (v[i] < upper)
        public int RangeFreq(int l, int r, long upper)
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

        public int RangeFreq(int l, int r, long lower, long upper)
        {
            return RangeFreq(l, r, upper) - RangeFreq(l, r, lower);
        }

        // max v[i] s.t. (l <= i < r) && (v[i] < upper)
        public long PrevValue(int l, int r, long upper)
        {
            int cnt = RangeFreq(l, r, upper);
            return cnt == 0 ? 0 : KthSmallest(l, r, cnt - 1);
        }

        // min v[i] s.t. (l <= i < r) && (lower <= v[i])
        public long NextValue(int l, int r, long lower)
        {
            int cnt = RangeFreq(l, r, lower);
            return cnt == r - l ? 0 : KthSmallest(l, r, cnt);
        }
        

        class BitVector
        {
            private const uint W = 64;
            private List<ulong> block;
            private List<uint> count;
            private uint n;
            public uint Zeros { get; set; }

            [MethodImpl(256)]
            public uint Get(uint i) => (uint)(block[(int)(i / W)] >> (int)(i % W)) & 1u;

            [MethodImpl(256)]
            public void Set(uint i) => block[(int)(i / W)] |= 1UL << (int)(i % W);

            public BitVector()
            {
            }

            public BitVector(int n) => Init(n);

            [MethodImpl(256)]
            private void Init(int n)
            {
                this.n = Zeros = (uint)n;
                block = Enumerable.Repeat(0UL, n / (int)W + 1).ToList();
                count = Enumerable.Repeat(0U, block.Count).ToList();
            }

            [MethodImpl(256)]
            public void Build()
            {
                for (int i = 1; i < block.Count; ++i)
                    count[i] = count[i - 1] + (uint)BitOperations.PopCount(block[i - 1]);
                Zeros = Rank0(n);
            }

            [MethodImpl(256)]
            public uint Rank0(uint i) => i - Rank1(i);

            [MethodImpl(256)]
            public uint Rank1(uint i)
            {
                ulong val = block[(int)(i / W)];
                if (i%W == 0)
                {
                    val = 0;
                }
                else
                {
                    val <<= (int)(64 - (i % W));
                }

                return count[(int)(i / W)] + (uint)BitOperations.PopCount(val);
            }
        }
    }
}