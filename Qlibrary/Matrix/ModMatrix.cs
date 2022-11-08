using System;
using System.Diagnostics;
using System.Linq;

namespace Qlibrary
{
    public class ModMatrix
    {
        private readonly ModInt[][] box;
        public int Height { get; }
        public int Width { get; }
        public ModInt[] this[int k] { get => box[k]; set => box[k] = value; }

        public ModMatrix(int h, int w)
        {
            Height = h;
            Width = w;
            box = Enumerable.Range(0, h).Select(x => new ModInt[w]).ToArray();
        }

        public ModMatrix I()
        {
            Debug.Assert(Height == Width);
            var mat = new ModMatrix(Height, Width);
            for (int i = 0; i < Height; i++) mat[i][i] = 1;
            return mat;
        }

        public static ModMatrix operator +(ModMatrix a, ModMatrix b)
        {
            for (int i = 0; i < a.Height; i++)
            for (int j = 0; j < a.Width; j++)
                a[i][j] += b[i][j];
            return a;
        }

        public static ModMatrix operator -(ModMatrix a, ModMatrix b)
        {
            for (int i = 0; i < a.Height; i++)
            for (int j = 0; j < a.Width; j++)
                a[i][j] -= b[i][j];
            return a;
        }

        public static ModMatrix operator *(ModMatrix a, ModMatrix b)
        {
            var c = new ModMatrix(a.Height, a.Width);
            for (int i = 0; i < a.Height; i++)
            for (int k = 0; k < a.Height; k++)
            for (int j = 0; j < a.Height; j++)
                c[i][j] = a[i][k] * b[k][j];
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