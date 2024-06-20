using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    public class UndoableUnionFind<T>
    {
        private int num;
        private readonly int[] data;
        private readonly T[] val;
        private readonly Stack<(int, int, T)> hist;
        private readonly Func<T, T, T> onUnite;

        public UndoableUnionFind(T[] values, Func<T, T, T> onUnite)
        {
            this.onUnite = onUnite;
            num = values.Length;
            data = new int[num];
            data.Init(-1);
            val = values;
            hist = new Stack<(int, int, T)>();
        }

        [MethodImpl(256)]
        private int Find(int x)
        {
            while (true)
            {
                if (data[x] < 0) return x;
                x = data[x];
            }
        }

        [MethodImpl(256)]
        public bool Unite(int x, int y)
        {
            x = Find(x);
            y = Find(y);
            hist.Push((-1, num, default));
            hist.Push((x, data[x], val[x]));
            hist.Push((y, data[y], val[y]));
            if (x == y) return false;
            if (data[x] > data[y]) (x, y) = (y, x);
            data[x] += data[y];
            data[y] = x;
            val[x] = onUnite(val[x], val[y]);
            return true;
        }

        [MethodImpl(256)]
        public int Size(int x) => -data[Find(x)];
        
        [MethodImpl(256)]
        public bool Same(int x, int y) => Find(x) == Find(y);

        [MethodImpl(256)]
        public bool Undo()
        {
            if (hist.Count == 0) return false;
            while (true)
            {
                var(x, y, z) = hist.Pop();
                if (x < 0)
                {
                    num = y;
                    break;
                }
                data[x] = y;
                val[x] = z;
            }
            return true;
        }

        [MethodImpl(256)]
        public T Get(int x) => val[Find(x)];

        [MethodImpl(256)]
        public void Update(int x, T value)
        {
            x = Find(x);
            hist.Push((-1, num, default));
            hist.Push((x, data[x], val[x]));
            val[x] = onUnite(val[x], value);
        }

        [MethodImpl(256)]
        public int Num() => num;
    }
}