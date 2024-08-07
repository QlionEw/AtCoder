using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    public class TreeStructure<T> where T : INumber<T>
    {
        private readonly Graph<T> g;
        public TreeStructure(Graph<T> g)
        {
            this.g = g;
        }
        public TreeStructure(int[][] tree)
        {
            g = new Graph<T>(tree.Length + 1, tree, false);
        }
        
        private T[] costArray;
        public IEnumerable<T> CheckCost(int origin, bool isTouring = true)
        {
            costArray = new T[g.Count];
            if (isTouring)
            {
                int time = 0;
                TourTime = new List<(int, int)>();
                FirstTime = new int[g.Count];
                CheckCostAndTour(origin, T.Zero, 0, ref time);
            }
            else
            {
                CheckCost(origin, T.Zero, 0);
            }
            return costArray;
        }

        public List<(int Current, int Depth)> TourTime { get; set; }
        public int[] FirstTime { get; set; }
        private void CheckCostAndTour(int current, T cost, int from, ref int time, int depth = 0)
        {
            costArray[current] = cost;
            if (FirstTime[current] == 0)
            {
                FirstTime[current] = time++;
            }
            TourTime.Add((current, depth));
            foreach (var way in g[current])
            {
                if (way.To == from)
                {
                    continue;
                }
                CheckCostAndTour(way.To, cost + way.Cost, current, ref time, depth + 1);
                TourTime.Add((current, depth));
                time++;
            }
        }

        [MethodImpl(256)]
        private void CheckCost(int current, T cost, long from)
        {
            costArray[current] = cost;
            foreach (var way in g[current])
            {
                if (way.To == from)
                {
                    continue;
                }
                CheckCost(way.To, cost + way.Cost, current);
            }
        }

        [MethodImpl(256)]
        public void SolveWayToDp(int current, Action<int, int> action) => SolveWayToDp(current, -1, action);
        [MethodImpl(256)]
        private void SolveWayToDp(int current, int from, Action<int,int> action)
        {
            if (from != -1)
            {
                action(from, current);
            }
            foreach (var way in g[current])
            {
                if (way.To == from)
                {
                    continue;
                }
                SolveWayToDp(way.To, current, action);
            }
        }
        
        [MethodImpl(256)]
        public void SolveWayBackDp(int current, Action<int, int> action) => SolveWayBackDp(current, -1, action);
        [MethodImpl(256)]
        public void SolveWayBackDp(int current, int from, Action<int,int> action)
        {
            foreach (var way in g[current])
            {
                if (way.To == from)
                {
                    continue;
                }
                SolveWayBackDp(way.To, current, action);
            }
            if (from != -1)
            {
                action(current, from);
            }
        }

        private SegmentTreeExtend<(int,int)> seg;
        public int GetLowestCommonAncestor(int index1, int index2)
        {
            if (seg == null)
            {
                InitLca();
            }
            var min = Math.Min(FirstTime[index1], FirstTime[index2]);
            var max = Math.Max(FirstTime[index1], FirstTime[index2]);
            var item = seg.Query(min, max);
            return item.Item1;
        }

        private void InitLca()
        {
            seg = new SegmentTreeExtend<(int,int)>();
            seg.Init(TourTime.Count, (Common.InfinityInt,Common.InfinityInt), (a,b) => a.Item2 < b.Item2 ? a : b);
            seg.Build(i => TourTime[i]);
        }

        public int GetDistance(int index1, int index2)
        {
            if (seg == null)
            {
                InitLca();
            }
            var target = GetLowestCommonAncestor(index1, index2);
            return TourTime[FirstTime[index1]].Item2 + TourTime[FirstTime[index2]].Item2 -
                   2 * TourTime[FirstTime[target]].Item2;
        }

        public T GetCost(int index1, int index2)
        {
            if (seg == null)
            {
                InitLca();
            }
            var target = GetLowestCommonAncestor(index1, index2);
            return costArray[index1] + costArray[index2] - T.CreateChecked(2) * costArray[target];
        }
    }
}