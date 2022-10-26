using System;
using System.Collections.Generic;

namespace Qlibrary
{
    public class ZobristHash : ZobristHash<long>{}
    public class ZobristHash<T>
    {
        private readonly Dictionary<T, long> hashDict = new Dictionary<T, long>();
        private readonly Random rand = new Random();

        public void Register(params T[] values)
        {
            foreach (var value in values)
            {
                if (hashDict.ContainsKey(value)) {continue;}
                
                long randomValue = ((long)rand.Next(0, int.MaxValue) << 32) + rand.Next(0, int.MaxValue);
                hashDict.Add(value, randomValue);
            }
        }
        public long this[T key] => hashDict[key];
    }
}