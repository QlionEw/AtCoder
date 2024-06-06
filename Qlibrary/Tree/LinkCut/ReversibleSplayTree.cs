using System;

namespace Qlibrary
{
    public class ReversibleSplayTree<T> : SplayTreeBase<T>
    {
        private ReversibleBBST<T> bbst;

        public ReversibleSplayTree(Func<T, T, T> f, Func<T, T> ts)
        {
            bbst = new ReversibleBBST<T>(f, ts, Merge, Split);
        }

        public override void Push(LinkCutNode<T> node)
        {
            bbst.Push(node);
        }

        public override LinkCutNode<T> Update(LinkCutNode<T> node)
        {
            return bbst.Update(node);
        }

        public void Toggle(LinkCutNode<T> node)
        {
            bbst.Toggle(node);
        }
    }
}