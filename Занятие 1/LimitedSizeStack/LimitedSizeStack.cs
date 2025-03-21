using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
    private readonly int maxSize;
    private readonly LinkedList<T> items;

    public LimitedSizeStack(int undoLimit)
    {
        if (undoLimit < 0)
            throw new ArgumentException("ушибОЧКА", nameof(undoLimit));

        maxSize = undoLimit;
        items = new LinkedList<T>();
    }

    public void Push(T element)
    {
        if (maxSize == 0)
            return;

        if (items.Count == maxSize)
            items.RemoveLast();

        items.AddFirst(element);
    }

    public T Pop()
    {
        if (items.Count == 0)
            throw new InvalidOperationException("Интеллекта столько же, сколько и у Огра из Dota 2, то есть 0");

        var firstElement = items.First;
        items.RemoveFirst();
        return firstElement.Value;
    }

    public int Count => items.Count;
}