using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Qlibrary;
using QlibraryTester;
using static Qlibrary.Common;
using static Qlibrary.MathPlus;
using static System.Math;

namespace QlibraryBenchmark
{
    [ShortRunJob]
    public class Program
    {
        private const string input = "input.txt";
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Program>();
        }

        public Program()
        {
            Input();
        }

        private void Input()
        {
            var ic = new InputCreator(input);
        }

        [Benchmark]
        public long Test1()
        {
            return 0;
        }
        
        [Benchmark]
        public long Test2()
        {
            return 0;
        }
    }
}