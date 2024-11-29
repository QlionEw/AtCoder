using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    internal abstract class SplayTreeBase<T>
    {
        public abstract void Push(LinkCutNode<T> node);
        public abstract LinkCutNode<T> Update(LinkCutNode<T> node);

        [MethodImpl(256)]
        public bool IsRoot(LinkCutNode<T> node)
        {
            return node.Parent == null || (node.Parent.Left != node && node.Parent.Right != node);
        }

        [MethodImpl(256)]
        public int Size(LinkCutNode<T> node)
        {
            return node?.Count ?? 0;
        }

        [MethodImpl(256)]
        public void Splay(LinkCutNode<T> node)
        {
            if (node == null) return;
            Push(node);
            while (!IsRoot(node))
            {
                var parent = node.Parent;
                if (IsRoot(parent))
                {
                    Push(parent);
                    Push(node);
                    Rotate(node);
                }
                else
                {
                    var grandparent = parent.Parent;
                    Push(grandparent);
                    Push(parent);
                    Push(node);
                    if (Pos(parent) == Pos(node))
                    {
                        Rotate(parent);
                        Rotate(node);
                    }
                    else
                    {
                        Rotate(node);
                        Rotate(node);
                    }
                }
            }
        }

        [MethodImpl(256)]
        public LinkCutNode<T> GetLeft(LinkCutNode<T> node)
        {
            while (node.Left != null)
            {
                Push(node);
                node = node.Left;
            }
            return node;
        }

        [MethodImpl(256)]
        public LinkCutNode<T> GetRight(LinkCutNode<T> node)
        {
            while (node.Right != null)
            {
                Push(node);
                node = node.Right;
            }
            return node;
        }

        [MethodImpl(256)]
        public (LinkCutNode<T>, LinkCutNode<T>) Split(LinkCutNode<T> node, int k)
        {
            if (node == null) return (null, null);
            if (k == 0) return (null, node);
            if (k == Size(node)) return (node, null);
            Push(node);
            if (k <= Size(node.Left))
            {
                var(left, right) = Split(node.Left, k);
                node.Left = right;
                if (right != null) right.Parent = node;
                return (left, Update(node));
            }
            else
            {
                var(left, right) = Split(node.Right, k - Size(node.Left) - 1);
                node.Right = left;
                if (left != null) left.Parent = node;
                return (Update(node), right);
            }
        }

        [MethodImpl(256)]
        public LinkCutNode<T> Merge(LinkCutNode<T> left, LinkCutNode<T> right)
        {
            if (left == null) return right;
            if (right == null) return left;
            left = GetRight(left);
            Splay(left);
            left.Right = right;
            right.Parent = left;
            return Update(left);
        }

        [MethodImpl(256)]
        public LinkCutNode<T> Build(IList<T> values)
        {
            return Build(0, values.Count, values);
        }

        [MethodImpl(256)]
        public LinkCutNode<T> Build(int l, int r, IList<T> values)
        {
            if (l == r) return null;
            return l + 1 == r ? new LinkCutNode<T>(values[l]) : Merge(Build(l, (l + r) / 2, values), Build((l + r) / 2, r, values));
        }

        [MethodImpl(256)]
        public void Insert(ref LinkCutNode<T> node, int k, T key)
        {
            Splay(node);
            var(left, right) = Split(node, k);
            node = Merge(Merge(left, new LinkCutNode<T>(key)), right);
        }

        [MethodImpl(256)]
        public void Erase(ref LinkCutNode<T> node, int k)
        {
            Splay(node);
            var(left, right) = Split(node, k);
            var(middle, newRight) = Split(right, 1);
            node = Merge(left, newRight);
        }

        [MethodImpl(256)]
        public int Count(LinkCutNode<T> node)
        {
            return node?.Count ?? 0;
        }

        [MethodImpl(256)]
        public int Pos(LinkCutNode<T> node)
        {
            if (node.Parent == null) return 0;
            if (node.Parent.Left == node) return -1;
            return node.Parent.Right == node ? 1 : 0;
        }

        [MethodImpl(256)]
        public void Rotate(LinkCutNode<T> node)
        {
            var parent = node.Parent;
            var grandparent = parent.Parent;
            if (Pos(node) == -1)
            {
                if ((parent.Left = node.Right) != null) node.Right.Parent = parent;
                node.Right = parent;
            }
            else
            {
                if ((parent.Right = node.Left) != null) node.Left.Parent = parent;
                node.Left = parent;
            }
            parent.Parent = node;
            Update(parent);
            Update(node);
            if ((node.Parent = grandparent) != null)
            {
                if (grandparent.Left == parent) grandparent.Left = node;
                if (grandparent.Right == parent) grandparent.Right = node;
            }
        }
    }
}