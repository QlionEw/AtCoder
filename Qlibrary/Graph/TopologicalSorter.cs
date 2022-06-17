using System.Collections.Generic;
using System.Linq;

namespace Qlibrary
{
    public class TopologicalSorter
    {
        private List<DagPath> list;

        public TopologicalSorter(int size)
        {
            list = Enumerable.Range(0, size).Select(i => new DagPath() {Index = i}).ToList();
        }

        public void Connect(int from, int to, params long[] additionalInfo)
        {
            list[from].To.Add(to);
            list[to].From.Add(from);
        }

        public IEnumerable<DagPath> Get()
        {
            var dagPaths = new List<DagPath>();
            var queue = new Queue<DagPath>();

            foreach (DagPath dagPath in list)
            {
                dagPath.InDegree = dagPath.From.Count;
                if (dagPath.InDegree == 0)
                {
                    queue.Enqueue(dagPath);
                }
            }

            while (queue.Count != 0)
            {
                var top = queue.Dequeue();

                foreach (int index in top.To)
                {
                    list[index].InDegree--;
                    if (list[index].InDegree == 0)
                    {
                        queue.Enqueue(list[index]);
                    }
                }

                dagPaths.Add(top);
            }

            return dagPaths;
        }
    }

}