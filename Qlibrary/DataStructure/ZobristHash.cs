using System;
using System.Collections.Generic;

namespace Qlibrary
{
    public class ZobristHash : ZobristHash<long>{}
    public class ZobristHash<T>
    {
        public bool IsSize64 { get; set; } = true;
        private readonly Dictionary<T, long> hashDict = new Dictionary<T, long>();
        private readonly Random rand = new Random(DateTime.Now.Millisecond);

        public void Register(params T[] values)
        {
            foreach (var value in values)
            {
                if (hashDict.ContainsKey(value)) {continue;}
                
                long randomValue = rand.Next(0, int.MaxValue);
                if (IsSize64)
                {
                    randomValue += ((long)rand.Next(0, int.MaxValue) << 32);
                }
                hashDict.Add(value, randomValue);
            }
        }
        public long this[T key] => hashDict[key];
    }
}