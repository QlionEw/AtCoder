using System.Collections.Generic;
using System.Linq;

namespace Qlibrary
{
    public class BitDpHelper
    {
        public int Digit { get; private set; }
        public int Max { get; private set; }
        
        public BitDpHelper(int digit)
        {
            Digit = digit;
            Max = (1 << digit);
        }

        public (List<int> From, List<int> To) GetTransition(int flag)
        {
            var from = new List<int>();
            var to = new List<int>();
            for (int i = 0; i < Digit; i++)
            {
                if (flag % 2 == 1)
                {
                    from.Add(i);
                }
                else
                {
                    to.Add(i);
                }
                flag /= 2;
            }
            return (from, to);
        }
    }
}