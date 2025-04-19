using System;
using System.Collections.Generic;
using System.Linq;

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
		var list = items.ToList();

		if (list.Count == 0)
		{
			throw new InvalidOperationException("Последовательность не содержит элементов.");
		}

		list.Sort();
		var count = list.Count;
		var mid = count / 2;

		if (count % 2 == 0)
		{
			return (list[mid - 1] + list[mid]) / 2.0;
		}
		else
		{
			return list[mid];
		}
	}

	/// <returns>
	/// Возвращает последовательность, состоящую из пар соседних элементов.
	/// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
	/// </returns>
	public static IEnumerable<(T First, T Second)> GetBigrams<T>(this IEnumerable<T> items)
	{
		using (var iterator = items.GetEnumerator())
		{
			if (!iterator.MoveNext())
			{
				yield break;
			}

			var first = iterator.Current;

			while (iterator.MoveNext())
			{
				var second = iterator.Current;
				yield return (first, second);
				first = second;
			}
		}
	}
}
