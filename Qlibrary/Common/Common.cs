using System;
using System.Collections.Generic;
using System.Linq;

namespace Qlibrary
{
    public static class Common
    {
        public const int InfinityInt = 1 << 29;
        public const long Infinity = (long) 1 << 60;
        public static readonly StreamScanner Scanner = new StreamScanner(Console.OpenStandardInput());
        public static int Si() => Scanner.Integer();
        public static long Sl() => Scanner.Long();
        public static string Ss() => Scanner.Scan();
        public static int[] Sai(int count) => Scanner.ArrayInt(count);
        public static long[] Sal(int count) => Scanner.ArrayLong(count);
        public static int[][] Sqi(int yCount, int xCount) => Scanner.SquareInt(yCount, xCount);
        public static long[][] Sql(int yCount, int xCount) => Scanner.SquareLong(yCount, xCount);
        public static string[] Sss(int count) => Enumerable.Repeat(0, count).Select(_ => Ss()).ToArray();
        public static T[] Make<T>(int n, Func<T> creator) => Enumerable.Repeat(0, n).Select(_ => creator()).ToArray();

        public static void Loop(long n, Action action)
        {
            for (int i = 0; i < n; i++)
            {
                action();
            }
        }
        
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
        
        public static void Init<T>(this T[] array, T value) => Array.Fill(array, value);

        public static void Init<T>(this T[,] array, T value)
        {
            var l1 = array.GetLength(0);
            var l2 = array.GetLength(1);
            for (int i = 0; i < l1; i++)
            {
                for (int j = 0; j < l2; j++)
                {
                    array[i, j] = value;
                }
            }
        }
        
        public static void Init<T>(this T[,,] array, T value)
        {
            var l1 = array.GetLength(0);
            var l2 = array.GetLength(1);
            var l3 = array.GetLength(2);
            for (int i = 0; i < l1; i++)
            {
                for (int j = 0; j < l2; j++)
                {
                    for (int k = 0; k < l3; k++)
                    {
                        array[i, j, k] = value;
                    }
                }
            }
        }
    }
}