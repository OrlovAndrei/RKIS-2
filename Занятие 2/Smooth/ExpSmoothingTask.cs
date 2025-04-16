using System.Collections.Generic;

namespace yield
{
    public static class ExpSmoothingTask
    {
        public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
        {
            double previousSmoothedY = 0;
            bool isFirst = true;

            foreach (var dataPoint in data)
            {
                double smoothedY;
                if (isFirst)
                {
                    smoothedY = dataPoint.OriginalY;
                    isFirst = false;
                }
                else
                {
                    smoothedY = alpha * dataPoint.OriginalY + (1 - alpha) * previousSmoothedY;
                }
                yield return new DataPoint(dataPoint.X, dataPoint.OriginalY, smoothedY);
                previousSmoothedY = smoothedY;
            }
        }
    }

    public class DataPoint
    {
        public double X { get; }
        public double OriginalY { get; }
        public double SmoothedY { get; } // Переименовано для соответствия тестам.

        public DataPoint(double x, double originalY, double smoothedY)
        {
            X = x;
            OriginalY = originalY;
            SmoothedY = smoothedY;
        }
    }
}
