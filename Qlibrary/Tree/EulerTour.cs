using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Qlibrary
{
    public class EulerTour<T> where T : INumber<T>
    {
        private readonly Graph<T> g;
        public EulerTour(Graph<T> g)
        {
            this.g = g;
        }
        
        public List<int> SolveVertex(int origin, bool isRequireBack)
        {
            if (tourVertex.Count == 0)
            {
                ProcessTour(origin, -1, isRequireBack);
            }
            return tourVertex;
        }

        public List<Edge<T>> SolvePath(int origin, bool isRequireBack)
        {
            if (tourPath.Count == 0)
            {
                ProcessTour(origin, -1, isRequireBack);
            }
            return tourPath;
        }

        private readonly List<int> tourVertex = new List<int>();
        private readonly List<Edge<T>> tourPath = new List<Edge<T>>();
        private void ProcessTour(int current, int from, bool isRequireBack)
        {
            tourVertex.Add(current);
            foreach (var edge in g[current].Where(edge => edge.To != from))
            {
                tourPath.Add(edge);
                ProcessTour(edge.To, current, isRequireBack);
                if (isRequireBack) tourPath.Add(edge);
            }

            if (isRequireBack) tourVertex.Add(-current);
        }
    }
}