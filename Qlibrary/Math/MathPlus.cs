using System;
using System.Collections.Generic;
using System.Linq;

namespace Qlibrary
{
    public static class MathPlus
    {
        public static long CeilingLong(long value, long div)
        {
            return value % div == 0 ? value / div : value / div + 1;
        }

        public static int Digit(long num, int b)
        {
            int digit = 0;
            while (num > 0)
            {
                num /= b;
                digit++;
            }
            return digit;
        }

        public static int GetNumberCount(long value, int digit, int ary, int searched)
        {
            int count = 0;
            for (int i = 0; i < digit; i++)
            {
                if (value % ary == searched)
                {
                    count++;
                }
                value /= 2;
            }
            return count;
        }

        public static long Gcd(long a, long b)
        {
            return a > b ? GcdRecursive(a, b) : GcdRecursive(b, a);
        }

        private static long GcdRecursive(long a, long b)
        {
            while (true)
            {
                if (b == 0) return a;
                long a1 = a;
                a = b;
                b = a1 % b;
            }
        }
        
        /// <summary> a*x + b*y = 1 となるx,yを求める(1をnにする場合返り値をn倍) </summary>
        public static (long, long, long) ExtGcd(long a, long b, long x = 0, long y = 0)
        {
            if (b == 0) {
                return (1, 0, a);
            }
            (long xx, long yy, long aa) = ExtGcd(b, a%b, y, x);
            xx -= a/b * yy;
            return (yy, xx, aa);
        }

        /// <summary> Ax ≡ B mod Mとなるxを求める </summary>
        public static long Inv(long a, long b, long mod)
        {
            var dd = ExtGcd(a, mod);
            if (dd.Item1 < 0)
            {
                dd.Item1 += mod;
            }
            return (dd.Item1 * b) % mod;
        }

        public static long Lcm(long a, long b)
        {
            checked
            {
                return (a / Gcd(a, b)) * b;
            }
        }

        public static long Combination(long n, long m)
        {
            if (m == 0) return 1;
            if (n == 0) return 0;
            return n * Combination(n - 1, m - 1) / m;
        }

        public static long Permutation(long n, long m)
        {
            if (m == 0) return 1;
            if (n == 0) return 0;
            long value = 1;
            for (long i = n; i >= n - m + 1; i--)
            {
                value *= i;
            }

            return value;
        }

        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(params T[] array) where T : IComparable
        {
            var a = new List<T>(array);
            int n = a.Count;

            yield return array;

            bool next = true;
            while (next)
            {
                next = false;
                int i;
                for (i = n - 2; i >= 0; i--)
                {
                    if (a[i].CompareTo(a[i + 1]) < 0) break;
                }

                if (i < 0) break;

                int j = n;
                do
                {
                    j--;
                } while (a[i].CompareTo(a[j]) > 0);

                if (a[i].CompareTo(a[j]) < 0)
                {
                    (a[i], a[j]) = (a[j], a[i]);
                    a.Reverse(i + 1, n - i - 1);
                    yield return a;
                    next = true;
                }
            }
        }

        public static IEnumerable<T[]> GetCombinations<T>(IEnumerable<T> items, int k, bool withRepetition = false)
        {
            if (k == 1)
            {
                foreach (var item in items)
                    yield return new T[] {item};
                yield break;
            }

            IEnumerable<T> enumerable = items as T[] ?? items.ToArray();
            foreach (var item in enumerable)
            {
                var leftside = new T[] {item};

                var unused = withRepetition ? enumerable : enumerable.SkipWhile(e => !e.Equals(item)).Skip(1).ToList();

                foreach (var rightside in GetCombinations(unused, k - 1, withRepetition))
                {
                    yield return leftside.Concat(rightside).ToArray();
                }
            }
        }
        
        public static long BigPow(long baseValue, long pow, long mod = long.MaxValue)
        {
            long p = baseValue % mod;
            long x = 1;
            long ret = 1;

            while (true) {
                if ((pow & x) > 0) {
                    ret = (ret * p) % mod;
                }

                x *= 2;
                if (x > pow) return ret;
                p = (p * p) % mod;
            }
        }

        /// <summary> 傾き </summary>
        public static decimal Slope((decimal x, decimal y) a, (decimal x, decimal y) b)
        {
            return a.x - b.x == 0 ? decimal.MaxValue : (b.y - a.y) / (b.x - a.x);
        }

        /// <summary> 切片 </summary>
        public static decimal Intercept((decimal x, decimal y) a, (decimal x, decimal y) b)
        {
            return a.x - b.x == 0 ? decimal.MaxValue : (a.x * b.y - a.y * b.x) / (a.x - b.x);
        }
    }
}