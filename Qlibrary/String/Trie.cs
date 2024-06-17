using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    public class Trie
    {
        private const int X = 26;
        private const char Margin = 'a';

        public class Node
        {
            public int[] Next { get; private set; }
            public List<int> Indexes { get; private set; }
            public int Index { get; set; }
            public char Key { get; private set; }

            public Node(char c)
            {
                Next = new int[X];
                for (int i = 0; i < X; i++) Next[i] = -1;
                Indexes = new List<int>();
                Index = -1;
                Key = c;
            }
        }

        public List<Node> Nodes { get; }

        public Trie(char c = '$')
        {
            Nodes = new List<Node> { new(c) };
        }

        private ref int Next(int i, int j) => ref Nodes[i].Next[j];

        public void Add(string s, int x)
        {
            int pos = 0;
            foreach (var t in s)
            {
                int k = t - Margin;
                if (Next(pos, k) != -1)
                {
                    pos = Next(pos, k);
                    continue;
                }
                int npos = Nodes.Count;
                Next(pos, k) = npos;
                Nodes.Add(new Node(t));
                pos = npos;
            }
            Nodes[pos].Index = x;
            Nodes[pos].Indexes.Add(x);
        }

        public int Find(string s)
        {
            int pos = 0;
            foreach (var k in s.Select(t => t - Margin))
            {
                if (Next(pos, k) < 0) return -1;
                pos = Next(pos, k);
            }
            return pos;
        }

        public int Move(int pos, char c)
        {
            if (pos >= Nodes.Count) throw new ArgumentOutOfRangeException(nameof(pos));
            return pos < 0 ? -1 : Next(pos, c - Margin);
        }

        public int Index(int pos) => pos < 0 ? -1 : Nodes[pos].Index;
        public List<int> Indexes(int pos) => pos < 0 ? new List<int>() : Nodes[pos].Indexes;
        public int Count => Nodes.Count;
        public IEnumerable<int> this[int node] => Nodes[node].Next.Where(x => x != -1);
    }
}