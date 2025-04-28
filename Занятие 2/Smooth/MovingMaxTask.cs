using System;
using System.Collections.Generic;
using System.Linq;
namespace yield;
public static class MovingMaxTask
{
	public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
	{
		if (windowWidth <= 0){
            throw new ArgumentOutOfRangeException(nameof(windowWidth));
		}

        int Index = 0;

        LinkedList<double> potentialMaxValues = new LinkedList<double>();
        Queue<double> slidingWindow = new Queue<double>();

        foreach (var point in data){
            if (Index < windowWidth){
                slidingWindow.Enqueue(point.OriginalY);
                Index++;
            }else{
                if (potentialMaxValues.Count > 0 && potentialMaxValues.First.Value == slidingWindow.Dequeue()){
                    potentialMaxValues.RemoveFirst();
				}
            }

            slidingWindow.Enqueue(point.OriginalY);

            while (potentialMaxValues.Count > 0 && potentialMaxValues.Last.Value <= point.OriginalY){
                potentialMaxValues.RemoveLast();
			}

            potentialMaxValues.AddLast(point.OriginalY);

            var smoothedPoint = point.WithMaxY(potentialMaxValues.First.Value);
            yield return smoothedPoint;
		}
	}
}
