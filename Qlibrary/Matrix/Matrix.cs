using System;
using System.Diagnostics;
using System.Linq;

namespace Qlibrary
{
    public class Matrix
    {
        private readonly long[][] box;
        public int Height { get; }
        public int Width { get; }
        public long[] this[int k] { get => box[k]; set => box[k] = value; }

        public Matrix(int h, int w)
        {
            Height = h;
            Width = w;
            box = Enumerable.Range(0, h).Select(x => new long[w]).ToArray();
        }

        public Matrix I()
        {
            Debug.Assert(Height == Width);
            var mat = new Matrix(Height, Width);
            for (int i = 0; i < Height; i++) mat[i][i] = 1;
            return mat;
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            for (int i = 0; i < a.Height; i++)
            for (int j = 0; j < a.Width; j++)
                a[i][j] += b[i][j];
            return a;
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            for (int i = 0; i < a.Height; i++)
            for (int j = 0; j < a.Width; j++)
                a[i][j] -= b[i][j];
            return a;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            var c = new Matrix(a.Height, a.Width);
            for (int i = 0; i < a.Height; i++)
            for (int k = 0; k < a.Height; k++)
            for (int j = 0; j < a.Height; j++)
                c[i][j] = a[i][k] * b[k][j];
            return c;
        }

        public static Matrix operator ^(Matrix a, long k)
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

        public static bool operator ==(Matrix a, Matrix b)
        {
            for (int i = 0; i < a.Height; i++)
            for (int j = 0; j < a.Width; j++)
                if (a[i][j] != b[i][j])
                    return false;
            return true;
        }

        public static bool operator !=(Matrix a, Matrix b)
        {
            for (int i = 0; i < a.Height; i++)
            for (int j = 0; j < a.Width; j++)
                if (a[i][j] != b[i][j])
                    return true;
            return false;
        }

        public long Determinant(int n = -1)
        {
            if (n == -1) n = Height;
            Matrix b = new Matrix(Height, Width);
            b += this;
            long ret = 1;
            for (int i = 0; i < n; i++)
            {
                int idx = -1;
                for (int j = i; j < n; j++)
                {
                    if (b[j][i] != 0)
                    {
                        idx = j;
                        break;
                    }
                }

                if (idx == -1) return 0;
                if (i != idx)
                {
                    ret *= -1;
                    (b[idx], b[i]) = (b[i], b[idx]);
                }

                ret *= b[i][i];
                long inv = 1 / b[i][i];
                for (int j = 0; j < n; j++)
                {
                    b[i][j] *= inv;
                }

                for (int j = i + 1; j < n; j++)
                {
                    long a = b[j][i];
                    if (a == 0) continue;
                    for (int k = i; k < n; k++)
                    {
                        b[j][k] -= b[i][k] * a;
                    }
                }
            }

            return (ret);
        }
        
        protected bool Equals(Matrix other) => this == other;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Matrix)obj);
        }

        public override int GetHashCode()
        {
            return (box != null ? box.GetHashCode() : 0);
        }
    }
}