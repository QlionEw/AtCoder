using System;

namespace Qlibrary
{
    public class ReversibleBBST<T>
    {
        protected Func<T, T, T> mergeFunc;
        protected Func<T, T> toggleSumFunc;

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

        public void Toggle(LinkCutNode<T> node)
        {
            if (node == null) return;
            (node.Left, node.Right) = (node.Right, node.Left);
            node.Sum = toggleSumFunc(node.Sum);
            node.Reversed ^= true;
        }

        protected T Fold(ref LinkCutNode<T> node, int a, int b)
        {
            var(left, mid) = Split(node, a);
            var(midLeft, right) = Split(mid, b - a);
            var result = GetSum(midLeft);
            node = Merge(left, Merge(midLeft, right));
            return result;
        }

        protected void Reverse(ref LinkCutNode<T> node, int a, int b)
        {
            var(left, mid) = Split(node, a);
            var(midLeft, right) = Split(mid, b - a);
            Toggle(midLeft);
            node = Merge(left, Merge(midLeft, right));
        }

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

        protected T GetSum(LinkCutNode<T> node)
        {
            return node == null ? default(T) : node.Sum;
        }

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