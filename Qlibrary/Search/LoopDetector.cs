using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    public class LoopDetector
    {
        List<long> loops = new List<long>();
        private int[] loopBox;
        int loopStart;

        public LoopDetector(int size)
        {
            loopBox = new int[size];
            loops.Add(0);
        }

        [MethodImpl(256)]
        public bool SetNext(int to, long value)
        {
            if (loops.Count == 0)
            {
                loops.Add(value);
            }
            else
            {
                loops.Add(value + loops[^1]);
            }
            if (loopBox[to] != 0)
            {
                loopStart = loopBox[to];
                return true;
            }
            
            loopBox[to] = loops.Count;
            return false;
        }

        public long ValueOf(long moveTimes) => Sum(moveTimes) - Sum(moveTimes - 1);
        
        [MethodImpl(256)]
        public long Sum(long moveTimes)
        {
            if (moveTimes < loops.Count)
            {
                return loops[(int)moveTimes];
            }

            long sum = 0;
            moveTimes -= loopStart - 1;
            sum += loops[loopStart - 1];
            long multiples = moveTimes / (loops.Count - loopStart);
            long multiplier = loops[^1] - loops[loopStart - 1];
            sum += multiplier * multiples;
            moveTimes %= (loops.Count - loopStart);
            sum += loops[(int)moveTimes + loopStart - 1] - loops[loopStart - 1];
            return sum;
        }
    }
}