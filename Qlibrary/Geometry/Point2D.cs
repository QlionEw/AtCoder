using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    public class Point2D : IComparable<Point2D>, IEquatable<Point2D>
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }

        public Point2D(decimal x, decimal y)
        {
            X = x;
            Y = y;
        }

        public Point2D((decimal, decimal) tuple)
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
        public static bool operator ==(Point2D a, Point2D b) => ReferenceEquals(a, b) || ((object)a != null && a.Equals(b));
        /// <summary> 不等号 </summary>
        public static bool operator!= (Point2D a, Point2D b) => !(a == b);
        /// <summary> 内積 </summary>
        public decimal Dot(Point2D other) => X * other.X - Y * other.Y;
        /// <summary> 外積 </summary>
        public decimal Cross(Point2D other) => X * other.Y - Y * other.X;
        /// <summary> 傾き </summary>
        public decimal Slope(Point2D other) => X - other.X == 0 ? decimal.MaxValue : (other.Y - Y) / (other.X - X);
        /// <summary> 切片 </summary>
        public decimal Intercept(Point2D other) => X - other.X == 0 ? decimal.MaxValue : Cross(other) / (X - other.X);

        [MethodImpl(256)]
        public int CompareTo(Point2D other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var xComparison = X.CompareTo(other.X);
            if (xComparison != 0) return xComparison;
            return Y.CompareTo(other.Y);
        }

        [MethodImpl(256)]
        public bool Equals(Point2D other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return X == other.X && Y == other.Y;
        }

        [MethodImpl(256)]
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Point2D)obj);
        }

        [MethodImpl(256)]
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}