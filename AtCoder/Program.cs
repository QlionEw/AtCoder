using System;
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
        #endregion
    }

    #region Utility Class

    public static class Common
    {
        public const int InfinityInt = 1 << 30;
        public const long Infinity = (long)1 << 60;
    }
    public class StreamScanner
    {
        private const int Size = 1024 * 16;
        public StreamScanner(Stream stream) { str = stream; }
        private readonly Stream str;
        private readonly byte[] buf = new byte[Size];
        private int len, ptr;
        public bool IsEof { get; private set; }
        private byte Read()
        {
            if (IsEof) throw new EndOfStreamException();
            if (ptr >= len) {
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
            return (char)b; 
        }
        public string Scan()
        {
            var sb = new StringBuilder();
            for (var b = Char(); b >= 33 && b <= 126; b = (char)Read())
                sb.Append(b);
            return sb.ToString();
        }
        public string ScanIncludeSpace()
        {
            var sb = new StringBuilder();
            for (var b = Char(); b >= 32 && b <= 126; b = (char)Read())
                sb.Append(b);
            return sb.ToString();
        }
        public long Long()
        {
            long ret = 0; byte b = 0; var ng = false;
            do b = Read();
            while (b != '-' && (b < '0' || '9' < b));
            if (b == '-') { ng = true; b = Read(); }
            for (; true; b = Read())
            {
                if (b < '0' || '9' < b)
                    return ng ? -ret : ret;
                else ret = ret * 10 + b - '0';
            }
        }
        public int Integer() { return (int)Long(); }
        public double Double() { return double.Parse(Scan(), CultureInfo.InvariantCulture); }
        
        /// <summary> 数値読み込み </summary>
        public long[] ArrayLong(int count = 0)
        {
            var scan = new long[count];
            for (int i = 0; i < count; i++)
            {
                scan[i] = Long();
            }
            return scan;
        }
        /// <summary> 数値読み込み </summary>
        public int[] ArrayInt(int count = 0)
        {
            var scan = new int[count];
            for (int i = 0; i < count; i++)
            {
                scan[i] = Integer();
            }
            return scan;
        }
        public long[][] SquareLong(int row, int col)
        {
            var scan = new long[row][];
            for (int i = 0; i < row; i++)
            {
                scan[i] = ArrayLong(col);
            }
            return scan;
        }
        public int[][] SquareInt(int row, int col)
        {
            var scan = new int[row][];
            for (int i = 0; i < row; i++)
            {
                scan[i] = ArrayInt(col);
            }
            return scan;
        }
    }
    public class Answerer
    {
        /// <summary> Yes/No型出力 </summary>
        public static void YesNo(bool condition)
        {
            Console.WriteLine(condition ? "Yes" : "No");
        }
        
        /// <summary> 一括出力 </summary>
        public static void OutAllLine<T>(IEnumerable<T> items)
        {
            var sb = new StringBuilder();
            foreach (var result in items)
            {
                sb.Append(result + "\n");
            }
            sb = sb.Remove(sb.Length - 1, 1);
            Console.WriteLine(sb);
        }

        public static void OutEachSpace<T>(IEnumerable<T> items)
        {
            var sb = new StringBuilder();
            foreach (var result in items)
            {
                sb.Append(result + " ");
            }
            sb = sb.Remove(sb.Length - 1, 1);
            Console.WriteLine(sb);
        }
    }
    public class DynamicProgramming : DynamicProgramming<long>
    {
        private readonly bool isGetMax;
        
        public DynamicProgramming(bool isGetMax, params int[] counts)
            : base(isGetMax ? -Common.Infinity : Common.Infinity, counts)
        {
            this.isGetMax = isGetMax;
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

        public DynamicProgramming(T firstValue, params int[] counts)
        {
            XCount = counts[0];
            YCount = counts.Length >= 2 ? counts[1] : 1;
            ZCount = counts.Length >= 3 ? counts[2] : 1;
            
            Table = Enumerable.Repeat(0,ZCount)
                .Select(_ => Enumerable.Repeat(0, YCount)
                    .Select(__ => Enumerable.Repeat(firstValue, XCount).ToArray()).ToArray()).ToArray();
        }

        public void SetOrigin(T originValue)
        {
            Table[0][0][0] = originValue;
            isSetOrigin = true;
        }

        public T Origin { set => Table[0][0][0] = value; }

        public void SetFirstValue(T firstValue, bool isXAll = false, bool isYAll = false, bool isZAll = false)
        {
            if (isXAll)
            {
                for (int x = 0; x < XCount; x++)
                {
                    Table[0][0][x] = firstValue;
                }
            }

            if (isYAll)
            {
                for (int y = 0; y < YCount; y++)
                {
                    Table[0][y][0] = firstValue;
                }
            }

            if (isZAll)
            {
                for (int z = 0; z < ZCount; z++)
                {
                    Table[z][0][0] = firstValue;
                }
            }
        }

        private int xCurrent, yCurrent, zCurrent;
        public void Solve(Func<int,int,int,T> calcFunc)
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
                        Table[zCurrent][yCurrent][xCurrent] = calcFunc(xCurrent,yCurrent,zCurrent);
                    }
                }
            }
        }

        private void SolveBack(Func<int,int,int,T> calcFunc)
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
                        Table[zCurrent][yCurrent][xCurrent] = calcFunc(xCurrent,yCurrent,zCurrent);
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

        public T GetPrevious(int prevX)
        {
            return xCurrent - prevX >= 0
                ? Table[zCurrent][yCurrent][xCurrent - prevX] 
                : InvalidValue;
        }

        public T Answer1D => Table[0][0][XCount - 1];
        public IEnumerable<T> Last2D => Table[0].Select(xs => xs[XCount - 1]);
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
            if (num < 2) {return false;}
            if (num == 2) {return true;}
            if (num % 2 == 0) {return false;}

            double sqrtNum = Math.Sqrt(num);
            for (int i = 3; i <= sqrtNum; i += 2)
            {
                if (num % i == 0) { return false; }
            }
            return true;
        }

        public IEnumerable<long> GetPrimeFactors(long n)
        {
            int i = 2;
            long tmp = n;

            while (i * i <= tmp)
            {
                if(tmp % i == 0){
                    tmp /= i;
                    yield return i;
                    if (IsPrime(tmp))
                    {
                        yield return tmp;
                        tmp = 1;
                        break;
                    }
                }else{
                    i++;
                }
            }
            if(tmp != 1) yield return tmp;
        }

        public int GetDivisor(long n)
        {
            var count = 0;
            var sq = (long)Math.Sqrt(n);
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
    }
    public struct ModInt
    {
        long value;
        public const int _7 = 1000000007;
        public const int _9 = 1000000009;
        public const int _3 = 998244353;
        public static int ModValue { get; set; } = _7;
        static List<ModInt> fact = new List<ModInt> { 1 };
        public ModInt(long value) => this.value = value;
        public static implicit operator ModInt(long a) => new ModInt(a % ModValue + (a < 0 ? ModValue : 0));
        public static explicit operator int(ModInt a) => (int)a.value;
        public override string ToString() => value.ToString();
        public static ModInt operator +(ModInt a, ModInt b) => a.value + b.value;
        public static ModInt operator -(ModInt a, ModInt b) => a.value - b.value;
        public static ModInt operator *(ModInt a, ModInt b) => a.value * b.value;
        public static ModInt operator /(ModInt a, ModInt b) => a * Inv(b);
        public static ModInt Pow(ModInt a, int n) { if (n == 0) return 1; if (n == 1) return a; var p = Pow(a, n / 2); return p * p * Pow(a, n % 2); }
        public static ModInt Inv(ModInt a) => Pow(a, ModValue - 2);
        public static ModInt Fact(int n) { for (int i = fact.Count; i <= n; i++) fact.Add(fact[^1] * i); return fact[n]; }
        public static ModInt Perm(int n, int r) => Fact(n) / Fact(n - r);
        public static ModInt Comb(int n, int r) => Fact(n) / Fact(n - r) / Fact(r);
    }
    public class BitArrayMaker
    {
        public List<int> Integer(int bitValue, int length)
        {
            var list = new List<int>();
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
        
        public bool[] Boolean(int bitValue, int length)
        {
            var list = new bool[length];
            for (int i = 0; i < length; i++)
            {
                list[i] = bitValue % 2 == 1;
                bitValue /= 2;
            }
            return list;
        }
    }
    public struct PathInfo : IComparable<PathInfo>
    {
        public long From { get; set; }
        public long To { get; set; }
        public long Cost { get; set; }

        public int CompareTo(PathInfo other)
        {
            return (int)(Cost - other.Cost);
        }
    }
    public class BellmanFord
    {
        public bool[] IsLoop { get; }
        public long[] Distances { get; }
        private PathInfo[] pathInfos;

        public BellmanFord(int nodeCount, IEnumerable<PathInfo> paths)
        {
            Distances = Enumerable.Repeat(Common.Infinity, nodeCount + 1).ToArray();
            IsLoop = Enumerable.Repeat(false, nodeCount + 1).ToArray();
            pathInfos = paths.ToArray();
        }

        public void Solve(int point, bool isDetectLoop = false)
        {
            Distances[point] = 0;
            int count;
            for (count = 0; count < Distances.Length; count++)
            {
                var isUpdated = false;
                foreach (var path in pathInfos)
                {
                    if (Distances[path.From] == Common.Infinity) {continue;}
                    if (Distances[path.To] <= Distances[path.From] + path.Cost) {continue;}
                    
                    Distances[path.To] = Distances[path.From] + path.Cost;
                    isUpdated = true;
                }

                if (!isUpdated){ break; }
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
                foreach (var path in pathInfos)
                {
                    if (Distances[path.From] == Common.Infinity) {continue;}

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
    }
    public class Dijkstra
    {
        private readonly List<PathInfo>[] pathInfos;
        public long[] Distances { get; private set; }
        public int[] Color { get; }

        public Dijkstra(int nodeCount)
        {
            Distances = Enumerable.Repeat(Common.Infinity, nodeCount + 1).ToArray();
            pathInfos = Enumerable.Repeat(0, nodeCount + 1).Select(_ => new List<PathInfo>()).ToArray();
            Color = Enumerable.Repeat(-1, nodeCount + 1).ToArray();
        }

        public void Init()
        {
            Distances = Enumerable.Repeat(Common.Infinity, Distances.Length).ToArray();
        }

        public void AddPath(long from, long to, long cost, params long[] additionalInfo)
        {
            pathInfos[from].Add(new PathInfo
            {
                From = from,
                To = to,
                Cost = cost
            });
        }

        public void Solve(int point)
        {
            var queue = new PriorityQueue<PathInfo>(pathInfos.Select(x => x.Count).Sum() + 1);
            Distances[point] = 0;
            queue.Enqueue(new PathInfo {To = point, Cost = 0});

            while (queue.Count != 0)
            {
                var pop = queue.Dequeue();
                if (Distances[pop.To] < pop.Cost) { continue; }

                foreach (var path in pathInfos[pop.To])
                {
                    var nextValue = Distances[pop.To] + path.Cost;
                    if (Distances[path.To] <= nextValue) { continue; }
                    Distances[path.To] = nextValue;
                    queue.Enqueue(new PathInfo
                    {
                        From = path.From,
                        To = path.To,
                        Cost = Distances[path.To]
                    });
                }
            }
        }
    }
    public class BinarySearch
    {
        public bool IsCheckListRange { get; set; }
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
            this.min = min;
            this.max = max;
        }
        
        public long CountUnder(Func<long,bool> judge) => SolveMax(judge);
        public long SolveMax(Func<long,bool> judge)
        {
            var addition = IsCheckListRange ? -1 : 0;
            var ok = min + addition;
            var ng = max + 1 + addition;
            long i = (ok + ng) / 2;
            while (ok + 1 < ng)
            {
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

            return IsCheckListRange ? ok + 1 : ok;
        }
        
        public long CountOver(Func<long,bool> judge) => SolveMin(judge);
        public long SolveMin(Func<long,bool> judge)
        {
            var ok = max;
            var ng = min - 1;
            long i = (ok + ng) / 2;
            while (ng + 1 < ok)
            {
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
 
            return IsCheckListRange ? max - ok : ok;
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
            var ret = heap[0];
            var item = heap[--Count];

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
            Parents = Enumerable.Repeat((long)-1, n).ToArray();
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
                var tmp = x;
                x = y;
                y = tmp;
            }

            Parents[x] += Parents[y];
            Parents[y] = x;
            
            return true;
        }
    }
    public class SegmentTree
    {
        private long[] data;
        private Func<long, long, long> updateMethod;
        private long firstValue;
        private int n;

        public void Init(int count, long firstValue, Func<long,long,long> updateMethod)
        {
            this.updateMethod = updateMethod;
            this.firstValue = firstValue;
            
            n = 1;
            while (n < count)
            {
                n *= 2;
            }
            data = Enumerable.Repeat(firstValue, 2 * n - 1).ToArray();
        }

        public void Update(int index, long value)
        {
            index += n - 1;
            data[index] = value;
            while (index > 0)
            {
                index = (index - 1) / 2;
                data[index] = updateMethod(data[index * 2 + 1], data[index * 2 + 2]);
            }
        }

        public long Query(int indexStart, int indexEnd)
        {
            return Query(indexStart, indexEnd, 0, 0, n);
        }
        
        private long Query(int indexStart, int indexEnd, int current, int left, int right)
        {
            if (right <= indexStart || indexEnd <= left) { return firstValue; }
            if (indexStart <= left && right <= indexEnd) { return data[current]; }

            long leftValue = Query(indexStart, indexEnd, current * 2 + 1, left, (left + right) / 2);
            long rightValue = Query(indexStart, indexEnd, current * 2 + 2, (left + right) / 2, right);

            return updateMethod(leftValue, rightValue);
        }
    }
    #endregion
}