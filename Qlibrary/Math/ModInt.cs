using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    public struct ModInt
    {
        long value;
        public const int _1000000007 = 1000000007;
        public const int _1000000009 = 1000000009;
        public const int _998244353 = 998244353;
        public static int ModValue { get; set; } = _998244353;
        static List<ModInt> fact = new List<ModInt> {1};
        static List<ModInt> inv = new List<ModInt> {1};
        private ModInt(long value) => this.value = value;
        [MethodImpl(256)]
        public static implicit operator ModInt(long a) => new ModInt(a % ModValue + (a < 0 ? ModValue : 0));
        [MethodImpl(256)]
        public static explicit operator int(ModInt a) => (int) a.value;
        [MethodImpl(256)]
        public override string ToString() => value.ToString();
        [MethodImpl(256)]
        public static ModInt operator +(ModInt a, ModInt b) => a.value + b.value;
        [MethodImpl(256)]
        public static ModInt operator -(ModInt a, ModInt b) => a.value - b.value;
        [MethodImpl(256)]
        public static ModInt operator *(ModInt a, ModInt b) => a.value * b.value;
        [MethodImpl(256)]
        public static ModInt operator /(ModInt a, ModInt b) => a * MathPlus.Pow((long)b, ModValue - 2, ModValue);
        [MethodImpl(256)]
        public static ModInt Fraction(long top, long bottom) => new ModInt(top) / bottom;
        [MethodImpl(256)]
        public static ModInt Pow(ModInt a, long n) => MathPlus.Pow((long)a, n, ModValue);
        [MethodImpl(256)]
        public static ModInt Inv(int n)
        {
            for (int i = inv.Count; i <= n; i++) inv.Add(inv[^1] / i);
            return inv[n];
        }
        [MethodImpl(256)]
        public static ModInt Fact(int n)
        {
            for (int i = fact.Count; i <= n; i++) fact.Add(fact[^1] * i);
            return fact[n];
        }
        [MethodImpl(256)]
        public static ModInt Perm(int n, int r)
        {
            if (r < 0 || n < 0) return 0;
            if (r == 0) return 1;
            if (n <= 0) return 0;
            if (n < r) return 0;
            
            return Fact(n) * Inv(n - r);
        }
        [MethodImpl(256)]
        public static ModInt Comb(int n, int r)
        {
            if (r < 0 || n < 0) return 0;
            if (r == 0) return 1;
            if (n <= 0) return 0;
            if (n < r) return 0;

            return Fact(n) * Inv(n - r) * Inv(r);
        }
    }
}