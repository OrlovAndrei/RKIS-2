using System.Collections.Generic;

namespace yield
{
    public static class MovingAverageTask
    {
        public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
        {
            if (windowWidth <= 0)
                throw new System.ArgumentOutOfRangeException();

            Queue<double> lastYs = new Queue<double>();
            double sum = 0;

            foreach (var dataPoint in data)
            {
                lastYs.Enqueue(dataPoint.OriginalY);
                sum += dataPoint.OriginalY;

                if (lastYs.Count > windowWidth)
                {
                    sum -= lastYs.Dequeue();
                }

                int count = lastYs.Count;
                if (count < windowWidth)
                {
                    count = lastYs.Count;
                }

                double result = sum / count;

                var newDataPoint = dataPoint.WithAvgSmoothedY(result);
                yield return newDataPoint;
            }
        }
    }
}