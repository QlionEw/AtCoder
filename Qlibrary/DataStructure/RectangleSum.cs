using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    internal class RectangleSumInner<TY, TWeight>
        where TY : INumber<TY>, IShiftOperators<TY, int, TY>, IBitwiseOperators<TY, TY, TY>
        where TWeight : INumber<TWeight>, INumberBase<TWeight>
    {
        private readonly BitVector[] matrix;
        private readonly TWeight[][] wl;
        private readonly int[] mid;

        public RectangleSumInner(TY[] ys, TWeight[] ws)
        {
            int size = GetSize();
            int length = ys.Length;
            if (ys.Length != ws.Length) throw new ArgumentException("v and d must have the same size.");

            matrix = new BitVector[size];
            wl = new TWeight[size][];
            mid = new int[size];

            var l = new int[length];
            var r = new int[length];
            var ord = Enumerable.Range(0, length).ToArray();

            for (int level = size - 1; level >= 0; level--)
            {
                matrix[level] = new BitVector(length + 1);
                int left = 0, right = 0;

                for (int i = 0; i < length; i++)
                {
                    if (((ys[ord[i]] >> level) & TY.One) != TY.Zero)
                    {
                        matrix[level].Set((uint)i);
                        r[right++] = ord[i];
                    }
                    else
                    {
                        l[left++] = ord[i];
                    }
                }

                mid[level] = left;
                matrix[level].Build();

                Array.Copy(l, 0, ord, 0, left);
                Array.Copy(r, 0, ord, left, right);

                wl[level] = new TWeight[length + 1];
                for (int i = 0; i < length; i++)
                {
                    var current = wl[level][i];
                    var addition = ws[ord[i]];
                    wl[level][i + 1] = current + addition;
                }
            }
        }

        private static int GetSize()
        {
            return TY.Zero switch
            {
                long => 64,
                _ => 32
            };
        }

        [MethodImpl(256)]
        private (int, int) Succ(bool f, int l, int r, int level) => Succ(f ? 1 : 0, (uint)l, (uint)r, level);
        [MethodImpl(256)]
        private (int, int) Succ(int f, uint l, uint r, int level)
        {
            return f == 1
                ? ((int)matrix[level].Rank1(l) + mid[level], (int)matrix[level].Rank1(r) + mid[level])
                : ((int)matrix[level].Rank0(l), (int)matrix[level].Rank0(r));
        }

        public TWeight RectSum(int l, int r, TY upper)
        {
            var ret = default(TWeight);
            int maxLog = matrix.Length;

            for (int level = maxLog - 1; level >= 0; level--)
            {
                bool f = ((upper >> level) & TY.One) != TY.Zero;
                if (f)
                {
                    var left = wl[level][(int)matrix[level].Rank0((uint)r)];
                    var right = wl[level][(int)matrix[level].Rank0((uint)l)];
                    ret += left - right;
                }
                (l, r) = Succ(f, l, r, level);
            }

            return ret;
        }

        [MethodImpl(256)]
        public TWeight RectSum(int l, int r, TY lower, TY upper) => RectSum(l, r, upper) - RectSum(l, r, lower);
    }

    public class RectangleSum<TY, TWeight>
        where TY : INumber<TY>, IShiftOperators<TY, int, TY>, IBitwiseOperators<TY, TY, TY>
        where TWeight : INumber<TWeight>, INumberBase<TWeight>
    {
        private readonly RectangleSumInner<int, TWeight> mat;
        private readonly Set<TY> ySet;

        public RectangleSum(TY[] v, TWeight[] d)
        {
            ySet = new Set<TY>(v, true);
            var t = v.Select(Get).ToArray();
            mat = new RectangleSumInner<int, TWeight>(t, d);
        }

        [MethodImpl(256)]
        private int Get(TY x) => ySet.LowerBound(x) + 1;

        [MethodImpl(256)]
        public TWeight RectSum(int l, int r, TY lower, TY upper) => mat.RectSum(l, r, Get(lower), Get(upper));
    }
    
    // ReSharper disable InconsistentNaming
    public class RectangleSumXY<TX, TY, TWeight>
        where TX : INumber<TY>, IShiftOperators<TY, int, TY>, IBitwiseOperators<TY, TY, TY>
        where TY : INumber<TY>, IShiftOperators<TY, int, TY>, IBitwiseOperators<TY, TY, TY>
        where TWeight : INumber<TWeight>, INumberBase<TWeight>
    {
        private readonly Set<(TX,int)> xSet = new Set<(TX, int)>(true);
        private readonly RectangleSum<TY, TWeight> rs;

        public RectangleSumXY(TX[] xs, TY[] ys, TWeight[] ws)
        {
            var yys = new TY[xs.Length];
            var wws = new TWeight[xs.Length];
            int i = 0;
            foreach (var o in xs.ToIndexPairs().OrderBy(x => x.Value))
            {
                xSet.Add((o.Value, o.Index));
                yys[i] = ys[o.Index];
                wws[i] = ws[o.Index];
                i++;
            }
            rs = new RectangleSum<TY, TWeight>(yys, wws);
        }

        [MethodImpl(256)]
        private int Get(TX x) => xSet.LowerBound((x, -1)) + 1;

        [MethodImpl(256)]
        public TWeight RectSum(TX l, TX r, TY lower, TY upper) => rs.RectSum(Get(l), Get(r), lower, upper);
    }
}