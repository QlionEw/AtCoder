using System;
using System.Collections.Generic;
using System.Text;

namespace Qlibrary
{
    public static class Answerer
    {
        private static StringBuilder sb;
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
                sb.Append(result);
                sb.Append("\n");
            }

            if (sb.Length == 0)
            {
                Console.WriteLine();
                return;
            }

            sb.Remove(sb.Length - 1, 1);
            Console.WriteLine(sb);
        }

        public static void OutEachSpace<T>(IEnumerable<T> items)
        {
            var sb = new StringBuilder();
            foreach (T result in items)
            {
                sb.Append(result);
                sb.Append(" ");
            }

            if (sb.Length == 0)
            {
                Console.WriteLine();
                return;
            }
            
            sb.Remove(sb.Length - 1, 1);
            Console.WriteLine(sb);
        }
    }
}