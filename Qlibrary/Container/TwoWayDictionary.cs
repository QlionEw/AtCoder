using System.Collections.Generic;
using System.Linq;

namespace Qlibrary
{
    public class TwoWayDictionary<T1, T2>
    {
        public Dictionary<T1, T2> Fore { get; } = new Dictionary<T1, T2>();
        public Dictionary<T2, T1> Back { get; } = new Dictionary<T2, T1>();
        public int Count => Fore.Count;

        public void Add(T1 key, T2 value)
        {
            Fore.Add(key, value);
            Back.Add(value, key);
        }

        public void Remove(T1 key)
        {
            var value = Fore[key];
            Fore.Remove(key);
            Back.Remove(value);
        }

        public void Clear()
        {
            Fore.Clear();
            Back.Clear();
        }
    }
}