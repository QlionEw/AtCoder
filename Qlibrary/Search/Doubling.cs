using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using static System.Math;

namespace Qlibrary
{
    public class Doubling
    {
        private bool isBuild;
        private readonly int size;
        private readonly int logSize;
        private readonly int[,] table;

        public Doubling(int size, long maxOperationCount)
        {
            this.size = size;
            logSize = 64 - BitOperations.LeadingZeroCount((ulong)maxOperationCount);
            table = new int[size, logSize];
            table.Init(-1);
        }

        [MethodImpl(256)]
        public void SetNext(int from, int to)
        { 
            Debug.Assert(!isBuild, "[Doubling] Do not SetNext() after querying.");
            table[from,0] = to;
        }

        [MethodImpl(256)]
        public void Build()
        {
            for (int k = 0; k + 1 < logSize; k++)
            {
                for (int i = 0; i < size; i++)
                {
                    if (table[i,k] == -1)
                    {
                        table[i, k + 1] = -1;
                    }
                    else
                    {
                        table[i, k + 1] = table[table[i,k], k];
                    }
                }
            }
            isBuild = true;
        }

        [MethodImpl(256)]
        public int Query(int from, long count)
        {
            if (!isBuild) Build();
            for (int k = logSize - 1; k >= 0; k--)
            {
                if (((count >> k) & 1) == 1)
                {
                    from = table[from,k];
                }
            }
            return from;
        }
    }
}