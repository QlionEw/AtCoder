using System.Collections.Generic;

namespace Qlibrary
{
    public class DagPath
    {
        public int Index { get; set; }
        public HashSet<int> To { get; } = new HashSet<int>();
        public HashSet<int> From { get; } = new HashSet<int>();
        public int InDegree { get; set; }
        public long CurrentCost { get; set; }
    }
}