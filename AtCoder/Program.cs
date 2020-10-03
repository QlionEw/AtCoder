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
        public StreamScanner(Stream stream) { str = stream; }
        private readonly Stream str;
        private readonly byte[] buf = new byte[1024];
        private int len, ptr;
        public bool IsEof { get; private set; }
        private byte Read()
        {
            if (IsEof) throw new EndOfStreamException();
            if (ptr >= len) {
                ptr = 0;
                if ((len = str.Read(buf, 0, 1024)) <= 0)
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
        public List<long> ArrayLong(int count = 0)
        {
            var scan = new List<long>();
            for (int i = 0; i < count; i++)
            {
                scan.Add(Long());
            }
            return scan;
        }
        /// <summary> 数値読み込み </summary>
        public List<int> ArrayInt(int count = 0)
        {
            var scan = new List<int>();
            for (int i = 0; i < count; i++)
            {
                scan.Add(Integer());
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
        public long[][] Table { get; private set; }
        private readonly int itemCount;
        private readonly int selectionCount;

        public DynamicProgramming(int itemCount, int selectionCount = 1)
        {
            this.itemCount = itemCount;
            this.selectionCount = selectionCount;
        }

        private static long Min(long a, long b) => a > b ? b : a;
        public long GetMin(int fallbackCount, Func<int,int,int,long> calcFunc, Func<int, long> getFirstValue, int fallbackStep = 1)
        {
            Table = Enumerable.Repeat(0,itemCount).Select(_ => Enumerable.Repeat(long.MaxValue, selectionCount).ToArray()).ToArray();
            for (int i = 0; i < Table[0].Length; i++)
            {
                Table[0][i] = getFirstValue(i);
            }

            return Calculation(fallbackCount, (i, fi, sel) => Min(Table[i][sel], calcFunc(i, fi, sel)), fallbackStep).Min();
        }

        private static long Max(long a, long b) => a < b ? b : a;
        public long GetMax(int fallbackCount, Func<int, int, int, long> calcFunc, Func<int, long> getFirstValue, int fallbackStep = 1)
        {
            Table = Enumerable.Repeat(0,itemCount).Select(_ => Enumerable.Repeat(long.MinValue, selectionCount).ToArray()).ToArray();
            for (int i = 0; i < Table[0].Length; i++)
            {
                Table[0][i] = getFirstValue(i);
            }

            return Calculation(fallbackCount, (i, fi, sel) => Max(Table[i][sel], calcFunc(i, fi, sel)), fallbackStep).Max();
        }

        private IEnumerable<long> Calculation(int fallbackCount, Func<int, int, int, long> calcFunc, int fallbackStep = 1)
        {
            var calcLength = Table.Length;
            for (int i = 0; i < calcLength; i++)
            {
                for (int fi = i - 1; fi >= 0 && fi >= i - fallbackCount * fallbackStep; fi -= fallbackStep)
                {
                    for (int selection = 0; selection < Table[0].Length; selection++)
                    {
                        Table[i][selection] = calcFunc(i, fi, selection);
                    }
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
    #endregion
}
