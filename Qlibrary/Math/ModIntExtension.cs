using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Qlibrary
{
    public static class ModIntExtension
    {
        [MethodImpl(256)] 
        public static ModInt Sum(this IEnumerable<ModInt> self) => self.Aggregate((a, b) => a + b);
    }
}