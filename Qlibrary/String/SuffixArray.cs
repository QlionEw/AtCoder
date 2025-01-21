using System; 
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Qlibrary 
{ 
    public class SuffixArray 
    {
        private readonly int size; 
        private readonly int[] sa;
        public string StringForOutput { get; set; }

        public SuffixArray(ReadOnlySpan<char> str)
        {
            size = str.Length; 
            var c = new int[size + 1]; // Add a null character at the end 
            for (int i = 0; i < size; i++)
            {
                c[i] = str[i];
            }
            sa = Enumerable.Range(0, size).OrderBy(a => c[a]).ThenByDescending(a => a).Prepend(size).ToArray();
            
            var sSize = size + 1;
            var classes = new int[sSize]; 

            for (int len = 1; len < sSize; len <<= 1) 
            { 
                for (int i = 0; i < sSize; i++) 
                { 
                    if (i > 0 && c[sa[i - 1]] == c[sa[i]] && 
                        sa[i - 1] + len < sSize && 
                        c[sa[i - 1] + len / 2] == c[sa[i] + len / 2]) 
                    { 
                        classes[sa[i]] = classes[sa[i - 1]]; 
                    } 
                    else 
                    { 
                        classes[sa[i]] = i; 
                    } 
                } 
 
                int[] counts = Enumerable.Range(0, sSize).ToArray(); 
                List<int> temp = new List<int>(sa); 
                for (int i = 0; i < sSize; i++) 
                { 
                    int s1 = temp[i] - len; 
                    if (s1 >= 0) 
                        sa[counts[classes[s1]]++] = s1; 
                } 
                (classes, c) = (c, classes); 
            } 
        } 
 
        public int Size => size + 1;

        public int this[int k] => sa[k]; 
 
        public override string ToString() 
        {
            Debug.Assert(StringForOutput != null, "[SuffixArray] Please set StringForOutput.");
            var sb = new StringBuilder(); 
            sb.AppendLine("SA\tidx\tstr"); 
            for (int i = 0; i < Size; i++)
            {
                sb.Append($"{i}: \t{sa[i]} \t");
                sb.AppendLine(sa[i] != size ? StringForOutput.Substring(sa[i], size - sa[i]) : "$");
            } 
            sb.AppendLine(); 
            return sb.ToString(); 
        } 
    } 
 
    public class LcpArray 
    {
        public string StringForOutput
        {
            get => SuffixArray.StringForOutput;
            set => SuffixArray.StringForOutput = value;
        }
        public SuffixArray SuffixArray { get; } 
        public int[] Lcp { get; }
        public int[] Rank { get; }
        private readonly int sSize;

        public LcpArray(ReadOnlySpan<char> s) 
        {
            SuffixArray = new SuffixArray(s);
            sSize = SuffixArray.Size;
            Lcp = new int[sSize]; 
            Rank = new int[sSize]; 
 
            for (int i = 0; i < sSize; i++) Rank[SuffixArray[i]] = i; 
 
            int h = 0; 
            for (int i = 0; i < sSize - 1; i++) 
            { 
                if (Rank[i] == 0) continue; 
                int j = SuffixArray[Rank[i] - 1]; 
                if (h > 0) h--; 
                while (Math.Max(i, j) + h < sSize - 1 && s[i + h] == s[j + h]) h++; 
                Lcp[Rank[i] - 1] = h; 
            } 
        } 
 
        public override string ToString() 
        { 
            Debug.Assert(StringForOutput != null, "[LcpArray] Please set StringForOutput.");
            var sb = new StringBuilder(); 
            sb.AppendLine("SA\tidx\tLCP\tstr"); 
            for (int i = 0; i < sSize; i++) 
            { 
                sb.Append($"{i}\t{SuffixArray[i]}\t{Lcp[i]}\t");
                sb.AppendLine(SuffixArray[i] == sSize - 1
                    ? "$"
                    : StringForOutput.Substring(SuffixArray[i], sSize - 1 - SuffixArray[i]));
            } 
            return sb.ToString(); 
        } 
    }
}