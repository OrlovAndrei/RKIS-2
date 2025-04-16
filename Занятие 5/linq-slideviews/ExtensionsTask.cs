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

        public static double GetMedian(this IEnumerable<double> items)
        {

            var sortedList = items.ToList();
            int count = sortedList.Count;

            if (count == 0)
            {
                throw new InvalidOperationException("Sequence contains no elements");
            }

            sortedList.Sort();

            if (count % 2 == 0)
            {
                return (sortedList[count / 2] + sortedList[(count / 2) - 1]) / 2.0;
            }
            else
            {
                return sortedList[count / 2];
            }
        }
    }
}

namespace linq_slideviews;

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
		throw new NotImplementedException();
	}

	/// <returns>
	/// Возвращает последовательность, состоящую из пар соседних элементов.
	/// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
	/// </returns>
	public static IEnumerable<(T First, T Second)> GetBigrams<T>(this IEnumerable<T> items)
	{
		throw new NotImplementedException();
	}
}