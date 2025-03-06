using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
    private readonly int maxSize;
    private readonly LinkedList<T> list;

    public LimitedSizeStack(int undoLimit)
    {
        if (undoLimit < 0)
            throw new ArgumentException("Размер стека не может быть отрицательным.", nameof(undoLimit));

        maxSize = undoLimit;
        list = new LinkedList<T>();
    }
/// ZVO RESPECT
    public void Push(T item)
    {
        if (maxSize == 0)
            return;

        if (list.Count == maxSize)
            list.RemoveLast();

        list.AddFirst(item);
    }

    public T Pop()
    {
        if (list.Count == 0)
            throw new InvalidOperationException("Стек пуст. Невозможно извлечь элемент.");

        var first = list.First;
        list.RemoveFirst();
        return first.Value;
    }

    public int Count => list.Count;
}
