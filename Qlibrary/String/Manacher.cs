using System;
using System.Collections.Generic;
using static System.Math;

namespace Qlibrary;

public static class Manacher<T>
{
    private static int[] Execute(IReadOnlyList<T> vec)
    {
        var res = new int[vec.Count];
        int i = 0, j = 0;
        while (i < vec.Count)
        {
            while (i - j >= 0 && i + j < vec.Count && vec[i - j].Equals(vec[i + j])) j++;
            res[i] = j;
            int k = 1;
            while (i - k >= 0 && i + k < vec.Count && k + res[i - k] < j)
            {
                res[i + k] = res[i - k];
                k++;
            }
            i += k;
            j -= k;
        }

        return res;
    }

    // 中心軸を固定したときの各軸に対して極大な回文を左から列挙(空文字列を含む)
    public static IEnumerable<(int, int)> GetPalindromes(T[] vec, T invalid)
    {
        List<T> v = new List<T>();
        int n = vec.Length;
        for (int i = 0; i < n - 1; i++)
        {
            v.Add(vec[i]);
            v.Add(invalid);
        }
        v.Add(vec[^1]);
        var man = Execute(v);
        List<(int, int)> ret = new List<(int, int)>();
        for (int i = 0; i < n * 2 - 1; i++)
        {
            if ((i & 1) >= 1)
            {
                int w = man[i] / 2;
                ret.Add(((i + 1) / 2 - w, (i + 1) / 2 + w));
            }
            else
            {
                int w = (man[i] - 1) / 2;
                ret.Add((i / 2 - w, i / 2 + w + 1));
            }
        }
        return ret;
    }

    // ret[r] : s[l, r] が回文である最小の l
    public static IEnumerable<int> LeftmostPalindromes(T[] vec, T invalid)
    {
        int[] v = new int[vec.Length];
        Array.Fill(v, 1);
        foreach (var (l, r) in GetPalindromes(vec, invalid))
        {
            v[r - 1] = Max(v[r - 1], r - l);
        }
        for (int i = vec.Length - 2; i >= 0; i--) v[i] = Max(v[i], v[i + 1] - 2);
        int[] ret = new int[vec.Length];
        for (int i = 0; i < vec.Length; i++) ret[i] = i + 1 - v[i];
        return ret;
    }
}