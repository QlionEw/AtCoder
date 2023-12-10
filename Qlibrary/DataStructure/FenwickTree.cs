using System.Numerics;
using static System.Math;

namespace Qlibrary
{
    public class FenwickTree<T> where T : INumber<T>
    {
        protected int Size { get; }
        protected T[] Data { get; }

        public FenwickTree(int size)
        {
            this.Size = size + 2;
            Data = new T[size + 3];
        }

        public T Sum(int k)
        {
            if (k < 0) return default;  // return 0 if k < 0
            T ret = default;
            for (++k; k > 0; k -= k & -k) ret += Data[k];
            return ret;
        }
        public T Sum(int l, int r) => Sum(r) - Sum(l - 1);
        public T this[int k] => Sum(k) -  Sum(k - 1);

        public void Add(int k, T x)
        {
            for (++k; k < Size; k += k & -k) Data[k] += x;
        }
        
        /// <summary> 要素が全て非負の時、[0, k]の区間和がw以下となるような最小のkを求める。 </summary>
        public int LowerBound(T w) {
            int x = 0;
            for (int k = 1 << (int)Log(Size); k > 0;  k >>= 1)
            {
                if (x + k > Size - 1 || Data[x + k] >= w) continue;
                w -= Data[x + k];
                x += k;
            }
            return x;
        }
    }
}