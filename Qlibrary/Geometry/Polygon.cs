using System.Collections.Generic;
using System.Linq;

namespace Qlibrary
{
    public class Polygon
    {
        private (decimal X, decimal Y)[] edges;
        
        public IEnumerable<(decimal X, decimal Y)> ConvexHull((decimal X, decimal Y)[] ps)
        {
            ps = ps.OrderBy(x => x).ToArray();
            int n = ps.Length;
            int k = 0;
            var ch = new (decimal X, decimal Y)[2 * n];
            for (int i = 0; i < n; ch[k++] = ps[i++])
            {
                while (k > 1 && Cross((ch[k - 1].X - ch[k - 2].X, ch[k - 1].Y - ch[k - 2].Y),
                           (ps[i].X - ch[k - 1].X, ps[i].Y - ch[k - 1].Y)) <= 0) k--;
            }

            int t = k;
            for (int i = n - 2; i >= 0; ch[k++] = ps[i--])
            {
                while (k > t && Cross((ch[k - 1].X - ch[k - 2].X, ch[k - 1].Y - ch[k - 2].Y),
                           (ps[i].X - ch[k - 1].X, ps[i].Y - ch[k - 1].Y)) <= 0) k--;
            }

            edges = ch.Take(k - 1).ToArray();
            return edges;
        }

        private static decimal Cross((decimal X, decimal Y) point1, (decimal X, decimal Y) point2)
            => point1.X * point2.Y - point1.Y * point2.X;
        public decimal Area => edges.Select((t, i) => Cross(t, edges[(i + 1) % edges.Length])).Sum() * (decimal)0.5;
    }
}