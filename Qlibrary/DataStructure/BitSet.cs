using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using static System.Math;

namespace Qlibrary
{
    public class BitSet : IEnumerable<ulong>
    {
        private static readonly ulong[] Bit = Enumerable.Range(0, 64).Select(x => 1UL << 63 - x).ToArray();
        public readonly ulong[] Box;

        public BitSet(IEnumerable<bool> bits)
        {
            var array = bits.ToArray();
            Box = new ulong[array.Length / 64 + 1];
            for (int i = 0; i < array.Length; i++)
            {
                Box[i / 64] += Bit[i%64] * (array[i] ? 1UL : 0UL);
            }
        }
        
        public BitSet(int size)
        {
            Box = Enumerable.Repeat(0UL, size / 64 + 1).ToArray();
        }
        
        public BitSet(BitSet bitSet)
        {
            Box = new ulong[bitSet.Box.Length];
            for (int i = 0; i < Box.Length; i++)
            {
                Box[i] = bitSet.Box[i];
            }
        }

        public bool this[int w]
        {
            [MethodImpl(256)] get => (Box[w / 64] & Bit[w % 64]) != 0;
            [MethodImpl(256)] set
            {
                Box[w / 64] -= (this[w] ? 1UL : 0UL) * Bit[w % 64];
                Box[w / 64] += (value ? 1UL : 0UL) * Bit[w % 64];
            }
        }
        [MethodImpl(256)] 
        public static BitSet operator &(BitSet a, BitSet b)
        {
            int size = Min(a.Box.Length, b.Box.Length);
            for (int i = 0; i < size; i++) a.Box[i] &= b.Box[i];
            return a;
        }
        [MethodImpl(256)] 
        public static BitSet operator |(BitSet a, BitSet b)
        {
            int size = Min(a.Box.Length, b.Box.Length);
            for (int i = 0; i < size; i++) a.Box[i] |= b.Box[i];
            return a;
        }
        [MethodImpl(256)] 
        public static BitSet operator ^(BitSet a, BitSet b)
        {
            int size = Min(a.Box.Length, b.Box.Length);
            for (int i = 0; i < size; i++) a.Box[i] ^= b.Box[i];
            return a;
        }
        public int PopCount => Box.Sum(BitOperations.PopCount);
        public int Length => Box.Length;
        [MethodImpl(256)] public IEnumerator<ulong> GetEnumerator() => Box.ToList().GetEnumerator();
        [MethodImpl(256)] IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}