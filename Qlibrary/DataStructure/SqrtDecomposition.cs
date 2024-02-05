using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;
using static Qlibrary.MathPlus;

namespace Qlibrary
{
    public class SqrtDecomposition<TValue, TBucket>
        : SqrtDecomposition<TValue, TBucket, TValue>
    {
        public SqrtDecomposition(
            IReadOnlyList<TValue> array, 
            Func<TValue, TValue, TValue> indexUpdate, 
            Func<List<TValue>, TBucket> allUpdate, 
            Func<TValue, TValue> indexQuery, 
            Func<TBucket, TValue> allQuery) : base(array, indexUpdate, allUpdate, indexQuery, allQuery)
        {
        }
    }
    
    public class SqrtDecomposition<TValue, TBucket, TQuery>
    {
        private readonly SqrtBucket[] buckets;
        private readonly int bucketSize;

        public SqrtDecomposition(IReadOnlyList<TValue> array, 
            Func<TValue, TValue, TValue> indexUpdate, 
            Func<List<TValue>, TBucket> allUpdate,
            Func<TValue, TQuery> indexQuery,
            Func<TBucket, TQuery> allQuery)
        {
            var size = array.Count;
            bucketSize = (int)Sqrt(size);
            var bucketLength = (int)CeilingLong(size, bucketSize);
            buckets = Enumerable.Repeat(0, bucketLength)
                .Select(_ => new SqrtBucket(indexUpdate, allUpdate, indexQuery, allQuery)).ToArray();
            for (int i = 0; i < size; i++)
            {
                buckets[i / bucketSize].Create(array[i]);
            }
            foreach (var bucket in buckets)
            {
                bucket.InitializeBucket();
            }
        }

        public void Update(int index, TValue value)
        {
            int bucketIndex = index / bucketSize;
            int innerIndex = index % bucketSize;
            buckets[bucketIndex].Update(value, innerIndex);
        }

        public IEnumerable<TQuery> Query(int l, int r)
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
                yield return buckets[i].GetAll();
            }
            foreach (var value in buckets[rightBucket].GetValues(0, rightIndex))
            {
                yield return value;
            }
        }

        private class SqrtBucket
        {
            private readonly Func<TValue, TValue, TValue> indexUpdate;
            private readonly Func<List<TValue>, TBucket> allUpdate;
            private readonly Func<TValue, TQuery> indexQuery;
            private readonly Func<TBucket, TQuery> allQuery;
            private readonly List<TValue> list = new();
            private TBucket bucket = default;

            public SqrtBucket(
                Func<TValue, TValue, TValue> indexUpdate, 
                Func<List<TValue>, TBucket> allUpdate,
                Func<TValue, TQuery> indexQuery,
                Func<TBucket, TQuery> allQuery)
            {
                this.indexUpdate = indexUpdate;
                this.allUpdate = allUpdate;
                this.indexQuery = indexQuery;
                this.allQuery = allQuery;
            }

            public void Create(TValue value) => list.Add(value);

            public void InitializeBucket() => bucket = allUpdate(list);

            public void Update(TValue value, int index)
            {
                var old = list[index];
                list[index] = indexUpdate(old, value);
                bucket = allUpdate(list);
            }

            public IEnumerable<TQuery> GetValues(int left, int right)
            {
                var max = Min(list.Count - 1, right);
                for (int i = left; i <= max; i++)
                {
                    yield return indexQuery(list[i]);
                }
            }

            public TQuery GetAll() => allQuery(bucket);
        }
    }
}