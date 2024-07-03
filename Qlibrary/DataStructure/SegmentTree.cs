using System;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    public class SegmentTree : SegmentTreeExtend<long>
    {
        public void Add(int index, long value) => SetValue(index, value + Data[index + N - 1]);
    }
    
    public class ModSegmentTree : SegmentTreeExtend<ModInt>
    {
        public void Add(int index, ModInt value) => SetValue(index, value + Data[index + N - 1]);
    }

    public class SegmentTreeExtend<T>
    {
        protected T[] Data;
        private Func<T, T, T> updateMethod;
        private T firstValue;
        protected int N;
        private int count;

        public void Init(int count, T firstValue, Func<T, T, T> updateMethod)
        {
            this.updateMethod = updateMethod;
            this.firstValue = firstValue;
            this.count = count;

            N = 1;
            while (N < count)
            {
                N *= 2;
            }

            Data = Enumerable.Repeat(firstValue, 2 * N - 1).ToArray();
        }
        
        public void Build(Func<int, T> update)
        {
            for (int i = 0; i < count; i++)
            {
                Data[N + i - 1] = update(i);
            }
            for (int i = N - 2; i >= 0; i--)
            {
                Data[i] = updateMethod(Data[2 * i + 1], Data[2 * i + 2]);
            }
        }
        
        [MethodImpl(256)]
        public void SetValue(int index, T value)
        {
            index += N - 1;
            Data[index] = value;
            while (index > 0)
            {
                index = (index - 1) / 2;
                Data[index] = updateMethod(Data[index * 2 + 1], Data[index * 2 + 2]);
            }
        }
        
        [MethodImpl(256)]
        public T Query(int index) => Query(index, index);

        [MethodImpl(256)]
        public T Query(int indexStart, int indexEnd) => Query(indexStart, indexEnd + 1, 0, 0, N);

        [MethodImpl(256)]
        private T Query(int indexStart, int indexEnd, int current, int left, int right)
        {
            if (right <= indexStart || indexEnd <= left) { return firstValue; }

            if (indexStart <= left && right <= indexEnd) { return Data[current]; }

            T leftValue = Query(indexStart, indexEnd, current * 2 + 1, left, (left + right) / 2);
            T rightValue = Query(indexStart, indexEnd, current * 2 + 2, (left + right) / 2, right);

            return updateMethod(leftValue, rightValue);
        }
    }
}