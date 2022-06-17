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
            long i = (ok + ng) / 2;
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

                i = (ok + ng) / 2;
            }

            return ok;
        }

        public long SolveMin(Func<long, bool> judge)
        {
            long ok = max;
            long ng = min;
            long i = (ok + ng) / 2;
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

                i = (ok + ng) / 2;
            }

            return ok;
        }
        
        public double SolveDoubleMax(Func<double, bool> judge, int loops = 100)
        {
            double ok = min;
            double ng = max;
            double i = (ok + ng) / 2.0;
            for (int j = 0; j < loops; j++)
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

                i = (ok + ng) / 2.0;
            }

            return ok;
        }

        public double SolveDoubleMin(Func<double, bool> judge, int loops = 100)
        {
            double ok = max;
            double ng = min;
            double i = (ok + ng) / 2.0;
            for (int j = 0; j < 100; j++)
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

                i = (ok + ng) / 2.0;
            }

            return ok;
        }
    }
}