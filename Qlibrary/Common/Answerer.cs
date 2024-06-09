using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qlibrary
{
    public static class Answerer
    {
        private static StringBuilder sb;
        /// <summary> 構築 </summary>
        public static void Build<T>(T item) => BuildByLine(item);
        /// <summary> 構築 </summary>
        public static void BuildYesNo(bool condition) => BuildByLine(condition ? "Yes" : "No");
        /// <summary> 構築 </summary>
        public static void BuildByLine<T>(T item) => Build(item, '\n');
        /// <summary> 構築 </summary>
        public static void BuildBySpace<T>(T item) => Build(item, ' ');
        /// <summary> 構築 </summary>
        public static void Build<T>(T item, char splitter)
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

        /// <summary> 構築したものを出力 </summary>
        public static void OutBuild() => Console.WriteLine(sb);

        /// <summary> Yes出力 </summary>
        public static void Yes() => Console.WriteLine("Yes");
        
        /// <summary> No出力 </summary>
        public static void No() => Console.WriteLine("No");
        
        /// <summary> Yes/No型出力 </summary>
        public static void YesNo(bool condition) => Console.WriteLine(condition ? "Yes" : "No");

        /// <summary> 一括出力 </summary>
        public static void OutAllLine<T>(IEnumerable<T> items) => Console.WriteLine(HoldAllLine(items));

        /// <summary> 文字列構築 </summary>
        public static string HoldAllLine<T>(IEnumerable<T> items) => Hold(items, '\n');

        /// <summary> 一括出力 </summary>
        public static void OutEachSpace<T>(IEnumerable<T> items) => Console.WriteLine(HoldEachSpace(items));

        /// <summary> 文字列構築 </summary>
        public static string HoldEachSpace<T>(IEnumerable<T> items) => Hold(items, ' ');

        /// <summary> 2D一括出力 </summary>
        public static void Out2D<T>(IEnumerable<IEnumerable<T>> items) =>
            Console.WriteLine(HoldAllLine(items.Select(HoldEachSpace)));
        
        /// <summary> 2D一括出力 </summary>
        public static void Out2D<T>(T[,] items) =>
            Console.WriteLine(HoldAllLine(items.ToJaggedArray().Select(HoldEachSpace)));
        
        /// <summary> 2D一括出力(空白なし) </summary>
        public static void Out2DNoSpace<T>(T[,] items) =>
            Console.WriteLine(HoldAllLine(items.ToJaggedArray().Select(x => Hold(x, (char)0))));

        private static string Hold<T>(IEnumerable<T> items, char splitter)
        {
            var sbi = new StringBuilder();
            foreach (T result in items)
            {
                sbi.Append(result);
                if (splitter != 0)
                {
                    sbi.Append(splitter);
                }
            }

            if (sbi.Length == 0)
            {
                return "";
            }
            
            if (splitter != 0)
            {
                sbi.Remove(sbi.Length - 1, 1);
            }
            return sbi.ToString();
        }
    }
}