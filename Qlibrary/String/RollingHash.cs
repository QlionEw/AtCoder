using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    public class RollingHash : IEquatable<RollingHash>
    {
        private long Hash => (long)modHash;
        private ModInt modHash;
        private static int digit;
        private static Dictionary<long, int> hashDict;
        private static List<ModInt> Digits = new List<ModInt>() { 1 };
        public RollingHash(IEnumerable<char> values)
        {
            var ar = values.ToArray();
            GetDigit(ar.Length);
            for (int i = 0; i < ar.Length; i++)
            {
                modHash += hashDict[ar[i]] * GetDigit(i);
            }
        }

        [MethodImpl(256)]
        private ModInt GetDigit(int n)
        {
            while (Digits.Count <= n)
            {
                Digits.Add(Digits[^1] * digit);
            }
            return Digits[n];
        }

        [MethodImpl(256)]
        public void Add(int index, long value) => modHash += hashDict[value] * GetDigit(index);
        [MethodImpl(256)]
        public void Remove(int index, long value) => modHash -= hashDict[value] * GetDigit(index);
        [MethodImpl(256)]
        public void Replace(int index, long from, long to) => modHash += (hashDict[to] - hashDict[from]) * GetDigit(index);
        [MethodImpl(256)]
        public void LeftShift(int length, long prev, long next)
        {
            Remove(length - 1, prev);
            modHash *= digit;
            Add(0, next);
        }
        [MethodImpl(256)]
        public void RightShift(int length, long prev, long next)
        {
            Remove(0, prev);
            modHash /= digit;
            Add(length - 1, next);
        }

        [MethodImpl(256)]
        public void Add(int index, char value) => Add(index, (long)value);
        [MethodImpl(256)]
        public void Remove(int index, char value) => Remove(index, (long)value);
        [MethodImpl(256)]
        public void Replace(int index, char from, char to) => Replace(index, (long)from, (long)to);
        [MethodImpl(256)]
        public void LeftShift(int length, char prev, char next) => LeftShift(length, (long)prev, (long)next);
        [MethodImpl(256)]
        public void RightShift(int length, char prev, char next) => RightShift(length, (long)prev, (long)next);

        public static void SetStringRange() => SetRange(Enumerable.Range('A', 26).Concat(Enumerable.Range('a', 26)).Select(x => (long)x));
        public static void SetUpperStringRange() => SetRange(Enumerable.Range('A', 26).Select(x => (long)x));
        public static void SetLowerStringRange() => SetRange(Enumerable.Range('a', 26).Select(x => (long)x));
        private static void SetRange(IEnumerable<long> range)
        {
            var ar = range.ToArray();
            digit = ar.Length;
            hashDict = new Dictionary<long, int>();
            for (int i = 0; i < digit; i++)
            {
                hashDict.Add(ar[i], i);
            }
        }
        [MethodImpl(256)]
        public static bool operator ==(RollingHash a, RollingHash b) => ReferenceEquals(a, b) || ((object)a != null && a.Equals(b));
        [MethodImpl(256)]
        public static bool operator!= (RollingHash a, RollingHash b) => !(a == b);
        [MethodImpl(256)]
        public bool Equals(RollingHash other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Hash.Equals(other.Hash);
        }
        [MethodImpl(256)]
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RollingHash)obj);
        }
        [MethodImpl(256)]
        public override int GetHashCode()
        {
            return (int)Hash;
        }
    }
}