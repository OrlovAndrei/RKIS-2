using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class ListModel<TItem>
{
	public List<TItem> Items { get; }
	public int UndoLimit { get; };
        
	// Стек для хранения истории действий
        private readonly LimitedSizeStack<(TypeAction Action, TItem Item, int Index)> _history;

        // Перечисление возможных действий
        private enum TypeAction { AddItem, RemoveItem }

        // Конструктор для пустого списка
        public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
        {
			Items = items ?? throw new ArgumentNullException(nameof(items));
            UndoLimit = undoLimit;
            _history = new LimitedSizeStack<(TypeAction, TItem, int)>(undoLimit);
        }

	public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
	{
	}

	public ListModel(List<TItem> items, int undoLimit)
	{
		Items = items;
		UndoLimit = undoLimit;
	}
	
	public void AddItem(TItem item)
	{
		if (item == null)
                throw new ArgumentNullException(nameof(item));

        // Записываем действие в историю
        _history.Push((TypeAction.AddItem, item, Items.Count));

        // Добавляем элемент в список
        Items.Add(item);
	}

	public void RemoveItem(int index)
	{
		if (index < 0 || index >= Items.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            // Записываем действие в историю
            _history.Push((TypeAction.RemoveItem, Items[index], index));

            // Удаляем элемент из списка
            Items.RemoveAt(index);
	}

	public bool CanUndo()
	{
		return _history.Count > 0;
	}

	public void Undo()
	{
		if (!CanUndo())
             throw new InvalidOperationException("No actions to undo.");

        // Получаем последнее действие из истории
        var (action, item, index) = _history.Pop();

        // Отменяем действие
        if (action == TypeAction.AddItem)
        {
            // Если действие было добавлением, удаляем элемент
            Items.RemoveAt(index);
        }
        else
        {
            // Если действие было удалением, добавляем элемент обратно
            Items.Insert(index, item);
        }
	}
}