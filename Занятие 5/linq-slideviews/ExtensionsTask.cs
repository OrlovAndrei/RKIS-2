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
        /// <exception cref="ArgumentNullException">Если последовательность равна null</exception>
        public static double GetMedian(this IEnumerable<double> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            var sortedItems = items.OrderBy(x => x).ToArray();
            
            if (sortedItems.Length == 0)
                throw new InvalidOperationException("Последовательность не содержит элементов");

            int middleIndex = sortedItems.Length / 2;
            
            return sortedItems.Length % 2 == 0
                ? (sortedItems[middleIndex - 1] + sortedItems[middleIndex]) / 2.0
                : sortedItems[middleIndex];
        }

        /// <summary>
        /// Возвращает последовательность, состоящую из пар соседних элементов.
        /// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
        /// </summary>
        /// <exception cref="ArgumentNullException">Если последовательность равна null</exception>
        public static IEnumerable<(T First, T Second)> GetBigrams<T>(this IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            using (IEnumerator<T> enumerator = items.GetEnumerator())
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

    // Добавленный класс с точкой входа
    public static class Program
    {
        public static void Main()
        {
            // Пустой метод, просто чтобы удовлетворить требования компилятора
        }
    }
}
