using System.Collections.Generic;

namespace Qlibrary
{
    public static class Array2D 
    {
        public static IEnumerable<IEnumerable<T>> ToJaggedArray<T>(this T[,] self)
        {
            var l1 = self.GetLength(0);
            var l2 = self.GetLength(1);
            for (int i = 0; i < l1; i++)
            {
                var box = new T[l2];
                for (int j = 0; j < box.Length; j++)
                {
                    box[j] = self[i, j];
                }
                yield return box;
            }
        }

        public static T[,] ToDimensionalArray<T>(this T[][] self)
        {
            var box = new T[self.Length, self[0].Length];
            for (int i = 0; i < self.Length; i++)
            {
                for (int j = 0; j < self[0].Length; j++)
                {
                    box[i, j] = self[i][j];
                }
            }
            return box;
        }
    }
}