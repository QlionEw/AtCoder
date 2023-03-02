﻿using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Qlibrary;
using static Qlibrary.Common;
using static Qlibrary.MathPlus;

namespace QlibraryBenchmark
{
    public class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Program>();
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