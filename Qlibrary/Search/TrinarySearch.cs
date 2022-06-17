using System;

namespace Qlibrary
{
    public class TrinarySearch
    {
        // 実際は黄金分割探索をやる
        private static readonly double Phi = (1 + Math.Sqrt(5.0)) * 0.5;
        private double min;
        private double max;

        public TrinarySearch()
        {
        }

        public TrinarySearch(double min, double max)
        {
            SetRange(min, max);
        }

        public void SetRange(double min, double max)
        {
            this.min = min;
            this.max = max;
        }

        public double SolveMax(Func<double, double> func, int loops = 100)
        {
            double left = min;
            double right = max;
            double leftValue = func((left * Phi + right) / (1 + Phi));
            double rightValue = func((left + Phi * right) / (1 + Phi));
            for (int j = 0; j < loops; j++)
            {
                if (leftValue > rightValue)
                {
                    right = (left + Phi * right) / (1 + Phi);
                    rightValue = leftValue;
                    leftValue = func((left * Phi + right) / (1 + Phi));
                }
                else
                {
                    left = (left * Phi + right) / (1 + Phi);
                    leftValue = rightValue;
                    rightValue = func((left + Phi * right) / (1 + Phi));
                }
            }

            return (right + left) * 0.5;
        }
        
        public double SolveMin(Func<double, double> func, int loops = 100)
        {
            double left = min;
            double right = max;
            double leftValue = func((left * Phi + right) / (1 + Phi));
            double rightValue = func((left + Phi * right) / (1 + Phi));
            for (int j = 0; j < loops; j++)
            {
                if (leftValue < rightValue)
                {
                    right = (left + Phi * right) / (1 + Phi);
                    rightValue = leftValue;
                    leftValue = func((left * Phi + right) / (1 + Phi));
                }
                else
                {
                    left = (left * Phi + right) / (1 + Phi);
                    leftValue = rightValue;
                    rightValue = func((left + Phi * right) / (1 + Phi));
                }
            }

            return (right + left) * 0.5;
        }
    }

}