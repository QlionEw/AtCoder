using System.Numerics;

namespace Qlibrary
{
    // https://ei1333.github.io/library/structure/convex-hull-trick/dynamic-li-chao-tree.hpp
    // -> 静的な Li-Chao-Tree と動的な Li-Chao-Tree の速度差は(感覚的には)小さいため, 基本的には最小値クエリの先読みが不要な動的な方がおすすめ.
    // とあるため、現在は別名として定義しておく
    public class LiChaoTree<T> : DynamicLiChaoTree<T> where T : INumber<T>, IShiftOperators<T,int,T>, IBitwiseOperators<T,T,T>
    {
        public LiChaoTree(T xMin, T xMax) : base(xMin, xMax)
        {
        }
    }
}