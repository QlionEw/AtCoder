using System;
using System.Collections.Generic;
using System.Numerics;
using static System.Math;

namespace Qlibrary
{
    public record Circle(Point2D P, double Radius)
    {
        private const double Eps = 1e-8;
        
        public Point2D[] CrossPoint(Circle other)
        {
            double d = P.Distance(other.P);
            double r = Radius + other.Radius;

            if (d > r || d + Radius < other.Radius)
                return Array.Empty<Point2D>();

            double a = Acos((Pow(Radius, 2) - Pow(other.Radius, 2) + Pow(d, 2)) / (2 * Radius * d));
            double t = Atan2(other.P.Y - P.Y, other.P.X - P.X);

            Point2D p = new Point2D(
                P.X + Radius * Cos(t + a),
                P.Y + Radius * Sin(t + a)
            );

            Point2D q = new Point2D(
                P.X + Radius * Cos(t - a),
                P.Y + Radius * Sin(t - a)
            );

            if (Abs(p.X - q.X) < Eps && Abs(p.Y - q.Y) < Eps)
                return new[] { p };

            return new[] { p, q };
        }
    }
}