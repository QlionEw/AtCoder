using System.Collections.Generic;

namespace Qlibrary
{
    /// <summary> 完全順列（攪乱順列）</summary>
    /// <remarks>
    /// 1, 2, 3, …, n を要素とする順列において、i 番目 (i ≦ n) が i でない順列。
    /// 完全順列の総数をモンモール数 (英: Montmort number) という。
    /// </remarks>
    public static class Montmort
    {
        public static ModInt Get(int n)
        {
            if(n == 1) return 0;
            if(n == 2) return 1;
            ModInt a1 = 0;
            ModInt a2 = 1;
            for (int i = 3; i <= n; i++)
            {
                var a3 = (i - 1) * (a1 + a2);
                a1 = a2;
                a2 = a3;
            }
            return a2;
        }
        
        public static IEnumerable<ModInt> All()
        {
            yield return 0;
            yield return 1;
            ModInt a1 = 0;
            ModInt a2 = 1;
            int i = 3;
            while (true)
            {
                var a3 = (i - 1) * (a1 + a2);
                yield return a3;
                a1 = a2;
                a2 = a3;
                i++;
            }
        }
    }
}