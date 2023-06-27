using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using T = System.Int64;
using static System.Math;

namespace Qlibrary
{
    [SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
    public class ConvexFullTrick
    {
        private readonly List<ConvexFullTrickMonotone> cht = new List<ConvexFullTrickMonotone>();
        
        public int Length => cht.Sum(x => x.Length);

        [MethodImpl(256)] 
        public void Clear() => cht.Clear();

        [MethodImpl(256)] 
        public void Add(T a, T b)
        {
            cht.Add(new ConvexFullTrickMonotone(true));
            cht[^1].Add(a, b);
            while (cht.Count >= 2 && cht[^1].H.Length >= cht[^2].H.Length)
            {
                var x = new List<(long, long)>(cht[^1].H);
                cht.RemoveAt(cht.Count - 1);
                var y = new List<(long, long)>(cht[^1].H);
                cht.RemoveAt(cht.Count - 1);
                x.Reverse();
                y.Reverse();
                var c = new ConvexFullTrickMonotone(true);
                int k = 0;
                foreach (var p in x)
                {
                    while (k < y.Count && y[k].Item1 < p.Item1)
                    {
                        c.Add(y[k].Item1, y[k].Item2);
                        k++;
                    }
                    c.Add(p.Item1, p.Item2);
                }
                while (k < y.Count)
                {
                    c.Add(y[k].Item1, y[k].Item2);
                    k++;
                }
                cht.Add(c);
            }
        }
        
        [MethodImpl(256)]
        public T Query(T x) => cht.Aggregate(T.MaxValue, (current, c) => Min(current, c.Query(x)));
    }
}