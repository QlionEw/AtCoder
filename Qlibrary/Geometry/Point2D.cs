using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static System.Math;

namespace Qlibrary
{
    public readonly struct Point2D : IComparable<Point2D>, IEquatable<Point2D>
    {
        public static Point2D Invalid = new Point2D(double.PositiveInfinity, double.PositiveInfinity);
        private const double Tol = 1e-8;
        public double X { get; }
        public double Y { get; }

        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point2D((double, double) tuple)
        {
            X = tuple.Item1;
            Y = tuple.Item2;
        }

        public Point2D(IReadOnlyList<long> array)
        {
            X = array[0];
            Y = array[1];
        }
        
        /// <summary> 和 </summary>
        public static Point2D operator+ (Point2D a, Point2D b) => new Point2D(a.X + b.X, a.Y + b.Y);
        /// <summary> 差 </summary>
        public static Point2D operator- (Point2D a, Point2D b) => new Point2D(a.X - b.X, a.Y - b.Y);
        /// <summary> 等号 </summary>
        public static bool operator ==(Point2D a, Point2D b) => (a.Equals(b));
        /// <summary> 不等号 </summary>
        public static bool operator!= (Point2D a, Point2D b) => !(a == b);
        /// <summary> 内積 </summary>
        public double Dot(Point2D other) => X * other.X - Y * other.Y;
        /// <summary> 外積 </summary>
        public double Cross(Point2D other) => X * other.Y - Y * other.X;
        /// <summary> ノルム </summary>
        public double Norm() => X * X + Y * Y;
        /// <summary> 傾き </summary>
        public double Slope(Point2D other) => X - other.X == 0 ? double.MaxValue : (other.Y - Y) / (other.X - X);
        /// <summary> 切片 </summary>
        public double Intercept(Point2D other) => X - other.X == 0 ? double.MaxValue : Cross(other) / (X - other.X);
        /// <summary> 距離 </summary>
        public double Distance(Point2D other) => Sqrt(Pow(X - other.X, 2) + Pow(Y - other.Y, 2));
        /// <summary> 偏角 </summary>
        public double Angle(Point2D other) => Atan2(other.X - X, other.Y - Y);
        /// <summary> 3点間のなす角 </summary>
        public double Angle(Point2D other1, Point2D other2)
        {
            double baX = X - other1.X;
            double baY = Y - other1.Y;
            double bcX = other2.X - other1.X;
            double bcY = other2.Y - other1.Y;

            double dotProduct = baX * bcX + baY * bcY;
            double crossProduct = baX * bcY - baY * bcX;

            return Atan2(crossProduct, dotProduct);
        }

        [MethodImpl(256)]
        public int CompareTo(Point2D other)
        {
            var xComparison = X.CompareTo(other.X);
            return xComparison != 0 ? xComparison : Y.CompareTo(other.Y);
        }

        [MethodImpl(256)]
        public bool Equals(Point2D other)
        {
            return Abs(X - other.X) < Tol && Abs(Y - other.Y) < Tol;
        }

        [MethodImpl(256)]
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj.GetType() == GetType() && Equals((Point2D)obj);
        }

        [MethodImpl(256)]
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return $"{X} {Y}";
        }
    }
}