using System.Collections.Generic;
using System.Numerics;

namespace Qlibrary
{
    public static class ZetaTransform
    {
        public static void SupersetZetaTransform<T>(IList<T> f) where T : INumber<T>
        {
            int n = f.Count;
            for (int i = 1; i < n; i <<= 1)
            {
                for (int j = 0; j < n; j++)
                {
                    if ((j & i) != 0) continue;
                    f[j] += f[j | i];
                }
            }
        }

        public static void SubsetZetaTransform<T>(IList<T> f) where T : INumber<T>
        {
            int n = f.Count;
            for (int i = 1; i < n; i <<= 1)
            {
                for (int j = 0; j < n; j++)
                {
                    if ((j & i) != 0) continue;
                    f[j | i] += f[j];
                }
            }
        }
    }
}