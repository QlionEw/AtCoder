using System;
using System.Collections.Generic;
using System.Linq;

namespace Qlibrary
{
    public class TreeStructure
    {
        public class TreeVertex
        {
            public List<int> Ways { get; } = new List<int>();
            public List<long> Costs { get; } = new List<long>();
            public Set<int> VisitTime { get; } = new Set<int>();

            public IEnumerable<(int Way, long Cost)> GetWayData() => Ways.Select((t, i) => (t, Costs[i]));
        }

        public TreeVertex[] Vertexes { get; }
        public TreeStructure(int size)
        {
            Vertexes = Enumerable.Range(0, size + 1).Select(_ => new TreeVertex()).ToArray();
        }

        public void Connect(int index, int index2, long cost = 1)
        {
            Vertexes[index].Ways.Add(index2);
            Vertexes[index].Costs.Add(cost);
        }

        public void ConnectEach(int index, int index2, long cost = 1)
        {
            Connect(index, index2, cost);
            Connect(index2, index, cost);
        }

        private long[] costArray;
        public IEnumerable<long> CheckCost(int origin, bool isTouring = true)
        {
            costArray = new long[Vertexes.Length];
            if (isTouring)
            {
                int time = 0;
                TourTime = new List<(int, int)>();
                FirstTime = new int[Vertexes.Length];
                CheckCostAndTour(origin, 0, 0, ref time);
            }
            else
            {
                CheckCost(origin, 0, 0);
            }
            return costArray;
        }

        public List<(int, int)> TourTime { get; set; }
        public int[] FirstTime { get; set; }
        private void CheckCostAndTour(int current, long cost, int from, ref int time, int depth = 0)
        {
            costArray[current] = cost;
            if (FirstTime[current] == 0)
            {
                FirstTime[current] = time++;
            }
            TourTime.Add((current, depth));
            Vertexes[current].VisitTime.Add(FirstTime[current]);
            foreach ((int way, long l) in Vertexes[current].GetWayData())
            {
                if (way == from)
                {
                    continue;
                }
                CheckCostAndTour(way, cost + l, current, ref time, depth + 1);
                TourTime.Add((current, depth));
                Vertexes[current].VisitTime.Add(time);
                time++;
            }
        }
        
        private void CheckCost(long current, long cost, long from)
        {
            costArray[current] = cost;
            foreach ((long way, long l) in Vertexes[current].GetWayData())
            {
                if (way == from)
                {
                    continue;
                }

                CheckCost(way, cost + l, current);
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

        public long GetCost(int index1, int index2)
        {
            if (seg == null)
            {
                InitLca();
            }
            var target = GetLowestCommonAncestor(index1, index2);
            return costArray[index1] + costArray[index2] - 2 * costArray[target];
        }
    }
}