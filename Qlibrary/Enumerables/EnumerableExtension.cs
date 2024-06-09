using System;
using System.Collections.Generic;
using System.Linq;

namespace Qlibrary
{
    public static class EnumerableExtensions
    {
        /// <summary> 値とインデックスをペアにする </summary>
        public static IEnumerable<(T Value, int Index)> ToIndexPairs<T>(this IEnumerable<T> self, int indexed = 0)
            => self.Select((x, i) => (x, i + indexed));
        /// <summary> 値と出現回数をペアにする </summary>
        public static IEnumerable<(T Key, int Count)> ToOccurrencePairs<T>(this IEnumerable<T> self)
            => self.GroupBy(x => x).Select(x => (x.Key, x.Count()));
    }
}