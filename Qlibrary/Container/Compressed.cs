using System;
using System.Collections.Generic;
using System.Linq;

namespace Qlibrary
{
    public class Compressed<T> where T : IComparable<T>, IEquatable<T>
    {
        private List<T> list;
        private Dictionary<T, int> dict;
        private Dictionary<int, T> reversed;

        public Compressed(){ }

        public Compressed(IEnumerable<T> array)
        {
            foreach (var item in array)
            {
                Add(item);
            }
        }
        
        public void Add(T value)
        {
            if (list == null)
            {
                list = new List<T>();
            }

            list.Add(value);
        }

        public int this[T index] => IndexOf(index);

        public int IndexOf(T index)
        {
            if (list != null && dict == null)
            {
                Generate(list);
            }

            return dict[index];
        }

        public int Count()
        {
            if (list != null && dict == null)
            {
                Generate(list);
            }
            return dict.Count;
        }

        public T Restore(int index)
        {
            return reversed.Count > index ? reversed[index] : default;
        }

        private void Generate(IEnumerable<T> array)
        {
            var converted = array.Distinct().OrderBy(x => x).Select((x, i) => (x, i)).ToArray();
            dict = converted.ToDictionary(x => x.x, x => x.i);
            reversed = converted.ToDictionary(x => x.i, x => x.x);
        }

        public static Compressed<T> Create(IEnumerable<T> array)
        {
            var cp = new Compressed<T>();
            cp.Generate(array);
            return cp;
        }
    }
}