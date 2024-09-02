using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    public static class Common
    {
        public const int Power9 = 1000000000;
        public const long Power18 = 1000000000000000000L;
        public const int InfinityInt = 1 * Power9;
        public const long Infinity = 4 * Power18;
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
        [MethodImpl(256)]
        public static (T, T) SorTuple<T>(T a, T b) where T : IComparable<T> => a.CompareTo(b) < 0 ? (a,b) : (b,a);
        [MethodImpl(256)] 
        public static (T, T) SorTuple<T>((T, T) t) where T : IComparable<T> => SorTuple(t.Item1, t.Item2);

        [MethodImpl(256)]
        public static void Loop(long n, Action action)
        {
            for (int i = 0; i < n; i++)
            {
                action();
            }
        }

        [MethodImpl(256)] 
        public static T GetInfinity<T>() where T : INumberBase<T>
        {
            return T.Zero switch
            {
                int => T.CreateSaturating(InfinityInt),
                long => T.CreateSaturating(Infinity),
                double => T.CreateSaturating(double.MaxValue / 2.1),
                decimal => T.CreateSaturating(decimal.MaxValue / 2.1m),
                _ => T.CreateSaturating(long.MaxValue) / T.CreateSaturating(2.1)
            };
        }

        [MethodImpl(256)] 
        public static T Epsilon<T>() where T : INumberBase<T> => T.Zero switch
        {
            double => T.CreateSaturating(1e-8),
            _ => T.Zero
        };

        [MethodImpl(256)] public static T GMin<T>(T v1, T v2) where T : INumber<T> => v1 <= v2 ? v1 : v2;
        [MethodImpl(256)] public static T GMax<T>(T v1, T v2) where T : INumber<T> => v1 >= v2 ? v1 : v2;

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