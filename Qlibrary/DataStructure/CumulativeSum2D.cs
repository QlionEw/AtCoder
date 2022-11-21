using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    public class CumulativeSum2D : IEnumerable<IEnumerable<long>>
    {
        private bool isCalculated;
        private readonly long[][] box;
        
        public CumulativeSum2D(int height, int width)
        {
            box = Enumerable.Repeat(0, height).Select(_ => new long[width]).ToArray();
        }

        [MethodImpl(256)]
        public void Add(int height, int width, long value)
        {
            box[height][width] += value;
        }
        
        [MethodImpl(256)]
        public void AddSquare((int h,int w) start, (int h,int w) end, long value)
        {
            box[start.h][start.w] += value;
            if (end.h + 1 < box.Length)
            {
                box[end.h + 1][start.w] -= value;
            }
            if (end.w + 1 < box[0].Length)
            {
                box[start.h][end.w + 1] -= value;
            }
            if (end.h + 1 < box.Length && end.w + 1 < box[0].Length)
            {
                box[end.h + 1][end.w + 1] += value;
            }
        }

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
                if (!isCalculated)
                {
                    Calculate();
                }
                return box[h][w];
            }
        }

        public IEnumerator<IEnumerable<long>> GetEnumerator()
        {
            if (!isCalculated)
            {
                Calculate();
            }
            return box.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (!isCalculated)
            {
                Calculate();
            }
            return GetEnumerator();
        }
    }
}