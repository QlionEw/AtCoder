using System.Collections.Generic;
using System.Linq;

namespace Qlibrary
{
    public static class InversionNumbersExtensions
    {
        public static IEnumerable<long> InversionNumbers(this IEnumerable<int> a)
        {
            var ar = a.ToArray();
            var ft = new FenwickTree(ar.Length);
            for (int i = 0; i < ar.Length; i++)
            {
                ft.Add(ar[i], 1);
                yield return i + 1 - (int)ft.Sum(0, ar[i]);
            }
        }
        
        public static IEnumerable<long> InversionNumbers<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            var ar = a.ToArray();
            var dict = ar.Select((x,i) => (x,i)).ToDictionary(x => x.x, x => x.i);

            return b.Select(l => dict[l]).InversionNumbers();
        }
        
        public static IEnumerable<long> InversionWith<T>(this IEnumerable<long> a, IEnumerable<long> b)
        {
            var ar = a.ToArray();
            var cp = new Compressed<long>(ar);

            return b.Select(l => cp.IndexOf(l)).InversionNumbers();
        }
    }
}