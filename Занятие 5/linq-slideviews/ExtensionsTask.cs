using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public static class ExtensionsTask
{
    public static double GetMedian(this IEnumerable<double> items)
    {
        if (items == null)
            throw new ArgumentNullException(nameof(items));
        var sortedList = items.OrderBy(x => x).ToList();
        if (sortedList.Count == 0)
        {
            throw new InvalidOperationException("Sequence contains no elements");
        }
        int middleIndex = sortedList.Count / 2;
        if (sortedList.Count % 2 == 1)
        {
            return sortedList[middleIndex];
        }
        else
        {
            return (sortedList[middleIndex - 1] + sortedList[middleIndex]) / 2.0;
        }
    }

    public static IEnumerable<(T First, T Second)> GetBigrams<T>(this IEnumerable<T> items)
    {
        if (items == null)
            throw new ArgumentNullException(nameof(items));
        T previous = default;
        bool hasPrevious = false;
        foreach (var current in items)
        {
            if (hasPrevious)
            {
                yield return (previous, current);
            }
            else
            {
                hasPrevious = true;
            }
            previous = current;
        }
    }
}