using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static System.Math;
using static Qlibrary.MathPlus;

namespace Qlibrary
{
    public class SternBrocotTree
    {
        public static (Fraction Lower, Fraction Upper) BinarySearch(Predicate<Fraction> f,
            long max)
        {
            var m = new SternBrocotTreeNode();
            if (max == 0) return (m.LowerBound(), m.UpperBound());
            bool Over(bool returnValue) => m.X > max || m.Y > max || f(m.Get()) == returnValue;
            if (f(new Fraction(0, 1))) return (m.LowerBound(), m.LowerBound());
            bool goLeft = Over(true);
            while (true)
            {
                if (goLeft)
                {
                    long a = 1;
                    while (true)
                    {
                        m.GoLeft(a);
                        if (Over(false))
                        {
                            m.GoParent(a);
                            break;
                        }
                        a <<= 1;
                    }
                    a >>= 1;
                    while (a != default)
                    {
                        m.GoLeft(a);
                        if (Over(false)) m.GoParent(a);
                        a >>= 1;
                    }
                    m.GoLeft(1);
                    if (m.X > max || m.Y > max) return (m.LowerBound(), m.UpperBound());
                }
                else
                {
                    long a = 1;
                    while (true)
                    {
                        m.GoRight(a);
                        if (Over(true))
                        {
                            m.GoParent(a);
                            break;
                        }
                        a <<= 1;
                    }
                    a >>= 1;
                    while (a != default)
                    {
                        m.GoRight(a);
                        if (Over(true)) m.GoParent(a);
                        a >>= 1;
                    }
                    m.GoRight(1);
                    if (m.X > max || m.Y > max) return (m.LowerBound(), m.UpperBound());
                }
                goLeft = !goLeft;
            }
        }
    }
    public class SternBrocotTreeNode
    {
        public long X { get; set; }
        public long Y { get; set; }
        private long lx, ly, rx, ry;
        private readonly List<long> seq = new List<long>();
        public SternBrocotTreeNode()
        {
            lx = 0;
            ly = 1;
            X = 1;
            Y = 1;
            rx = 1;
            ry = 0;
        }
        public SternBrocotTreeNode(long x, long y) : this()
        {
            Debug.Assert(x >= 1 && y >= 1);
            var g = Gcd(x, y);
            x /= g;
            y /= g;
            while (Min(x, y) > 0)
            {
                if (x > y)
                {
                    var d = x / y;
                    x = x - d * y;
                    GoRight(x == 0 ? d - 1 : d);
                }
                else
                {
                    var d = y / x;
                    y = y - d * x;
                    GoLeft(y == 0 ? d - 1 : d);
                }
            }
        }
        public SternBrocotTreeNode((long, long) xy) : this(xy.Item1, xy.Item2) { }
        public SternBrocotTreeNode(List<long> seq) : this()
        {
            foreach (var d in seq)
            {
                Debug.Assert(!d.Equals(0));
                if (d > 0) GoRight(d);
                if (d < 0) GoLeft(d);
            }
            Debug.Assert(this.seq.SequenceEqual(seq));
        }
        public Fraction Get() => new Fraction(X, Y);
        public Fraction LowerBound() => new Fraction(lx, ly);
        public Fraction UpperBound() => new Fraction(rx, ry);
        public long Depth() => seq.Aggregate(0L, (acc, s) => acc + Abs(s));
        public void GoLeft(long d = default)
        {
            if (d <= 0) return;
            if (!seq.Any() || seq.Last() > 0) seq.Add(default);
            seq[^1] = seq.Last() - d;
            rx += lx * d;
            ry += ly * d;
            X = rx + lx;
            Y = ry + ly;
        }
        
        public void GoRight(long d = default)
        {
            if (d <= 0) return;
            if (!seq.Any() || seq.Last() < 0) seq.Add(default);
            seq[^1] = seq.Last() + d;
            lx += rx * d;
            ly += ry * d;
            X = rx + lx;
            Y = ry + ly;
        }
        
        public bool GoParent(long d = default)
        {
            if (d <= 0) return true;
            while (d != 0)
            {
                if (!seq.Any()) return false;
                var d2 = Min(d, Abs(seq.Last()));
                if (seq.Last() > 0)
                {
                    X -= rx * d2;
                    Y -= ry * d2;
                    lx = X - rx;
                    ly = Y - ry;
                    seq[^1] = seq.Last() - d2;
                }
                else
                {
                    X -= lx * d2;
                    Y -= ly * d2;
                    rx = X - lx;
                    ry = Y - ly;
                    seq[^1] = seq.Last() + d2;
                }
                d -= d2;
                if (seq.Last() == 0) seq.RemoveAt(seq.Count - 1);
                if (d2 == 0) break;
            }
            return true;
        }
        
        public static SternBrocotTreeNode Lca(SternBrocotTreeNode lhs, SternBrocotTreeNode rhs)
        {
            var n = new SternBrocotTreeNode();
            for (var i = 0; i < Min(lhs.seq.Count, rhs.seq.Count); i++)
            {
                var val1 = lhs.seq[i];
                var val2 = rhs.seq[i];
                if ((val1 < 0) != (val2 < 0)) break;
                if (val1 < 0) n.GoLeft(Min(-val1, -val2));
                if (val1 > 0) n.GoRight(Min(val1, val2));
                if (!val1.Equals(val2)) break;
            }
            return n;
        }
    }
}