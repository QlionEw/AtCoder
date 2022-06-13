using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Numerics;
using static System.Math;

namespace Others
{
    internal static class OtherJudge
    {
        // public static void Main(string[] args)
        // {
        //     Solve();
        // }
        
        public static void Solve()
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

    public static class Common
    {
        public const int InfinityInt = 1 << 29;
        public const long Infinity = (long) 1 << 60;
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
}