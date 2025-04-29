using System;
using System.Collections.Generic;

namespace LimitedSizeStack
{
    public class ListModel<TItem>
    {
        public List<TItem> Items { get; }
        public int UndoLimit { get; }
        private readonly LimitedSizeStack<Action> undoStack;

        public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit) { }

        public ListModel(List<TItem> items, int undoLimit)
        {
            Items = items;
            UndoLimit = undoLimit;
            undoStack = new LimitedSizeStack<Action>(undoLimit);
        }

        public void AddItem(TItem item)
        {
            Items.Add(item);
            undoStack.Push(() => Items.RemoveAt(Items.Count - 1));
        }

        public void RemoveItem(int index)
        {
            var item = Items[index];
            Items.RemoveAt(index);
            undoStack.Push(() => Items.Insert(index, item));
        }

        public bool CanUndo() => undoStack.Count > 0;

        public void Undo()
        {
            undoStack.Pop()();
        }
    }
}