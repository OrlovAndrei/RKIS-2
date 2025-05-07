using System;

namespace LimitedSizeStack
{
    public class LimitedSizeStack<T>
    {
        private T[] items;
        private int top = 0;
        private int size = 0; // Более понятное имя переменной
        private readonly int capacity;

        public LimitedSizeStack(int undoLimit)
        {
            if (undoLimit <= 0)
                throw new ArgumentOutOfRangeException(nameof(undoLimit), "undoLimit must be greater than zero.");
            capacity = undoLimit;
            items = new T[capacity];
        }

        public void Push(T item)
        {
            items[top] = item;
            top = (top + 1) % capacity;
            if (size < capacity)
            {
                size++;
            }
        }

        public T Pop()
        {
            if (size == 0)
                throw new InvalidOperationException("Stack is empty");

            top = (capacity + top - 1) % capacity;
            T item = items[top]; // Сохраняем значение
            size--;
            return item;       // Возвращаем сохранённое значение
        }

        public int Count => size;
    }
}
