using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static Qlibrary.Common;

namespace Qlibrary
{
    public static class GraphDiameter
    {
        /// <summary> グラフのループをどれか一つ検出 </summary>
        public static (T Length, int Root) Diameter<T>(this Graph<T> g) where T : INumber<T>
        {
            var gs = new GraphSolver<T>(g);
            gs.Dijkstra(1);
            var d = gs.Distances.ToIndexPairs().Where(x => x.Value != GetInfinity<T>()).MaxBy(x => x.Value).Index;
            gs.Init();
            gs.Dijkstra(d);
            return (gs.Distances.Where(x => x != GetInfinity<T>()).Max(),d);
        }
    }
}