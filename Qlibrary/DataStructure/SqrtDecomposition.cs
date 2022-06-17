using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;
using static Qlibrary.MathPlus;

namespace Qlibrary
{
    public class SqrtDecomposition<T>
    {
        private readonly SqrtBucket[] buckets;
        private readonly int bucketSize;

        public SqrtDecomposition(IReadOnlyList<T> array, bool isBuildSet, Func<T, T, T> indexOperation,
            Func<IEnumerable<T>, T> allOperation)
        {
            var size = array.Count;
            bucketSize = (int)Sqrt(size);
            var bucketLength = (int)CeilingLong(size, bucketSize);
            buckets = Enumerable.Repeat(0, bucketLength)
                .Select(_ => new SqrtBucket(isBuildSet, indexOperation, allOperation)).ToArray();
            for (int i = 0; i < size; i++)
            {
                buckets[i / bucketSize].Create(array[i]);
            }
            foreach (var bucket in buckets)
            {
                bucket.InitializeBucket();
            }
        }

        public void Update(int index, T value)
        {
            int bucketIndex = index / bucketSize;
            int innerIndex = index % bucketSize;
            buckets[bucketIndex].Update(value, innerIndex);
        }

        public IEnumerable<T> QueryValue(int l, int r)
        {
            int leftBucket = l / bucketSize;
            int leftIndex = l % bucketSize;
            int rightBucket = r / bucketSize;
            int rightIndex = r % bucketSize;

            if (leftBucket == rightBucket)
            {
                foreach (var value in buckets[leftBucket].GetValues(leftIndex, rightIndex))
                {
                    yield return value;
                }
                yield break;
            }

            foreach (var value in buckets[leftBucket].GetValues(leftIndex, bucketSize))
            {
                yield return value;
            }
            for (int i = leftBucket + 1; i <= rightBucket - 1; i++)
            {
                yield return buckets[i].Value;
            }
            foreach (var value in buckets[rightBucket].GetValues(0, rightIndex))
            {
                yield return value;
            }
        }
        
        public IEnumerable<Set<T>> QuerySet(int l, int r)
        {
            int leftBucket = l / bucketSize;
            int leftIndex = l % bucketSize;
            int rightBucket = r / bucketSize;
            int rightIndex = r % bucketSize;

            var s = new Set<T>(true);
            if (leftBucket == rightBucket)
            {
                foreach (var value in buckets[leftBucket].GetValues(leftIndex, rightIndex))
                {
                    s.Add(value);
                }
                yield return s;
                yield break;
            }

            foreach (var value in buckets[leftBucket].GetValues(leftIndex, bucketSize))
            {
                s.Add(value);
            }
            foreach (var value in buckets[rightBucket].GetValues(0, rightIndex))
            {
                s.Add(value);
            }
            yield return s;
            for (int i = leftBucket + 1; i <= rightBucket - 1; i++)
            {
                yield return buckets[i].BucketSet;
            }
        }

        class SqrtBucket
        {
            private readonly Func<T, T, T> indexOperation;
            private readonly Func<IEnumerable<T>, T> allOperation;
            public T Value { get; private set; }
            public Set<T> BucketSet { get; private set; }
            private readonly bool isBuildSet;
            private readonly List<T> bucket = new List<T>();

            public SqrtBucket(bool isBuildSet, Func<T, T, T> indexOperation, Func<IEnumerable<T>, T> allOperation)
            {
                this.isBuildSet = isBuildSet;
                this.indexOperation = indexOperation;
                this.allOperation = allOperation;
                if (isBuildSet)
                {
                    BucketSet = new Set<T>(true);
                }
            }

            public void Create(T value)
            {
                bucket.Add(value);
            }
            
            public void InitializeBucket()
            {
                Value = allOperation(bucket);
            }

            public void Update(T value, int index)
            {
                var old = bucket[index];
                bucket[index] = indexOperation(old, value);
                Value = allOperation(bucket);
            }

            public IEnumerable<T> GetValues(int left, int right)
            {
                var max = Math.Min(bucket.Count - 1, right);
                for (int i = left; i <= max; i++)
                {
                    yield return bucket[i];
                }
            }
        }
    }
}