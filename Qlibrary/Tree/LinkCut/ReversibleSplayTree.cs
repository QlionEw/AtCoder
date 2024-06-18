using System;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    public class ReversibleSplayTree<T> : SplayTreeBase<T>
    {
        private ReversibleBBST<T> bbst;

        public ReversibleSplayTree(Func<T, T, T> f, Func<T, T> ts)
        {
            bbst = new ReversibleBBST<T>(f, ts, Merge, Split);
        }

        [MethodImpl(256)]
        public override void Push(LinkCutNode<T> node) => bbst.Push(node);
        [MethodImpl(256)]
        public override LinkCutNode<T> Update(LinkCutNode<T> node) => bbst.Update(node);
        [MethodImpl(256)]
        public void Toggle(LinkCutNode<T> node) => bbst.Toggle(node);
    }
}