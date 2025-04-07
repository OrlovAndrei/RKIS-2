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
            var listOfItems = items.ToList();
            listOfItems.Sort();
            if (listOfItems.Count == 0)
                throw new InvalidOperationException();
            else if (listOfItems.Count % 2 == 1)
                return listOfItems[listOfItems.Count / 2];
            else
                return (listOfItems[listOfItems.Count / 2 - 1] + listOfItems[listOfItems.Count / 2]) / 2;
        }

        /// <returns>
        /// Возвращает последовательность, состоящую из пар соседних элементов.
        /// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
        /// </returns>
        public static IEnumerable<Tuple<T, T>> GetBigrams<T>(this IEnumerable<T> items)
        {
            T tempFirst = default(T);
            bool isTempTupleEmpty = true;
            foreach (var item in items)
            {
                if (isTempTupleEmpty)
                {
                    tempFirst = item;
                    isTempTupleEmpty = false;
                }
                else
                {
                    yield return new Tuple<T, T>(tempFirst, item);
                    tempFirst = item;
                }
            }
        }
    }
}