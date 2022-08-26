using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using static System.Math;
using Qlibrary;
using static Qlibrary.Common;
using static Qlibrary.MathPlus;

namespace AtCoder
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            SourceExpander.Expander.Expand();
            checked
            {
                string s = Ss();
                string t = Ss();
 
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == t[i])
                    {
                        continue;
                    }
                    if (s[i] == '@' && "atcoder".Contains(t[i]))
                    {
                        continue;
                    }
                    if (t[i] == '@' && "atcoder".Contains(s[i]))
                    {
                        continue;
                    }
                    
                    Console.WriteLine("You will lose");
                    return;
                }
 
                Console.WriteLine("You can win");
            }
        }
    }
}

