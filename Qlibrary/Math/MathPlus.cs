using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using static System.Math;

namespace Qlibrary
{
    public static class MathPlus
    {
        [MethodImpl(256)]
        public static long CeilingLong(long value, long div)
        {
            if (value >= 0)
            {
                return value % div == 0 ? value / div : value / div + 1;
            }
            return value / div;
        }

        [MethodImpl(256)]
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

        [MethodImpl(256)]
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

        [MethodImpl(256)]
        public static long Gcd(long a, long b)
        {
            return a > b ? GcdRecursive(a, b) : GcdRecursive(b, a);
        }

        [MethodImpl(256)]
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
        
        /// <summary>
        /// a*x + b*y = gcd(a,b) となるx,yを求める。
        /// (この式にはgcd(a,b)以下の解はない)
        /// gcd(a,b)以上の値Nで解を求めたい場合は帰ってきた(x,y)を(N/gcd(a,b))倍する
        /// (このときは、(N/gcd(a,b))が整数にならない場合も解なしのはず)
        /// </summary>
        [MethodImpl(256)]
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
        [MethodImpl(256)]
        public static long Inv(long a, long b, long mod)
        {
            var dd = ExtGcd(a, mod);
            if (dd.Item1 < 0)
            {
                dd.Item1 += mod;
            }
            return (dd.Item1 * b) % mod;
        }

        [MethodImpl(256)]
        public static long Lcm(long a, long b)
        {
            checked
            {
                return (a / Gcd(a, b)) * b;
            }
        }

        [MethodImpl(256)]
        public static long Combination(long n, long m)
        {
            if (m == 0) return 1;
            if (n == 0) return 0;
            return n * Combination(n - 1, m - 1) / m;
        }

        [MethodImpl(256)]
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

        [MethodImpl(256)]
        public static void GetPermutations<T>(IEnumerable<T> array, Action<List<T>> action)
            where T : IComparable<T>
        {
            var a = new List<T>(array);
            int n = a.Count;
            a.Sort();

            action(a);

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
                } while (a[i].CompareTo(a[j]) >= 0);

                if (a[i].CompareTo(a[j]) < 0)
                {
                    (a[i], a[j]) = (a[j], a[i]);
                    a.Reverse(i + 1, n - i - 1);
                    action(a);
                    next = true;
                }
            }
        }
        
        [MethodImpl(256)]
        public static void GetCombinations<T>(IEnumerable<T> items, int k, Action<T[]> action, bool withRepetition = false)
        {
            var itemsArray = items as T[] ?? items.ToArray();
            int n = itemsArray.Length;

            if (k == 0 || n == 0 || k > n && !withRepetition)
            {
                action(Array.Empty<T>());
                return;
            }

            int[] indices = new int[k];
            for (int i = 0; i < k; i++)
            {
                indices[i] = withRepetition ? 0 : i;
            }

            while (true)
            {
                T[] combination = new T[k];
                for (int i = 0; i < k; i++)
                {
                    combination[i] = itemsArray[indices[i]];
                }
                action(combination);

                int pos = k - 1;
                while (pos >= 0 && (withRepetition ? indices[pos] == n - 1 : indices[pos] == n - k + pos))
                {
                    pos--;
                }

                if (pos < 0)
                {
                    break;
                }

                indices[pos]++;
                for (int i = pos + 1; i < k; i++)
                {
                    indices[i] = withRepetition ? indices[pos] : indices[pos] + i - pos;
                }
            }
        }
        
        [MethodImpl(256)]
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
        
        [MethodImpl(256)]
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
        
        [MethodImpl(256)]
        public static double ToDegree(double radian) => radian * (180.0 / Math.PI);

        [MethodImpl(256)]
        public static long ArithmeticSum(long end) => ArithmeticSum(1, end);
        
        // 等差数列の和
        [MethodImpl(256)]
        public static long ArithmeticSum(long start, long end) => (start + end) * (end - start + 1) / 2;
        
        // sum_{0 <= i < N} (ai + b) // m
        [MethodImpl(256)]
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

        [MethodImpl(256)]
        public static long FloorSqrt(long value)
        {
            var sq = (long)Math.Sqrt(value);
            while ((sq+1) * (sq+1) <= value)
            {
                sq++;
            }
            while ((sq) * (sq) > value)
            {
                sq--;
            }
            return sq;
        }

        [MethodImpl(256)]
        public static long CeilingSqrt(long value)
        {
            var sq = (long)Math.Sqrt(value);
            while ((sq) * (sq) < value)
            {
                sq++;
            }
            while ((sq-1) * (sq-1) >= value)
            {
                sq--;
            }
            return sq;
        }

        [MethodImpl(256)]
        public static long AbsMod(this long l, long mod)
        {
            mod = Abs(mod);
            var lm = Abs(l) % mod;
            if (l < 0)
            {
                lm = (mod - lm);
                lm %= mod;
            }
            return lm;
        }
    }
}