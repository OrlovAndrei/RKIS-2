using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class ListModel<TItem>
{
    public List<TItem> Items { get; }
    public int UndoLimit { get; }
    private readonly LimitedSizeStack<Action> _undoStack; //хранение истории действий

    public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
	{	
	}

    public ListModel(List<TItem> items, int undoLimit)
    {
        if (undoLimit <= 0)
        {
            throw new ArgumentException("undoLimit must be greater than 0.");
        }
        Items = items;
        UndoLimit = undoLimit;
        _undoStack = new LimitedSizeStack<Action>(undoLimit); //инициализация стека ограниченного размера
    }

    public void AddItem(TItem item)
    {
        Items.Add(item);
        _undoStack.Push(() => Items.Remove(item)); // Сохранение действия для отмены
    }

    public void RemoveItem(int index)
    {
        if (index < 0 || index >= Items.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        var item = Items[index];
        Items.RemoveAt(index);
        _undoStack.Push(() => Items.Insert(index, item)); // Сохранение действия для отмены
    }

    public bool CanUndo()
    {
        return _undoStack.Count > 0; //проверка есть ли действие для отмены
    }

    public void Undo() //извлечение последнего действие и его удаление
    {
        if (_undoStack.Count == 0) 
        {
            throw new InvalidOperationException("No actions to undo.");
        }
        var undoAction = _undoStack.Pop();
        undoAction();
    }
}