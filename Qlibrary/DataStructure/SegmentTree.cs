using System;
using System.Linq;

namespace Qlibrary
{
    public class SegmentTree : SegmentTreeExtend<long>
    {
    }

    public class SegmentTreeExtend<T>
    {
        private T[] data;
        private Func<T, T, T> updateMethod;
        private T firstValue;
        private int n;
        private int count;

        public void Init(int count, T firstValue, Func<T, T, T> updateMethod)
        {
            this.updateMethod = updateMethod;
            this.firstValue = firstValue;
            this.count = count;

            n = 1;
            while (n < count)
            {
                n *= 2;
            }

            data = Enumerable.Repeat(firstValue, 2 * n - 1).ToArray();
        }
        
        public void Build(Func<int, T> update)
        {
            for (int i = 0; i < count; i++)
            {
                data[n + i - 1] = update(i);
            }
            for (int i = n - 2; i >= 0; i--)
            {
                data[i] = updateMethod(data[2 * i + 1], data[2 * i + 2]);
            }
        }

        public void Update(int index, T value)
        {
            index += n - 1;
            data[index] = value;
            while (index > 0)
            {
                index = (index - 1) / 2;
                data[index] = updateMethod(data[index * 2 + 1], data[index * 2 + 2]);
            }
        }

        public T Query(int index)
        {
            return Query(index, index);
        }

        public T Query(int indexStart, int indexEnd)
        {
            return Query(indexStart, indexEnd + 1, 0, 0, n);
        }

        private T Query(int indexStart, int indexEnd, int current, int left, int right)
        {
            if (right <= indexStart || indexEnd <= left) { return firstValue; }

            if (indexStart <= left && right <= indexEnd) { return data[current]; }

            T leftValue = Query(indexStart, indexEnd, current * 2 + 1, left, (left + right) / 2);
            T rightValue = Query(indexStart, indexEnd, current * 2 + 2, (left + right) / 2, right);

            return updateMethod(leftValue, rightValue);
        }
    }
}