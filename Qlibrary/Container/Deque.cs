using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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
        
        public Deque(IEnumerable<T> initials, int capacity = 16) : this(capacity)
        {
            foreach (var initial in initials)
            {
                PushBack(initial);
            }
        }

        [MethodImpl(256)]
        public void PushBack(T data)
        {
            if (Length == capacity) Resize();
            buffer[LastIndex] = data;
            Length++;
        }

        [MethodImpl(256)]
        public void PushFront(T data)
        {
            if (Length == capacity) Resize();
            var index = firstIndex - 1;
            if (index < 0) index = capacity - 1;
            buffer[index] = data;
            Length++;
            firstIndex = index;
        }

        [MethodImpl(256)]
        public T PopBack()
        {
            var data = PeekBack();
            Length--;
            return data;
        }

        [MethodImpl(256)]
        public T PopFront()
        {
            var data = PeekFront();
            firstIndex++;
            firstIndex %= capacity;
            Length--;
            return data;
        }

        [MethodImpl(256)]
        public T PeekBack()
        {
            if (Length == 0) throw new InvalidOperationException("データが空です。");
            return this[Length - 1];
        }
        
        [MethodImpl(256)]
        public T PeekFront()
        {
            if (Length == 0) throw new InvalidOperationException("データが空です。");
            return this[0];
        }

        [MethodImpl(256)]
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