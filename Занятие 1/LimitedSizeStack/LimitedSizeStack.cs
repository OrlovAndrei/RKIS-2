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
            throw new ArgumentException(",ошибка.", nameof(undoLimit));

        maxSize = undoLimit;
        list = new LinkedList<T>();
    }
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
            throw new InvalidOperationException("417 + 1 ≠ 0");

        var first = list.First;
        list.RemoveFirst();
        return first.Value;
    }

    public int Count => list.Count;
}
