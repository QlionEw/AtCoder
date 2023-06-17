using System;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private T[] heap;
        private int capacity;
        public int Count { get; private set; }

        public PriorityQueue(int capacity = 16)
        {
            this.capacity = capacity;
            heap = new T[capacity];
        }

        [MethodImpl(256)]
        public void Enqueue(T item)
        {
            if (Count == capacity) Resize();
            int index = Count++;
            while (index > 0)
            {
                int p = (index - 1) / 2;
                if (heap[p].CompareTo(item) <= 0) { break; }

                heap[index] = heap[p];
                index = p;
            }

            heap[index] = item;
        }

        [MethodImpl(256)]
        public T Dequeue()
        {
            T ret = heap[0];
            T item = heap[--Count];

            int index = 0;
            while (index * 2 + 1 < Count)
            {
                int a = index * 2 + 1;
                int b = index * 2 + 2;
                if (b < Count && heap[b].CompareTo(heap[a]) < 0)
                {
                    a = b;
                }

                if (heap[a].CompareTo(item) >= 0) { break; }

                heap[index] = heap[a];
                index = a;
            }

            heap[index] = item;

            return ret;
        }

        [MethodImpl(256)]
        private void Resize()
        {
            var newArray = new T[capacity * 2];
            for (int i = 0; i < Count; i++)
            {
                newArray[i] = heap[i];
            }

            capacity *= 2;
            heap = newArray;
        }

        [MethodImpl(256)]
        public void Clear() => Count = 0;
    }
}