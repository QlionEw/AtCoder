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
        private static int Si => Scanner.Integer();
        private static long[] Sal(int count) => Scanner.ArrayLong(count);
        private static long[][] Sql(int yCount, int xCount) => Scanner.SquareLong(yCount, xCount);
        
        private static void Main(string[] args)
        {
        }

        #region Utility
        private static readonly StreamScanner Scanner = new StreamScanner(Console.OpenStandardInput());
        #endregion
    }

    #region Utility Class
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
    }
    public class DynamicProgramming
    {
        public const long MaxValue = (long) 1 << 60;
        public const long MinValue = -((long) 1 << 60);
        public long[][][] Table { get; private set; }
        private readonly int xCount;
        private readonly int yCount;
        private readonly int zCount;
        private readonly bool isGetMax;

        public DynamicProgramming(bool isGetMax, params int[] counts)
        {
            this.isGetMax = isGetMax;
            xCount = counts[0] + 1;
            yCount = counts.Length >= 2 ? counts[1] : 1;
            zCount = counts.Length >= 3 ? counts[2] : 1;

            var initialDefaultValue = isGetMax ? MinValue : MaxValue;
            Table = Enumerable.Repeat(0,zCount)
                .Select(_ => Enumerable.Repeat(0, yCount)
                    .Select(__ => Enumerable.Repeat(initialDefaultValue, xCount).ToArray()).ToArray()).ToArray();
        }

        public void SetFirstValue(long firstValue)
        {
            for (int z = 0; z < zCount; z++)
            {
                for (int y = 0; y < yCount; y++)
                {
                    Table[z][y][0] = firstValue;
                }
            }
        }

        private int xCurrent, yCurrent, zCurrent;
        public void Solve(Func<int,int,int,long> calcFunc)
        {
            for (xCurrent = 1; xCurrent < xCount; xCurrent++)
            {
                for (yCurrent = 0; yCurrent < yCount; yCurrent++)
                {
                    for (zCurrent = 0; zCurrent < zCount; zCurrent++)
                    {
                        Table[zCurrent][yCurrent][xCurrent] = calcFunc(xCurrent-1,yCurrent,zCurrent);
                    }
                }
            }
        }

        public long GetPrevious(int prevStep, int yIndex, int zIndex)
        {
            return xCurrent - prevStep >= 0
                ? Table[zIndex][yIndex][xCurrent - prevStep]
                : Table[zIndex][yIndex][0];
        }
        public long GetPrevious(int prevStep, int yIndex) => GetPrevious(prevStep, yIndex, zCurrent);
        public long GetPrevious(int prevStep) => GetPrevious(prevStep, yCurrent, zCurrent);

        public long Answer1D => Table[0][0][xCount - 1];
        public long Answer2D => isGetMax ? Table[0].Max(xs => xs[xCount - 1]) : Table[0].Min(xs => xs[xCount - 1]);
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
    public class Mod109
    {
        public const int _7 = 1000000007;
        public const int _9 = 1000000009;
        public const int _3 = 998244353;
        
        private readonly int modValue;

        public Mod109(int modValue = Mod109._7)
        {
            this.modValue = modValue;
        }
        
        /// <summary> 和 </summary>
        public long Addition(long start, params long[] nums)
        {
            return nums.Aggregate(start, (current, num) => ModifyPositive((current + num) % modValue));
        }
        
        /// <summary> 差 </summary>
        public long Subtraction(long start, params long[] nums)
        {
            return nums.Aggregate(start, (current, num) => ModifyPositive((current - num) % modValue));
        }
        
        /// <summary> 積 </summary>
        public long Multiplication(long start, params long[] nums)
        {
            return nums.Aggregate(start, (current, num) => ModifyPositive((current * num) % modValue));
        }

        private long ModifyPositive(long value)
        {
            return value < 0 ? (value + modValue) : value;
        }
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
            Distances = Enumerable.Repeat(long.MaxValue, nodeCount + 1).ToArray();
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
                    if (Distances[path.From] == long.MaxValue) {continue;}
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
                    if (Distances[path.From] == long.MaxValue) {continue;}

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
        public long[] Distances { get; }

        public Dijkstra(int nodeCount)
        {
            Distances = Enumerable.Repeat(long.MaxValue, nodeCount + 1).ToArray();
            pathInfos = Enumerable.Repeat(0, nodeCount + 1).Select(_ => new List<PathInfo>()).ToArray();
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
        private long min;
        private long max;
        
        public BinarySearch(long min, long max)
        {
            this.min = min;
            this.max = max;
        }
        
        public long Solve(Func<long,bool> judge)
        {
            var ok = min;
            var ng = max + 1;
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

    #endregion
}