using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;
using static Qlibrary.Common;
using static Qlibrary.MathPlus;

namespace Qlibrary
{
    public interface ISqrtBucket<in T, out TQuery>
    {
        void Create(T[] array);
        void IndexUpdate(int left, int right, T value);
        void AllUpdate(T value);
        TQuery IndexQuery(int left, int right);
        TQuery AllQuery();
    }
    
    public class SqrtDecomposition<TBucket, TValue, TQuery> where TBucket : ISqrtBucket<TValue, TQuery>, new()
    {
        private readonly TBucket[] buckets;
        private readonly int bucketSize;

        public SqrtDecomposition(IReadOnlyCollection<TValue> array)
        {
            int size = array.Count;
            bucketSize = (int)Sqrt(size);
            var bucketLength = (int)CeilingLong(size, bucketSize);
            buckets = Make(bucketLength, () => new TBucket());
            foreach (var p in array.Chunk(bucketSize).Zip(buckets))
            {
                p.Second.Create(p.First);
            }
        }

        public void Update(int index, TValue value)
        {
            int bucketIndex = index / bucketSize;
            int innerIndex = index % bucketSize;
            buckets[bucketIndex].IndexUpdate(innerIndex, innerIndex, value);
        }

        public void Update(int l, int r, TValue value)
        {
            int leftBucket = l / bucketSize;
            int leftIndex = l % bucketSize;
            int rightBucket = r / bucketSize;
            int rightIndex = r % bucketSize;

            if (leftBucket == rightBucket)
            {
                buckets[leftBucket].IndexUpdate(leftIndex, rightIndex, value);
                return;
            }

            buckets[leftBucket].IndexUpdate(leftIndex, bucketSize - 1, value);
            for (int i = leftBucket + 1; i <= rightBucket - 1; i++)
            {
                buckets[i].AllUpdate(value);
            }
            buckets[rightBucket].IndexUpdate(0, rightIndex, value);
        }

        public IEnumerable<TQuery> Query(int l, int r)
        {
            int leftBucket = l / bucketSize;
            int leftIndex = l % bucketSize;
            int rightBucket = r / bucketSize;
            int rightIndex = r % bucketSize;

            if (leftBucket == rightBucket)
            {
                yield return buckets[leftBucket].IndexQuery(leftIndex, rightIndex);
                yield break;
            }

            yield return buckets[leftBucket].IndexQuery(leftIndex, bucketSize - 1);
            for (int i = leftBucket + 1; i <= rightBucket - 1; i++)
            {
                yield return buckets[i].AllQuery();
            }
            yield return buckets[rightBucket].IndexQuery(0, rightIndex);
        }
    }
}