using System.Collections.Generic;
using System.Linq;

namespace yield
{
    public static class MovingAverageTask
    {
        public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
        {
            if (windowWidth <= 0)
                throw new System.ArgumentOutOfRangeException();

            var window = new Queue<double>();
            double sum = 0;
            var dataList = data.ToList(); // Преобразование в List для доступа по индексу

            for (int i = 0; i < dataList.Count; i++)
            {
                var dataPoint = dataList[i];
                window.Enqueue(dataPoint.OriginalY);
                sum += dataPoint.OriginalY;

                int count = window.Count;
                double avg = count < windowWidth ? (count == 0 ? 0 : sum / count) : sum / windowWidth;

                yield return new DataPoint(dataPoint.X, dataPoint.OriginalY, avg);


                if (count > windowWidth)
                {
                    sum -= window.Dequeue();
                }
            }
        }
    }

    public class DataPoint
    {
        public double X { get; }
        public double OriginalY { get; }
        public double AvgSmoothedY { get; }

        public DataPoint(double x, double originalY, double avgSmoothedY)
        {
            X = x;
            OriginalY = originalY;
            AvgSmoothedY = avgSmoothedY;
        }
    }
}
