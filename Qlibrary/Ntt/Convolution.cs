using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static System.Math;

namespace Qlibrary
{
    public class Convolution
    {
        private static uint GetPr()
        {
            uint _mod = mod;
            ulong[] ds = new ulong[32];
            int idx = 0;
            ulong m = _mod - 1;
            for (ulong i = 2; i * i <= m; ++i)
            {
                if (m % i == 0)
                {
                    ds[idx++] = i;
                    while (m % i == 0) m /= i;
                }
            }

            if (m != 1) ds[idx++] = m;

            uint _pr = 2;
            while (true)
            {
                int flg = 1;
                for (int i = 0; i < idx; ++i)
                {
                    ulong a = _pr, b = (_mod - 1) / ds[i], r = 1;
                    while (b != 0)
                    {
                        if ((b & 1) == 1) r = r * a % _mod;
                        a = a * a % _mod;
                        b >>= 1;
                    }

                    if (r == 1)
                    {
                        flg = 0;
                        break;
                    }
                }

                if (flg == 1) break;
                ++_pr;
            }

            return _pr;
        }

        private static uint mod =>(uint)ModInt.ModValue;
        private static uint pr = GetPr();
        private static int level => BitOperations.LeadingZeroCount((ulong)(mod - 1));
        private ModInt[] dw = new ModInt[level];
        private ModInt[] dy = new ModInt[level];

        void setwy(int k)
        {
            ModInt[] w = new ModInt[level];
            ModInt[] y = new ModInt[level];
            w[k - 1] = ModInt.Pow(pr, (mod - 1) / (1 << k));
            y[k - 1] = 1 / w[k - 1];
            for (int i = k - 2; i > 0; --i)
            {
                w[i] = w[i + 1] * w[i + 1];
                y[i] = y[i + 1] * y[i + 1];
            }

            dw[1] = w[1];
            dy[1] = y[1];
            dw[2] = w[2];
            dy[2] = y[2];
            for (int i = 3; i < k; ++i)
            {
                dw[i] = dw[i - 1] * y[i - 2] * w[i];
                dy[i] = dy[i - 1] * w[i - 2] * y[i];
            }
        }

        public Convolution()
        {
            setwy(level);
        }

        void fft4(ModInt[] a, int k)
        {
            if (a.Length <= 1) return;
            if (k == 1)
            {
                ModInt a1 = a[1];
                a[1] = a[0] - a[1];
                a[0] = a[0] + a1;
                return;
            }

            if ((k & 1) == 1)
            {
                int vv = 1 << (k - 1);
                for (int j = 0; j < vv; ++j)
                {
                    ModInt ajv = a[j + vv];
                    a[j + vv] = a[j] - ajv;
                    a[j] += ajv;
                }
            }

            int u = 1 << (2 + (k & 1));
            int v = 1 << (k - 2 - (k & 1));
            ModInt one = 1;
            ModInt imag = dw[1];
            while (v != 0)
            {
                // jh = 0
                {
                    int j0 = 0;
                    int j1 = v;
                    int j2 = j1 + v;
                    int j3 = j2 + v;
                    for (; j0 < v; ++j0, ++j1, ++j2, ++j3)
                    {
                        ModInt t0 = a[j0], t1 = a[j1], t2 = a[j2], t3 = a[j3];
                        ModInt t0p2 = t0 + t2, t1p3 = t1 + t3;
                        ModInt t0m2 = t0 - t2, t1m3 = (t1 - t3) * imag;
                        a[j0] = t0p2 + t1p3;
                        a[j1] = t0p2 - t1p3;
                        a[j2] = t0m2 + t1m3;
                        a[j3] = t0m2 - t1m3;
                    }
                }
                // jh >= 1
                ModInt ww = one, xx = one * dw[2], wx = one;
                for (int jh = 4; jh < u;)
                {
                    ww = xx * xx;
                    wx = ww * xx;
                    int j0 = jh * v;
                    int je = j0 + v;
                    int j2 = je + v;
                    for (; j0 < je; ++j0, ++j2)
                    {
                        ModInt t0 = a[j0], t1 = a[j0 + v] * xx, t2 = a[j2] * ww, t3 = a[j2 + v] * wx;
                        ModInt t0p2 = t0 + t2, t1p3 = t1 + t3;
                        ModInt t0m2 = t0 - t2, t1m3 = (t1 - t3) * imag;
                        a[j0] = t0p2 + t1p3;
                        a[j0 + v] = t0p2 - t1p3;
                        a[j2] = t0m2 + t1m3;
                        a[j2 + v] = t0m2 - t1m3;
                    }

                    xx *= dw[BitOperations.TrailingZeroCount((ulong)(jh += 4))];
                }

                u <<= 2;
                v >>= 2;
            }
        }

