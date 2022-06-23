using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    public class ZAlgorithm : ZAlgorithm<char>
    {
        public ZAlgorithm(string str) : this(str.ToArray()){}
        public ZAlgorithm(char[] array) : base(array){}
    }

    public class ZAlgorithm<T>
    {
        private readonly T[] array;

        public ZAlgorithm(T[] array)
        {
            this.array = array;
        }

        [MethodImpl(256)]
        public IEnumerable<int> Solve(int index = 0)
        {
            var n = array.Length - index;
            var a = new int[n];
            a[0] = n;
            int i = 1, j = 0;
            while (i < n)
            {
                while (i + j < n && array[j + index].Equals(array[i + j + index])) ++j;
                a[i] = j;
                if (j == 0)
                {
                    i++;
                    continue;
                }

                int k = 1;
                while (i + k < n && k + a[k] < j)
                {
                    a[i + k] = a[k];
                    k++;
                }

                i += k;
                j -= k;
            }

            return a;
        }
    }
}