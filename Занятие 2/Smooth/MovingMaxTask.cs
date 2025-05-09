using System;
using System.Collections.Generic;
using System.Linq;

namespace yield;

public static class MovingMaxTask
{
	public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
    {
        if (windowWidth <= 0)
            throw new ArgumentException("Window width must be positive", nameof(windowWidth));

        var deque = new LinkedList<double>(); // Используем двустороннюю очередьдля хранения потенциальных максимумов
        var window = new Queue<double>(); // Очередь для хранения текущих эл в окне

        foreach (var point in data)
        {
            if (window.Count == windowWidth && window.Dequeue() == deque.First.Value)
                deque.RemoveFirst(); // Если окно заполнено, удаляем самый старый эл; если удалённый эл был максимумом удаляем его из deque

            window.Enqueue(point.OriginalY); // длобавляем новый эл в окно

            while (deque.Count > 0 && deque.Last.Value < point.OriginalY) // Удаляем из deque все эле, к-е < нового эл
                deque.RemoveLast();

            deque.AddLast(point.OriginalY);

            yield return point.WithMaxY(deque.First.Value); // Текущий максимум — это первый эл в deque
        }
    }
}
