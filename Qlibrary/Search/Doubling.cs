using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using static System.Math;

namespace Qlibrary
{
    public class Doubling : Doubling<long>
    {
        public Doubling(int size, long limit, long initValue = 0) : base(size, limit, initValue)
        {
        }

        protected override long Addition(long a, long b) => a + b;
    }

    public class ModDoubling : Doubling<ModInt>
    {
        public ModDoubling(int size, long limit, long initValue = 0) : base(size, limit, initValue)
        {
        }

        protected override ModInt Addition(ModInt a, ModInt b) => a + b;
    }

    public abstract class Doubling<T>
    {
        private readonly int size;
        private readonly int logSize;
        private readonly (int first, T second)[][] table;
        private readonly T initValue;

        protected Doubling(int size, long limit, T initValue)
        {
            this.size = size;
            logSize = (int)Log(limit + 2);
            this.initValue = initValue;
            table = Enumerable.Repeat(0, size).Select(_ => Enumerable.Repeat((-1, initValue), logSize).ToArray())
                .ToArray();
        }

        protected abstract T Addition(T a, T b);
        public void SetNext(int from, int to, T value) => table[from][0] = (to, value);

        public void Build()
        {
            for (int k = 0; k + 1 < logSize; ++k)
            {
                for (int i = 0; i < size; ++i)
                {
                    int pre = table[i][k].first;
                    if (pre == -1)
                    {
                        table[i][k + 1] = table[i][k];
                    }
                    else
                    {
                        table[i][k + 1] = (table[pre][k].first, Addition(table[i][k].second, table[pre][k].second));
                    }
                }
            }
        }

        [MethodImpl(256)]
        public (int MoveTo, T Sum) Query(int from, long count)
        {
            T d = initValue;
            for (int k = logSize - 1; k >= 0; k--)
            {
                if (((count >> k) & 1) == 1)
                {
                    d = Addition(d, table[from][k].second);
                    from = table[from][k].first;
                }

                if (from == -1) break;
            }

            return (from, d);
        }

        // 2^n個先の要素を取得
        public (int MoveTo, T Sum) QueryPow(int from, int count2N) => table[from][count2N];

        // 未検証
        [MethodImpl(256)]
        public (long, (int, T)) SolveMax(int i, int t)
        {
            int threshold = i;
            T d = initValue;
            long times = 0;
            for (int k = logSize - 1; k >= 0; k--)
            {
                int nxt = table[threshold][k].first;
                if (nxt == -1 || nxt > t) continue;
                d = Addition(d, table[threshold][k].second);
                threshold = nxt;
                times += 1L << k;
            }

            return (times, (threshold, d));
        }

        // 未検証
        [MethodImpl(256)]
        public (long, (int, T)) SolveMin(int i, int t)
        {
            int threshold = i;
            T d = initValue;
            long times = 0;
            for (int k = logSize - 1; k >= 0; k--)
            {
                int nxt = table[threshold][k].first;
                if (nxt == -1 || nxt < t) continue;
                d = Addition(d, table[threshold][k].second);
                threshold = nxt;
                times += 1L << k;
            }

            return (times, (threshold, d));
        }
    }
}