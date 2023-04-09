using System.Collections.Generic;
using System.Linq;

namespace Qlibrary
{
    public class Polygon
    {
        private const decimal Epsilon = (decimal)1e-8;
        private Point2D[] edges;

        public Polygon(Point2D[] edges)
        {
            this.edges = edges;
        }
        
        public IEnumerable<Point2D> ConvexHull()
        {
            var ps = edges.OrderBy(x => x).ToArray();
            int n = ps.Length;
            int k = 0;
            var ch = new Point2D[2 * n];
            for (int i = 0; i < n; ch[k++] = ps[i++])
            {
                while (k > 1 && (ch[k - 1] - ch[k - 2]).Cross(ps[i] - ch[k - 1]) <= Epsilon) k--;
            }

            int t = k;
            for (int i = n - 2; i >= 0; ch[k++] = ps[i--])
            {
                while (k > t && (ch[k - 1] - ch[k - 2]).Cross(ps[i] - ch[k - 1]) <= Epsilon) k--;
            }

            edges = ch.Take(k - 1).ToArray();
            return edges;
        }

        public decimal Area => edges.Select((t, i) => t.Cross(edges[(i + 1) % edges.Length])).Sum() * (decimal)0.5;
    }
}