using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    public class BitVector
    {
        private const uint W = 64;
        private List<ulong> block;
        private List<uint> count;
        private uint n;
        public uint Zeros { get; set; }

        [MethodImpl(256)]
        public uint Get(uint i) => (uint)(block[(int)(i / W)] >> (int)(i % W)) & 1u;

        [MethodImpl(256)]
        public void Set(uint i) => block[(int)(i / W)] |= 1UL << (int)(i % W);

        public BitVector()
        {
        }

        public BitVector(int n) => Init(n);

        [MethodImpl(256)]
        private void Init(int n)
        {
            this.n = Zeros = (uint)n;
            block = Enumerable.Repeat(0UL, n / (int)W + 1).ToList();
            count = Enumerable.Repeat(0U, block.Count).ToList();
        }

        [MethodImpl(256)]
        public void Build()
        {
            for (int i = 1; i < block.Count; ++i)
                count[i] = count[i - 1] + (uint)BitOperations.PopCount(block[i - 1]);
            Zeros = Rank0(n);
        }

        [MethodImpl(256)]
        public uint Rank0(uint i) => i - Rank1(i);

        [MethodImpl(256)]
        public uint Rank1(uint i)
        {
            ulong val = block[(int)(i / W)];
            if (i % W == 0)
            {
                val = 0;
            }
            else
            {
                val <<= (int)(64 - (i % W));
            }

            return count[(int)(i / W)] + (uint)BitOperations.PopCount(val);
        }
    }
}