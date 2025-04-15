using System;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
    private T[] items;
    private int top = 0;
    private int count = 0;
    public LimitedSizeStack(int undoLimit)
    {
        items = new T[undoLimit];
    }

    public void Push(T item)
    {
        if (items.Length == 0)
            return;
        items[top] = item;
        top = (top + 1) % items.Length;
        if (count < items.Length)
            count++;
    }

    public T Pop()
    {
        if (count == 0)
            throw new System.InvalidOperationException("Stack is empty");
        top = (items.Length + top - 1) % items.Length;
        count--;
        return items[top];
    }

    public int Count => count;
}
