using System;
using System.Runtime.CompilerServices;
// ReSharper disable InconsistentNaming

namespace Qlibrary
{
    internal class ReversibleBBST<T>
    {
        protected readonly Func<T, T, T> mergeFunc;
        protected readonly Func<T, T> toggleSumFunc;

        public ReversibleBBST(Func<T, T, T> mergeFunc, Func<T, T> toggleSumFunc,
            Func<LinkCutNode<T>, LinkCutNode<T>, LinkCutNode<T>> lctMerge,
            Func<LinkCutNode<T>, int, (LinkCutNode<T>, LinkCutNode<T>)> lctSplit)
        {
            this.mergeFunc = mergeFunc;
            this.toggleSumFunc = toggleSumFunc;
            Merge = lctMerge;
            Split = lctSplit;
        }

        private readonly Func<LinkCutNode<T>, LinkCutNode<T>, LinkCutNode<T>> Merge;
        private readonly Func<LinkCutNode<T>, int, (LinkCutNode<T>, LinkCutNode<T>)> Split;

        [MethodImpl(256)]
        public void Toggle(LinkCutNode<T> node)
        {
            if (node == null) return;
            (node.Left, node.Right) = (node.Right, node.Left);
            node.Sum = toggleSumFunc(node.Sum);
            node.Reversed ^= true;
        }

        [MethodImpl(256)]
        protected T Fold(ref LinkCutNode<T> node, int a, int b)
        {
            var(left, mid) = Split(node, a);
            var(midLeft, right) = Split(mid, b - a);
            var result = GetSum(midLeft);
            node = Merge(left, Merge(midLeft, right));
            return result;
        }

        [MethodImpl(256)]
        protected void Reverse(ref LinkCutNode<T> node, int a, int b)
        {
            var(left, mid) = Split(node, a);
            var(midLeft, right) = Split(mid, b - a);
            Toggle(midLeft);
            node = Merge(left, Merge(midLeft, right));
        }

        [MethodImpl(256)]
        public LinkCutNode<T> Update(LinkCutNode<T> node)
        {
            if (node == null) return null;
            node.Count = 1;
            node.Sum = node.Key;
            if (node.Left != null)
            {
                node.Count += node.Left.Count;
                node.Sum = mergeFunc(node.Left.Sum, node.Sum);
            }
            if (node.Right != null)
            {
                node.Count += node.Right.Count;
                node.Sum = mergeFunc(node.Sum, node.Right.Sum);
            }
            return node;
        }

        [MethodImpl(256)]
        protected T GetSum(LinkCutNode<T> node)
        {
            return node == null ? default(T) : node.Sum;
        }

        [MethodImpl(256)]
        public void Push(LinkCutNode<T> node)
        {
            if (node == null) return;
            if (node.Reversed)
            {
                if (node.Left != null) Toggle(node.Left);
                if (node.Right != null) Toggle(node.Right);
                node.Reversed = false;
            }
        }
    }
}