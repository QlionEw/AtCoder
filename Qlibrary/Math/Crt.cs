using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    public static class Crt
    {
        [MethodImpl(256)]
        private static long SafeMod(long x, long m) {
            x %= m;
            if (x < 0) x += m;
            return x;
        }

        [MethodImpl(256)]
        private static (long, long) InvGcd(long a, long b) {
            a = SafeMod(a, b);
            if (a == 0) return (b, 0);

            long s = b, t = a;
            long m0 = 0, m1 = 1;

            while (t != 0) 
            {
                long u = s / t;
                s -= t * u;
                m0 -= m1 * u;
                (s, t) = (t, s);
                (m0, m1) = (m1, m0);
            }
            if (m0 < 0) m0 += b / s;
            return (s, m0);
        }

        [MethodImpl(256)]
        public static (long Value, long Modulo) Solve(IEnumerable<long> remains, IEnumerable<long> mod)
        {
            var r = remains.ToArray();
            var m = mod.ToArray();
            int n = r.Length;
            long r0 = 0, m0 = 1;
            for (int i = 0; i < n; i++)
            {
                long r1 = SafeMod(r[i], m[i]), m1 = m[i];
                if (m0 < m1) {
                    (r0, r1) = (r1, r0);
                    (m0, m1) = (m1, m0);
                }
                if (m0 % m1 == 0)
                {
                    if (r0 % m1 != r1){ return (0, 0); }
                    continue;
                }
                var v = InvGcd(m0, m1);
                long g = v.Item1;
                long im = v.Item2;
                long u1 = (m1 / g);
                if ((r1 - r0) % g != 0) return (0, 0);
                
                long x = (r1 - r0) / g % u1 * im % u1;
                r0 += x * m0;
                m0 *= u1;
                if (r0 < 0) r0 += m0;
            }
            return (r0, m0);
        }
    }
}