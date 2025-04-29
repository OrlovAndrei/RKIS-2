using System;
using System.Collections.Generic;

namespace yield
{
    public class DataPoint
    {
        public double Time { get; set; }
        public double Value { get; set; }
    }

    public static class ExpSmoothingTask
    {
        public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
        {
            // Проверка корректности значения alpha
            if (alpha < 0 || alpha > 1)
            {
                throw new ArgumentException("Alpha должно быть в пределах от 0 до 1.");
            }

            // Переменная для хранения предыдущего сглаженного значения
            double? previousSmoothedValue = null;

            foreach (var dataPoint in data)
            {
                // Для первого элемента устанавливаем начальное значение сглаживания
                if (previousSmoothedValue == null)
                {
                    previousSmoothedValue = dataPoint.Value;
                }
                else
                {
                    // Вычисление сглаженного значения с использованием формулы
                    previousSmoothedValue = alpha * dataPoint.Value + (1 - alpha) * previousSmoothedValue;
                }

                // Возвращаем сглаженную точку данных
                yield return new DataPoint
                {
                    Time = dataPoint.Time,
                    Value = previousSmoothedValue.Value
                };
            }
        }
    }

    class Program
    {
        static void Main()
        {
            var data = new[]
            {
                new DataPoint { Time = 1, Value = 10 },
                new DataPoint { Time = 2, Value = 12 },
                new DataPoint { Time = 3, Value = 14 },
                new DataPoint { Time = 4, Value = 13 },
                new DataPoint { Time = 5, Value = 15 }
            };

            var alpha = 0.3;
            var smoothedData = data.SmoothExponentialy(alpha);

            foreach (var dataPoint in smoothedData)
            {
                Console.WriteLine($"Time: {dataPoint.Time}, Smoothed Value: {dataPoint.Value}");
            }
        }
    }
}
