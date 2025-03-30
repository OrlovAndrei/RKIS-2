using System.Collections.Generic;

namespace yield;

public static class MovingAverageTask
{
    public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
    {
        var values = new List<double>();

        foreach (var point in data)
        {
            values.Add(point.OriginalY);

            if (values.Count > windowWidth)
            {
                values.RemoveAt(0);
            }

            double sum = 0;
            foreach (var value in values)
            {
                sum += value;
            }

            double average = sum / values.Count;
            yield return point.WithAvgSmoothedY(average);
        }
    }
}