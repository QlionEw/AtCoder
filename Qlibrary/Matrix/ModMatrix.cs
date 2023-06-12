using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    public readonly struct ModMatrix 
    {
        private readonly long[][] box;
        public int Height { get; }
        public int Width { get; }
        public long[] this[int k]
        {
            [MethodImpl(256)]get => box[k];
            [MethodImpl(256)]set => box[k] = value;
        }

        public ModMatrix(int h, int w)
        {
            Height = h;
            Width = w;
            box = Array.ConvertAll(new bool[h], _ =>  new long[w]);
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
                a[i][j] %= ModInt.ModValue;
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
                if(a[i][j] < 0) a[i][j] += ModInt.ModValue;
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
                c[i][j] %= ModInt.ModValue;
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
    }
}