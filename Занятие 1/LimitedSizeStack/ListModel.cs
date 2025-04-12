
﻿using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class ListModel<TItem>
{
    public enum TypeAction { AddItem, RemoveItem }

    public List<TItem> Items { get; }

    public int UndoLimit { get; }

    private LimitedSizeStack<(TypeAction Action, TItem Item, int Index)> History { get; }

    public ListModel(List<TItem>? items, int undoLimit)
    {
        Items = items ?? new List<TItem>();
        UndoLimit = undoLimit;
        History = new LimitedSizeStack<(TypeAction, TItem, int)>(undoLimit);
    }

    public ListModel(int undoLimit) : this(null, undoLimit) { }

    public void AddItem(TItem item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

        History.Push((TypeAction.AddItem, item, Items.Count));
        Items.Add(item);
    }

    public void RemoveItem(int index)
    {
        if (index < 0 || index >= Items.Count) throw new ArgumentOutOfRangeException(nameof(index));

        History.Push((TypeAction.RemoveItem, Items[index], index));
        Items.RemoveAt(index);
    }

    public bool CanUndo() => History.Count > 0;

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
