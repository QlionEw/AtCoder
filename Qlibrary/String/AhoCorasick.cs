using System.Collections.Generic;
using System.Linq;

namespace Qlibrary
{
    public class AhoCorasick : Trie
    {
        private readonly int words;
        private int[] counts;

        public AhoCorasick() : this(26, 'a')
        {
        }

        public AhoCorasick(int words, char start) : base(words + 1, start)
        {
            this.words = words;
        }

        public void Build(bool heavy = true)
        {
            int n = Size;
            counts = new int[n];
            for (int i = 0; i < n; i++)
            {
                if (heavy) Indexes(i).Sort();
                counts[i] = Indexes(i).Count;
            }
            var queue = new Queue<int>();
            for (int i = 0; i < words; i++)
            {
                if (Next(0, i) != Invalid)
                {
                    Next(Next(0, i), words) = 0;
                    queue.Enqueue(Next(0, i));
                }
                else
                {
                    Next(0, i) = 0;
                }
            }
            while (queue.Count > 0)
            {
                int u = queue.Dequeue();
                for (int i = 0; i < words; i++)
                {
                    if (Next(u, i) == Invalid)
                    {
                        Next(u, i) = Next(Next(u, words), i);
                        continue;
                    }
                    
                    int v = Next(u, i);
                    queue.Enqueue(v);
                    int f = Next(u, words);
                    while (Next(f, i) == Invalid) f = Next(f, words);
                    Next(v, words) = Next(f, i);
                    counts[v] += counts[Next(v, words)];
                    if (!heavy) continue;
                    List<int> merged = new List<int>(Indexes(v));
                    merged.AddRange(Indexes(Next(v, words)));
                    merged.Sort();
                    Nodes[v].Indexes = merged;
                }
            }
        }

        public int[] Match(string s, bool heavy = true)
        {
            int[] res = new int[heavy ? Size : 1];
            int pos = 0;
            foreach (var c in s)
            {
                pos = Next(pos, c - Start);
                if (heavy)
                {
                    foreach (var idx in Indexes(pos)) res[idx]++;
                }
                else
                {
                    res[0] += counts[pos];
                }
            }
            return res;
        }

        public int WordCount(int pos) => counts[pos];
    }
}