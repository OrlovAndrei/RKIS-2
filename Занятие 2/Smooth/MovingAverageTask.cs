using System;
using System.Collections.Generic;	using System.Collections.Generic;


namespace yield;	namespace yield;


public static class MovingAverageTask	public static class MovingAverageTask
{	{
	public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)		public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
	{	    {
		//Fix me!	        if (data == null)
		return data;	            throw new ArgumentNullException(nameof(data));
	}	        if (windowWidth <= 0)
}	            throw new ArgumentOutOfRangeException(nameof(windowWidth), "Window width must be > 0");

        var queue = new Queue<double>(); // Очередь для хранения элементов текущего окна
        double sum = 0; // Сумма эл в текущем окне

        foreach (var point in data)
        {
            // Добавляем текущее значение в очередь и к сумме
            queue.Enqueue(point.OriginalY);
            sum += point.OriginalY;

            // Если окно превысило заданную ширину удаляем самый старый эл
            if (queue.Count > windowWidth)
                sum -= queue.Dequeue();

            // Вычисляем сред значение для текущего окна
            double average = sum / Math.Min(queue.Count, windowWidth);
            yield return point.WithAvgSmoothedY(average);
        }
    }
}
