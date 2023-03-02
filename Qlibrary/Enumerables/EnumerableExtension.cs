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
        /// <summary> 特定の条件の最大値を取る値を取得 </summary>
        public static T MaxBy<T,T2>(this IEnumerable<T> self, Func<T,T2> selector, Comparer<T2> comp = null) where T : IComparable<T>
        {
            comp ??= Comparer<T2>.Default;
            var ar = self.ToArray();
            var max = ar[0];
            foreach (var value in ar)
            {
                if (comp.Compare(selector(max), selector(value)) < 0) max = value;
            }
            return max;
        }
        /// <summary> 特定の条件の最小値を取る値を取得 </summary>
        public static T MinBy<T,T2>(this IEnumerable<T> self, Func<T,T2> selector, Comparer<T2> comp = null) where T : IComparable<T>
        {
            comp ??= Comparer<T2>.Default;
            var ar = self.ToArray();
            var min = ar[0];
            T2 selected = selector(min);
            foreach (var value in ar)
            {
                var current = selector(value);
                if (comp.Compare(selected, current) <= 0) continue;
                min = value;
                selected = current;
            }
            return min;
        }
        /// <summary> 特定個数ごとにまとめる </summary>
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> self, int size)
        {
            var ar = self.ToArray();
            var box = new T[size];
            for (int i = 0; i < ar.Length; i++)
            {
                box[i % size] = ar[i];
                if (i % size == size - 1)
                {
                    yield return box;
                }
            }
            if (ar.Length % size != 0)
            {
                yield return box.Take(ar.Length % size);
            }
        }
    }
}