using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using static Qlibrary.Common;

namespace Qlibrary
{
    public class Box<T> : IEnumerable<T[]>
    {
        private T[][] plane;
        private int row;
        private int column;
        private T defValue;

        public Box(int h, int w, T def = default)
        {
            Init(h, w, def, () => Make(h, () => new T[w]));
            if (def.Equals(default(T))) {return;}
            foreach (var array in plane)
            {
                Array.Fill(array, def);
            }
        }

        private void Init(int h, int w, T def, Func<T[][]> setPlane)
        {
            row = h;
            column = w;
            defValue = def;
            plane = setPlane();
        }

        public Box(T[][] value, T def = default) => Init(value.Length, value[0].Length, def, () => value);
        public Box(IEnumerable<IEnumerable<T>> value, T def = default)
            : this(value.Select(x => x.ToArray()).ToArray(), def){ }
        public Box(T[,] value, T def = default) : this(value.ToJaggedArray(), def) { }

        public T this[int h, int w]
        {
            [MethodImpl(256)] get => 0 <= h && h < row && 0 <= w && w < column ? plane[h][w] : defValue;
            [MethodImpl(256)] set { if(IsInRange(h, w)) plane[h][w] = value; }
        }

        [MethodImpl(256)] public T Left(int h, int w) => IsInRange(h, w-1) ? plane[h][w - 1] : defValue;
        [MethodImpl(256)] public T Right(int h, int w) => IsInRange(h, w+1) ? plane[h][w + 1] : defValue;
        [MethodImpl(256)] public T Up(int h, int w) => IsInRange(h-1, w) ? plane[h - 1][w] : defValue;
        [MethodImpl(256)] public T Down(int h, int w) => IsInRange(h+1, w) ? plane[h + 1][w] : defValue;
        [MethodImpl(256)] public bool IsInRange(int h, int w) => 0 <= h && h < row && 0 <= w && w < column;
        [MethodImpl(256)] public int Index1D(int h, int w) => h * column + w;

        public override string ToString()
            => Answerer.HoldAllLine(Enumerable.Range(0, row).Select(x => Answerer.HoldEachSpace(plane[x])));

        public IEnumerator<T[]> GetEnumerator() => ((IEnumerable<T[]>)plane).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}