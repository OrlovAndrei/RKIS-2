using System.Collections.Generic;

namespace yield
{
    public static class ExpSmoothingTask
    {
        public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> inputData, double smoothingFactor)
        {
            double previousSmoothedValue = 0;
            bool isInitialValue = true;

            foreach (DataPoint point in inputData)
            {
                if (isInitialValue)
                {
                    previousSmoothedValue = point.OriginalY;
                    isInitialValue = false;
                }
                else
                {
                    previousSmoothedValue = smoothingFactor * point.OriginalY + (1 - smoothingFactor) * previousSmoothedValue;
                }

                var smoothedPoint = point.WithExpSmoothedY(previousSmoothedValue);
                yield return smoothedPoint;
            }
        }
    }

}