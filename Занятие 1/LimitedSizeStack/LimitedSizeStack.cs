using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
	private readonly int _size; //размер стека
	private readonly LinkedList<T> _items = new LinkedList<T>(); // Двухсвязный список для хранения элементов
	
	// принимает размер стека
	public LimitedSizeStack(int undoLimit)
	{
		if (undoLimit < 0)
            {
                throw new ArgumentException("size must be greater than or equal to 0.");
            }
		_size = undoLimit;
	}

	// Метод для добавления элемента в стек
	public void Push(T item)
	{
		if (_size == 0)
    	{
        	return; // игнор операций если undoLimit равен нулю
    	}

		if (_items.Count == _size)
        {
            _items.RemoveFirst(); // Если стек достиг макс размера, удаляет самый глубокий элемент (первый)
        }
        _items.AddLast(item); // Добавляем новый элемент в конец списка
	}

	// Метод для извлечения элемента из стека
	public T Pop()
	{
		if (_items.Count == 0)
		{
			throw new InvalidOperationException("Stack is empty.");
		}
        T lastItem = _items.Last!.Value; // Получаем последний элемент
        _items.RemoveLast(); // Удаляем его из списка
        return lastItem;
	}

	public int Count => _items.Count;
}
