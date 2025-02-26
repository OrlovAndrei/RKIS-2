using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class ListModel<TItem>
{
	public enum TypeAction { AddItem, RemoveItem }
	public List<TItem> Items { get; }
	public int UndoLimit;

	private LimitedSizeStack<(TypeAction action, TItem item, int index)> history { get; }
        
	public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
	{
		Items = Items ?? new List<TItem>();
		UndoLimit = undoLimit;
		history = new LimitedSizeStack<(TypeAction, TItem, int)>(undoLimit);
	}

	public void AddItem(TItem item)
	{
		if (item == null) throw new ArgumentNullException(nameof(item));

		history.Push((TypeAction.AddItem, item, Items.Count));
		Items.Add(item)
	}

	public void RemoveItem(int index)
	{
		if (index < 0 || index >= Items.Count) throw new ArgumentOutOfRangeException(nameof(index));

		history.Push((TypeAction.RemoveItem, Items[index], index));
		Items.RemoveAt(index);
	}

	public bool CanUndo()
	{
		return history.Count > 0;
	}

	public void Undo()
	{
        if (!CanUndo()) throw new InvalidOperationException("No action to undo items+");

        var (action, item, index) = history.Pop();

        if (action == TypeAction.AddItem) {
            Items.RemoveAt(index);
		} else
        {
			Items.Insert(index, item);
        }
    }
}