using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    public class RollingHash : IEquatable<RollingHash>
    {
        public int Length { get; private set; }
        private long Hash => (long)modHash;
        private ModInt modHash;
        private static int digit;
        private static Dictionary<long, int> hashDict;
        private static List<ModInt> Digits = new List<ModInt>() { 1 };

        public RollingHash()
        {
        }

        public RollingHash(long value)
        {
            Length = 1;
            modHash += hashDict[value];
        }

        public RollingHash(string values) : this(values.Select(x => (long)x))
        {
        }

        public RollingHash(IEnumerable<long> values)
        {
            var array = values.ToList();
            Length = array.Count;
            GetDigit(array.Count);
            for (int i = 0; i < array.Count; i++)
            {
                modHash += hashDict[array[i]] * GetDigit(i);
            }
        }
        
        public RollingHash(ModInt value, int length)
        {
            Length = length;
            modHash = value;
        }

        [MethodImpl(256)]
        private static ModInt GetDigit(int n)
        {
            while (Digits.Count <= n)
            {
                Digits.Add(Digits[^1] * digit);
            }
            return Digits[n];
        }

        [MethodImpl(256)]
        public void Add(int index, long value) => modHash += value * GetDigit(index);
        [MethodImpl(256)]
        public void Remove(int index, long value) => modHash -= value * GetDigit(index);
        [MethodImpl(256)]
        public void Replace(int index, long from, long to) => modHash += (hashDict[to] - hashDict[from]) * GetDigit(index);
        [MethodImpl(256)]
        public void LeftShift(long prev, long next)
        {
            Remove(0, prev);
            modHash /= digit;
            Add(Length - 1, next);
        }
        [MethodImpl(256)]
        public void RightShift(long prev, long next)
        {
            Remove(Length - 1, prev);
            modHash *= digit;
            Add(0, next);
        }

        [MethodImpl(256)]
        public void Replace(int index, char from, char to) => Replace(index, hashDict[from], hashDict[to]);
        [MethodImpl(256)]
        public void LeftShift(char prev, char next) => LeftShift(hashDict[prev], hashDict[next]);
        [MethodImpl(256)]
        public void RightShift(char prev, char next) => RightShift(hashDict[prev], hashDict[next]);

        public static void SetStringRange() => SetRange(Enumerable.Range('A', 26).Concat(Enumerable.Range('a', 26)).Select(x => (long)x));
        public static void SetUpperStringRange() => SetRange(Enumerable.Range('A', 26).Select(x => (long)x));
        public static void SetLowerStringRange() => SetRange(Enumerable.Range('a', 26).Select(x => (long)x));
        public static void SetRange(IEnumerable<long> range)
        {
            var rangeArray = range.ToArray();
            digit = rangeArray.Length + 5;
            hashDict = new Dictionary<long, int>();
            for (int i = 1; i <= rangeArray.Length; i++)
            {
                hashDict.Add(rangeArray[i-1], i);
            }
        }

        [MethodImpl(256)]
        public static RollingHash operator +(RollingHash a, RollingHash b)
            => new(a.Hash + b.Hash * GetDigit(a.Length), a.Length + b.Length);
        [MethodImpl(256)]
        public static bool operator ==(RollingHash a, RollingHash b)
            => ReferenceEquals(a, b) || ((object)a != null && a.Equals(b));
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