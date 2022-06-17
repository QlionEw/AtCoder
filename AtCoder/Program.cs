using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Runtime.CompilerServices;
using static System.Math;
using Qlibrary;
using static Qlibrary.Common;

namespace AtCoder
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            SourceExpander.Expander.Expand();
            checked
            {
                int n = Si();
                int[] a = Sai(n);
                int q = Si();
                int[][] lr = Sqi(q, 2);
 
                var mo = new Mo(n, q);
                foreach (var i in lr)
                {
                    mo.Insert(i[0] - 1, i[1] - 1);
                }
 
                int[] ans = new int[q]; 
                int total = 0;
                var d = new int[n + 1];
                mo.Run(i =>
                {
                    if (d[a[i]] % 2 == 1)
                    {
                        total++;
                    }
                    d[a[i]]++;
                }, i =>
                {
                    if (d[a[i]] % 2 == 0)
                    {
                        total--;
                    }
                    d[a[i]]--;
                }, i =>
                {
                    ans[i] = total;
                });
 
                Answerer.OutAllLine(ans);
            }
        }
    }
}

