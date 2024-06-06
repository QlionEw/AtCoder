using System;
using System.Collections.Generic;

namespace Qlibrary
{
    public class PersistentUnionFind
    {
        public PersistentNode<int> Root { get; private set; }
        private readonly PersistentArray<int> arr;

        public PersistentUnionFind(int n)
        {
            List<int> initialData = new List<int>(new int[n]);
            for (int i = 0; i < n; i++)
            {
                initialData[i] = -1; // initialize with -1 to represent individual components
            }
            arr = new PersistentArray<int>(initialData, -1);
            Root = arr.Root;
        }

        private (int, int) FindWithSize(int i, PersistentNode<int> r = null)
        {
            while (true)
            {
                r = r ?? Root;
                int n = arr.Get(r, i);
                if (n < 0) return (i, n);
                i = n;
            }
        }

        public int Find(int i, PersistentNode<int> r = null)
        {
            r = r ?? Root;
            return FindWithSize(i, r).Item1;
        }

        public int Size(int i, PersistentNode<int> r = null)
        {
            r = r ?? Root;
            return -FindWithSize(i, r).Item2;
        }

        public bool Same(int i, int j, PersistentNode<int> r = null)
        {
            r = r ?? Root;
            return Find(i, r) == Find(j, r);
        }

        public bool Unite(int i, int j, PersistentNode<int> r = null)
        {
            r = r ?? Root;
            int isize, jsize;
            (i, isize) = FindWithSize(i, r);
            (j, jsize) = FindWithSize(j, r);

            if (i == j) return false; // already in the same set

            if (isize > jsize)
            {
                // Make i the smaller set
                (i, j) = (j, i);
                (isize, jsize) = (jsize, isize);
            }

            // Merge j into i
            r = arr.Update(r, i, isize + jsize);
            r = arr.Update(r, j, i);
            Root = r;

            return true;
        }
    }

}