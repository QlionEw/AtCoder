using System.Collections.Generic;
using System.Numerics;

namespace Qlibrary
{
    /// <summary>
    /// 添字or/andに関する畳み込み
    /// https://www.mathwills.com/posts/45
    /// </summary>
    public static class SetConvolution
    {
        public static IList<T> Or<T>(IList<T> a, IList<T> b) where T : INumber<T>
        {
            ZetaTransform.SubsetZetaTransform(a);
            ZetaTransform.SubsetZetaTransform(b);
            for (int i = 0; i < a.Count; i++) a[i] *= b[i];
            MobiusTransform.SubsetMobiusTransform(a);
            return a;
        }
        
        public static IList<T> And<T>(IList<T> a, IList<T> b) where T : INumber<T>
        {
            ZetaTransform.SupersetZetaTransform(a);
            ZetaTransform.SupersetZetaTransform(b);
            for (int i = 0; i < a.Count; i++) a[i] *= b[i];
            MobiusTransform.SupersetMobiusTransform(a);
            return a;
        }
    }
}