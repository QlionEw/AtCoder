using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using static System.Math;

namespace Qlibrary
{
    public class Doubling
    {
        private readonly int size;
        private readonly int logSize;
        private readonly int[,] table;

        public Doubling(int size, long maxOperationCount)
        {
            this.size = size;
            logSize = 64 - BitOperations.LeadingZeroCount((ulong)maxOperationCount);
            table = new int[logSize, size];
            table.Init(-1);
        }

        [MethodImpl(256)]
        public void SetNext(int k, int x) => table[0,k] = x;

        [MethodImpl(256)]
        public void Build()
        {
            for (int k = 0; k + 1 < logSize; k++)
            {
                for (int i = 0; i < size; i++)
                {
                    if (table[k,i] == -1)
                    {
                        table[k + 1,i] = -1;
                    }
                    else
                    {
                        table[k + 1,i] = table[k,table[k,i]];
                    }
                }
            }
        }

        [MethodImpl(256)]
        public int Query(int k, long t)
        {
            for (int i = logSize - 1; i >= 0; i--)
            {
                if (((t >> i) & 1) == 1)
                {
                    k = table[i,k];
                }
            }
            return k;
        }
    }
}