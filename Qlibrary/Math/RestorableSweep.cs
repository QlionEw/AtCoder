using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Qlibrary
{
    public class RestorableSweep<T> where T : INumber<T>, IBitwiseOperators<T,T,T>
    {
        private class P
        {
            public T First { get; set; }
            public HashSet<int> Second { get; set; }

            public P(T first, HashSet<int> second)
            {
                First = first;
                Second = second;
            }
        }

        private readonly List<P> basis;

        public RestorableSweep()
        {
            basis = new List<P>();
        }

        public RestorableSweep(IEnumerable<T> v)
        {
            basis = new List<P>();
            int index = 0;
            foreach (var x in v)
            {
                Add(x, index++);
            }
        }

        // Add x with an ID
        public void Add(T x, int id)
        {
            var v = new P(x, new HashSet<int> { id });
            foreach (var b in basis.Where(b => v.First > (v.First ^ b.First)))
            {
                Apply(v, b);
            }
            if (v.First != T.Zero)
            {
                basis.Add(v);
            }
        }

        // Restore x and get IDs
        public (bool, List<int>) Restore(T x)
        {
            var v = new P(x, new HashSet<int>());
            foreach (var b in basis.Where(b => v.First > (v.First ^ b.First)))
            {
                Apply(v, b);
            }
            if (v.First != T.Zero)
            {
                return (false, new List<int>());
            }

            var res = v.Second.ToList();
            res.Sort();
            return (true, res);
        }

        private static void Apply(P p, P o)
        {
            p.First ^= o.First;
            foreach (var x in o.Second)
            {
                Apply(p.Second, x);
            }
        }

        private static void Apply(HashSet<int> s, int x)
        {
            if (!s.Add(x))
            {
                s.Remove(x);
            }
        }
    }
}