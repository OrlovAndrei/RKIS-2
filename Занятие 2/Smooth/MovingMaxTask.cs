using System;
using System.Collections.Generic;
using System.Linq;

namespace yield
{
    public static class NewBaseType
    {
        public static IEnumerable<int> MovingMax(this IEnumerable<int> data, int windowWidth)
        {
            LinkedList<int> deque = new LinkedList<int>();
            int index = 0;

            foreach (var value in data)
            {

                if (deque.Count > 0 && deque.First.Value <= index - windowWidth)
                {
                    deque.RemoveFirst();
                }


                while (deque.Count > 0 && data.ElementAt(deque.Last.Value) <= value)
                {
                    deque.RemoveLast();
                }


                deque.AddLast(index);

                if (index >= windowWidth - 1)
                {
                    yield return data.ElementAt(deque.First.Value);
                }

                index++;
            }
        }
        public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
        {
            if (windowWidth <= 0)
                throw new System.ArgumentOutOfRangeException();
            int i = 1;
            LinkedList<double> maxPotentials = new LinkedList<double>();
            Queue<double> windowNumbers = new Queue<double>();
            foreach (DataPoint dataPoint in data)
            {
                if (i <= windowWidth)
                    i++;
                else if (maxPotentials.Count == 0)
                    windowNumbers.Dequeue();
                else if (maxPotentials.First.Value == windowNumbers.Dequeue())
                    maxPotentials.RemoveFirst();
                windowNumbers.Enqueue(dataPoint.OriginalY);
                while (maxPotentials.Count > 0 && maxPotentials.Last.Value <= dataPoint.OriginalY)
                    maxPotentials.RemoveLast();
                maxPotentials.AddLast(dataPoint.OriginalY);
                var newDataPoint = dataPoint.WithMaxY(maxPotentials.First.Value);
                yield return newDataPoint;
            }
        }
    }

    public static class MovingMaxTask : NewBaseType
    {
    }
}