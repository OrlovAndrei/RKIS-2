using System.Collections.Generic;
namespace yield;
public static class ExpSmoothingTask
{
	public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
	{
		double smoothedValue = null;

		foreach (point in data){
			if(smoothedValue == null){
				smoothedValue = point.OriginalY;
			}else{
				smoothedValue = alpha * point.OriginalY + (1 - alpha) * smoothedValue;
			}
			yield return point.WithExpSmoothedY(smoothedValue.Value);
		}
	}
}
