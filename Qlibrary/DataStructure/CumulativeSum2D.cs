using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using static System.Math;

namespace Qlibrary
{
    public class CumulativeSum2D
    {
        private readonly int height;
        private readonly int width;
        private bool isCalculated;
        private readonly long[][] box;
        
        public CumulativeSum2D(int height, int width)
        {
            this.height = height;
            this.width = width;
            box = Enumerable.Repeat(0, height + 2).Select(_ => new long[width + 2]).ToArray();
        }

        [MethodImpl(256)]
        public void Add(int h, int w, long value)
        {
            box[h+1][w+1] += value;
        }
        
        [MethodImpl(256)]
        public void AddSquare((int h,int w) start, (int h,int w) end, long value)
        {
            start = (start.h + 1, start.w + 1);
            end = (end.h + 1, end.w + 1);
            box[RoundH(start.h)][RoundW(start.w)] += value;
            box[RoundH(end.h + 1)][RoundW(start.w)] -= value;
            box[RoundH(start.h)][RoundW(end.w + 1)] -= value;
            box[RoundH(end.h + 1)][RoundW(end.w + 1)] += value;
        }

        [MethodImpl(256)]
        private int RoundH(int v) => Min(Max(0, v), height + 1);
        [MethodImpl(256)]
        private int RoundW(int v) => Min(Max(0, v), width + 1);

        public void Calculate()
        {
            foreach (var row in box)
            {
                for (int j = 0; j < box[0].Length - 1; j++)
                {
                    row[j + 1] += row[j];
                }
            }
                    
            for (int i = 0; i < box[0].Length; i++)
            {
                for (int j = 0; j < box.Length - 1; j++)
                {
                    box[j + 1][i] += box[j][i];
                }
            }
            isCalculated = true;
        }

        public long this[int h, int w]
        {
            get
            {
                if (h < 0 || w < 0)
                {
                    return 0;
                }
                if (!isCalculated)
                {
                    Calculate();
                }
                return box[h + 1][w + 1];
            }
        }

        public long GetSquare((int h,int w) c1, (int h,int w) c2)
        {
            if (!isCalculated)
            {
                Calculate();
            }
            var s = (Min(c1.h, c2.h) - 1, Min(c1.w, c2.w) - 1);
            var t = (Max(c1.h, c2.h), Max(c1.w, c2.w));
            return this[t.Item1, t.Item2] - this[t.Item1, s.Item2] - this[s.Item1, t.Item2] + this[s.Item1,s.Item2];
        }
    }
}