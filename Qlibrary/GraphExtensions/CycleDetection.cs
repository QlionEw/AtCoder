using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static Qlibrary.Common;

namespace Qlibrary
{
    public static class CycleDetection
    {
        /// <summary> 頂点xが含まれるループを検出 </summary>
        public static List<(int, int)> CycleOf<T>(this Graph<T> g, int x)
            where T : INumber<T> => Cycles(g, x, x + 1).FirstOrDefault() ?? new List<(int, int)>();
        
        /// <summary> グラフのループをどれか一つ検出 </summary>
        public static List<(int, int)> Cycle<T>(this Graph<T> g)
            where T : INumber<T> => Cycles(g, 0, g.Count).FirstOrDefault() ?? new List<(int, int)>();
        
        /// <summary>
        /// グラフのループをできるだけ検出 <br/>
        /// 1-2-3 , 4-5-6 でループになっている場合のみ対応 <br/>
        /// 1-2-3 , 1-4-3 とかは無理
        /// </summary>
        public static IEnumerable<List<(int, int)>> Cycles<T>(this Graph<T> g)
            where T : INumber<T> => Cycles(g, 0, g.Count);
        
        private static IEnumerable<List<(int, int)>> Cycles<T>(this Graph<T> g, int start, int end)
            where T : INumber<T>
        {
            for (int i = start; i < end; i++)
            {
                if (g[i].Select(x => x.To).All(j => i != j)) continue;
                yield return new List<(int, int)> { (i, i) };
            }

            var pIdx = new int[g.Count];
            var isVisit = new bool[g.Count];
            pIdx.Init(-1);

            var cycle = new List<(int, int)>();
            bool isFinished = false;
            for (int i = start; i < end; i++)
            {
                if (isVisit[i]) continue;
                var d = Dfs(i, i, -1);
                if (cycle.Count == 0) continue;
                cycle.Reverse();
                yield return cycle;
                isFinished = false;
                cycle = new List<(int, int)>();
            }

            yield break;

            int Dfs(int cur, int pVal, int par)
            {
                pIdx[cur] = pVal;
                isVisit[cur] = true;
                foreach (var dst in g[cur].Select(x => x.To))
                {
                    if (isFinished) return -1;
                    if (!g.IsDirected && dst == par) continue;
                    if (pIdx[dst] == pIdx[cur])
                    {
                        cycle.Add((cur, dst));
                        return dst;
                    }
                    if (isVisit[dst]) continue;
                    int nx = Dfs(dst, pVal, cur);
                    if (nx == -1) continue;
                    cycle.Add((cur, dst));
                    if (cur != nx) return nx;
                    isFinished = true;
                    return -1;
                }
                pIdx[cur] = -1;
                return -1;
            }
        }
    }
}