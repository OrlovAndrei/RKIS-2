using System.Collections.Generic;

namespace yield;

public static class ExpSmoothingTask
{
    public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
    {
        double? prev = null;

        foreach (var point in data)
        {
            if (prev == null)
                prev = point.OriginalY;
            else
                prev = alpha * point.OriginalY + (1 - alpha) * prev.Value;

            yield return point.WithExpSmoothedY(prev.Value);
        }
    }
}
