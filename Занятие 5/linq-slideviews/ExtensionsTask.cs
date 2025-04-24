﻿﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
    public static class ExtensionsTask
    {
        public static double GetMedian(this IEnumerable<double> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            var sortedItems = items.OrderBy(x => x).ToList();
            int count = sortedItems.Count;

            if (count == 0)
                throw new InvalidOperationException("Последовательность не содержит элементов");

            if (count % 2 == 0)
            {
                return (sortedItems[count / 2 - 1] + sortedItems[count / 2]) / 2.0;
            }
            else
            {
                return sortedItems[count / 2];
            }
        }


        public static IEnumerable<(T First, T Second)> GetBigrams<T>(this IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            using var enumerator = items.GetEnumerator();

            if (!enumerator.MoveNext())
                yield break;

            T previous = enumerator.Current;

            while (enumerator.MoveNext())
            {
                yield return (previous, enumerator.Current);
                previous = enumerator.Current;
        }
    }
}