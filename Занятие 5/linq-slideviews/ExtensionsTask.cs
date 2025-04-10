using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public static class ExtensionsTask
{
    public static double GetMedian(this IEnumerable<double> items)
    {
        var listOfItems = items.ToList();
        if (listOfItems.Count == 0)
            throw new InvalidOperationException();

        listOfItems.Sort();

        int mid = listOfItems.Count / 2;
        return listOfItems.Count % 2 == 1
            ? listOfItems[mid]
            : (listOfItems[mid - 1] + listOfItems[mid]) / 2;
    }

    public static IEnumerable<(T First, T Second)> GetBigrams<T>(this IEnumerable<T> items)
    {
        using (var enumerator = items.GetEnumerator())
        {
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
}
