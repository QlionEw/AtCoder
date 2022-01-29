﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Numerics;

namespace AtCoder
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            checked
            {
                
            }
        }

        #region Utility

        private const int InfinityInt = Common.InfinityInt;
        private const long Infinity = Common.Infinity;
        private static readonly StreamScanner Scanner = new StreamScanner(Console.OpenStandardInput());
        private static int Si() => Scanner.Integer();
        private static long Sl() => Scanner.Long();
        private static string Ss() => Scanner.Scan();
        private static int[] Sai(int count) => Scanner.ArrayInt(count);
        private static long[] Sal(int count) => Scanner.ArrayLong(count);
        private static int[][] Sqi(int yCount, int xCount) => Scanner.SquareInt(yCount, xCount);
        private static long[][] Sql(int yCount, int xCount) => Scanner.SquareLong(yCount, xCount);
        private static string[] Sss(int count) => Enumerable.Repeat(0, count).Select(_ => Ss()).ToArray();

        private static void Loop(long n, Action action)
        {
            for (int i = 0; i < n; i++)
            {
                action();
            }
        }

        #endregion
    }

    #region Utility Class

    public static class Common
    {
        public const int InfinityInt = 1 << 29;
        public const long Infinity = (long) 1 << 60;
    }

    public static class MathPlus
    {
        public static long CeilingLong(long value, long div)
        {
            return value % div == 0 ? value / div : value / div + 1;
        }

        public static long Gcd(long a, long b)
        {
            return a > b ? GcdRecursive(a, b) : GcdRecursive(b, a);
        }

        private static long GcdRecursive(long a, long b)
        {
            while (true)
            {
                if (b == 0) return a;
                long a1 = a;
                a = b;
                b = a1 % b;
            }
        }
        
        /// <summary> a*x + b*y = 1 となるx,yを求める(1をnにする場合返り値をn倍) </summary>
        public static (long, long, long) ExtGcd(long a, long b, long x = 0, long y = 0)
        {
            if (b == 0) {
                return (1, 0, a);
            }
            (long xx, long yy, long aa) = ExtGcd(b, a%b, y, x);
            xx -= a/b * yy;
            return (yy, xx, aa);
        }

        /// <summary> Ax ≡ B mod Mとなるxを求める </summary>
        public static long Inv(long a, long b, long mod)
        {
            var dd = ExtGcd(a, mod);
            if (dd.Item1 < 0)
            {
                dd.Item1 += mod;
            }
            return (dd.Item1 * b) % mod;
        }

        public static long Lcm(long a, long b)
        {
            checked
            {
                return (a / Gcd(a, b)) * b;
            }
        }

        public static long Combination(long n, long m)
        {
            if (m == 0) return 1;
            if (n == 0) return 0;
            return n * Combination(n - 1, m - 1) / m;
        }

        public static long Permutation(long n, long m)
        {
            if (m == 0) return 1;
            if (n == 0) return 0;
            long value = 1;
            for (long i = n; i >= n - m + 1; i--)
            {
                value *= i;
            }

            return value;
        }

        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(params T[] array) where T : IComparable
        {
            var a = new List<T>(array);
            int n = a.Count;

            yield return array;

            bool next = true;
            while (next)
            {
                next = false;
                int i;
                for (i = n - 2; i >= 0; i--)
                {
                    if (a[i].CompareTo(a[i + 1]) < 0) break;
                }

                if (i < 0) break;

                int j = n;
                do
                {
                    j--;
                } while (a[i].CompareTo(a[j]) > 0);

                if (a[i].CompareTo(a[j]) < 0)
                {
                    T tmp = a[i];
                    a[i] = a[j];
                    a[j] = tmp;
                    a.Reverse(i + 1, n - i - 1);
                    yield return a;
                    next = true;
                }
            }
        }

        public static IEnumerable<T[]> GetCombinations<T>(IEnumerable<T> items, int k, bool withRepetition = false)
        {
            if (k == 1)
            {
                foreach (var item in items)
                    yield return new T[] {item};
                yield break;
            }

            IEnumerable<T> enumerable = items as T[] ?? items.ToArray();
            foreach (var item in enumerable)
            {
                var leftside = new T[] {item};

                var unused = withRepetition ? enumerable : enumerable.SkipWhile(e => !e.Equals(item)).Skip(1).ToList();

                foreach (var rightside in GetCombinations(unused, k - 1, withRepetition))
                {
                    yield return leftside.Concat(rightside).ToArray();
                }
            }
        }
    }

    public static class Utility
    {
        private static T[,] Rotate90<T>(this T[,] self)
        {
            int rows = self.GetLength(0);
            int columns = self.GetLength(1);
            var result = new T[columns, rows];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    result[j, rows - i - 1] = self[i, j];
                }
            }

            return result;
        }

        public static T[,] Rotate90<T>(this T[,] self, int count)
        {
            for (int i = 0; i < count; i++)
            {
                self = self.Rotate90();
            }

            return self;
        }

        public static T[,] Rotate90<T>(this T[][] self, int count)
        {
            var rows = self.Length;
            var columns = self.First().Length;
            T[,] array = new T[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    array[i, j] = self[i][j];
                }
            }

            return Rotate90(array, count);
        }

        public static void SafeAdd<T1, T2>(this Dictionary<T1, T2> self, T1 key, Func<T2,T2> onContain, T2 initial)
        {
            if (self.ContainsKey(key))
            {
                self[key] = onContain(self[key]);
            }
            else
            {
                self.Add(key, initial);
            }
        }
    }

    public static class BaseN
    {
        public static string ToString(long value, int baseValue)
        {
            string s = "";

            while (value > 0)
            {
                var c = value % baseValue;
                s = c.ToString() + s;
                value /= baseValue;
            }

            return string.IsNullOrEmpty(s) ? "0" : s;
        }

        public static long ToLong(string s, int baseValue)
        {
            long value = 0;
            for (int i = 0; i < s.Length; i++)
            {
                value = ((value * baseValue) + (s[i] - '0'));
            }

            return value;
        }

        public static string Convert(string s, int beforeBase, int afterBase)
        {
            return ToString(ToLong(s, beforeBase), afterBase);
        }
    }

    public class StreamScanner
    {
        private const int Size = 1024 * 16;

        public StreamScanner(Stream stream)
        {
            str = stream;
        }

        private readonly Stream str;
        private readonly byte[] buf = new byte[Size];
        private int len, ptr;
        public bool IsEof { get; private set; }

        private byte Read()
        {
            if (IsEof) throw new EndOfStreamException();
            if (ptr >= len)
            {
                ptr = 0;
                if ((len = str.Read(buf, 0, Size)) <= 0)
                {
                    IsEof = true;
                    return 0;
                }
            }

            return buf[ptr++];
        }

        public char Char()
        {
            byte b = 0;
            do b = Read();
            while (b < 33 || 126 < b);
            return (char) b;
        }

        public string Scan()
        {
            var sb = new StringBuilder();
            for (char b = Char(); b >= 33 && b <= 126; b = (char) Read())
                sb.Append(b);
            return sb.ToString();
        }

        public string ScanIncludeSpace()
        {
            var sb = new StringBuilder();
            for (char b = Char(); b >= 32 && b <= 126; b = (char) Read())
                sb.Append(b);
            return sb.ToString();
        }

        public long Long()
        {
            long ret = 0;
            byte b = 0;
            bool ng = false;
            do b = Read();
            while (b != '-' && (b < '0' || '9' < b));
            if (b == '-')
            {
                ng = true;
                b = Read();
            }

            for (; true; b = Read())
            {
                if (b < '0' || '9' < b)
                    return ng ? -ret : ret;
                else ret = ret * 10 + b - '0';
            }
        }

        public int Integer()
        {
            return (int) Long();
        }

        public double Double()
        {
            return double.Parse(Scan(), CultureInfo.InvariantCulture);
        }

        /// <summary> 数値読み込み </summary>
        public long[] ArrayLong(int count = 0)
        {
            long[] scan = new long[count];
            for (int i = 0; i < count; i++)
            {
                scan[i] = Long();
            }

            return scan;
        }

        /// <summary> 数値読み込み </summary>
        public int[] ArrayInt(int count = 0)
        {
            int[] scan = new int[count];
            for (int i = 0; i < count; i++)
            {
                scan[i] = Integer();
            }

            return scan;
        }

        public long[][] SquareLong(int row, int col)
        {
            long[][] scan = new long[row][];
            for (int i = 0; i < row; i++)
            {
                scan[i] = ArrayLong(col);
            }

            return scan;
        }

        public int[][] SquareInt(int row, int col)
        {
            int[][] scan = new int[row][];
            for (int i = 0; i < row; i++)
            {
                scan[i] = ArrayInt(col);
            }

            return scan;
        }
    }

    public class Answerer
    {
        /// <summary> Yes出力 </summary>
        public static void Yes() => Console.WriteLine("Yes");
        
        /// <summary> Yes出力 </summary>
        public static void No() => Console.WriteLine("No");
        
        /// <summary> Yes/No型出力 </summary>
        public static void YesNo(bool condition)
        {
            Console.WriteLine(condition ? "Yes" : "No");
        }

        /// <summary> 一括出力 </summary>
        public static void OutAllLine<T>(IEnumerable<T> items)
        {
            var sb = new StringBuilder();
            foreach (T result in items)
            {
                sb.Append(result + "\n");
            }

            sb = sb.Remove(sb.Length - 1, 1);
            Console.WriteLine(sb);
        }

        public static void OutEachSpace<T>(IEnumerable<T> items)
        {
            var sb = new StringBuilder();
            foreach (T result in items)
            {
                sb.Append(result + " ");
            }

            sb = sb.Remove(sb.Length - 1, 1);
            Console.WriteLine(sb);
        }
    }

    public class Set<T>
    {
        private Node root;
        private readonly IComparer<T> comparer;
        private readonly Node nil;
        public bool IsMultiSet { get; set; }

        public Set(IComparer<T> comparer, bool isMultiSet = false)
        {
            nil = new Node(default(T));
            root = nil;
            this.comparer = comparer;
            IsMultiSet = isMultiSet;
        }

        public Set(Comparison<T> comparision, bool isMultiSet = false) : this(Comparer<T>.Create(comparision), isMultiSet)
        {
        }

        public Set(bool isMultiSet = false) : this(Comparer<T>.Default, isMultiSet)
        {
        }

        public bool Add(T v)
        {
            return Insert(ref root, v);
        }

        public bool Remove(T v)
        {
            return Remove(ref root, v);
        }

        public T this[int index] => Find(root, index);
        public int Count => root.Count;

        public void RemoveAt(int k)
        {
            if (k < 0 || k >= root.Count) throw new ArgumentOutOfRangeException();
            RemoveAt(ref root, k);
        }

        public T[] Items
        {
            get
            {
                var ret = new T[root.Count];
                var k = 0;
                Walk(root, ret, ref k);
                return ret;
            }
        }

        private static void Walk(Node t, IList<T> a, ref int k)
        {
            while (true)
            {
                if (t.Count == 0) return;
                Walk(t.Lst, a, ref k);
                a[k++] = t.Key;
                t = t.Rst;
            }
        }

        private bool Insert(ref Node t, T key)
        {
            if (t.Count == 0)
            {
                t = new Node(key);
                t.Lst = t.Rst = nil;
                t.Update();
                return true;
            }

            var cmp = comparer.Compare(t.Key, key);
            bool res;
            if (cmp > 0)
                res = Insert(ref t.Lst, key);
            else if (cmp == 0)
            {
                if (IsMultiSet) res = Insert(ref t.Lst, key);
                else return false;
            }
            else res = Insert(ref t.Rst, key);

            Balance(ref t);
            return res;
        }

        private bool Remove(ref Node t, T key)
        {
            if (t.Count == 0) return false;
            var cmp = comparer.Compare(key, t.Key);
            bool ret;
            if (cmp < 0) ret = Remove(ref t.Lst, key);
            else if (cmp > 0) ret = Remove(ref t.Rst, key);
            else
            {
                ret = true;
                var k = t.Lst.Count;
                if (k == 0)
                {
                    t = t.Rst;
                    return true;
                }

                if (t.Rst.Count == 0)
                {
                    t = t.Lst;
                    return true;
                }


                t.Key = Find(t.Lst, k - 1);
                RemoveAt(ref t.Lst, k - 1);
            }

            Balance(ref t);
            return ret;
        }

        private void RemoveAt(ref Node t, int k)
        {
            var cnt = t.Lst.Count;
            if (cnt < k) RemoveAt(ref t.Rst, k - cnt - 1);
            else if (cnt > k) RemoveAt(ref t.Lst, k);
            else
            {
                if (cnt == 0)
                {
                    t = t.Rst;
                    return;
                }

                if (t.Rst.Count == 0)
                {
                    t = t.Lst;
                    return;
                }

                t.Key = Find(t.Lst, k - 1);
                RemoveAt(ref t.Lst, k - 1);
            }

            Balance(ref t);
        }

        private static void Balance(ref Node t)
        {
            var balance = t.Lst.Height - t.Rst.Height;
            if (balance == -2)
            {
                if (t.Rst.Lst.Height - t.Rst.Rst.Height > 0) { RotR(ref t.Rst); }

                RotL(ref t);
            }
            else if (balance == 2)
            {
                if (t.Lst.Lst.Height - t.Lst.Rst.Height < 0) RotL(ref t.Lst);
                RotR(ref t);
            }
            else t.Update();
        }

        private T Find(Node t, int k)
        {
            if (k < 0 || k > root.Count) throw new ArgumentOutOfRangeException();
            for (;;)
            {
                if (k == t.Lst.Count) return t.Key;
                else if (k < t.Lst.Count) t = t.Lst;
                else
                {
                    k -= t.Lst.Count + 1;
                    t = t.Rst;
                }
            }
        }

        public int LowerBound(T v)
        {
            var k = 0;
            var t = root;
            for (;;)
            {
                if (t.Count == 0) return k;
                if (comparer.Compare(v, t.Key) <= 0) t = t.Lst;
                else
                {
                    k += t.Lst.Count + 1;
                    t = t.Rst;
                }
            }
        }

        public int UpperBound(T v)
        {
            var k = 0;
            var t = root;
            for (;;)
            {
                if (t.Count == 0) return k;
                if (comparer.Compare(t.Key, v) <= 0)
                {
                    k += t.Lst.Count + 1;
                    t = t.Rst;
                }
                else t = t.Lst;
            }
        }

        private static void RotR(ref Node t)
        {
            var l = t.Lst;
            t.Lst = l.Rst;
            l.Rst = t;
            t.Update();
            l.Update();
            t = l;
        }

        private static void RotL(ref Node t)
        {
            var r = t.Rst;
            t.Rst = r.Lst;
            r.Lst = t;
            t.Update();
            r.Update();
            t = r;
        }


        private class Node
        {
            public Node(T key)
            {
                Key = key;
            }

            public int Count { get; private set; }
            public sbyte Height { get; private set; }
            public T Key { get; set; }
            public Node Lst, Rst;

            public void Update()
            {
                Count = 1 + Lst.Count + Rst.Count;
                Height = (sbyte) (1 + Math.Max(Lst.Height, Rst.Height));
            }

            public override string ToString()
            {
                return $"Count = {Count}, Key = {Key}";
            }
        }
    }

    public class Deque<T> : IEnumerable<T>
    {
        public T this[int i]
        {
            get => buffer[(firstIndex + i) % capacity];
            set
            {
                if (i < 0) throw new ArgumentOutOfRangeException();
                buffer[(firstIndex + i) % capacity] = value;
            }
        }

        private T[] buffer;
        private int capacity;
        private int firstIndex;
        private int LastIndex => (this.firstIndex + this.Length) % this.capacity;
        public int Length { get; private set; }

        public Deque(int capacity = 16)
        {
            this.capacity = capacity;
            buffer = new T[this.capacity];
            firstIndex = 0;
        }

        public void PushBack(T data)
        {
            if (Length == capacity) Resize();
            buffer[LastIndex] = data;
            Length++;
        }

        public void PushFront(T data)
        {
            if (Length == capacity) Resize();
            var index = firstIndex - 1;
            if (index < 0) index = capacity - 1;
            buffer[index] = data;
            Length++;
            firstIndex = index;
        }

        public T PopBack()
        {
            if (Length == 0) throw new InvalidOperationException("データが空です。");
            var data = this[Length - 1];
            Length--;
            return data;
        }

        public T PopFront()
        {
            if (Length == 0) throw new InvalidOperationException("データが空です。");
            var data = this[0];
            firstIndex++;
            firstIndex %= capacity;
            Length--;
            return data;
        }

        private void Resize()
        {
            var newArray = new T[capacity * 2];
            for (int i = 0; i < Length; i++)
            {
                newArray[i] = this[i];
            }

            firstIndex = 0;
            capacity *= 2;
            buffer = newArray;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
            {
                yield return this[i];
            }
        }
    }

    public class DynamicProgramming : DynamicProgramming<long>
    {
        private readonly bool isGetMax;

        public DynamicProgramming(bool isGetMax, params int[] counts) : base(
            isGetMax ? -Common.Infinity : Common.Infinity, counts)
        {
            this.isGetMax = isGetMax;
            InvalidValue = isGetMax ? -Common.Infinity : Common.Infinity;
        }

        public double Answer2D => isGetMax ? Table[0].Max(xs => xs[XCount - 1]) : Table[0].Min(xs => xs[XCount - 1]);
    }

    public class DynamicProgramming<T>
    {
        public T InvalidValue { get; set; } = default;
        public T[][][] Table { get; private set; }
        public int[] Initials { get; set; } = new[] {0, 0, 0};
        public bool IsSolveBack { get; set; }
        protected readonly int XCount;
        protected readonly int YCount;
        protected readonly int ZCount;
        private bool isSetOrigin;
        public T Answer1D => Table[0][0][XCount - 1];
        public IEnumerable<T> Last2D => Table[0].Select(xs => xs[XCount - 1]);

        public DynamicProgramming(T firstValue, params int[] counts)
        {
            XCount = counts[0];
            YCount = counts.Length >= 2 ? counts[1] : 1;
            ZCount = counts.Length >= 3 ? counts[2] : 1;

            Table = Enumerable.Repeat(0, ZCount).Select(_ =>
                    Enumerable.Repeat(0, YCount).Select(__ => Enumerable.Repeat(firstValue, XCount).ToArray())
                        .ToArray())
                .ToArray();
        }

        public void SetOrigin(T originValue)
        {
            Table[0][0][0] = originValue;
            isSetOrigin = true;
        }

        public void SetFirstValue(T firstValue, bool isXAll = false, bool isYAll = false, bool isZAll = false)
        {
            if (isXAll)
            {
                for (int x = 0; x < XCount; x++)
                {
                    Table[0][0][x] = firstValue;
                }

                Initials[0] = 1;
            }

            if (isYAll)
            {
                for (int y = 0; y < YCount; y++)
                {
                    Table[0][y][0] = firstValue;
                }

                Initials[1] = 1;
            }

            if (isZAll)
            {
                for (int z = 0; z < ZCount; z++)
                {
                    Table[z][0][0] = firstValue;
                }

                Initials[2] = 1;
            }
        }

        private int xCurrent, yCurrent, zCurrent;

        public void Solve(Func<int, int, int, T> calcFunc)
        {
            if (IsSolveBack)
            {
                SolveBack(calcFunc);
                return;
            }

            bool isSkipFirst = isSetOrigin;
            for (xCurrent = Initials[0]; xCurrent < XCount; xCurrent++)
            {
                for (yCurrent = Initials[1]; yCurrent < YCount; yCurrent++)
                {
                    for (zCurrent = Initials[2]; zCurrent < ZCount; zCurrent++)
                    {
                        if (isSkipFirst)
                        {
                            isSkipFirst = false;
                            continue;
                        }

                        Table[zCurrent][yCurrent][xCurrent] = calcFunc(xCurrent, yCurrent, zCurrent);
                    }
                }
            }
        }

        private void SolveBack(Func<int, int, int, T> calcFunc)
        {
            bool isSkipFirst = isSetOrigin;
            for (xCurrent = XCount - 1; xCurrent >= Initials[0]; xCurrent--)
            {
                for (yCurrent = YCount - 1; yCurrent >= Initials[1]; yCurrent--)
                {
                    for (zCurrent = ZCount - 1; zCurrent >= Initials[2]; zCurrent--)
                    {
                        if (isSkipFirst)
                        {
                            isSkipFirst = false;
                            continue;
                        }

                        Table[zCurrent][yCurrent][xCurrent] = calcFunc(xCurrent, yCurrent, zCurrent);
                    }
                }
            }
        }

        public T GetPrevious(int prevX, int prevY, int prevZ)
        {
            return xCurrent - prevX >= 0 && yCurrent - prevY >= 0 && zCurrent - prevZ >= 0
                ? Table[zCurrent - prevZ][yCurrent - prevY][xCurrent - prevX]
                : InvalidValue;
        }

        public T GetPrevious(int prevX, int prevY)
        {
            return xCurrent - prevX >= 0 && yCurrent - prevY >= 0
                ? Table[zCurrent][yCurrent - prevY][xCurrent - prevX]
                : InvalidValue;
        }

        public T GetPreviousIndex(int prevX, int index)
        {
            return xCurrent - prevX >= 0 ? Table[zCurrent][index][xCurrent - prevX] : InvalidValue;
        }

        public T GetPrevious(int prevX)
        {
            return xCurrent - prevX >= 0 ? Table[zCurrent][yCurrent][xCurrent - prevX] : InvalidValue;
        }
    }

    public class LargeCalc
    {
        public IEnumerable<long> Surplus(long baseNum, long power, int division)
        {
            long value = 1;
            for (int i = 1; i <= power; i++)
            {
                value = value * baseNum % division;
                yield return value;
                if (value == 0)
                {
                    break;
                }
            }
        }
    }

    public class Primer
    {
        /// <summary> 素数判定 </summary>
        public bool IsPrime(long num)
        {
            if (num < 2) { return false; }

            if (num == 2) { return true; }

            if (num % 2 == 0) { return false; }

            double sqrtNum = Math.Sqrt(num);
            for (int i = 3; i <= sqrtNum; i += 2)
            {
                if (num % i == 0) { return false; }
            }

            return true;
        }

        public IEnumerable<long> GetPrimeFactors(long n)
        {
            long i = 2;
            long tmp = n;

            while (i * i <= tmp)
            {
                if (tmp % i == 0)
                {
                    tmp /= i;
                    yield return i;
                    if (IsPrime(tmp))
                    {
                        yield return tmp;
                        tmp = 1;
                        break;
                    }
                }
                else
                {
                    i++;
                }
            }

            if (tmp != 1) yield return tmp;
        }

        public int GetDivisorCount(long n)
        {
            int count = 0;
            long sq = (long) Math.Sqrt(n);
            for (long i = 1; i <= sq; i++)
            {
                if (n % i == 0)
                {
                    count += 2;
                }
            }

            if (sq * sq == n)
            {
                count--;
            }

            return count;
        }

        public IEnumerable<long> GetDivisors(long n)
        {
            HashSet<long> divisors = new HashSet<long>();

            for (long i = 1; i * i <= n; i++)
            {
                if (n % i != 0) { continue; }

                divisors.Add(i);
                divisors.Add(n / i);
            }

            return divisors.OrderBy(x => x).ToArray();
        }
    }

    public struct ModInt
    {
        long value;
        public const int _1000000007 = 1000000007;
        public const int _1000000009 = 1000000009;
        public const int _998244353 = 998244353;
        public static int ModValue { get; set; } = _998244353;
        static List<ModInt> fact = new List<ModInt> {1};
        public ModInt(long value) => this.value = value;
        public static implicit operator ModInt(long a) => new ModInt(a % ModValue + (a < 0 ? ModValue : 0));
        public static explicit operator int(ModInt a) => (int) a.value;
        public override string ToString() => value.ToString();
        public static ModInt operator +(ModInt a, ModInt b) => a.value + b.value;
        public static ModInt operator -(ModInt a, ModInt b) => a.value - b.value;
        public static ModInt operator *(ModInt a, ModInt b) => a.value * b.value;
        public static ModInt operator /(ModInt a, ModInt b) => a * Inv(b);

        public static ModInt Pow(ModInt a, int n)
        {
            if (n == 0) return 1;
            if (n == 1) return a;
            ModInt p = Pow(a, n / 2);
            return p * p * Pow(a, n % 2);
        }

        public static ModInt Inv(ModInt a) => Pow(a, ModValue - 2);

        public static ModInt Fact(int n)
        {
            for (int i = fact.Count; i <= n; i++) fact.Add(fact[^1] * i);
            return fact[n];
        }

        public static ModInt Perm(int n, int r) => Fact(n) / Fact(n - r);
        public static ModInt Comb(int n, int r) => Fact(n) / Fact(n - r) / Fact(r);
    }

    public class BitArrayMaker
    {
        public List<int> Integer(long bitValue, long length)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < length; i++)
            {
                if (bitValue % 2 == 1)
                {
                    list.Add(i);
                }

                bitValue /= 2;
            }

            return list;
        }

        public bool[] Boolean(long bitValue, long length)
        {
            bool[] list = new bool[length];
            for (int i = 0; i < length; i++)
            {
                list[i] = bitValue % 2 == 1;
                bitValue /= 2;
            }

            return list;
        }
    }

    public class GraphSolver
    {
        public struct PathInfo : IComparable<PathInfo>
        {
            public int From { get; set; }
            public int To { get; set; }
            public long Cost { get; set; }

            public int CompareTo(PathInfo other)
            {
                return (int) (Cost - other.Cost);
            }
        }

        private readonly List<PathInfo>[] pathInfos;
        public long[] Distances { get; private set; }
        private int pathCount = 0;
        private int nodeCount;

        public GraphSolver(int nodeCount)
        {
            this.nodeCount = nodeCount;
            Distances = Enumerable.Repeat(Common.Infinity, nodeCount + 1).ToArray();
            pathInfos = Enumerable.Repeat(0, nodeCount + 1).Select(_ => new List<PathInfo>()).ToArray();
        }

        public void Init()
        {
            Distances = Enumerable.Repeat(Common.Infinity, Distances.Length).ToArray();
        }

        public void AddPath(int from, int to, long cost, params long[] additionalInfo)
        {
            pathCount++;
            pathInfos[from].Add(new PathInfo {From = from, To = to, Cost = cost});
        }

        public void Dijkstra(int point)
        {
            PriorityQueue<PathInfo> queue = new PriorityQueue<PathInfo>(pathCount + 1);
            Distances[point] = 0;
            queue.Enqueue(new PathInfo {To = point, Cost = 0});

            while (queue.Count != 0)
            {
                PathInfo pop = queue.Dequeue();
                if (Distances[pop.To] < pop.Cost) { continue; }

                foreach (PathInfo path in pathInfos[pop.To])
                {
                    long nextValue = Distances[pop.To] + path.Cost;
                    if (Distances[path.To] > nextValue)
                    {
                        Distances[path.To] = nextValue;
                        queue.Enqueue(new PathInfo {From = path.From, To = path.To, Cost = Distances[path.To]});
                    }
                }
            }
        }

        public void _01Bfs(int point)
        {
            Deque<int> deque = new Deque<int>();
            Distances[point] = 0;
            deque.PushBack(point);

            while (deque.Length != 0)
            {
                int index = deque.PopFront();
                foreach (PathInfo path in pathInfos[index])
                {
                    long d = Distances[index] + path.Cost;
                    if (d < Distances[path.To])
                    {
                        Distances[path.To] = d;
                        if (Distances[path.Cost] != 0)
                        {
                            deque.PushBack(path.To);
                        }
                        else
                        {
                            deque.PushFront(path.To);
                        }
                    }
                }
            }
        }

        public long Kruskal()
        {
            var edges = pathInfos.SelectMany(pathInfo => pathInfo).OrderBy(x => x.Cost).ToList();
            long totalCost = 0;
            var uft = new UnionFindTree(nodeCount + 1);
            foreach (PathInfo edge in edges.Where(edge => !uft.Same(edge.From, edge.To)))
            {
                uft.Union(edge.From, edge.To);
                totalCost += edge.Cost;
            }
            return totalCost;
        }

        private PathInfo[] bellmanFordList;
        public bool[] IsLoop { get; private set; }

        public void BellmanFord(int point, bool isDetectLoop = false)
        {
            bellmanFordList = pathInfos.SelectMany(x => x).ToArray();
            IsLoop = Enumerable.Repeat(false, nodeCount + 1).ToArray();
            Distances[point] = 0;
            int count;
            for (count = 0; count < Distances.Length; count++)
            {
                bool isUpdated = false;
                foreach (PathInfo path in bellmanFordList)
                {
                    if (Distances[path.From] == Common.Infinity) { continue; }

                    if (Distances[path.To] <= Distances[path.From] + path.Cost) { continue; }

                    Distances[path.To] = Distances[path.From] + path.Cost;
                    isUpdated = true;
                }

                if (!isUpdated) { break; }
            }

            if (isDetectLoop)
            {
                DetectBellmanFordLoop();
            }
        }

        private void DetectBellmanFordLoop()
        {
            for (int i = 0; i <= Distances.Length; i++)
            {
                foreach (PathInfo path in bellmanFordList)
                {
                    if (Distances[path.From] == Common.Infinity) { continue; }

                    if (Distances[path.To] > Distances[path.From] + path.Cost)
                    {
                        Distances[path.To] = Distances[path.From] + path.Cost;
                        IsLoop[path.To] = true;
                    }

                    if (IsLoop[path.From])
                    {
                        IsLoop[path.To] = true;
                    }
                }
            }
        }

        private bool[] sccUsed;
        private List<int> sccOrder;
        private List<PathInfo>[] reversePathInfos;
        private int[] sccGroups;

        public IEnumerable<int> StronglyConnectedComponent()
        {
            sccUsed = new bool[nodeCount + 1];
            sccOrder = new List<int>();
            reversePathInfos = Enumerable.Repeat(0, nodeCount + 1).Select(_ => new List<PathInfo>()).ToArray();
            foreach (var path in pathInfos.SelectMany(x => x))
            {
                reversePathInfos[path.To].Add(new PathInfo {From = path.To, To = path.From, Cost = path.Cost});
            }

            for (int i = 1; i <= nodeCount; i++)
            {
                if (!sccUsed[i])
                {
                    SccDfs(i);
                }
            }

            sccUsed = new bool[nodeCount + 1];
            sccGroups = new int[nodeCount];
            int groupNumber = 0;
            for (int i = sccOrder.Count - 1; i >= 0; i--)
            {
                if (!sccUsed[sccOrder[i]])
                {
                    ReverseScc(sccOrder[i], groupNumber++);
                }
            }

            return sccGroups;
        }

        private void SccDfs(int index)
        {
            sccUsed[index] = true;
            foreach (var to in pathInfos[index].Select(x => x.To))
            {
                if (!sccUsed[to])
                {
                    SccDfs(to);
                }
            }

            sccOrder.Add(index);
        }

        private void ReverseScc(int index, int groupNumber)
        {
            sccUsed[index] = true;
            sccGroups[index - 1] = groupNumber;
            foreach (var to in reversePathInfos[index].Select(x => x.To))
            {
                if (!sccUsed[to])
                {
                    ReverseScc(to, groupNumber);
                }
            }
        }

        private long[][] warshallFloydDp;

        public long[][] WarshallFloyd()
        {
            warshallFloydDp = Enumerable.Repeat(0, nodeCount)
                .Select(_ => Enumerable.Repeat(Common.Infinity, nodeCount).ToArray()).ToArray();
            for (int i = 0; i < nodeCount; i++)
            {
                warshallFloydDp[i][i] = 0;
            }

            foreach (var path in pathInfos.SelectMany(x => x))
            {
                warshallFloydDp[path.From - 1][path.To - 1] = path.Cost;
            }

            for (int k = 0; k < nodeCount; k++)
            {
                for (int i = 0; i < nodeCount; i++)
                {
                    for (int j = 0; j < nodeCount; j++)
                    {
                        warshallFloydDp[i][j] = Math.Min(warshallFloydDp[i][j],
                            warshallFloydDp[i][k] + warshallFloydDp[k][j]);
                    }
                }
            }

            return warshallFloydDp;
        }
    }

    public class BinarySearch
    {
        private long min;
        private long max;

        public BinarySearch()
        {
        }

        public BinarySearch(long min, long max)
        {
            SetRange(min, max);
        }

        public void SetRange(long min, long max)
        {
            this.min = min - 1;
            this.max = max + 1;
        }

        public long CountUnder(Func<long, bool> judge) => SolveMax(judge);

        public long SolveMax(Func<long, bool> judge)
        {
            long ok = min;
            long ng = max;
            long i = (ok + ng) / 2;
            while (ok + 1 < ng)
            {
                if (i == min || i == max)
                {
                    break;
                }

                if (judge(i))
                {
                    ok = i;
                }
                else
                {
                    ng = i;
                }

                i = (ok + ng) / 2;
            }

            return ok;
        }

        public long CountOver(Func<long, bool> judge) => SolveMin(judge);

        public long SolveMin(Func<long, bool> judge)
        {
            long ok = max;
            long ng = min;
            long i = (ok + ng) / 2;
            while (ng + 1 < ok)
            {
                if (i == min || i == max)
                {
                    break;
                }

                if (judge(i))
                {
                    ok = i;
                }
                else
                {
                    ng = i;
                }

                i = (ok + ng) / 2;
            }

            return ok;
        }
    }

    public class PriorityQueue<T> where T : IComparable<T>
    {
        private readonly T[] heap;
        public int Count { get; private set; }

        public PriorityQueue(int totalSize)
        {
            heap = new T[totalSize];
        }

        public void Enqueue(T item)
        {
            int index = Count++;
            while (index > 0)
            {
                int p = (index - 1) / 2;
                if (heap[p].CompareTo(item) <= 0) { break; }

                heap[index] = heap[p];
                index = p;
            }

            heap[index] = item;
        }

        public T Dequeue()
        {
            T ret = heap[0];
            T item = heap[--Count];

            int index = 0;
            while (index * 2 + 1 < Count)
            {
                int a = index * 2 + 1;
                int b = index * 2 + 2;
                if (b < Count && heap[b].CompareTo(heap[a]) < 0)
                {
                    a = b;
                }

                if (heap[a].CompareTo(item) >= 0) { break; }

                heap[index] = heap[a];
                index = a;
            }

            heap[index] = item;

            return ret;
        }
    }

    public class UnionFindTree
    {
        public long[] Parents { get; }

        public UnionFindTree(int n)
        {
            Parents = Enumerable.Repeat((long) -1, n).ToArray();
        }

        public long Find(long x)
        {
            if (Parents[x] < 0) return x;
            Parents[x] = Find(Parents[x]);
            return Parents[x];
        }

        public long Size(long x) => -Parents[Find(x)];
        public bool Same(long x, long y) => Find(x) == Find(y);

        public bool Union(long x, long y)
        {
            x = Find(x);
            y = Find(y);

            if (x == y) return false;

            if (Size(x) < Size(y))
            {
                (x, y) = (y, x);
            }

            Parents[x] += Parents[y];
            Parents[y] = x;

            return true;
        }
    }

    public class SegmentTree : SegmentTreeExtend<long>
    {
    }

    public class SegmentTreeExtend<T>
    {
        private T[] data;
        private Func<T, T, T> updateMethod;
        private T firstValue;
        private int n;
        private int count;

        public void Init(int count, T firstValue, Func<T, T, T> updateMethod)
        {
            this.updateMethod = updateMethod;
            this.firstValue = firstValue;
            this.count = count;

            n = 1;
            while (n < count)
            {
                n *= 2;
            }

            data = Enumerable.Repeat(firstValue, 2 * n - 1).ToArray();
        }
        
        public void Build(Func<int, T> update)
        {
            for (int i = 0; i < count; i++)
            {
                data[n + i - 1] = update(i);
            }
            for (int i = n - 2; i >= 0; i--)
            {
                data[i] = updateMethod(data[2 * i + 1], data[2 * i + 2]);
            }
        }

        public void Update(int index, T value)
        {
            index += n - 1;
            data[index] = value;
            while (index > 0)
            {
                index = (index - 1) / 2;
                data[index] = updateMethod(data[index * 2 + 1], data[index * 2 + 2]);
            }
        }

        public T Query(int index)
        {
            return Query(index, index);
        }

        public T Query(int indexStart, int indexEnd)
        {
            return Query(indexStart, indexEnd + 1, 0, 0, n);
        }

        private T Query(int indexStart, int indexEnd, int current, int left, int right)
        {
            if (right <= indexStart || indexEnd <= left) { return firstValue; }

            if (indexStart <= left && right <= indexEnd) { return data[current]; }

            T leftValue = Query(indexStart, indexEnd, current * 2 + 1, left, (left + right) / 2);
            T rightValue = Query(indexStart, indexEnd, current * 2 + 2, (left + right) / 2, right);

            return updateMethod(leftValue, rightValue);
        }
    }

    public enum LazySegmentTreeMode
    {
        UpdateMin,
        UpdateMax,
        Addition,
    }
    
    public class LazySegmentTree
    {
        private long[] data;
        private long[] lazyData;
        public Func<long, long, long> UpdateMethod { get; set; } 
        public Func<long, long, long> UpdateSelfMethod { get; set; }
        public Func<long, long, long> UpdateChildMethod { get; set; }
        private long firstValue;
        private int n;
        private int count;

        public void Init(int count, long firstValue, LazySegmentTreeMode mode)
        {
            this.firstValue = firstValue;
            this.count = count;

            n = 1;
            while (n < count)
            {
                n *= 2;
            }

            data = Enumerable.Repeat(firstValue, 2 * n - 1).ToArray();
            lazyData = Enumerable.Repeat(firstValue, 2 * n - 1).ToArray();

            SetMode(mode);
        }

        private void SetMode(LazySegmentTreeMode mode)
        {
            UpdateMethod = mode switch
            {
                LazySegmentTreeMode.UpdateMin => Math.Min,
                LazySegmentTreeMode.UpdateMax => Math.Max,
                _ => (l1, l2) => l1 + l2,
            };
            UpdateSelfMethod = mode switch
            {
                LazySegmentTreeMode.Addition => (l1, l2) => l1 + l2,
                _ => (x, m) => m
            };
            UpdateChildMethod = mode switch
            {
                LazySegmentTreeMode.Addition => (l1, l2) => l1 + l2,
                _ => (m1, m2) => m2
            };
        }
        
        public void Build(Func<int, long> update)
        {
            for (int i = 0; i < count; i++)
            {
                data[n + i - 1] = update(i);
            }
            for (int i = n - 2; i >= 0; i--)
            {
                data[i] = UpdateMethod(data[2 * i + 1], data[2 * i + 2]);
            }
        }

        private void Evaluate(int k)
        {
            if (lazyData[k] == firstValue) {return;}

            if (k < n - 1)
            {
                lazyData[2 * k + 1] = UpdateChildMethod(lazyData[2 * k + 1], lazyData[k]);
                lazyData[2 * k + 2] = UpdateChildMethod(lazyData[2 * k + 2], lazyData[k]);
            }

            data[k] = UpdateSelfMethod(data[k], lazyData[k]);
            lazyData[k] = firstValue;
        }

        public void Update(int left, int right, long value)
        {
            Update(left, right + 1, value, 0, 0, n);
        }

        private void Update(int a, int b, long x, int k, int l, int r)
        {
            Evaluate(k);
            if (r <= a || b <= l) {return;}
            if (a <= l && r <= b)
            {
                lazyData[k] = UpdateChildMethod(lazyData[k], x);
                Evaluate(k);
            }
            else
            {
                Update(a, b, x, 2 * k + 1, l, (l + r) / 2);
                Update(a, b, x, 2 * k + 2, (l + r) / 2, r);
                data[k] = UpdateMethod(data[2 * k + 1], data[2 * k + 2]);
            }
        }

        public long Query(int left, int right)
        {
            return Query(left, right + 1, 0, 0, n);
        }

        private long Query(int a, int b, int k, int l, int r)
        {
            Evaluate(k);
            if (r <= a || b <= l) return firstValue;
            if (a <= l && r <= b) return data[k];
            long vl = Query(a, b, 2 * k + 1, l, (l + r) / 2);
            long vr = Query(a, b, 2 * k + 2, (l + r) / 2, r);
            return UpdateMethod(vl, vr);
        }
    }

    public class Compressed<T> where T : IComparable<T>, IEquatable<T>
    {
        private List<T> list;
        private Dictionary<T, int> dict;
        private Dictionary<int, T> reversed;

        public void Add(T value)
        {
            if (list == null)
            {
                list = new List<T>();
            }

            list.Add(value);
        }

        public int this[T index] => IndexOf(index);

        public int IndexOf(T index)
        {
            if (list != null && dict == null)
            {
                Generate(list);
            }

            return dict[index];
        }

        public T Restore(int index)
        {
            return reversed.Count > index ? reversed[index] : default;
        }

        private void Generate(IEnumerable<T> array)
        {
            var converted = array.Distinct().OrderBy(x => x).Select((x, i) => (x, i)).ToArray();
            dict = converted.ToDictionary(x => x.x, x => x.i);
            reversed = converted.ToDictionary(x => x.i, x => x.x);
        }

        public static Compressed<T> Create(IEnumerable<T> array)
        {
            var cp = new Compressed<T>();
            cp.Generate(array);
            return cp;
        }
    }

    public class TreeStructure
    {
        public class TreePath
        {
            public List<int> Ways { get; } = new List<int>();
            public List<long> Costs { get; } = new List<long>();

            public IEnumerable<(int Way, long Cost)> GetWayData() => Ways.Select((t, i) => (t, Costs[i]));
        }

        public TreePath[] Vertexes { get; }
        public TreeStructure(int size)
        {
            Vertexes = Enumerable.Range(0, size + 1).Select(_ => new TreePath()).ToArray();
        }

        public void Connect(int index, int index2, long cost = 1)
        {
            Vertexes[index].Ways.Add(index2);
            Vertexes[index].Costs.Add(cost);
        }

        public void ConnectEach(int index, int index2, long cost = 1)
        {
            Connect(index, index2, cost);
            Connect(index2, index, cost);
        }

        private long[] costArray;
        public IEnumerable<long> CheckCost(int origin, bool isTouring = true)
        {
            costArray = new long[Vertexes.Length];
            if (isTouring)
            {
                int time = 0;
                tourTime = new List<(int, int)>();
                firstTime = new int[Vertexes.Length];
                CheckCostAndTour(origin, 0, 0, ref time);
            }
            else
            {
                CheckCost(origin, 0, 0);
            }
            return costArray;
        }

        private List<(int, int)> tourTime;
        private int[] firstTime;
        private void CheckCostAndTour(int current, long cost, int from, ref int time, int depth = 0)
        {
            costArray[current] = cost;
            if (firstTime[current] == 0)
            {
                firstTime[current] = time++;
            }
            tourTime.Add((current, depth));
            foreach ((int way, long l) in Vertexes[current].GetWayData())
            {
                if (way == from)
                {
                    continue;
                }
                CheckCostAndTour(way, cost + l, current, ref time, depth + 1);
                tourTime.Add((current, depth));
                time++;
            }
        }
        
        private void CheckCost(long current, long cost, long from)
        {
            costArray[current] = cost;
            foreach ((long way, long l) in Vertexes[current].GetWayData())
            {
                if (way == from)
                {
                    continue;
                }

                CheckCost(way, cost + l, current);
            }
        }

        private SegmentTreeExtend<(int,int)> seg;
        public int GetLowestCommonAncestor(int index1, int index2)
        {
            if (seg == null)
            {
                InitLca();
            }
            var min = Math.Min(firstTime[index1], firstTime[index2]);
            var max = Math.Max(firstTime[index1], firstTime[index2]);
            var item = seg.Query(min, max);
            return item.Item1;
        }

        private void InitLca()
        {
            seg = new SegmentTreeExtend<(int,int)>();
            seg.Init(tourTime.Count, (Common.InfinityInt,Common.InfinityInt), (a,b) => a.Item2 < b.Item2 ? a : b);
            seg.Build(i => tourTime[i]);
        }

        public int GetDistance(int index1, int index2)
        {
            if (seg == null)
            {
                InitLca();
            }
            var target = GetLowestCommonAncestor(index1, index2);
            return tourTime[firstTime[index1]].Item2 + tourTime[firstTime[index2]].Item2 -
                   2 * tourTime[firstTime[target]].Item2;
        }

        public long GetCost(int index1, int index2)
        {
            if (seg == null)
            {
                InitLca();
            }
            var target = GetLowestCommonAncestor(index1, index2);
            return costArray[index1] + costArray[index2] - 2 * costArray[target];
        }
    }

    public class DagPath
    {
        public int Index { get; set; }
        public List<int> To { get; } = new List<int>();
        public List<int> From { get; } = new List<int>();
        public List<long> Cost { get; } = new List<long>();
        public int InDegree { get; set; }
        public long CurrentCost { get; set; }
    }

    public class TopologicalListGenerator
    {
        private List<DagPath> list;

        public TopologicalListGenerator(int size)
        {
            list = Enumerable.Range(0, size).Select(i => new DagPath() {Index = i}).ToList();
        }

        public void Connect(int from, int to, long cost, params long[] additionalInfo)
        {
            list[from].To.Add(to);
            list[from].Cost.Add(cost);
            list[to].From.Add(from);
        }

        public IEnumerable<DagPath> Get()
        {
            var dagPaths = new List<DagPath>();
            var queue = new Queue<DagPath>();

            foreach (DagPath dagPath in list)
            {
                dagPath.InDegree = dagPath.From.Count;
                if (dagPath.InDegree == 0)
                {
                    queue.Enqueue(dagPath);
                }
            }

            while (queue.Count != 0)
            {
                var top = queue.Dequeue();

                foreach (int index in top.To)
                {
                    list[index].InDegree--;
                    if (list[index].InDegree == 0)
                    {
                        queue.Enqueue(list[index]);
                    }
                }

                dagPaths.Add(top);
            }

            return dagPaths;
        }
    }

    #endregion
}