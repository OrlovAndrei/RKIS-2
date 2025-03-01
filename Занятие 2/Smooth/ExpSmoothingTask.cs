using System.Collections.Generic;

namespace yield
{
public static class ExpSmoothingTask
{
	public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
	{
            var e = data.GetEnumerator();
            double middle;

            if (e.MoveNext())
            {
                middle = e.Current.ExpSmoothedY = e.Current.OriginalY;
	}
}
