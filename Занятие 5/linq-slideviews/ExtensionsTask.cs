using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
    public static class ExtensionsTask
    {
        /// <summary>
        /// Медиана списка из нечетного количества элементов — это серединный элемент списка после сортировки.
        /// Медиана списка из четного количества элементов — это среднее арифметическое
        /// двух серединных элементов списка после сортировки.
        /// </summary>
        /// <exception cref="InvalidOperationException">Если последовательность не содержит элементов</exception>
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
                // Среднее арифметическое двух средних элементов
                return (sortedItems[count / 2 - 1] + sortedItems[count / 2]) / 2.0;
            }
            else
            {
                // Средний элемент
                return sortedItems[count / 2];
            }
        }

        /// <returns>
        /// Возвращает последовательность, состоящую из пар соседних элементов.
        /// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
        /// </returns>
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
}
