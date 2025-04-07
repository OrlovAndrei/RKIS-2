using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
    public static class ExtensionsTask
    {
        /// <summary>
        /// Вычисляет медиану последовательности чисел.
        /// </summary>
        /// <param name="items">Последовательность чисел.</param>
        /// <returns>Медиана последовательности.</returns>
        public static double GetMedian(this IEnumerable<double> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            var sortedItems = items.OrderBy(x => x).ToList(); // Сортирует элементы по возрастанию
            int count = sortedItems.Count;

            if (count == 0)
                throw new InvalidOperationException("Последовательность не содержит элементов");

            if (count % 2 == 0)
            {
                // Если количество элементов чётное, берёт среднее из двух средних элементов
                return (sortedItems[count / 2 - 1] + sortedItems[count / 2]) / 2.0;
            }
            else
            {
                // Если количество элементов нечётное, берёт средний элемент
                return sortedItems[count / 2];
            }
        }

        /// <summary>
        /// Возвращает последовательность пар элементов из исходной последовательности.
        /// </summary>
        /// <typeparam name="T">Тип элементов в последовательности.</typeparam>
        /// <param name="items">Исходная последовательность элементов.</param>
        /// <returns>Последовательность пар элементов (биграмм).</returns>
        public static IEnumerable<(T First, T Second)> GetBigrams<T>(this IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            using var enumerator = items.GetEnumerator();

            if (!enumerator.MoveNext())
                yield break; // Если последовательность пуста, возвращаем пустую последовательность биграмм

            T previous = enumerator.Current;

            while (enumerator.MoveNext())
            {
                // Формирует биграмму из текущего и предыдущего элементов
                yield return (previous, enumerator.Current);
                previous = enumerator.Current; // Переходит к следующему элементу для формирования следующей биграммы
            }
        }
    }
}
