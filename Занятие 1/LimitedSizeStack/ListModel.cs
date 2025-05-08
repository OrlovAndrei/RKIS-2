using System;
using System.Collections.Generic;

namespace LimitedSizeStack
{
    public class ListModel<TItem>
    {
        public enum TypeAction
        {
            AddItem,
            RemoveItem
        }

        public List<TItem> Items { get; private set; }
        public int UndoLimit { get; private set; }
        private LimitedSizeStack<Tuple<TypeAction, TItem, int>> StoryAction { get; set; }

        public ListModel(int undoLimit)
        {
            Items = new List<TItem>();
            UndoLimit = undoLimit;
            StoryAction = new LimitedSizeStack<Tuple<TypeAction, TItem, int>>(undoLimit);
        }

        public ListModel(List<TItem> items, int undoLimit)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            UndoLimit = undoLimit;
            StoryAction = new LimitedSizeStack<Tuple<TypeAction, TItem, int>>(undoLimit);
        }

        public void AddItem(TItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            StoryAction.Push(Tuple.Create(TypeAction.AddItem, item, Items.Count));
            Items.Add(item);
        }

        public void RemoveItem(int index)
        {
            if (index < 0 || index >= Items.Count)
            {
                throw new IndexOutOfRangeException("Index is out of range.");
            }

            StoryAction.Push(Tuple.Create(TypeAction.RemoveItem, Items[index], index));
            Items.RemoveAt(index);
        }

        public bool CanUndo()
        {
            return StoryAction.Count > 0;
        }

        public void Undo()
        {
            if (!CanUndo())
            {
                throw new InvalidOperationException("No actions to undo.");
            }

            var lastAction = StoryAction.Pop();
            switch (lastAction.Item1)
            {
                case TypeAction.AddItem:
                    Items.RemoveAt(lastAction.Item3);
                    break;
                case TypeAction.RemoveItem:
                    Items.Insert(lastAction.Item3, lastAction.Item2);
                    break;

            }
        }
    }
}
