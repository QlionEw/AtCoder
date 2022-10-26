using System.Collections.Generic;

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
        private ModInt(long value) => this.value = value;
        public static implicit operator ModInt(long a) => new ModInt(a % ModValue + (a < 0 ? ModValue : 0));
        public static explicit operator int(ModInt a) => (int) a.value;
        public override string ToString() => value.ToString();
        public static ModInt operator +(ModInt a, ModInt b) => a.value + b.value;
        public static ModInt operator -(ModInt a, ModInt b) => a.value - b.value;
        public static ModInt operator *(ModInt a, ModInt b) => a.value * b.value;
        public static ModInt operator /(ModInt a, ModInt b) => a * Inv(b);

        public static ModInt Fraction(long top, long bottom)
        {
            return new ModInt(top) / bottom;
        }
        
        public static ModInt Pow(ModInt a, long n)
        {
            if (n == 0) return 1;
            if (n == 1) return a;
            ModInt p = Pow(a, n / 2);
            return p * p * Pow(a, n % 2);
        }

        public static ModInt Inv(ModInt a) => Pow(a, ModValue - 2);

        public static ModInt Fact(int n)
        {
            for (int i = fact.Count; i <= n; i++) fact.Add(fact[^1] * i);
            return fact[n];
        }

        public static ModInt Perm(int n, int r)
        {
            if (r < 0 || n < 0) return 0;
            if (r == 0) return 1;
            if (n <= 0) return 0;
            if (n < r) return 0;
            
            return Fact(n) / Fact(n - r);
        }

        public static ModInt Comb(int n, int r)
        {
            if (r < 0 || n < 0) return 0;
            if (r == 0) return 1;
            if (n <= 0) return 0;
            if (n < r) return 0;

            return Fact(n) / Fact(n - r) / Fact(r);
        }
    }
}