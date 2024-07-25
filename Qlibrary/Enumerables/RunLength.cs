using System.Collections.Generic;

namespace Qlibrary
{
    public static class RunLengthExtensions
    {
        public static List<(T Value, int Length)> RunLength<T>(this IEnumerable<T> self)
        {
            var list = new List<(T Value, int Length)>(); 
            bool isFirst = true;
            T current = default;
            int count = 0;
            foreach (var c in self)
            {
                if (!c.Equals(current) && !isFirst)
                {
                    list.Add((current, count));
                }
                if (!c.Equals(current) || isFirst)
                {
                    isFirst = false;
                    current = c;
                    count = 0;
                }
                count++;
            }
            list.Add((current, count));
            return list;
        }
    }
}