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
    }
}