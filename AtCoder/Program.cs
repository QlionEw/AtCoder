using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace AtCoder
{
    class Program
    {
        private static void Main(string[] args)
        {

        }
        
        #region Utility
        static readonly StreamScanner Scanner = new StreamScanner(Console.OpenStandardInput());

        /// <summary> 一括出力 </summary>
        private static void OutAllLine(IEnumerable<dynamic> items)
        {
            var sb = new StringBuilder();
            foreach (var result in items)
            {
                sb.Append(result + "\n");
            }
            sb = sb.Remove(sb.Length - 1, 1);
            Console.WriteLine(sb);
        }
        #endregion
    }

    #region Utility Class
    public class StreamScanner
    {
        public StreamScanner(Stream stream) { str = stream; }
        private readonly Stream str;
        private readonly byte[] buf = new byte[1024];
        private int len, ptr;
        public bool isEof = false;
        public bool IsEndOfStream { get { return isEof; } }
        private byte read()
        {
            if (isEof) throw new EndOfStreamException();
            if (ptr >= len) {
                ptr = 0;
                if ((len = str.Read(buf, 0, 1024)) <= 0)
                {
                    isEof = true;
                    return 0;
                }
            }
            return buf[ptr++];
        }
        public char Char() 
        {
            byte b = 0;
            do b = read();
            while (b < 33 || 126 < b);
            return (char)b; 
        }
        public string Scan()
        {
            var sb = new StringBuilder();
            for (var b = Char(); b >= 33 && b <= 126; b = (char)read())
                sb.Append(b);
            return sb.ToString();
        }
        public long Long()
        {
            long ret = 0; byte b = 0; var ng = false;
            do b = read();
            while (b != '-' && (b < '0' || '9' < b));
            if (b == '-') { ng = true; b = read(); }
            for (; true; b = read())
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
    public class Primer
    {
        /// <summary> 素数判定 </summary>
        public static bool IsPrime(int num)
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

        public static IEnumerable<int> GetPrimeFactors(int n)
        {
            int i = 2;
            int tmp = n;

            while (i * i <= n) //※1
            {
                if(tmp % i == 0){
                    tmp /= i;
                    yield return i;
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
        
        private readonly int ModValue;

        public Mod109(int modValue = Mod109._7)
        {
            ModValue = modValue;
        }
        
        /// <summary> 和 </summary>
        public long Addition(IEnumerable<long> nums)
        {
            return nums.Aggregate<long, long>(0, (current, num) => ModifyPositive((current + num) % ModValue));
        }
        
        /// <summary> 差 </summary>
        public long Subtraction(IEnumerable<long> nums)
        {
            return nums.Aggregate<long, long>(0, (current, num) => ModifyPositive((current - num) % ModValue));
        }
        
        /// <summary> 積 </summary>
        public long Multiplication(IEnumerable<long> nums)
        {
            return nums.Aggregate<long, long>(1, (current, num) => ModifyPositive((current * num) % ModValue));
        }

        private long ModifyPositive(long value)
        {
            return value < 0 ? (value + ModValue) : value;
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