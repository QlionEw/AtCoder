using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    public readonly struct ModInt : INumber<ModInt>
    {
        private readonly long value;
        public const int _1000000007 = 1000000007;
        public const int _1000000009 = 1000000009;
        public const int _998244353 = 998244353;
        public static int ModValue { get; set; } = _998244353;
        private static readonly List<ModInt> Facts = new() {1};
        private static readonly List<ModInt> Inverts = new() {1};
        private ModInt(long value) => this.value = value;
        [MethodImpl(256)]
        public static implicit operator ModInt(long a) => new ModInt(a % ModValue + (a < 0 ? ModValue : 0));
        [MethodImpl(256)]
        public static explicit operator int(ModInt a) => (int) a.value;
        [MethodImpl(256)]
        public override string ToString() => value.ToString();
        [MethodImpl(256)]
        public static ModInt operator +(ModInt a, ModInt b) => a.value + b.value;
        [MethodImpl(256)]
        public static ModInt operator -(ModInt a, ModInt b) => a.value - b.value;
        [MethodImpl(256)]
        public static ModInt operator *(ModInt a, ModInt b) => a.value * b.value;
        [MethodImpl(256)]
        public static ModInt operator /(ModInt a, ModInt b) => a * MathPlus.BigPow((long)b, ModValue - 2, ModValue);
        [MethodImpl(256)]
        public static ModInt Fraction(long top, long bottom) => new ModInt(top) / bottom;
        [MethodImpl(256)]
        public static ModInt Pow(ModInt a, long n) => MathPlus.BigPow((long)a, n, ModValue);
        [MethodImpl(256)]
        public static ModInt Inv(int n)
        {
            for (int i = Inverts.Count; i <= n; i++) Inverts.Add(Inverts[^1] / i);
            return Inverts[n];
        }
        [MethodImpl(256)]
        public static ModInt Fact(int n)
        {
            for (int i = Facts.Count; i <= n; i++) Facts.Add(Facts[^1] * i);
            return Facts[n];
        }
        [MethodImpl(256)]
        public static ModInt Perm(int n, int r)
        {
            if (r < 0 || n < 0) return 0;
            if (r == 0) return 1;
            if (n <= 0) return 0;
            if (n < r) return 0;
            
            return Fact(n) * Inv(n - r);
        }
        [MethodImpl(256)]
        public static ModInt Comb(int n, int r)
        {
            if (r < 0 || n < 0) return 0;
            if (r == 0) return 1;
            if (n <= 0) return 0;
            if (n < r) return 0;

            return Fact(n) * Inv(n - r) * Inv(r);
        }
        [MethodImpl(256)]
        public bool Equals(ModInt other) => value == other.value;
        [MethodImpl(256)]
        public override bool Equals(object obj) => obj is ModInt other && Equals(other);
        [MethodImpl(256)]
        public override int GetHashCode() => value.GetHashCode();
        [MethodImpl(256)]
        public int CompareTo(ModInt other)
        {
            if (value == other.value) return 0;
            return value < other.value ? 1 : -1;
        }
        [MethodImpl(256)]
        public static ModInt operator %(ModInt left, ModInt right) => left.value % right.value;
        [MethodImpl(256)]
        public static ModInt operator +(ModInt value) => value;
        [MethodImpl(256)]
        public string ToString(string format, IFormatProvider formatProvider) => ((int)(ModInt)value).ToString();
        [MethodImpl(256)]
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider)
        {
            string result = ToString();

            if (result.TryCopyTo(destination)) {
                charsWritten = result.Length;
                return true;
            }

            charsWritten = 0;
            return false;
        }
        [MethodImpl(256)]
        public int CompareTo(object obj)
        {
            if(obj is not ModInt v) throw new InvalidOperationException();
            return CompareTo(v);
        }
        [MethodImpl(256)]
        public static ModInt Parse(string s, IFormatProvider provider) => Parse(s.AsSpan(), provider);
        [MethodImpl(256)]
        public static bool TryParse(string s, IFormatProvider provider, out ModInt result) => TryParse(s.AsSpan(), provider, out result);
        [MethodImpl(256)]
        public static ModInt Parse(ReadOnlySpan<char> s, IFormatProvider provider) => Parse(s);
        [MethodImpl(256)]
        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider provider, out ModInt result) => TryParse(s, out result);
        [MethodImpl(256)]
        public static ModInt Parse(ReadOnlySpan<char> s) => TryParse(s, out var v) ? v : throw new FormatException();
        [MethodImpl(256)]
        public static bool TryParse(ReadOnlySpan<char> s, out ModInt result)
        {
            bool isMinus = false;
            if (s[0] == '-' && s.Length >= 2)
            {
                isMinus = true;
                s = s[1..];
            }
            result = 0;
            foreach (var c in s)
            {
                if (c is < '0' or > '9')
                {
                    return false;
                }
                result = result * 10 + (c - '0');
            }
            if (isMinus)
            {
                result = -result;
            }
            return true;
        }
        [MethodImpl(256)]
        public static bool operator ==(ModInt left, ModInt right) => (int)left == (int)right;
        [MethodImpl(256)]
        public static bool operator !=(ModInt left, ModInt right) => (int)left != (int)right;
        [MethodImpl(256)]
        public static bool operator >(ModInt left, ModInt right) => (int)left > (int)right;
        [MethodImpl(256)]
        public static bool operator >=(ModInt left, ModInt right) => (int)left >= (int)right;
        [MethodImpl(256)]
        public static bool operator <(ModInt left, ModInt right) => (int)left < (int)right;
        [MethodImpl(256)]
        public static bool operator <=(ModInt left, ModInt right) => (int)left <= (int)right;
        [MethodImpl(256)]
        public static ModInt operator --(ModInt value) => value - 1;
        [MethodImpl(256)]
        public static ModInt operator ++(ModInt value) => value + 1;
        [MethodImpl(256)]
        public static ModInt operator -(ModInt value) => ModValue - value;
        [MethodImpl(256)]
        public static ModInt Abs(ModInt value) => value;
        [MethodImpl(256)]
        public static bool IsCanonical(ModInt value) => true;
        [MethodImpl(256)]
        public static bool IsComplexNumber(ModInt value) => false;
        [MethodImpl(256)]
        public static bool IsEvenInteger(ModInt value)
        {
            if (ModValue % 2 == 0)
            {
                return value.value % 2 == 0;
            }
            throw new InvalidOperationException();
        }
        [MethodImpl(256)]
        public static bool IsFinite(ModInt value) => true;
        [MethodImpl(256)]
        public static bool IsImaginaryNumber(ModInt value) => false;
        [MethodImpl(256)]
        public static bool IsInfinity(ModInt value) => false;
        [MethodImpl(256)]
        public static bool IsInteger(ModInt value) => true;
        [MethodImpl(256)]
        public static bool IsNaN(ModInt value) => false;
        [MethodImpl(256)]
        public static bool IsNegative(ModInt value) => throw new InvalidOperationException();
        [MethodImpl(256)]
        public static bool IsNegativeInfinity(ModInt value) => false;
        [MethodImpl(256)]
        public static bool IsNormal(ModInt value) => true;
        [MethodImpl(256)]
        public static bool IsOddInteger(ModInt value) => !IsEvenInteger(value);
        [MethodImpl(256)]
        public static bool IsPositive(ModInt value) => throw new InvalidOperationException();
        [MethodImpl(256)]
        public static bool IsPositiveInfinity(ModInt value) => false;
        [MethodImpl(256)]
        public static bool IsRealNumber(ModInt value) => true;
        [MethodImpl(256)]
        public static bool IsSubnormal(ModInt value) => false;
        [MethodImpl(256)]
        public static bool IsZero(ModInt value) => value == 0;
        [MethodImpl(256)]
        public static ModInt MaxMagnitude(ModInt x, ModInt y) => throw new InvalidOperationException(); // HACK: 比較することはない
        [MethodImpl(256)]
        public static ModInt MaxMagnitudeNumber(ModInt x, ModInt y) => throw new InvalidOperationException(); // HACK: 比較することはない
        [MethodImpl(256)]
        public static ModInt MinMagnitude(ModInt x, ModInt y) => throw new InvalidOperationException(); // HACK: 比較することはない
        [MethodImpl(256)]
        public static ModInt MinMagnitudeNumber(ModInt x, ModInt y) => throw new InvalidOperationException(); // HACK: 比較することはない
        [MethodImpl(256)]
        public static ModInt Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider) =>
            Parse(s, provider);
        [MethodImpl(256)]
        public static ModInt Parse(string s, NumberStyles style, IFormatProvider provider) =>
            Parse(s.AsSpan(), provider);
        [MethodImpl(256)]
        public static bool TryConvertFromChecked<TOther>(TOther value, out ModInt result) where TOther : INumberBase<TOther>
        {
            switch (value)
            {
                case long otherLong:
                    result = new ModInt(otherLong);
                    return true;
                case int other:
                    result = new ModInt(other);
                    return true;
                default:
                    result = default;
                    return false;
            }
        }
        [MethodImpl(256)]
        public static bool TryConvertFromSaturating<TOther>(TOther value, out ModInt result) where TOther : INumberBase<TOther>
            => TryConvertFromChecked(value, out result);
        [MethodImpl(256)]
        public static bool TryConvertFromTruncating<TOther>(TOther value, out ModInt result) where TOther : INumberBase<TOther>
            => TryConvertFromChecked(value, out result);
        [MethodImpl(256)]
        public static bool TryConvertToChecked<TOther>(ModInt value, out TOther result) where TOther : INumberBase<TOther>
        {
            return TOther.TryConvertFromChecked((int)value, out result);
        }
        [MethodImpl(256)]
        public static bool TryConvertToSaturating<TOther>(ModInt value, out TOther result) where TOther : INumberBase<TOther>
        {
            return TOther.TryConvertFromSaturating((int)value, out result);
        }
        [MethodImpl(256)]
        public static bool TryConvertToTruncating<TOther>(ModInt value, out TOther result) where TOther : INumberBase<TOther>
        {
            return TOther.TryConvertFromTruncating((int)value, out result);
        }
        [MethodImpl(256)]
        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider,
            out ModInt result)
            => TryParse(s, out result);
        [MethodImpl(256)]
        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out ModInt result) =>
            TryParse(s, out result);

        public static ModInt One => 1;
        public static int Radix => 10;
        public static ModInt Zero => 0;
        public static ModInt MultiplicativeIdentity => One;
        public static ModInt AdditiveIdentity => Zero;
    }
}