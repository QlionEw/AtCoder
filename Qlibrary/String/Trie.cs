using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    [SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
    public class Trie
    {
        public const int Invalid = -1;
        protected char Start { get; }
        private readonly int words;

        public class Node
        {
            public int[] Next { get; }
            public List<int> Indexes { get; set; }
            public int Index { get; set; }
            public char Key { get; private set; }

            public Node(char c, int wordCount)
            {
                Next = new int[wordCount];
                Next.Init(Invalid);
                Indexes = new List<int>();
                Index = Invalid;
                Key = c;
            }
        }

        public List<Node> Nodes { get; }

        public Trie(int words = 26, char start = 'a')
        {
            this.words = words;
            Start = start;
            Nodes = new List<Node> { new('$', this.words) };
        }

        protected ref int Next(int i, int j) => ref Nodes[i].Next[j];

        public void Add(string s, int x)
        {
            int pos = 0;
            foreach (var t in s)
            {
                int k = t - Start;
                if (Next(pos, k) != Invalid)
                {
                    pos = Next(pos, k);
                    continue;
                }
                int npos = Nodes.Count;
                Next(pos, k) = npos;
                Nodes.Add(new Node(t, words));
                pos = npos;
            }
            Nodes[pos].Index = x;
            Nodes[pos].Indexes.Add(x);
        }

        public int Find(string s)
        {
            int pos = 0;
            foreach (var k in s.Select(t => t - Start))
            {
                if (Next(pos, k) < 0) return Invalid;
                pos = Next(pos, k);
            }
            return pos;
        }

        public int Move(int pos, char c)
        {
            if (pos >= Nodes.Count) throw new ArgumentOutOfRangeException(nameof(pos));
            return pos < 0 ? Invalid : Next(pos, c - Start);
        }

        public int Index(int pos) => pos < 0 ? Invalid : Nodes[pos].Index;
        public List<int> Indexes(int pos) => pos < 0 ? new List<int>() : Nodes[pos].Indexes;
        public int Size => Nodes.Count;
    }
}