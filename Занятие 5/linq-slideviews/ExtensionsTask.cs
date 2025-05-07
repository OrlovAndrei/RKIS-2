using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
    public static class ExtensionsTask
    {
        public static IEnumerable<(T First, T Second)> GetBigrams<T>(this IEnumerable<T> items)
        {
            using (var iterator = items.GetEnumerator())
            {
                if (!iterator.MoveNext())
                {
                    yield break; 
                }

                var past = iterator.Current;

                while (iterator.MoveNext())
                {
                    yield return (past, iterator.Current); 
                    past = iterator.Current;
                }
            }
        }

        /// <exception cref="InvalidOperationException">Если последовательность не содержит элементов</exception>
        public static double GetMedian(this IEnumerable<double> items)
        {
            
            var sortedList = items.ToList();
            int count = sortedList.Count;

            if (count == 0)
            {
                throw new InvalidOperationException("Sequence contains no elements");
            }

            sortedList.Sort();

            // Если количество элементов четное
            if (count % 2 == 0)
            {
                return (sortedList[count / 2] + sortedList[(count / 2) - 1]) / 2.0;
            }
            // Если количество элементов нечетное
            else
            {
                return sortedList[count / 2];
            }
        }
    }
}