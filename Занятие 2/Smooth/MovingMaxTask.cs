using System;
using System.Collections.Generic;
using System.Linq;

namespace yield;

public static class MovingMaxTask
{
    public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
    {
        if (windowWidth <= 0) throw new ArgumentException("Øèðèíà îêíà äîëæíà áûòü ïîëîæèòåëüíîé.", nameof(windowWidth));

        var window = new LinkedList<double>();
        var queue = new Queue<DataPoint>();

        foreach (var point in data)
        {
            if (queue.Count >= windowWidth)
            {
                var oldPoint = queue.Dequeue();
                if (oldPoint.OriginalY == window.First.Value) window.RemoveFirst();
            }

            while (window.Count > 0 && window.Last.Value < point.OriginalY) window.RemoveLast();

            window.AddLast(point.OriginalY);
            queue.Enqueue(point);

            yield return point.WithMaxY(window.First.Value);
        }
    }
}
