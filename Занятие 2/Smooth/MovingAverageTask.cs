using System.Collections.Generic;

namespace yield
{
    public static class MovingAverageTask
    {
        public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
        {
            var value = 0.0d;
            var window = new Queue<double>();

            foreach (var point in data)
            {
                if (window.Count >= windowWidth)
                {
                    value -= window.Dequeue();
                }

                window.Enqueue(point.OriginalY);

                value += point.OriginalY;

                var item = point.WithAvgSmoothedY(value / window.Count);

                yield return item;
            }
        }
    }
}