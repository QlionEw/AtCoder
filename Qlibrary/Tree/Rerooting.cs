using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    public class Rerooting<T>
    {
        private readonly SimpleGraph g;
        private readonly T[] memo;
        private readonly T[] dp;
        // merge value of child node
        private Func<T, T, T> merge;
        // return value from child node to parent node
        private Func<(T Value, int From, int To), T> wayBack;
        private T initValue;

        public Rerooting(SimpleGraph graph, T initValue = default)
        {
            g = graph;
            memo = new T[graph.Count];
            dp = new T[graph.Count];
            SetInitValue(initValue);
        }

        private void SetInitValue(T v)
        {
            Array.Fill(memo, v);
            Array.Fill(dp, v);
            initValue = v;
        }

        public T[] Solve(Func<T, T, T> mergeFunc, Func<(T Value, int From, int To), T> wayBackFunc)
        {
            merge = mergeFunc;
            wayBack = wayBackFunc;
            Dfs(1, -1);
            Efs(1, -1, initValue);
            return dp;
        }

        [MethodImpl(256)]
        private void Dfs(int current, int parent)
        {
            foreach (var dst in g[current])
            {
                if (dst == parent) continue;
                Dfs(dst, current);
                memo[current] = merge(memo[current], wayBack((memo[dst], dst, current)));
            }
        }
        
        [MethodImpl(256)]
        private void Efs(int current, int parent, T pVal)
        {
            var buf = new List<T>();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var dst in g[current])
            {
                if (dst == parent) continue;
                buf.Add(wayBack((memo[dst], dst, current)));
            }
            int n = buf.Count;
            var head = new T[n + 1];
            var tail = new T[n + 1];
            head[0] = tail[n] = initValue;
            for (int i = 0; i <n; i++) head[i + 1] = merge(head[i], buf[i]);
            for (int i = n - 1; i >= 0; i--) tail[i] = merge(tail[i + 1], buf[i]);
            
            dp[current] = parent == -1 ? head[^1] : merge(pVal, head[^1]);
            
            int idx = 0;
            foreach (var dst in g[current])
            {
                if (dst == parent) continue;
                Efs(dst, current, wayBack((merge(pVal, merge(head[idx], tail[idx + 1])), current, dst)));
                idx++;
            }
        }
    }
}