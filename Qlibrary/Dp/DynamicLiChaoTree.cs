using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using static System.Math;
using static Qlibrary.Common;

namespace Qlibrary
{
    public class DynamicLiChaoTree
    {
        private readonly struct Line
        {
            private readonly long slope;
            private readonly long intercept;

            public Line(long slope, long intercept)
            {
                this.slope = slope;
                this.intercept = intercept;
            }
            public static Line Default => new Line(0, long.MaxValue);
            [MethodImpl(256)] public long Get(long x) => slope * x + intercept;
            [MethodImpl(256)] public bool Over(Line other, long x) => Get(x) < other.Get(x);
        };

        private readonly long xMin;
        private readonly long xMax;
        private readonly long size;
        private readonly Dictionary<long, Line> seg = new Dictionary<long, Line>();

        // [l , r]におけるLi Chao Tree
        public DynamicLiChaoTree(long xMin, long xMax)
        {
            this.xMin = xMin;
            this.xMax = xMax;
            size = 1;
            while (size < xMax - xMin + 1) size <<= 1;
        }
        
        [MethodImpl(256)] 
        private void Update(long a, long b, long left, long right, long index)
        {
            Line line = new Line(a, b);
            while (true)
            {
                seg.TryAdd(index, Line.Default);
                long mid = (left + right) >> 1;
                bool lOver = line.Over(seg[index], Min(xMax, left + xMin));
                bool rOver = line.Over(seg[index], Min(xMax, right - 1 + xMin));
                if (lOver == rOver)
                {
                    if (lOver) seg[index] = line;
                    return;
                }

                bool mOver = line.Over(seg[index], Min(xMax, mid + xMin));
                if (mOver) (seg[index], line) = (line, seg[index]);
                if (lOver != mOver)
                {
                    index = (index << 1);
                    right = mid;
                }
                else
                {
                    index = (index << 1) | 1;
                    left = mid;
                }
            }
        }

        [MethodImpl(256)] 
        private void Update(long a, long b, long index)
        {
            int upperBit = 63 - BitOperations.LeadingZeroCount((ulong)index);
            var left = (size >> upperBit) * (index - (1 << upperBit));
            var right = left + (size >> upperBit);
            Update(a, b, left, right, index);
        }

        // y = ax + bなる直線を追加
        [MethodImpl(256)] 
        public void Update(long a, long b)
        {
            Update(a, b, 0, size, 1);
        }

        // 閉区間x in [left , right]に線分y = ax + bを追加するクエリ
        [MethodImpl(256)] 
        public void UpdateLineSegment(long a, long b, long left, long right)
        {
            left -= xMin - size;
            right -= xMin - size - 1;
            for (; left < right; left >>= 1, right >>= 1)
            {
                if ((left & 1) == 1) Update(a, b, left++);
                if ((right & 1) == 1) Update(a, b, --right);
            }
        }

        // xにおける最小値クエリ
        [MethodImpl(256)] 
        public long Query(long x)
        {
            long left = 0, right = size, index = 1, idx = x - xMin;
            long ret = Infinity;
            while (true)
            {
                seg.TryAdd(index, Line.Default);
                long cur = seg[index].Get(x);
                // 線分追加クエリがない場合はここのコメントアウトを外して高速化可能(1.5倍程度？)
                // if(cur == Infinity) break;
                ret = Min(ret, cur);
                if (left + 1 >= right) break;
                long mid = (left + right) >> 1;
                if (idx < mid)
                {
                    index = (index << 1);
                    right = mid;
                }
                else
                {
                    index = (index << 1) | 1;
                    left = mid;
                }
            }

            return ret;
        }
    }
}