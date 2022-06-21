using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    public class TopologicalSorter
    {
        private readonly (int Index, List<int> To)[] list;
        private readonly int[] inDegree;

        public TopologicalSorter(int size)
        {
            list = Enumerable.Range(0, size).Select(i => (i, new List<int>())).ToArray();
            inDegree = new int[size];
        }

        [MethodImpl(256)]
        public void Connect(int from, int to, params long[] additionalInfo)
        {
            list[from].To.Add(to);
            inDegree[to]++;
        }

        [MethodImpl(256)]
        public IEnumerable<(int Index,List<int> To)> Get()
        {
            var queue = new Queue<(int Index,List<int> To)>();

            foreach ((int index, List<int> to) in list)
            {
                if (inDegree[index] == 0)
                {
                    queue.Enqueue((index, to));
                }
            }

            while (queue.Count != 0)
            {
                var top = queue.Dequeue();

                foreach (int index in top.To)
                {
                    inDegree[index]--;
                    if (inDegree[index] == 0)
                    {
                        queue.Enqueue(list[index]);
                    }
                }

                yield return top;
            }
        }
    }

}