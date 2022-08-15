using System.Collections.Generic;

namespace Qlibrary
{
    public static class RunLengthExtensions
    {
        public static IEnumerable<(T Value, int Length)> RunLength<T>(this IEnumerable<T> self)
        {
            bool isFirst = true;
            T current = default;
            int count = 0;
            foreach (var c in self)
            {
                if (!c.Equals(current) && !isFirst)
                {
                    yield return (current, count);
                }
                if (!c.Equals(current) || isFirst)
                {
                    isFirst = false;
                    current = c;
                    count = 0;
                }
                count++;
            }
            yield return (current, count);
        }
    }
}