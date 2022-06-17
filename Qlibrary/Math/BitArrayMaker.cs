using System.Collections.Generic;

namespace Qlibrary
{
    public class BitArrayMaker
    {
        public List<int> Integer(long bitValue, long length)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < length; i++)
            {
                if (bitValue % 2 == 1)
                {
                    list.Add(i);
                }

                bitValue /= 2;
            }

            return list;
        }

        public bool[] Boolean(long bitValue, long length)
        {
            bool[] list = new bool[length];
            for (int i = 0; i < length; i++)
            {
                list[i] = bitValue % 2 == 1;
                bitValue /= 2;
            }

            return list;
        }
    }
}