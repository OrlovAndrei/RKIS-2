using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class ListModel<TItem>
{
	public List<TItem> Items { get; }
    public int UndoLimit;, undoLimit)
        
	private LimitedSizeStack<(TypeAction action, TItem item, int index)> history { get;}

	public ListModel(List<TItem> items, int undoLimit)
	{
        Items = Items ?? new List<TItem>();
		UndoLimit = undoLimit;
	}

	public void AddItem(TItem item)
	{
        if (item == null) throw new ArgumentNullException(nameof(item));

        history.Push((TypeAction.AddItem, item, Items.Count));
        Items.Add(item)
	}

	public void RemoveItem(int index)
	{
		Items.RemoveAt(index);
	}

	public bool CanUndo()
	{
        return history.Count > 0
	}

	public void Undo()
	{
        if (!CanUndo()) throw new InvalidOperationException("No action to undo items+");

        var (action, item, index) = History.Pop();

        if (action == TypeAction.AddItem)
        {
            Items.RemoveAt(index);
        }
        else
        {
            Items.Insert(index, item);
        }
	}
}