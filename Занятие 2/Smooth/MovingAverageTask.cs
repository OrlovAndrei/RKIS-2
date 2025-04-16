using System.Collections.Generic;
using System.Linq;

namespace yield;

public static class MovingAverageTask
{
	public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
	{
		var queue = new Queue<double>(); // Очередь для хранения значений текущего окна
        foreach (var point in data)
        {
            queue.Enqueue(point.OriginalY); // Добавляем текущее значение в очередь

            // Если размер окна превышает windowWidth, удаляем самое старое значение
            if (queue.Count > windowWidth)
            {
                queue.Dequeue();
            }

            // Вычисляем среднее значение для текущего окна
            double average = queue.Average();

            // Возвращаем новую точку данных с усредненным значением
            yield return point.WithAvgSmoothedY(average);
        }
    }
}