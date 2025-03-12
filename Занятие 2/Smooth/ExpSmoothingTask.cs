using System.Collections.Generic;

namespace yield;

public static class ExpSmoothingTask
{
	public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
	{
		double smoothedValue = null;

		foreach (var point in data){
			if(smoothedValue == null){
				smoothedValue = point.OriginalY; //Если значение первое, сглаживание равно значению
			}else{
				smoothedValue = alpha * point.OriginalY + (1 - alpha) * smoothedValue; //иначе применяем формулу экспонинциального сглаживания
			}
			yield return point.WithExpSmoothedY(smoothedValue.Value); //возвращаем точку со сглаживанием
		}
	}
}
