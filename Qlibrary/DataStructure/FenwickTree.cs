using static System.Math;

namespace Qlibrary
{
    public class FenwickTree : FenwickTree<long>
    {
        public FenwickTree(int size) : base(size)
        {
        }
        
        protected override long Addition(long a, long b) => a + b;
        protected override long Subtraction(long a, long b) => a - b;
        
        /// <summary> 要素が全て非負の時、[0, k]の区間和がw以下となるような最小のkを求める。 </summary>
        public int LowerBound(long w) {
            int x = 0;
            for (int k = 1 << (int)Log(Size); k > 0;  k >>= 1)
            {
                if (x + k > Size - 1 || Data[x + k] >= w) continue;
                w = Subtraction(w, Data[x + k]);
                x += k;
            }
            return x;
        }
    }

    public class ModFenwickTree : FenwickTree<ModInt>
    {
        public ModFenwickTree(int size) : base(size)
        {
        }

        protected override ModInt Addition(ModInt a, ModInt b) => a + b;
        protected override ModInt Subtraction(ModInt a, ModInt b) => a - b;
    }

    public abstract class FenwickTree<T>
    {
        protected int Size { get; }
        protected T[] Data { get; }

        protected FenwickTree(int size)
        {
            this.Size = size + 2;
            Data = new T[size + 3];
        }

        protected abstract T Addition(T a, T b);
        protected abstract T Subtraction(T a, T b);
        
        public T Sum(int k)
        {
            if (k < 0) return default;  // return 0 if k < 0
            T ret = default;
            for (++k; k > 0; k -= k & -k) ret = Addition(ret, Data[k]);
            return ret;
        }
        public T Sum(int l, int r) => Subtraction(Sum(r) , Sum(l - 1));
        public T this[int k] => Subtraction(Sum(k), Sum(k - 1));

        public void Add(int k, T x)
        {
            for (++k; k < Size; k += k & -k) Data[k] = Addition(Data[k], x);
        }
    }
}