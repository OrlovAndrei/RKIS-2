using System;
using System.Collections.Generic;
using System.Linq;

namespace yield;

public static class MovingMaxTask
{
	public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
	{
		if (windowWidth <= 0)
            throw new ArgumentException("Ширина окна должна быть больше 0", nameof(windowWidth));

        var deque = new LinkedList<int>(); // Deque для хранения индексов потенциальных максимумов
        var window = new Queue<double>(); // Очередь для хранения значений текущего окна
        int index = 0;

        foreach (var point in data)
        {
            // Удаляем элементы из Deque, которые вышли за пределы окна
            if (deque.Count > 0 && deque.First.Value <= index - windowWidth)
                deque.RemoveFirst();

            // Удаляем элементы из Deque, которые меньше текущего значения
            while (deque.Count > 0 && point.OriginalY >= data.ElementAt(deque.Last.Value).OriginalY)
                deque.RemoveLast();

            // Добавляем текущий индекс в Deque
            deque.AddLast(index);

            // Добавляем текущее значение в окно
            window.Enqueue(point.OriginalY);

            // Если окно превысило размер, удаляем самое старое значение
            if (window.Count > windowWidth)
                window.Dequeue();

            // Текущий максимум — это первый элемент в Deque
            double currentMax = data.ElementAt(deque.First.Value).OriginalY;

            // Возвращаем новую точку данных с обновленным максимумом
            yield return point.WithMaxY(currentMax);

            index++;
        }
    }
}