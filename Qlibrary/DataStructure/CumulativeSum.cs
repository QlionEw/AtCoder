using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    public class CumulativeSum : IEnumerable<long>
    {
        private bool isCalculated;
        private long[] box;
        
        public CumulativeSum(int size)
        {
            box = new long[size];
        }

        [MethodImpl(256)]
        public void Add(int index, long value)
        {
            box[index] += value;
        }
        
        [MethodImpl(256)]
        public void AddRange(int start, int end, long value)
        {
            box[start] += value;
            if (end + 1 < box.Length)
            {
                box[end + 1] -= value;
            }
        }

        [MethodImpl(256)]
        public long Sum(int l, int r) => box[r] - (l == 0 ? 0 : box[l - 1]);

        public void Calculate()
        {
            for (int i = 0; i < box.Length - 1; i++)
            {
                box[i + 1] += box[i];
            }
            isCalculated = true;
        }

        public IEnumerator<long> GetEnumerator()
        {
            if (!isCalculated)
            {
                Calculate();
            }
            return box.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (!isCalculated)
            {
                Calculate();
            }
            return GetEnumerator();
        }
    }
}