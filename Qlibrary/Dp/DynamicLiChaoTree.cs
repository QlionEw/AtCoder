using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using static System.Math;
using static Qlibrary.Common;

namespace Qlibrary
{
    public class DynamicLiChaoTree<T> where T : INumber<T>, IShiftOperators<T,int,T>, IBitwiseOperators<T,T,T>
    {
        private readonly struct Line<TLine> where TLine : INumber<TLine>
        {
            private readonly TLine slope;
            private readonly TLine intercept;

            public Line(TLine slope, TLine intercept)
            {
                this.slope = slope;
                this.intercept = intercept;
            }
            public static Line<TLine> Default => new Line<TLine>(TLine.Zero, TLine.CreateSaturating(long.MaxValue));
            [MethodImpl(256)] public TLine Get(TLine x) => slope * x + intercept;
            [MethodImpl(256)] public bool Over(Line<TLine> other, TLine x) => Get(x) < other.Get(x);
        };

        private readonly T xMin;
        private readonly T xMax;
        private readonly T size;
        private readonly Dictionary<T, Line<T>> seg = new();

        // [l , r]におけるLi Chao Tree
        public DynamicLiChaoTree(T xMin, T xMax)
        {
            this.xMin = xMin;
            this.xMax = xMax;
            size = T.One;
            while (size < xMax - xMin + T.One) size <<= 1;
        }
        
        [MethodImpl(256)] 
        private void Update(T a, T b, T left, T right, T index)
        {
            Line<T> line = new Line<T>(a, b);
            while (true)
            {
                seg.TryAdd(index, Line<T>.Default);
                T mid = (left + right) >> 1;
                bool lOver = line.Over(seg[index], GMin(xMax, left + xMin));
                bool rOver = line.Over(seg[index], GMin(xMax, right - T.One + xMin));
                if (lOver == rOver)
                {
                    if (lOver) seg[index] = line;
                    return;
                }

                bool mOver = line.Over(seg[index], GMin(xMax, mid + xMin));
                if (mOver) (seg[index], line) = (line, seg[index]);
                if (lOver != mOver)
                {
                    index = (index << 1);
                    right = mid;
                }
                else
                {
                    index = (index << 1) | T.One;
                    left = mid;
                }
            }
        }

        [MethodImpl(256)] 
        private void Update(T a, T b, T index)
        {
            int upperBit = 63 - BitOperations.LeadingZeroCount(ulong.CreateChecked(index));
            var left = (size >> upperBit) * (index - (T.One << upperBit));
            var right = left + (size >> upperBit);
            Update(a, b, left, right, index);
        }

        // y = ax + bなる直線を追加
        [MethodImpl(256)] 
        public void Update(T a, T b)
        {
            Update(a, b, T.Zero, size, T.One);
        }

        // 閉区間x in [left , right]に線分y = ax + bを追加するクエリ
        [MethodImpl(256)] 
        public void UpdateLineSegment(T a, T b, T left, T right)
        {
            left -= xMin - size;
            right -= xMin - size - T.One;
            for (; left < right; left >>= 1, right >>= 1)
            {
                if ((left & T.One) == T.One) Update(a, b, left++);
                if ((right & T.One) == T.One) Update(a, b, --right);
            }
        }

        // xにおける最小値クエリ
        [MethodImpl(256)] 
        public T Query(T x)
        {
            T left = T.Zero, right = size, index = T.One, idx = x - xMin;
            T ret = GetInfinity<T>();
            while (true)
            {
                seg.TryAdd(index, Line<T>.Default);
                T cur = seg[index].Get(x);
                // 線分追加クエリがない場合はここのコメントアウトを外して高速化可能(1.5倍程度？)
                // if(cur == Infinity) break;
                ret = GMin(ret, cur);
                if (left + T.One >= right) break;
                T mid = (left + right) >> 1;
                if (idx < mid)
                {
                    index = (index << 1);
                    right = mid;
                }
                else
                {
                    index = (index << 1) | T.One;
                    left = mid;
                }
            }

            return ret;
        }
    }
}