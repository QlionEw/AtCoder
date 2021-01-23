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
        public static void OutAllLine(IEnumerable<object> items)
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
        public long[][] Table { get; private set; }
        private readonly int itemCount;
        private readonly int selectionCount;
        private readonly bool isGetMax;

        public DynamicProgramming(int itemCount, int selectionCount, bool isGetMax)
        {
            this.itemCount = itemCount;
            this.selectionCount = selectionCount;
            this.isGetMax = isGetMax;

            var initialDefaultValue = isGetMax ? MinValue : MaxValue;
            Table = Enumerable.Repeat(0,itemCount).Select(_ => Enumerable.Repeat(initialDefaultValue, selectionCount).ToArray()).ToArray();
        }

        public void SetFirstValue(long value)
        {
            for (int i = 0; i < Table[0].Length; i++)
            {
                Table[0][i] = value;
            }
        }

        public long Get(int startBy, Func<int, int, long> calcFunc)
        {
            return isGetMax 
                ? Calculation(calcFunc, startBy).Max() 
                : Calculation(calcFunc, startBy).Min();
        }

        private IEnumerable<long> Calculation(Func<int,int,long> calcFunc, int startBy)
        {
            for (int i = startBy; i < Table.Length; i++)
            {
                for (int selection = 0; selection < Table[0].Length; selection++)
                {
                    if (i < startBy)
                    {
                        continue;
                    }
                    Table[i][selection] = calcFunc(i, selection);
                }
            }
            return Table.Last();
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
    public class TreeStructure
    {
        private List<TreeItem> items = new List<TreeItem>();
        
        class TreeItem
        {
            public List<int> Connector { get; set; } = new List<int>();
            public long Value { get; set; }
        }

        public void CreatePoint(int number)
        {
            items = Enumerable.Range(0, number+1).Select(x => new TreeItem()).ToList();
        }

        public void Connect(int num1, int num2)
        {
            items[num1].Connector.Add(num2);
            items[num2].Connector.Add(num1);
        }
    }
    #endregion
}
