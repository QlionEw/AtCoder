using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static Qlibrary.Common;

namespace Qlibrary
{
    public class AuxiliaryTree<T> : HeavyLightDecomposition<T> where T : INumber<T>
    {
        public AuxiliaryTree(Graph<T> graph, int root = 1) : base(graph, root)
        {
        }

        public (Graph<T> Tree, List<int> Index) Get(IEnumerable<int> ps) => Get(ps.ToList());
        
        // ps : 頂点集合
        // 返り値 : (aux tree, aux tree の頂点と g の頂点の対応表)
        // aux tree は 親->子 の向きの辺のみ含まれる
        // ps が空の場合は空のグラフを返す
        public (Graph<T> Tree, List<int> Index) Get(List<int> ps)
        {
            if (!ps.Any()) return (new Graph<T>(0, false), new List<int>());

            ps = ps.OrderBy(x => Down[x]).ToList();

            for (int i = 0, ie = ps.Count; i + 1 < ie; i++)
            {
                ps.Add(GetLca(ps[i], ps[i + 1]));
            }

            ps = ps.OrderBy(x => Down[x]).Distinct().ToList();

            var aux = new Graph<T>(ps.Count, false);
            var rs = new List<int> { 0 };

            for (int i = 1; i < ps.Count; i++)
            {
                int l = GetLca(ps[rs[^1]], ps[i]);
                while (ps[rs[^1]] != l) rs.RemoveAt(rs.Count - 1);
                aux.AddPath(rs[^1], i, T.One);
                rs.Add(i);
            }

            return (aux, ps);
        }
    }
}