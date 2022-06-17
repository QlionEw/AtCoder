using System;
using System.Collections;
using System.Collections.Generic;

namespace Qlibrary
{
    public class Deque<T> : IEnumerable<T>
    {
        public T this[int i]
        {
            get => buffer[(firstIndex + i) % capacity];
            set
            {
                if (i < 0) throw new ArgumentOutOfRangeException();
                buffer[(firstIndex + i) % capacity] = value;
            }
        }

        private T[] buffer;
        private int capacity;
        private int firstIndex;
        private int LastIndex => (this.firstIndex + this.Length) % this.capacity;
        public int Length { get; private set; }

        public Deque(int capacity = 16)
        {
            this.capacity = capacity;
            buffer = new T[this.capacity];
            firstIndex = 0;
        }

        public void PushBack(T data)
        {
            if (Length == capacity) Resize();
            buffer[LastIndex] = data;
            Length++;
        }

        public void PushFront(T data)
        {
            if (Length == capacity) Resize();
            var index = firstIndex - 1;
            if (index < 0) index = capacity - 1;
            buffer[index] = data;
            Length++;
            firstIndex = index;
        }

        public T PopBack()
        {
            if (Length == 0) throw new InvalidOperationException("データが空です。");
            var data = this[Length - 1];
            Length--;
            return data;
        }

        public T PopFront()
        {
            if (Length == 0) throw new InvalidOperationException("データが空です。");
            var data = this[0];
            firstIndex++;
            firstIndex %= capacity;
            Length--;
            return data;
        }

        private void Resize()
        {
            var newArray = new T[capacity * 2];
            for (int i = 0; i < Length; i++)
            {
                newArray[i] = this[i];
            }

            firstIndex = 0;
            capacity *= 2;
            buffer = newArray;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
            {
                yield return this[i];
            }
        }
    }
}