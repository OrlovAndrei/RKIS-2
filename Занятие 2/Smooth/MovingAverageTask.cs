using System;
using System.Collections.Generic;

namespace yield;

public static class MovingAverageTask
{
    public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
    {
        if (windowWidth <= 0)
            throw new System.ArgumentOutOfRangeException();
        Queue<double> recentValues = new Queue<double>();
        double totalSum = 0;
        double smoothedValue = 0;

        foreach (var dataPoint in data)
        {
            if (recentValues.Count < windowWidth)
            {
                totalSum += dataPoint.OriginalY;
                smoothedValue = totalSum / (recentValues.Count + 1);
            }
            else
            {
                smoothedValue += (dataPoint.OriginalY - recentValues.Dequeue()) / windowWidth;
            }

            var updatedDataPoint = dataPoint.WithAvgSmoothedY(smoothedValue);
            yield return updatedDataPoint;
            recentValues.Enqueue(dataPoint.OriginalY);
        }





    }
}
