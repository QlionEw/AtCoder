using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    public readonly struct ModMatrix 
    {
        private readonly ModInt[][] box;
        public int Height { get; }
        public int Width { get; }
        public ModInt[] this[int k]
        {
            [MethodImpl(256)]get => box[k];
            [MethodImpl(256)]set => box[k] = value;
        }

        public ModMatrix(int h, int w)
        {
            Height = h;
            Width = w;
            box = Array.ConvertAll(new bool[h], _ =>  new ModInt[w]);
            AdditionalSweepLines = new List<ModInt[]>();
        }

        [MethodImpl(256)]
        public void Set(int[][] values)
        {
            for (int i = 0; i < Height; i++) for (int j = 0; j < Width; j++) box[i][j] = values[i][j];
        }
        
        [MethodImpl(256)]
        public void Set(long[][] values)
        {
            for (int i = 0; i < Height; i++) for (int j = 0; j < Width; j++) box[i][j] = values[i][j];
        }

        public ModMatrix I()
        {
            Debug.Assert(Height == Width);
            var mat = new ModMatrix(Height, Width);
            for (int i = 0; i < Height; i++) mat[i][i] = 1;
            return mat;
        }

        [MethodImpl(256)]
        public static ModMatrix operator +(ModMatrix a, ModMatrix b)
        {
            for (int i = 0; i < a.Height; i++)
            for (int j = 0; j < a.Width; j++)
            {
                a[i][j] += b[i][j];
            }
            return a;
        }

        [MethodImpl(256)]
        public static ModMatrix operator -(ModMatrix a, ModMatrix b)
        {
            for (int i = 0; i < a.Height; i++)
            for (int j = 0; j < a.Width; j++)
            {
                a[i][j] -= b[i][j];
            }
            return a;
        }

        [MethodImpl(256)]
        public static ModMatrix operator *(ModMatrix a, ModMatrix b)
        {
            var n = a.Height;
            var c = new ModMatrix(a.Height, a.Width);
            for (int i = 0; i < n; i++)
            for (int k = 0; k < n; k++)
            for (int j = 0; j < n; j++)
            {
                c[i][j] += a[i][k] * b[k][j];
            }
            return c;
        }

        public static ModMatrix operator ^(ModMatrix a, long k)
        {
            var b = a.I();
            while (k > 0)
            {
                if ((k & 1) == 1) b *= a;
                a *= a;
                k >>= 1;
            }
            return b;
        }

        public List<ModInt[]> AdditionalSweepLines { get; }
        [MethodImpl(256)]
        public void Sweep()
        {
            int sweepStart = 0;
            for (int i = 0; i < Width; i++)
            {
                int sweepIndex = -1;
                for (int j = sweepStart; j < Height; j++)
                {
                    if ((int)box[j][i] == 0) continue;
                    sweepIndex = j;
                    break;
                }
                if (sweepIndex == -1) continue;
                for (int j = 0; j < Width; j++)
                {
                    (box[sweepStart][j], box[sweepIndex][j]) = (box[sweepIndex][j], box[sweepStart][j]);
                }
                ModInt div = 1 / box[sweepStart][i];
                for (int j = 0; j < Width; j++)
                {
                    box[sweepStart][j] *= div;
                }
                for (int j = 0; j < Height; j++)
                {
                    if (j == sweepStart) continue;
                    if ((int)box[j][i] == 0) continue;
                    ModInt mul = box[j][i];
                    for (int k = 0; k < Width; k++)
                    {
                        box[j][k] -= box[sweepStart][k] * mul;
                    }
                }
                foreach (var line in AdditionalSweepLines)
                {
                    if ((int)line[i] == 0) continue;
                    ModInt mul = line[i];
                    for (int k = 0; k < Width; k++)
                    {
                        line[k] -= box[sweepStart][k] * mul;
                    }
                }
                sweepStart++;
            }
        }

        [MethodImpl(256)]
        public void AddSweepLine(int[] line) => AdditionalSweepLines.Add(Array.ConvertAll(line, x => (ModInt)x));
        [MethodImpl(256)]
        public void AddSweepLine(long[] line) => AdditionalSweepLines.Add(Array.ConvertAll(line, x => (ModInt)x));
    }
}