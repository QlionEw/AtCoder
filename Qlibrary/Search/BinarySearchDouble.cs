using System;

namespace Qlibrary
{
    public class BinarySearchDouble
    {
        private readonly int loops;
        private double min;
        private double max;

        public BinarySearchDouble()
        {
        }

        public BinarySearchDouble(double min, double max, int loops = 100)
        {
            this.loops = loops;
            SetRange(min, max);
        }

        public void SetRange(double min, double max)
        {
            this.min = min;
            this.max = max;
        }

        public double SolveMax(Func<double, bool> judge)
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

        public double SolveMin(Func<double, bool> judge)
        {
            double ok = max;
            double ng = min;
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
    }
}