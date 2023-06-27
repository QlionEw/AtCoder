using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using static Qlibrary.Common;
using static System.Math;
using T = System.Int64;

namespace Qlibrary
{
    [SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
    public class ConvexFullTrickMonotone
    {
        private readonly bool isMin;
        public readonly Deque<(T, T)> H = new Deque<(T, T)>();

        public ConvexFullTrickMonotone(bool isMin)
        {
            this.isMin = isMin;
        }

        public int Length => H.Length;

        [MethodImpl(256)] 
        public void Clear() => H.Clear();

        [MethodImpl(256)]
        private static int Sgn(T x) => x == 0 ? 0 : (x < 0 ? -1 : 1);

        [MethodImpl(256)]
        private static bool Check((T, T) a, (T, T) b, (T, T) c)
        {
            if (b.Item2 == a.Item2 || c.Item2 == b.Item2)
            {
                return Sgn(b.Item1 - a.Item1) * Sgn(c.Item2 - b.Item2) >=
                       Sgn(c.Item1 - b.Item1) * Sgn(b.Item2 - a.Item2);
            }

            if (typeof(T) == typeof(int) || typeof(T) == typeof(long))
            {
                return (b.Item2 - a.Item2) / (a.Item1 - b.Item1) >= (c.Item2 - b.Item2) / (b.Item1 - c.Item1);
            }
            return (b.Item1 - a.Item1) * Sgn(c.Item2 - b.Item2) / Abs(b.Item2 - a.Item2) >=
                   (c.Item1 - b.Item1) * Sgn(b.Item2 - a.Item2) / Abs(c.Item2 - b.Item2);
        }

        [MethodImpl(256)]
        public void Add(T a, T b)
        {
            if (!isMin)
            {
                a *= -1;
                b *= -1;
            }

            (T, T) line = (a, b);
            if (Length == 0)
            {
                H.PushFront(line);
                return;
            }

            if (H.PeekFront().Item1 <= a)
            {
                if (H.PeekFront().Item1 == a)
                {
                    if (H.PeekFront().Item2 <= b) return;
                    H.PopFront();
                }

                while (H.Length >= 2 && Check(line, H.PeekFront(), H[1])) H.PopFront();
                H.PushFront(line);
            }
            else
            {
                if (H.PeekBack().Item1 == a)
                {
                    if (H.PeekBack().Item2 <= b) return;
                    H.PopBack();
                }

                while (H.Length >= 2 && Check(H[^2], H.PeekBack(), line)) H.PopBack();
                H.PushBack(line);
            }
        }

        private static T GetY((T, T) a, T x) => a.Item1 * x + a.Item2;
        
        [MethodImpl(256)]
        public T Query(T x)
        {
            int l = -1, r = H.Length - 1;
            while (l + 1 < r)
            {
                int m = (l + r) >> 1;
                if (GetY(H[m], x) >= GetY(H[m + 1], x)) l = m;
                else r = m;
            }

            if (isMin) return GetY(H[r], x);
            return -GetY(H[r], x);
        }

        /// <summary> 単調増加のクエリ </summary>;
        [MethodImpl(256)]
        public T QueryMonotoneIncrease(T x)
        {
            while (H.Length >= 2 && GetY(H.PeekFront(), x) >= GetY(H[1], x)) H.PopFront();
            if (isMin) return GetY(H.PeekFront(), x);
            return -GetY(H.PeekFront(), x);
        }
        
        /// <summary> 単調減少のクエリ </summary>;
        [MethodImpl(256)]
        public T QueryMonotoneDecrease(T x)
        {
            while(H.Length >= 2 && GetY(H.PeekBack(), x) >= GetY(H[^2], x)) H.PopBack();
            if(isMin) return GetY(H.PeekBack(), x);
            return -GetY(H.PeekBack(), x);
        }
    };
}