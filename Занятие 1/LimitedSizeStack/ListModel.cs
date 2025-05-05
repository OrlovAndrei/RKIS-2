using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class ListModel<TItem>
{
    // Перечисление возможных действий
    public enum TypeAction { AddItem, RemoveItem }

    // Список элементов
    public List<TItem> Items { get; }

    // Ограничение на количество отменяемых действий
    public int UndoLimit { get; }

    // Стек для хранения истории действий
    private LimitedSizeStack<(TypeAction Action, TItem Item, int Index)> History { get; }

    // Основной конструктор
    public ListModel(List<TItem>? items, int undoLimit)
    {
        Items = items ?? new List<TItem>();
        UndoLimit = undoLimit;
        History = new LimitedSizeStack<(TypeAction, TItem, int)>(undoLimit);
    }

    // Конструктор для пустого списка
    public ListModel(int undoLimit) : this(null, undoLimit) { }

    // Добавление элемента с записью в историю
    public void AddItem(TItem item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

        History.Push((TypeAction.AddItem, item, Items.Count));
        Items.Add(item);
    }

    // Удаление элемента с записью в историю
    public void RemoveItem(int index)
    {
        if (index < 0 || index >= Items.Count) throw new ArgumentOutOfRangeException(nameof(index));

        History.Push((TypeAction.RemoveItem, Items[index], index));
        Items.RemoveAt(index);
    }

    // Проверка, можно ли выполнить отмену
    public bool CanUndo() => History.Count > 0;

    // Отмена последнего действия
    public void Undo()
    {
        if (!CanUndo()) throw new InvalidOperationException("No actions to undo.");

        var (action, item, index) = History.Pop();

        if (action == TypeAction.AddItem)
            Items.RemoveAt(index);
        else
            Items.Insert(index, item);
    }
}
