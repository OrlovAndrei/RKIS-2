using System.Collections.Generic;

namespace yield;

public static class ExpSmoothingTask
{
	public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
	{
		  double? previousSmoothedValue = null;

        foreach (var point in data)
        {
            if (previousSmoothedValue == null)
            {
                // Если это первое значение, то сглаженное значение равно самому значению
                previousSmoothedValue = point.OriginalY;
            }
            else
            {
                // Применяем формулу экспоненциального сглаживания
                previousSmoothedValue = alpha * point.OriginalY + (1 - alpha) * previousSmoothedValue.Value;
            }

            // Возвращаем новую точку данных с сглаженным значением
            yield return point.WithExpSmoothedY(previousSmoothedValue.Value);
        }
    }
}
