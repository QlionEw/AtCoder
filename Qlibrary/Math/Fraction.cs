using System;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using static System.Math;
using static Qlibrary.Common;
using static Qlibrary.MathPlus;

namespace Qlibrary
{
    /// <summary> 分数型 $\frac{X}{Y}$ </summary>
    public readonly struct Fraction : INumber<Fraction>
    {
        public long Numerator { get; }
        public long X => Numerator;
        public long Denominator { get; }
        public long Y => Denominator;
        public long Max => Max(X, Y);
        
        [MethodImpl(256)]
        public Fraction(long a, long b)
        {
            var gcd = Gcd(a, b);
            Numerator = a / gcd;
            Denominator = b / gcd;
            if (b >= 0) return;
            Numerator *= -1;
            Denominator *= -1;
        }
        [MethodImpl(256)]
        public Fraction(double value, long limit = Power9)
        {
            if (value >= 0)
            {
                var sbt = SternBrocotTree.BinarySearch(f => double.Abs(value) - (double)f.X / f.Y <= 0, limit);
                var min = (double)sbt.Lower.X / sbt.Lower.Y;
                var max = (double)sbt.Upper.X / sbt.Upper.Y;
                var v = double.Abs(min - value) <= double.Abs(max - value) ? sbt.Lower : sbt.Upper;
                Numerator = v.Numerator;
                Denominator = v.Denominator;
            }
            else
            {
                var sbt = SternBrocotTree.BinarySearch(f => double.Abs(value) - (double)f.X / f.Y <= 0, limit);
                var min = -(double)sbt.Lower.X / sbt.Lower.Y;
                var max = -(double)sbt.Upper.X / sbt.Upper.Y;
                var v = double.Abs(min - value) <= double.Abs(max - value) ? sbt.Lower : sbt.Upper;
                Numerator = -v.Numerator;
                Denominator = v.Denominator;
            }
        }
        [MethodImpl(256)]
        public Fraction(decimal value, long limit = Power9)
        {
            if (value >= 0)
            {
                var sbt = SternBrocotTree.BinarySearch(f => decimal.Abs(value) - (decimal)f.X / f.Y <= 0, limit);
                var min = (decimal)sbt.Lower.X / sbt.Lower.Y;
                var max = (decimal)sbt.Upper.X / sbt.Upper.Y;
                var v = decimal.Abs(min - value) <= decimal.Abs(max - value) ? sbt.Lower : sbt.Upper;
                Numerator = v.Numerator;
                Denominator = v.Denominator;
            }
            else
            {
                var sbt = SternBrocotTree.BinarySearch((f) => decimal.Abs(value) - (decimal)f.X / f.Y <= 0, limit);
                var min = -(decimal)sbt.Lower.X / sbt.Lower.Y;
                var max = -(decimal)sbt.Upper.X / sbt.Upper.Y;
                var v = decimal.Abs(min - value) <= decimal.Abs(max - value) ? sbt.Lower : sbt.Upper;
                Numerator = -v.Numerator;
                Denominator = v.Denominator;
            }
        }
        
        [MethodImpl(256)]
        public int CompareTo(object obj)
        {
            if (obj is Fraction other) return CompareTo(other);
            throw new ArithmeticException();
        }
        [MethodImpl(256)]
        public int CompareTo(Fraction other) => this < other ? -1 : this == other ? 0 : 1;
        [MethodImpl(256)]
        public bool Equals(Fraction other) => this == other;
        [MethodImpl(256)]
        public override bool Equals(object obj) => obj is Fraction other && Equals(other);
        [MethodImpl(256)]
        public override int GetHashCode() => HashCode.Combine(Denominator, Numerator);
        [MethodImpl(256)]
        public string ToString(string format, IFormatProvider formatProvider) => $"{X} / {Y}";
        [MethodImpl(256)]
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider)
        {
            string result = ToString();

            if (result != null && result.TryCopyTo(destination)) {
                charsWritten = result.Length;
                return true;
            }

            charsWritten = 0;
            return false;
        }
        [MethodImpl(256)]
        public static Fraction Parse(string s, IFormatProvider provider)
        {
            var v = s.Split('/');
            var x = long.Parse(v[0]);
            var y = v.Length > 1 ? long.Parse(v[1]) : 1;
            return new Fraction(x, y);
        }
        [MethodImpl(256)]
        public static bool TryParse(string s, IFormatProvider provider, out Fraction result)
        {
            try
            {
                if (s != null)
                {
                    result = Parse(s, provider);
                    return true;
                }
                result = Zero;
                return false;
            }
            catch (Exception)
            {
                result = Zero;
                return false;
            }
        }
        [MethodImpl(256)]
        public static Fraction Parse(ReadOnlySpan<char> s, IFormatProvider provider) => Parse(s.ToString(), provider);
        [MethodImpl(256)]
        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider provider, out Fraction result) =>
            TryParse(s.ToString(), provider, out result);
        [MethodImpl(256)]
        public static Fraction operator +(Fraction left, Fraction right) =>
            new Fraction(left.X * right.Y + left.Y * right.X, left.Y * right.Y);
        public static Fraction AdditiveIdentity => Zero;
        [MethodImpl(256)]
        public static bool operator ==(Fraction left, Fraction right) => left.X * right.Y - right.X * left.Y == 0;
        [MethodImpl(256)]
        public static bool operator !=(Fraction left, Fraction right) => !(left == right);
        [MethodImpl(256)]
        public static bool operator >(Fraction left, Fraction right) => left.X * right.Y - right.X * left.Y > 0;
        [MethodImpl(256)]
        public static bool operator >=(Fraction left, Fraction right) => !(left < right);
        [MethodImpl(256)]
        public static bool operator <(Fraction left, Fraction right) => left.X * right.Y - right.X * left.Y < 0;
        [MethodImpl(256)]
        public static bool operator <=(Fraction left, Fraction right) => !(left > right);
        [MethodImpl(256)]
        public static Fraction operator --(Fraction value) => new Fraction(value.X - value.Y, value.Y);
        [MethodImpl(256)]
        public static Fraction operator /(Fraction left, Fraction right) => new Fraction(left.X * right.Y, left.Y * right.X);
        [MethodImpl(256)]
        public static Fraction operator ++(Fraction value) => new Fraction(value.X + value.Y, value.Y);
        [MethodImpl(256)]
        public static Fraction operator %(Fraction left, Fraction right) =>
            new Fraction((left.X * right.Y) % (left.Y * right.X), left.Y * right.Y);
        public static Fraction MultiplicativeIdentity => One;
        [MethodImpl(256)]
        public static Fraction operator *(Fraction left, Fraction right) => new Fraction(left.X * right.X, left.Y * right.Y);
        [MethodImpl(256)]
        public static Fraction operator -(Fraction left, Fraction right) =>
            new Fraction(left.X * right.Y - left.Y * right.X, left.Y * right.Y);
        [MethodImpl(256)]
        public static Fraction operator -(Fraction value) => new Fraction(-value.X, value.Y);
        [MethodImpl(256)]
        public static Fraction operator +(Fraction value) => value;
        [MethodImpl(256)]
        public static Fraction Abs(Fraction value) => new Fraction(long.Abs(value.X), value.Y);
        [MethodImpl(256)]
        public static bool IsCanonical(Fraction value) => true;
        [MethodImpl(256)]
        public static bool IsComplexNumber(Fraction value) => false;
        [MethodImpl(256)]
        public static bool IsEvenInteger(Fraction value) => value.Y == 1 && long.IsEvenInteger(value.X);
        [MethodImpl(256)]
        public static bool IsFinite(Fraction value) => value.Y != 0;
        [MethodImpl(256)]
        public static bool IsImaginaryNumber(Fraction value) => false;
        [MethodImpl(256)]
        public static bool IsInfinity(Fraction value) => value.Y == 0;
        [MethodImpl(256)]
        public static bool IsInteger(Fraction value) => value.Y == 1;
        [MethodImpl(256)]
        public static bool IsNaN(Fraction value) => value is { X: 0, Y: 0 };
        [MethodImpl(256)]
        public static bool IsNegative(Fraction value) => value.X < 0;
        [MethodImpl(256)]
        public static bool IsNegativeInfinity(Fraction value) => value is { X: < 0, Y: 0 };
        [MethodImpl(256)]
        public static bool IsNormal(Fraction value) => true;
        [MethodImpl(256)]
        public static bool IsOddInteger(Fraction value) => value.Y == 1 && long.IsOddInteger(value.X);
        [MethodImpl(256)]
        public static bool IsPositive(Fraction value) => value.X >= 0;
        [MethodImpl(256)]
        public static bool IsPositiveInfinity(Fraction value) => value is { X: > 0, Y: 0 };
        [MethodImpl(256)]
        public static bool IsRealNumber(Fraction value) => true;
        [MethodImpl(256)]
        public static bool IsSubnormal(Fraction value) => false;
        [MethodImpl(256)]
        public static bool IsZero(Fraction value) => value.X == 0;
        [MethodImpl(256)]
        public static Fraction MaxMagnitude(Fraction x, Fraction y) => x > y ? x : y;
        [MethodImpl(256)]
        public static Fraction MaxMagnitudeNumber(Fraction x, Fraction y) => MaxMagnitude(x, y);
        [MethodImpl(256)]
        public static Fraction MinMagnitude(Fraction x, Fraction y) => x < y ? x : y;
        [MethodImpl(256)]
        public static Fraction MinMagnitudeNumber(Fraction x, Fraction y) => MinMagnitude(x, y);
        [MethodImpl(256)]
        public static Fraction Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider) => Parse(s, provider);
        [MethodImpl(256)]
        public static Fraction Parse(string s, NumberStyles style, IFormatProvider provider) => Parse(s, provider);
        [MethodImpl(256)]
        public static bool TryConvertFromChecked<TOther>(TOther value, out Fraction result) where TOther : INumberBase<TOther>
        {
            switch (value)
            {
                case double other: result = new Fraction(other); return true;
                case decimal other: result = new Fraction(other); return true;
                case int other: result = new Fraction(other, 1); return true;
                case long other: result = new Fraction(other, 1); return true;
                default: result = default; return false;
            }
        }
        [MethodImpl(256)]
        public static bool TryConvertFromSaturating<TOther>(TOther value, out Fraction result) where TOther : INumberBase<TOther> =>
            TryConvertFromChecked(value, out result);
        [MethodImpl(256)]
        public static bool TryConvertFromTruncating<TOther>(TOther value, out Fraction result) where TOther : INumberBase<TOther> =>
            TryConvertFromChecked(value, out result);
        [MethodImpl(256)]
        public static bool TryConvertToChecked<TOther>(Fraction value, out TOther result) where TOther : INumberBase<TOther> => 
            TOther.TryConvertFromChecked(value, out result);
        [MethodImpl(256)]
        public static bool TryConvertToSaturating<TOther>(Fraction value, out TOther result) where TOther : INumberBase<TOther> =>
            TOther.TryConvertFromChecked(value, out result);
        [MethodImpl(256)]
        public static bool TryConvertToTruncating<TOther>(Fraction value, out TOther result) where TOther : INumberBase<TOther> =>
            TOther.TryConvertFromChecked(value, out result);
        [MethodImpl(256)]
        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out Fraction result) =>
            TryParse(s, provider, out result);
        [MethodImpl(256)]
        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out Fraction result) =>
            TryParse(s, provider, out result);
        public static Fraction One { get; } = new Fraction(0, 1);
        public static int Radix => 10;
        public static Fraction Zero { get; } = new Fraction(0, 1);
    }
}