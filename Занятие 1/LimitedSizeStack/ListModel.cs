using System;
using System.Collections.Generic;

namespace LimitedSizeStack
{
    public class ListModel<TItem>
    {
        private readonly Stack<TItem> _history; // Stack to keep track of added items
        public List<TItem> Items { get; }
        public int UndoLimit { get; }

        public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
        {
            _history = new Stack<TItem>();
        }

        public ListModel(List<TItem> items, int undoLimit)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            UndoLimit = undoLimit > 0 ? undoLimit : throw new ArgumentOutOfRangeException(nameof(undoLimit), "Undo limit must be greater than zero.");
            _history = new Stack<TItem>();
        }

        public void AddItem(TItem item)
        {
            if (Items.Count >= UndoLimit)
            {
                // Remove the oldest item (first in the list)
                Items.RemoveAt(0);
            }

            Items.Add(item);
            _history.Push(item); // Track the added item for undo
        }

        public void RemoveItem(int index)
        {
            if (index < 0 || index >= Items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
            }

            // If we are removing an item, we should also remove it from history if it was the last added item
            var removedItem = Items[index];
            Items.RemoveAt(index);

            // Only remove from history if we are removing the most recent item
            if (_history.Count > 0 && EqualityComparer<TItem>.Default.Equals(_history.Peek(), removedItem))
            {
                _history.Pop();
            }
        }

        public bool CanUndo()
        {
            return _history.Count > 0;
        }

        public void Undo()
        {
            if (!CanUndo())
            {
                throw new InvalidOperationException("No actions to undo.");
            }

            TItem lastAdded = _history.Pop();
            Items.Remove(lastAdded); // Remove the last added item from the list
        }
    }
}
