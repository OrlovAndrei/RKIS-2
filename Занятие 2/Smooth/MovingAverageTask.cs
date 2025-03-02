using System;
using System.Collections.Generic;

namespace yield;

public static class MovingAverageTask
{
	public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
	{
        if (windowWidth <= 0)
            throw new ArgumentOutOfRangeException(nameof(windowWidth), "Window size must be greater than zero.");

        int currentIndex = 0;

        LinkedList<double> potentialMaxValues = new LinkedList<double>();
        Queue<double> slidingWindow = new Queue<double>();

        foreach (var point in data)
        {
            if (currentIndex < windowWidth)
            {
                slidingWindow.Enqueue(point.OriginalY);
                currentIndex++;
            }
            else
            {
                if (potentialMaxValues.Count > 0 && potentialMaxValues.First.Value == slidingWindow.Dequeue())
                    potentialMaxValues.RemoveFirst();
            }

            slidingWindow.Enqueue(point.OriginalY);

            while (potentialMaxValues.Count > 0 && potentialMaxValues.Last.Value <= point.OriginalY)
                potentialMaxValues.RemoveLast();

            potentialMaxValues.AddLast(point.OriginalY);

            var smoothedPoint = point.WithMaxY(potentialMaxValues.First.Value);
            yield return smoothedPoint;
        }





    }
}
