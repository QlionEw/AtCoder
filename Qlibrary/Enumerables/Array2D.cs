using System.Collections.Generic;

namespace Qlibrary
{
    public static class Array2D 
    {
        public static T[][] ToJaggedArray<T>(this T[,] self)
        {
            var l1 = self.GetLength(0);
            var l2 = self.GetLength(1);
            var boxes = new T[l1][];
            for (int i = 0; i < l1; i++)
            {
                var box = new T[l2];
                for (int j = 0; j < box.Length; j++)
                {
                    box[j] = self[i, j];
                }
                boxes[i] = box;
            }
            return boxes;
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