        void ifft4(ModInt[] a, int k)
        {
            if ((int)a.Length <= 1) return;
            if (k == 1)
            {
                ModInt a1 = a[1];
                a[1] = a[0] - a[1];
                a[0] = a[0] + a1;
                return;
            }

            int u = 1 << (k - 2);
            int v = 1;
            ModInt one = 1;
            ModInt imag = dy[1];
            while (u != 0)
            {
                // jh = 0
                {
                    int j0 = 0;
                    int j1 = v;
                    int j2 = v + v;
                    int j3 = j2 + v;
                    for (; j0 < v; ++j0, ++j1, ++j2, ++j3)
                    {
                        ModInt t0 = a[j0], t1 = a[j1], t2 = a[j2], t3 = a[j3];
                        ModInt t0p1 = t0 + t1, t2p3 = t2 + t3;
                        ModInt t0m1 = t0 - t1, t2m3 = (t2 - t3) * imag;
                        a[j0] = t0p1 + t2p3;
                        a[j2] = t0p1 - t2p3;
                        a[j1] = t0m1 + t2m3;
                        a[j3] = t0m1 - t2m3;
                    }
                }
                // jh >= 1
                ModInt ww = one, xx = one * dy[2], yy = one;
                u <<= 2;
                for (int jh = 4; jh < u;)
                {
                    ww = xx * xx;
                    yy = xx * imag;
                    int j0 = jh * v;
                    int je = j0 + v;
                    int j2 = je + v;
                    for (; j0 < je; ++j0, ++j2)
                    {
                        ModInt t0 = a[j0], t1 = a[j0 + v], t2 = a[j2], t3 = a[j2 + v];
                        ModInt t0p1 = t0 + t1, t2p3 = t2 + t3;
                        ModInt t0m1 = (t0 - t1) * xx, t2m3 = (t2 - t3) * yy;
                        a[j0] = t0p1 + t2p3;
                        a[j2] = (t0p1 - t2p3) * ww;
                        a[j0 + v] = t0m1 + t2m3;
                        a[j2 + v] = (t0m1 - t2m3) * ww;
                    }

                    xx *= dy[BitOperations.TrailingZeroCount((ulong)(jh += 4))];
                }

                u >>= 4;
                v <<= 2;
            }

            if ((k & 1) == 1)
            {
                u = 1 << (k - 1);
                for (int j = 0; j < u; ++j)
                {
                    ModInt ajv = a[j] - a[j + u];
                    a[j] += a[j + u];
                    a[j + u] = ajv;
                }
            }
        }

        void ntt(ModInt[] a)
        {
            if ((int)a.Length <= 1) return;
            fft4(a, BitOperations.TrailingZeroCount(a.Length));
        }

        void intt(ModInt[] a)
        {
            if ((int)a.Length <= 1) return;
            ifft4(a, BitOperations.TrailingZeroCount(a.Length));
            ModInt iv = (ModInt)1 / a.Length;
            for (int i = 0; i < a.Length; i++)
            {
                a[i] *= iv;
            }
        }

        public ModInt[] Solve(IEnumerable<int> aa, IEnumerable<int> bb)
            => Solve(aa.Select(x => (ModInt)x), bb.Select(x => (ModInt)x));
        public ModInt[] Solve(IEnumerable<long> aa, IEnumerable<long> bb)
            => Solve(aa.Select(x => (ModInt)x), bb.Select(x => (ModInt)x));
        
        public ModInt[] Solve(IEnumerable<ModInt> aa, IEnumerable<ModInt> bb)
        {
            var a = aa.Select(x => (ModInt)x).ToArray();
            var b = bb.Select(x => (ModInt)x).ToArray();
            int l = a.Length + b.Length - 1;
            if (Min(a.Length, b.Length) <= 40)
            {
                ModInt[] ss = new ModInt[l];
                for (int i = 0; i < (int)a.Length; ++i)
                for (int j = 0; j < (int)b.Length; ++j)
                    ss[i + j] += a[i] * b[j];
                return ss;
            }

            int k = 2, M = 4;
            while (M < l)
            {
                M <<= 1;
                ++k;
            }

            setwy(k);
            ModInt[] s = new ModInt[M];
            ModInt[] t = new ModInt[M];
            for (int i = 0; i < (int)a.Length; ++i) s[i] = a[i];
            for (int i = 0; i < (int)b.Length; ++i) t[i] = b[i];
            fft4(s, k);
            fft4(t, k);
            for (int i = 0; i < M; ++i) s[i] *= t[i];
            ifft4(s, k);
            Array.Resize(ref s, l);
            ModInt invm = (ModInt)1 / M;
            for (int i = 0; i < l; ++i) s[i] *= invm;
            return s;
        }

        void ntt_doubling(ModInt[] a)
        {
            int M = (int)a.Length;
            var b = a;
            intt(b);
            ModInt r = 1, zeta = ModInt.Pow(pr, ModInt.ModValue - 1 / (M << 1));
            for (int i = 0; i < M; i++)
            {
                b[i] *= r;
                r *= zeta;
            }
            ntt(b);
            Array.Copy(a, b, 0);
        }
    }
}