using System;

namespace Qlibrary
{
    public class BinarySearch
    {
        private long min;
        private long max;

        public BinarySearch()
        {
        }

        public BinarySearch(long min, long max)
        {
            SetRange(min, max);
        }

        public void SetRange(long min, long max)
        {
            this.min = min - 1;
            this.max = max + 1;
        }

        public long SolveMax(Func<long, bool> judge)
        {
            long ok = min;
            long ng = max;
            long i = (ok + ng) >> 1;
            while (ok + 1 < ng)
            {
                if (i == min || i == max)
                {
                    break;
                }

                if (judge(i))
                {
                    ok = i;
                }
                else
                {
                    ng = i;
                }

                i = (ok + ng) >> 1;
            }

            return ok;
        }

        public long SolveMin(Func<long, bool> judge)
        {
            long ok = max;
            long ng = min;
            long i = (ok + ng) >> 1;
            while (ng + 1 < ok)
            {
                if (i == min || i == max)
                {
                    break;
                }

                if (judge(i))
                {
                    ok = i;
                }
                else
                {
                    ng = i;
                }

                i = (ok + ng) >> 1;
            }

            return ok;
        }
    }
}