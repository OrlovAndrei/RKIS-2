using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
    public static class ExtensionsTask
    {
        public static double GetMedian(this IEnumerable<double> items)
        {
            var sortedItems = items.OrderBy(item => item).ToList();

            if (sortedItems.Count == 0)
                throw new InvalidOperationException("The collection is empty.");

            int count = sortedItems.Count;
            if (count % 2 == 1)
            {
                return sortedItems[count / 2];
            }
            else
            {
                return (sortedItems[(count / 2) - 1] + sortedItems[count / 2]) / 2.0;
            }
        }

        public static IEnumerable<(T First, T Second)> GetBigrams<T>(this IEnumerable<T> items)
        {
            return items.Zip(items.Skip(1), (first, second) => (first, second));
        }
    }
}
