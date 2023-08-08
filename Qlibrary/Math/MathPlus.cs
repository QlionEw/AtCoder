using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

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
        public static (long y, long x, long a) ExtGcd(long a, long b, long x = 0, long y = 0)
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
            => GetPermutations(array, 0, array.Length - 1);
        
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> items) where T : IComparable
        {
            var array = items.ToArray();
            return GetPermutations(array, 0, array.Length - 1);
        }
        
        private static IEnumerable<IEnumerable<T>> GetPermutations<T>(IList<T> list, int k, int m) where T : IComparable
        {
            if (k == m)
            {
                yield return list;
            }
            else
            {
                for (int i = k; i <= m; i++)
                {
                    (list[k], list[i]) = (list[i], list[k]);
                    foreach (var perm in GetPermutations(list, k + 1, m))
                    {
                        yield return perm;
                    }
                    (list[k], list[i]) = (list[i], list[k]);
                }
            }
        }
        
        public static IEnumerable<T[]> GetCombinations<T>(IEnumerable<T> items, int k, bool withRepetition = false)
        {
            if (k == 0)
            {
                yield return new T[] { };
                yield break;
            }
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
        
        public static BigInteger MoreBigPow(BigInteger baseValue, BigInteger pow, BigInteger mod)
        {
            BigInteger p = baseValue % mod;
            BigInteger x = 1;
            BigInteger ret = 1;

            while (true) {
                if ((pow & x) > 0) {
                    ret = (ret * p) % mod;
                }

                x *= 2;
                if (x > pow) return ret;
                p = (p * p) % mod;
            }
        }
        
        public static double ToDegree(double radian) => radian * (180.0 / Math.PI);

        // sum_{0 <= i < N} (ai + b) // m
        public static long FloorSum(long n, long m, long a, long b)
        {
            long ret = default;
            if (a >= m)
            {
                ret += (n - 1) * n * (a / m) / 2;
                a %= m;
            }
            if (b >= m)
            {
                ret += n * (b / m);
                b %= m;
            }
            var y = (a * n + b) / m;
            if (y == 0) return ret;
            var x = y * m - b;
            ret += (n - (x + a - 1) / a) * y;
            ret += FloorSum(y, a, m, (a - x % a) % a);
            return ret;
        }
    }
}