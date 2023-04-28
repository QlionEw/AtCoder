using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using static Qlibrary.Common;
using static System.Math;

namespace Qlibrary
{
    public class SegmentTreeBeats
    {
        private class Node
        {
            public long Sum { get; set; }
            public long G1 { get; set; }
            public long L1 { get; set; }
            public long G2 { get; set; } = -Infinity;
            public long Gc { get; set; } = 1;
            public long L2 { get; set; } = Infinity;
            public long Lc { get; set; } = 1;
            public long Add { get; set; }
        }

        private readonly Node[] v;
        private readonly int n;
        private readonly int log;

        public SegmentTreeBeats()
        {
        }

        public SegmentTreeBeats(int n) : this(new long[n])
        {
        }

        public SegmentTreeBeats(IEnumerable<long> vv)
        {
            var vc = vv.ToArray();
            n = 1;
            log = 0;
            while (n < vc.Length)
            {
                n <<= 1;
                log++;
            }

            v = Make(2 * n, () => new Node());
            for (int i = 0; i < vc.Length; ++i) { v[i + n].Sum = v[i + n].G1 = v[i + n].L1 = vc[i]; }

            for (int i = n - 1; i > 0; --i) Update(i);
        }

        [MethodImpl(256)] public void UpdateMin(int l, int r, long x) => InnerApply(l, r + 1, x, 1);
        [MethodImpl(256)] public void UpdateMax(int l, int r, long x) => InnerApply(l, r + 1, x, 2);
        [MethodImpl(256)] public void Add(int l, int r, long x) => InnerApply(l, r + 1, x, 3);
        [MethodImpl(256)] public void SetValue(int l, int r, long x) => InnerApply(l, r + 1, x, 4);
        [MethodImpl(256)] public long GetRangeMin(int l, int r) => InnerFold(l, r + 1, 1);
        [MethodImpl(256)] public long GetRangeMax(int l, int r) => InnerFold(l, r + 1, 2);
        [MethodImpl(256)] public long GetRangeSum(int l, int r) => InnerFold(l, r + 1, 3);

        [MethodImpl(256)]
        private void Update(int k)
        {
            Node p = v[k];
            Node l = v[k * 2 + 0];
            Node r = v[k * 2 + 1];
            p.Sum = l.Sum + r.Sum;
            if (l.G1 == r.G1)
            {
                p.G1 = l.G1;
                p.G2 = Max(l.G2, r.G2);
                p.Gc = l.Gc + r.Gc;
            }
            else
            {
                bool f = l.G1 > r.G1;
                p.G1 = f ? l.G1 : r.G1;
                p.Gc = f ? l.Gc : r.Gc;
                p.G2 = Max(f ? r.G1 : l.G1, f ? l.G2 : r.G2);
            }

            if (l.L1 == r.L1)
            {
                p.L1 = l.L1;
                p.L2 = Min(l.L2, r.L2);
                p.Lc = l.Lc + r.Lc;
            }
            else
            {
                bool f = l.L1 < r.L1;
                p.L1 = f ? l.L1 : r.L1;
                p.Lc = f ? l.Lc : r.Lc;
                p.L2 = Min(f ? r.L1 : l.L1, f ? l.L2 : r.L2);
            }
        }
        
        [MethodImpl(256)] 
        private void PushAdd(int k, long x)
        {
            Node p = v[k];
            p.Sum += x << (log + BitOperations.LeadingZeroCount((uint)k) - 31);
            p.G1 += x;
            p.L1 += x;
            if (p.G2 != -Infinity) p.G2 += x;
            if (p.L2 != Infinity) p.L2 += x;
            p.Add += x;
        }

        [MethodImpl(256)] 
        private void PushMin(int k, long x)
        {
            Node p = v[k];
            p.Sum += (x - p.G1) * p.Gc;
            if (p.L1 == p.G1) p.L1 = x;
            if (p.L2 == p.G1) p.L2 = x;
            p.G1 = x;
        }

        [MethodImpl(256)] 
        private void PushMax(int k, long x)
        {
            Node p = v[k];
            p.Sum += (x - p.L1) * p.Lc;
            if (p.G1 == p.L1) p.G1 = x;
            if (p.G2 == p.L1) p.G2 = x;
            p.L1 = x;
        }

