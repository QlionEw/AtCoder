using System;
using System.Collections.Generic;

namespace Qlibrary
{
    public static class Primer
    {
        /// <summary> 素数判定 </summary>
        public static bool IsPrime(long num)
        {
            if (num < 2) { return false; }

            if (num == 2) { return true; }

            if (num % 2 == 0) { return false; }

            double sqrtNum = Math.Sqrt(num);
            for (int i = 3; i <= sqrtNum; i += 2)
            {
                if (num % i == 0) { return false; }
            }

            return true;
        }

        public static IEnumerable<long> GetPrimeFactors(long n)
        {
            long i = 2;
            long tmp = n;

            while (i * i <= tmp)
            {
                if (tmp % i == 0)
                {
                    tmp /= i;
                    yield return i;
                    if (IsPrime(tmp))
                    {
                        yield return tmp;
                        tmp = 1;
                        break;
                    }
                }
                else
                {
                    i++;
                }
            }

            if (tmp != 1) yield return tmp;
        }

        public static int GetDivisorCount(long n)
        {
            int count = 0;
            long sq = (long) Math.Sqrt(n);
            for (long i = 1; i <= sq; i++)
            {
                if (n % i == 0)
                {
                    count += 2;
                }
            }

            if (sq * sq == n)
            {
                count--;
            }

            return count;
        }

        public static IEnumerable<long> GetDivisors(long n)
        {
            for (long i = 1; i * i <= n; i++)
            {
                if (n % i != 0) { continue; }

                yield return i;
                if (i != n / i)
                {
                    yield return n / i;
                }
            }
        }

        /// <summary>
        /// オイラーのϕ関数（トーシェント関数）
        /// </summary>
        /// <remarks>
        /// nと互いに素である 1 以上 n 以下の自然数の個数 φ(n) を与える数論的関数 φ <br />
        /// nが正の整数でaをnと互いに素な正の整数としたとき、以下が成立する。 <br />
        /// a^φ(N) ≡ 1(modN)
        /// </remarks>
        public static long Totient(long n)
        {
            long ans = n;
            for (int p = 2; p * p <= n; p++)
            {
                if (n % p != 0) continue;
                while (n % p == 0)
                {
                    n /= p;
                }
                ans -= ans / p;
            }
            if (n != 1)
            {
                ans -= ans / n;
            }
            return ans;
        }
    }
}