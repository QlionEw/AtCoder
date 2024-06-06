using System;
using System.Collections.Generic;

namespace Qlibrary
{
    public static class PersistentCommons
    {
        public const int Shift = 4; // shift size
        public const int Mask = (1 << Shift) - 1; // mask for indexing
    }
    
    public class PersistentNode<T>
    {
        public T Value { get; set; } = default;
        public PersistentNode<T>[] Ns { get; set; }

        public PersistentNode()
        {
            Ns = new PersistentNode<T>[1 << PersistentCommons.Shift];
        }

        public PersistentNode(PersistentNode<T> other)
        {
            Ns = (PersistentNode<T>[])other.Ns.Clone();
        }
    }
    
    public class PersistentArray<T> where T : IEquatable<T>
    {
        public PersistentNode<T> Root { get; private set; }
        private readonly int depth;
        private readonly T id; // Default value
        private const int Shift = PersistentCommons.Shift;
        private const int Mask = PersistentCommons.Mask;

        public PersistentArray(long max, T id = default(T))
        {
            Root = new PersistentNode<T>();
            depth = 0;
            this.id = id;
            while (max > 0)
            {
                depth++;
                max >>= Shift;
            }
        }

        public PersistentArray(IList<T> values, T id = default(T))
        {
            Root = new PersistentNode<T>();
            depth = 0;
            this.id = id;
            long max = values.Count;
            while (max > 0)
            {
                depth++;
                max >>=  Shift;
            }
            for (int i = 0; i < values.Count; i++)
            {
                PersistentNode<T> node = Root;
                for (int k = i, d = depth; d > 0; d--)
                {
                    if (node.Ns[k & Mask] == null)
                    {
                        if (d == 1)
                            node.Ns[k & Mask] = new PersistentNode<T> { Value = values[i] };
                        else
                            node.Ns[k & Mask] = new PersistentNode<T>();
                    }
                    node = node.Ns[k & Mask];
                    k >>=  Shift;
                }
            }
        }

        public T Get(PersistentNode<T> node, long k)
        {
            for (int i = depth; i > 0; i--)
            {
                if (node == null)
                    return id;
                node = node.Ns[k & Mask];
                k >>=  Shift;
            }
            return node != null ? node.Value : id;
        }

        public T Get(long k)
        {
            return Get(Root, k);
        }

        public PersistentNode<T> Update(PersistentNode<T> node, long k, T value)
        {
            Stack<(PersistentNode<T>, int)> stack = new Stack<(PersistentNode<T>, int)>();
            for (int i = depth; i > 0; i--)
            {
                stack.Push((node, (int)(k & Mask)));
                node = node?.Ns[k & Mask];
                k >>=  Shift;
            }
            PersistentNode<T> child = new PersistentNode<T> { Value = value }; // leaf
            while (stack.Count > 0)
            {
                var (parent, index) = stack.Pop();
                PersistentNode<T> newNode = parent != null ? new PersistentNode<T>(parent) : new PersistentNode<T>();
                newNode.Ns[index] = child;
                child = newNode;
            }
            return Root = child;
        }

        public PersistentNode<T> Update(long k, T value)
        {
            return Update(Root, k, value);
        }
    }
}