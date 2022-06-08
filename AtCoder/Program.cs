using System;
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
        private static T[] Make<T>(int n, Func<T> creator) => Enumerable.Repeat(0, n).Select(_ => creator()).ToArray();

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

        public static int Digit(long num, int b)
        {
            int digit = 0;
            while (num > 0)
            {
                num /= b;
                digit++;
            }
            return digit;
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
                    (a[i], a[j]) = (a[j], a[i]);
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
        
        public static long BigPow(long baseValue, long pow, long mod = long.MaxValue)
        {
            long p = baseValue % mod;
            long x = 1;
            long ret = 1;

            while (true) {
                if ((pow & x) > 0) {
                    ret = (ret * p) % mod;
                }

                x *= 2;
                if (x > pow) return ret;
                p = (p * p) % mod;
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

        public static void SafeAdd<T1, T2>(this Dictionary<T1, T2> self, T1 key, Action<T2> action) where T2 : new()
        {
            if (!self.ContainsKey(key))
            {
                self.Add(key, new T2());
            }
            action(self[key]);
        }

        public static void Init<T>(this T[,] array, int x, int y, T value)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    array[i, j] = value;
                }
            }
        }
        
        public static void Init<T>(this T[,,] array, int x, int y, int z, T value)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    for (int k = 0; k < z; k++)
                    {
                        array[i, j, k] = value;
                    }
                }
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

        public static StringBuilder sb;

        public static void Build<T>(T item) => Build(item, "\n");
        public static void BuildByLine<T>(T item) => Build(item, "\n");
        public static void BuildBySpace<T>(T item) => Build(item, " ");
        public static void Build<T>(T item, string splitter)
        {
            if (sb != null)
            {
                sb.Append(splitter);
            }
            else
            {
                sb = new StringBuilder();
            }
            sb.Append(item);
        }

        public static void OutBuild()
        {
            Console.WriteLine(sb);
        }
        
        /// <summary> Yes出力 </summary>
        public static void Yes() => Console.WriteLine("Yes");
        
        /// <summary> No出力 </summary>
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

            if (sb.Length == 0)
            {
                Console.WriteLine();
                return;
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

            if (sb.Length == 0)
            {
                Console.WriteLine();
                return;
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

        public Set(bool isMultiSet) : this(Comparer<T>.Default, isMultiSet)
        {
        }
        
        public Set() : this(false)
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
                if (t.Count == 0) return k - 1;
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
            for (long i = 1; i * i <= n; i++)
            {
                if (n % i != 0) { continue; }

                yield return i;
                if (i != n / i)
                {
                    yield return n / i;
                }
            }
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

        public static ModInt Fraction(long top, long bottom)
        {
            return new ModInt(top) / bottom;
        }
        
        public static ModInt Pow(ModInt a, long n)
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

        public static ModInt Perm(int n, int r)
        {
            if (r < 0 || n < 0) return 0;
            if (r == 0) return 1;
            if (n <= 0) return 0;
            if (n < r) return 0;
            
            return Fact(n) / Fact(n - r);
        }

        public static ModInt Comb(int n, int r)
        {
            if (r < 0 || n < 0) return 0;
            if (r == 0) return 1;
            if (n <= 0) return 0;
            if (n < r) return 0;

            return Fact(n) / Fact(n - r) / Fact(r);
        }
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
        private readonly int nodeCount;

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

        public void AddDirectedPath(int from, int to, long cost, params long[] additionalInfo)
        {
            AddPath(from, to, cost, additionalInfo);
        }
        
        public void AddUndirectedPath(int from, int to, long cost, params long[] additionalInfo)
        {
            AddPath(from, to, cost, additionalInfo);
            AddPath(to, from, cost, additionalInfo);
        }

        private void AddPath(int from, int to, long cost, params long[] additionalInfo)
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

        private bool[] isVisited; 
        public long _01Bfs(int start, int end)
        {
            Deque<int> deque = new Deque<int>();
            isVisited = new bool[nodeCount];
            Distances[start] = 0;
            deque.PushBack(start);

            while (deque.Length != 0)
            {
                int index = deque.PopFront();
                if (index == end)
                {
                    return Distances[index];
                }
                if (isVisited[index])
                {
                    continue;
                }
                isVisited[index] = true;
                foreach (PathInfo path in pathInfos[index])
                {
                    long d = Distances[index] + path.Cost;
                    if (d < Distances[path.To])
                    {
                        Distances[path.To] = d;
                        if (path.Cost != 0)
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

            return -1;
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
        private int[][] warshallFloydPathPrev;

        public long[][] WarshallFloyd(int nIndexed)
        {
            var loopCount = nodeCount + nIndexed;
            warshallFloydDp = Enumerable.Repeat(0, loopCount)
                .Select(_ => Enumerable.Repeat(Common.Infinity, loopCount).ToArray()).ToArray();
            for (int i = 0; i < loopCount; i++)
            {
                warshallFloydDp[i][i] = 0;
            }
            warshallFloydPathPrev = Enumerable.Repeat(0, loopCount)
                .Select(_ => Enumerable.Repeat(0, loopCount).ToArray()).ToArray();
            for (int i = nIndexed; i < loopCount; i++)
            {
                for (int j = nIndexed; j < loopCount; j++)
                {
                    warshallFloydPathPrev[i][j] = i;
                }
            }

            foreach (var path in pathInfos.SelectMany(x => x))
            {
                warshallFloydDp[path.From][path.To] = path.Cost;
            }

            for (int k = nIndexed; k < loopCount; k++)
            {
                for (int i = nIndexed; i < loopCount; i++)
                {
                    for (int j = nIndexed; j < loopCount; j++)
                    {
                        if (warshallFloydDp[i][k] + warshallFloydDp[k][j] < warshallFloydDp[i][j])
                        {
                            warshallFloydPathPrev[i][j] = warshallFloydPathPrev[k][j];
                            warshallFloydDp[i][j] = Math.Min(warshallFloydDp[i][j],
                                warshallFloydDp[i][k] + warshallFloydDp[k][j]);
                        }
                    }
                }
            }

            return warshallFloydDp;
        }
        
        public IEnumerable<(int,int)> GetWarshallFloydPaths(int start, int goal)
        {
            var stack = new Stack<int>();
            for (int cur = goal; cur != start; cur = warshallFloydPathPrev[start][cur])
            {
                stack.Push(cur);
            }

            int a = start;
            while (stack.Count != 0)
            {
                int c = stack.Pop();
                yield return (a, c);
                a = c;
            }
        }
    }

    public class PlanesWalker
    {
        public char Way { get; set; } = '.';
        public char Wall { get; set; } = '#';
        private readonly string[] plane;
        private readonly List<(int X, int Y)> moveDirectionList = new List<(int,int)>();
        private readonly int h;
        private readonly int w;
        
        public PlanesWalker(string[] plane, int h, int w)
        {
            this.plane = plane;
            this.h = h;
            this.w = w;
        }

        public void CommandCrossWalk()
        {
            TellMoveDirection((-1, 0), (1, 0), (0, -1), (0, 1));
        }

        public void TellMoveDirection(params (int,int)[] moveDirection)
        {
            foreach (var direction in moveDirection)
            {
                moveDirectionList.Add(direction);
            }
        }

        private int[,] distances; 
        private bool[,] isVisited; 
        public int _01Bfs((int Y, int X) start, (int Y, int X) end, Func<(int Y, int X), (int Y, int X), int> func)
        {
            Deque<(int Y, int X)> deque = new Deque<(int, int)>();
            isVisited = new bool[h, w];
            distances = new int[h, w];
            distances.Init(h, w, Common.InfinityInt);
            distances[start.Y, start.X] = 0;
            deque.PushBack((start.Y, start.X));

            while (deque.Length != 0)
            {
                var current = deque.PopFront();
                if (current.X == end.X && current.Y == end.Y)
                {
                    return distances[current.Y, current.X];
                }
                if (isVisited[current.Y, current.X]) { continue; }
                isVisited[current.Y, current.X] = true;
                foreach ((int y, int x) in moveDirectionList)
                {
                    int nextX = current.X + x;
                    int nextY = current.Y + y;
                    if ( nextX < 0 || w <= nextX ) { continue; }
                    if ( nextY < 0 || h <= nextY ) { continue; }
                    if ( plane[nextY][nextX] == Wall ) { continue; }

                    var next = (nextY, nextX);
                    int _01 = func(current, next);
                    int dd = distances[current.Y, current.X] + _01; 
                    if (dd < distances[nextY, nextX])
                    {
                        distances[nextY, nextX] = dd;
                        if (_01 == 0)
                        {
                            deque.PushFront(next);
                        }
                        else
                        {
                            deque.PushBack(next);
                        }
                    }
                }
            }

            return -1;
        }
        
        public int Dijkstra((int Y, int X) start, (int Y, int X) end, Func<(int Y, int X), (int Y, int X), int> func)
        {
            PriorityQueue<(int totalCost, int Y, int X)> pq = new PriorityQueue<(int, int, int)>(h * w);
            isVisited = new bool[h, w];
            distances = new int[h, w];
            distances.Init(h, w, Common.InfinityInt);
            distances[start.Y, start.X] = 0;
            pq.Enqueue((0, start.Y, start.X));

            while (pq.Count != 0)
            {
                var current = pq.Dequeue();
                if (current.X == end.X && current.Y == end.Y)
                {
                    return distances[current.Y, current.X];
                }
                if (isVisited[current.Y, current.X]) { continue; }
                isVisited[current.Y, current.X] = true;
                foreach ((int y, int x) in moveDirectionList)
                {
                    int nextX = current.X + x;
                    int nextY = current.Y + y;
                    if ( nextX < 0 || w <= nextX ) { continue; }
                    if ( nextY < 0 || h <= nextY ) { continue; }
                    if ( plane[nextY][nextX] == Wall ) { continue; }

                    var next = (nextY, nextX);
                    int cost = func((current.Y, current.X), next);
                    int dd = distances[current.Y, current.X] + cost; 
                    if (dd < distances[nextY, nextX])
                    {
                        distances[nextY, nextX] = dd;
                        pq.Enqueue((dd, nextY, nextX));
                    }
                }
            }

            return -1;
        }

        private int[,,] dirDistances; 
        private bool[,,] isDirVisited; 
        public int Direction01Bfs((int Y, int X) start, (int Y, int X) end, 
            Func<(int Y, int X, int Dir), (int Y, int X, int Dir), int> func)
        {
            var d = moveDirectionList.Count;
            Deque<(int Y, int X, int Dir)> deque = new Deque<(int, int, int)>();
            isDirVisited = new bool[h, w, d];
            dirDistances = new int[h, w, d];
            dirDistances.Init(h, w, d, Common.InfinityInt);
            for (int i = 0; i < d; i++)
            {
                dirDistances[start.Y, start.X, i] = 0;
                deque.PushBack((start.Y, start.X, i));
            }

            while (deque.Length != 0)
            {
                var current = deque.PopFront();
                if (current.X == end.X && current.Y == end.Y)
                {
                    return dirDistances[current.Y, current.X, current.Dir];
                }
                if (isDirVisited[current.Y, current.X, current.Dir]) { continue; }
                isDirVisited[current.Y, current.X, current.Dir] = true;
                for (int i = 0; i < d; i++)
                {
                    var (y, x) = moveDirectionList[i];
                    int nextX = current.X + x;
                    int nextY = current.Y + y;
                    if ( nextX < 0 || w <= nextX ) { continue; }
                    if ( nextY < 0 || h <= nextY ) { continue; }
                    if ( plane[nextY][nextX] == Wall ) { continue; }

                    var next = (nextY, nextX, i);
                    int _01 = func(current, next);
                    int dd = dirDistances[current.Y, current.X, current.Dir] + _01; 
                    if (dd < dirDistances[nextY, nextX, i])
                    {
                        dirDistances[nextY, nextX, i] = dd;
                        if (_01 == 0)
                        {
                            deque.PushFront(next);
                        }
                        else
                        {
                            deque.PushBack(next);
                        }
                    }
                }
            }

            return -1;
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
        
        public double SolveDoubleMax(Func<double, bool> judge, int loops = 100)
        {
            double ok = min;
            double ng = max;
            double i = (ok + ng) / 2.0;
            for (int j = 0; j < loops; j++)
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

                i = (ok + ng) / 2.0;
            }

            return ok;
        }

        public double SolveDoubleMin(Func<double, bool> judge, int loops = 100)
        {
            double ok = max;
            double ng = min;
            double i = (ok + ng) / 2.0;
            for (int j = 0; j < 100; j++)
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

                i = (ok + ng) / 2.0;
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

        public IEnumerable<List<int>> GetUnions(int count, int indexStart = 0)
        {
            Dictionary<long, List<int>> dict = new Dictionary<long, List<int>>();
            for (int i = indexStart; i < count + indexStart; i++)
            {
                var parent = Find(i);
                if (!dict.ContainsKey(parent))
                {
                    dict.Add(parent,new List<int>());
                }
                dict[parent].Add(i);
            }
            return dict.Values;
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
        ModAddition,
    }

    public class LazySegmentTree : LazySegmentTreeExtend<long>
    {
        public LazySegmentTree(int count, LazySegmentTreeMode mode) : base(count, GetInvalidValue(mode))
        {
            SetMode(mode);
        }

        private void SetMode(LazySegmentTreeMode mode)
        {
            UpdateMethod = mode switch
            {
                LazySegmentTreeMode.UpdateMin => Math.Min,
                LazySegmentTreeMode.UpdateMax => Math.Max,
                LazySegmentTreeMode.ModAddition => (l1, l2) => (l1 + l2) % ModInt.ModValue,
                _ => (l1, l2) => (l1 + l2),
            };
            UpdateLazyToActualMethod = mode switch
            {
                LazySegmentTreeMode.Addition => (l1, l2, count) => l1 + l2 * count ,
                LazySegmentTreeMode.ModAddition => (l1, l2, count) => (l1 + (l2 * count) % ModInt.ModValue) % ModInt.ModValue,
                _ => (x, m, _) => m
            };
            SetLazyDataMethod = mode switch
            {
                LazySegmentTreeMode.Addition => (l1, l2) => (l1 + l2),
                LazySegmentTreeMode.ModAddition => (l1, l2) => (l1 + l2) % ModInt.ModValue,
                _ => (m1, m2) => m2
            };
        }

        private static long GetInvalidValue(LazySegmentTreeMode mode) =>
            mode switch
            {
                LazySegmentTreeMode.UpdateMin => Common.Infinity,
                LazySegmentTreeMode.UpdateMax => -Common.Infinity,
                _ => 0
            };
    }
    
    public class LazySegmentTreeExtend<T> where T : IEquatable<T>
    {
        private T[] data;
        private T[] lazyData;
        public Func<T, T, T> UpdateMethod { get; set; }
        public Func<T, T, int, T> UpdateLazyToActualMethod { get; set; }
        public Func<T, T, T> SetLazyDataMethod { get; set; }
        public T InvalidValue { get; set; }
        private int n;
        private int count;

        public LazySegmentTreeExtend(int count, T invalidValue = default)
        {
            this.count = count;
            InvalidValue = invalidValue;

            n = 1;
            while (n < count)
            {
                n *= 2;
            }

            data = Enumerable.Repeat(InvalidValue, 2 * n - 1).ToArray();
            lazyData = Enumerable.Repeat(InvalidValue, 2 * n - 1).ToArray();
        }
        
        public void Build(Func<int, T> update)
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
            if (lazyData[k].Equals(InvalidValue)) {return;}

            if (k < n - 1)
            {
                lazyData[2 * k + 1] = SetLazyDataMethod(lazyData[2 * k + 1], lazyData[k]);
                lazyData[2 * k + 2] = SetLazyDataMethod(lazyData[2 * k + 2], lazyData[k]);
            }

            int cc = k switch
            {
                0 => n,
                _ => 1 << (MathPlus.Digit(n, 2) - MathPlus.Digit(k+1, 2))
            };
            data[k] = UpdateLazyToActualMethod(data[k], lazyData[k], cc);
            lazyData[k] = InvalidValue;
        }

        public void Update(int left, int right, T value)
        {
            Update(left, right + 1, value, 0, 0, n);
        }

        private void Update(int a, int b, T x, int k, int l, int r)
        {
            Evaluate(k);
            if (r <= a || b <= l) {return;}
            if (a <= l && r <= b)
            {
                lazyData[k] = SetLazyDataMethod(lazyData[k], x);
                Evaluate(k);
            }
            else
            {
                Update(a, b, x, 2 * k + 1, l, (l + r) / 2);
                Update(a, b, x, 2 * k + 2, (l + r) / 2, r);
                data[k] = UpdateMethod(data[2 * k + 1], data[2 * k + 2]);
            }
        }

        public T Query(int left, int right)
        {
            return Query(left, right + 1, 0, 0, n);
        }

        private T Query(int a, int b, int k, int l, int r)
        {
            Evaluate(k);
            if (r <= a || b <= l) return InvalidValue;
            if (a <= l && r <= b) return data[k];
            T vl = Query(a, b, 2 * k + 1, l, (l + r) / 2);
            T vr = Query(a, b, 2 * k + 2, (l + r) / 2, r);
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

        public int Count()
        {
            if (list != null && dict == null)
            {
                Generate(list);
            }
            return dict.Count;
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
        public class TreeVertex
        {
            public List<int> Ways { get; } = new List<int>();
            public List<long> Costs { get; } = new List<long>();
            public Set<int> VisitTime { get; } = new Set<int>();

            public IEnumerable<(int Way, long Cost)> GetWayData() => Ways.Select((t, i) => (t, Costs[i]));
        }

        public TreeVertex[] Vertexes { get; }
        public TreeStructure(int size)
        {
            Vertexes = Enumerable.Range(0, size + 1).Select(_ => new TreeVertex()).ToArray();
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
                TourTime = new List<(int, int)>();
                FirstTime = new int[Vertexes.Length];
                CheckCostAndTour(origin, 0, 0, ref time);
            }
            else
            {
                CheckCost(origin, 0, 0);
            }
            return costArray;
        }

        public List<(int, int)> TourTime { get; set; }
        public int[] FirstTime { get; set; }
        private void CheckCostAndTour(int current, long cost, int from, ref int time, int depth = 0)
        {
            costArray[current] = cost;
            if (FirstTime[current] == 0)
            {
                FirstTime[current] = time++;
            }
            TourTime.Add((current, depth));
            Vertexes[current].VisitTime.Add(FirstTime[current]);
            foreach ((int way, long l) in Vertexes[current].GetWayData())
            {
                if (way == from)
                {
                    continue;
                }
                CheckCostAndTour(way, cost + l, current, ref time, depth + 1);
                TourTime.Add((current, depth));
                Vertexes[current].VisitTime.Add(time);
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
            var min = Math.Min(FirstTime[index1], FirstTime[index2]);
            var max = Math.Max(FirstTime[index1], FirstTime[index2]);
            var item = seg.Query(min, max);
            return item.Item1;
        }

        private void InitLca()
        {
            seg = new SegmentTreeExtend<(int,int)>();
            seg.Init(TourTime.Count, (Common.InfinityInt,Common.InfinityInt), (a,b) => a.Item2 < b.Item2 ? a : b);
            seg.Build(i => TourTime[i]);
        }

        public int GetDistance(int index1, int index2)
        {
            if (seg == null)
            {
                InitLca();
            }
            var target = GetLowestCommonAncestor(index1, index2);
            return TourTime[FirstTime[index1]].Item2 + TourTime[FirstTime[index2]].Item2 -
                   2 * TourTime[FirstTime[target]].Item2;
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
        public HashSet<int> To { get; } = new HashSet<int>();
        public HashSet<int> From { get; } = new HashSet<int>();
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

        public void Connect(int from, int to, params long[] additionalInfo)
        {
            list[from].To.Add(to);
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

    public class Flow
    {
        private class FlowEdge
        {
            public int To { get; set; }
            public long Capacity { get; set; }
            public long Cost { get; set; }
            public int Reverse { get; set; }
        }

        private List<FlowEdge>[] graphs;
        private int[] level;
        private int[] iter;
        private int size;

        public Flow(int size)
        {
            this.size = size;
            graphs = Enumerable.Range(0, size+1).Select(_ => new List<FlowEdge>()).ToArray();
        }

        public void AddEdge(int from, int to, long capacity, long cost = 0)
        {
            graphs[from].Add(new FlowEdge{ To = to, Capacity = capacity, Cost = cost, Reverse = graphs[to].Count});
            graphs[to].Add(new FlowEdge{ To = from, Capacity = 0, Cost = -cost, Reverse = graphs[from].Count - 1});
        }

        public long GetMaxFlow(int from, int to)
        {
            long flow = 0;
            while (true)
            {
                FlowBfs(from);
                if (level[to] < 0)
                {
                    return flow;
                }
                iter = new int[size+1];
                long f;
                while ((f = FlowDfs(from, to, Common.Infinity)) > 0)
                {
                    flow += f;
                }
            }
        }

        public class PathInfo : IComparable<PathInfo>
        {
            public long Distance { get; set; }
            public int Number { get; set; }

            public int CompareTo(PathInfo other)
            {
                if (ReferenceEquals(this, other)) return 0;
                if (ReferenceEquals(null, other)) return 1;
                return Distance.CompareTo(other.Distance);
            }
        }
        
        private long[] distance;
        private long[] h;
        private long[] prevv;
        private long[] preve;
        public long GetMinCostFlow(int s, int t, long f)
        {
            long res = 0;
            h = new long[size+1];
            distance = new long[size+1];
            prevv = new long[size+1];
            preve = new long[size+1];
            while (f > 0)
            {
                PriorityQueue<PathInfo> queue = new PriorityQueue<PathInfo>(2 * size + 1);
                Array.Fill(distance, Common.Infinity);
                distance[s] = 0;
                queue.Enqueue(new PathInfo {Distance = 0, Number = s});

                while (queue.Count != 0)
                {
                    PathInfo pop = queue.Dequeue();
                    int v = pop.Number;
                    if (distance[v] < pop.Distance) { continue; }

                    for (int i = 0; i < graphs[v].Count; i++)
                    {
                        FlowEdge e = graphs[v][i];
                        if (e.Capacity > 0 && distance[e.To] > distance[v] + e.Cost + h[v] - h[e.To])
                        {
                            distance[e.To] = distance[v] + e.Cost + h[v] - h[e.To];
                            prevv[e.To] = v;
                            preve[e.To] = i;
                            queue.Enqueue(new PathInfo{ Distance = distance[e.To], Number = e.To});
                        }
                    }
                }

                if (distance[t] == Common.Infinity)
                {
                    return -1;
                }

                for (int i = 0; i < size+1; i++)
                {
                    h[i] += distance[i];
                }

                long d = f;
                for (long i = t; i != s; i = prevv[i])
                {
                    d = Math.Min(d, graphs[prevv[i]][(int)preve[i]].Capacity);
                }

                f -= d;
                res += d * h[t];
                for (long i = t; i != s; i = prevv[i])
                {
                    var e = graphs[prevv[i]][(int)preve[i]];
                    e.Capacity -= d;
                    graphs[i][e.Reverse].Capacity += d;
                }
            }

            return res;
        }

        private long FlowDfs(int from, int to, long flow)
        {
            if (from == to)
            {
                return flow;
            }
            for (; iter[from] < graphs[from].Count; iter[from]++)
            {
                int i = iter[from];
                var e = graphs[from][i];
                if (e.Capacity <= 0 || level[from] >= level[e.To]) {continue;}
                long d = FlowDfs(e.To, to, Math.Min(flow, e.Capacity));
                if (d <= 0) {continue;}
                e.Capacity -= d;
                graphs[e.To][e.Reverse].Capacity += d;
                return d;
            }
            return 0;
        }

        private void FlowBfs(int from)
        {
            level = Enumerable.Repeat(-1, size + 1).ToArray();
            var q = new Queue<int>();
            level[from] = 0;
            q.Enqueue(from);
            while (q.Count != 0)
            {
                int v = q.Dequeue();
                for (int i = 0; i < graphs[v].Count; i++)
                {
                    var e = graphs[v][i];
                    if (e.Capacity <= 0 || level[e.To] >= 0) {continue;}
                    level[e.To] = level[v] + 1;
                    q.Enqueue(e.To);
                }
            }
        }

        public long BipartiteMatching(int indexStart, int part1Size, int part2Size)
        {
            int start = size - 2;
            int end = size - 1;
            for (int i = indexStart; i < indexStart + part1Size; i++)
            {
                AddEdge(start, i, 1);
            }
            for (int i = indexStart + part1Size; i < indexStart + part1Size + part2Size; i++)
            {
                AddEdge(i, end, 1);
            }

            return GetMaxFlow(start, end);
        }
    }

    public class TrinarySearch
    {
        // 実際は黄金分割探索をやる
        private static readonly double Phi = (1 + Math.Sqrt(5.0)) * 0.5;
        private double min;
        private double max;

        public TrinarySearch()
        {
        }

        public TrinarySearch(double min, double max)
        {
            SetRange(min, max);
        }

        public void SetRange(double min, double max)
        {
            this.min = min;
            this.max = max;
        }

        public double SolveMax(Func<double, double> func, int loops = 100)
        {
            double left = min;
            double right = max;
            double leftValue = func((left * Phi + right) / (1 + Phi));
            double rightValue = func((left + Phi * right) / (1 + Phi));
            for (int j = 0; j < loops; j++)
            {
                if (leftValue > rightValue)
                {
                    right = (left + Phi * right) / (1 + Phi);
                    rightValue = leftValue;
                    leftValue = func((left * Phi + right) / (1 + Phi));
                }
                else
                {
                    left = (left * Phi + right) / (1 + Phi);
                    leftValue = rightValue;
                    rightValue = func((left + Phi * right) / (1 + Phi));
                }
            }

            return (right + left) * 0.5;
        }
        
        public double SolveMin(Func<double, double> func, int loops = 100)
        {
            double left = min;
            double right = max;
            double leftValue = func((left * Phi + right) / (1 + Phi));
            double rightValue = func((left + Phi * right) / (1 + Phi));
            for (int j = 0; j < loops; j++)
            {
                if (leftValue < rightValue)
                {
                    right = (left + Phi * right) / (1 + Phi);
                    rightValue = leftValue;
                    leftValue = func((left * Phi + right) / (1 + Phi));
                }
                else
                {
                    left = (left * Phi + right) / (1 + Phi);
                    leftValue = rightValue;
                    rightValue = func((left + Phi * right) / (1 + Phi));
                }
            }

            return (right + left) * 0.5;
        }
    }
    
    #endregion
}