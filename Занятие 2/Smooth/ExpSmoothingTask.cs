using System;
using System.Collections.Generic;

namespace yield;

public static class ExpSmoothingTask
{
	public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));
        if (alpha < 0 || alpha > 1)
            throw new ArgumentOutOfRangeException(nameof(alpha), "Alpha must be in the range [0, 1].");

        double? smoothedValue = null;
        bool isFirstElement = true;

        foreach (var point in data)
        {
            if (isFirstElement)
            {
                // Первый элемент не сглаживается
                smoothedValue = point.OriginalY;
                isFirstElement = false;
            }
            else
            {
                smoothedValue = alpha * point.OriginalY + (1 - alpha) * smoothedValue.Value;
            }

            yield return point.WithExpSmoothedY(smoothedValue.Value);
        }
    }
}