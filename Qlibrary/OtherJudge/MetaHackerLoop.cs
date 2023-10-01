using System;
using System.Collections.Generic;
using System.Linq;
using static Qlibrary.Common;

namespace Qlibrary
{
    public static class MetaHackerLoop
    {
        public static void Execute(Func<string> solve)
        {
            int t = Si();
            int cases = 0;
            Loop(t, () =>
            {
                Answerer.Build($"Case #{++cases}: {solve()}");
            });
            Answerer.OutBuild();
        }
        
        public static void Execute(Func<(string, string[])> solve)
        {
            int t = Si();
            int cases = 0;
            Loop(t, () =>
            {
                var s = solve();
                Answerer.Build($"Case #{++cases}: {s.Item1}");
                foreach (var s1 in s.Item2)
                {
                    Answerer.Build(s1);
                }
            });
            Answerer.OutBuild();
        }
    }
}