        [MethodImpl(256)] 
        private void Push(int k)
        {
            Node p = v[k];
            if (p.Add != 0)
            {
                PushAdd(k * 2 + 0, p.Add);
                PushAdd(k * 2 + 1, p.Add);
                p.Add = 0;
            }

            if (p.G1 < v[k * 2 + 0].G1) PushMin(k * 2 + 0, p.G1);
            if (p.L1 > v[k * 2 + 0].L1) PushMax(k * 2 + 0, p.L1);
            if (p.G1 < v[k * 2 + 1].G1) PushMin(k * 2 + 1, p.G1);
            if (p.L1 > v[k * 2 + 1].L1) PushMax(k * 2 + 1, p.L1);
        }

        [MethodImpl(256)] 
        private void SubtreeChMin(int k, long x)
        {
            if (v[k].G1 <= x) return;
            if (v[k].G2 < x)
            {
                PushMin(k, x);
                return;
            }

            Push(k);
            SubtreeChMin(k * 2 + 0, x);
            SubtreeChMin(k * 2 + 1, x);
            Update(k);
        }

        [MethodImpl(256)] 
        private void SubtreeChMax(int k, long x)
        {
            if (x <= v[k].L1) return;
            if (x < v[k].L2)
            {
                PushMax(k, x);
                return;
            }

            Push(k);
            SubtreeChMax(k * 2 + 0, x);
            SubtreeChMax(k * 2 + 1, x);
            Update(k);
        }

        [MethodImpl(256)] 
        private void Apply(int k, long x, int cmd)
        {
            if (cmd == 1) SubtreeChMin(k, x);
            if (cmd == 2) SubtreeChMax(k, x);
            if (cmd == 3) PushAdd(k, x);
            if (cmd == 4)
            {
                SubtreeChMin(k, x);
                SubtreeChMax(k, x);
            }
        }

        [MethodImpl(256)] 
        private void InnerApply(int l, int r, long x, int cmd)
        {
            if (l == r) return;
            l += n;
            r += n;
            for (int i = log; i >= 1; i--)
            {
                if (((l >> i) << i) != l) Push(l >> i);
                if (((r >> i) << i) != r) Push((r - 1) >> i);
            }

            {
                int l2 = l, r2 = r;
                while (l < r)
                {
                    if ((l & 1) == 1) Apply(l++, x, cmd);
                    if ((r & 1) == 1) Apply(--r, x, cmd);
                    l >>= 1;
                    r >>= 1;
                }

                l = l2;
                r = r2;
            }
            for (int i = 1; i <= log; i++)
            {
                if (((l >> i) << i) != l) Update(l >> i);
                if (((r >> i) << i) != r) Update((r - 1) >> i);
            }
        }

        [MethodImpl(256)] 
        private static long E(int cmd)
        {
            if (cmd == 1) return Infinity;
            if (cmd == 2) return -Infinity;
            return 0;
        }

        [MethodImpl(256)] 
        private static long Op(long a, Node b, int cmd)
        {
            if (cmd == 1) a = Min(a, b.L1);
            if (cmd == 2) a = Max(a, b.G1);
            if (cmd == 3) a += b.Sum;
            return a;
        }

        [MethodImpl(256)] 
        private long InnerFold(int l, int r, int cmd)
        {
            if (l == r) return E(cmd);
            l += n;
            r += n;
            for (int i = log; i >= 1; i--)
            {
                if (((l >> i) << i) != l) Push(l >> i);
                if (((r >> i) << i) != r) Push((r - 1) >> i);
            }

            long lx = E(cmd), rx = E(cmd);
            while (l < r)
            {
                if ((l & 1) == 1) lx = Op(lx, v[l++], cmd);
                if ((r & 1) == 1) rx = Op(rx, v[--r], cmd);
                l >>= 1;
                r >>= 1;
            }

            if (cmd == 1) lx = Min(lx, rx);
            if (cmd == 2) lx = Max(lx, rx);
            if (cmd == 3) lx += rx;
            return lx;
        }
    }
}