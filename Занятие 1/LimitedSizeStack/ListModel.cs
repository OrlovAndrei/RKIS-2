using System;
using System.Collections.Generic;

namespace LimitedSizeStack
{
    public class LimitedSizeStack<T>
    {
        private readonly int _maxSize;
        private readonly Stack<T> _stack;

        public LimitedSizeStack(int maxSize)
        {
            _maxSize = maxSize;
            _stack = new Stack<T>();
        }

        public int Count => _stack.Count;

        public void Push(T item)
        {
            if (_stack.Count >= _maxSize)
            {
                _stack.Pop();  // Remove the oldest item when we exceed the max size
            }
            _stack.Push(item);
        }

        public T Pop()
        {
            if (_stack.Count == 0)
            {
                throw new InvalidOperationException("The stack is empty.");
            }
            return _stack.Pop();
        }

        public T Peek()
        {
            if (_stack.Count == 0)
            {
                throw new InvalidOperationException("The stack is empty.");
            }
            return _stack.Peek();
        }
    }

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

    public class Program
    {
        public static void Main()
        {
            // Initialize ListModel with an undo limit of 3
            var listModel = new ListModel<string>(3);

            // Add some items
            listModel.AddItem("Item1");
            listModel.AddItem("Item2");
            listModel.AddItem("Item3");

            // Remove an item
            listModel.RemoveItem(1);  // Removes "Item2"

            Console.WriteLine("Current items: " + string.Join(", ", listModel.Items));  // "Item1, Item3"

            // Undo last action (removal of "Item2")
            listModel.Undo();
            Console.WriteLine("After undo: " + string.Join(", ", listModel.Items));  // "Item1, Item2, Item3"

            // Add a new item
            listModel.AddItem("Item4");
            Console.WriteLine("After adding Item4: " + string.Join(", ", listModel.Items));  // "Item1, Item2, Item3, Item4"

            // Undo last action (addition of "Item4")
            listModel.Undo();
            Console.WriteLine("After undoing Item4: " + string.Join(", ", listModel.Items));  // "Item1, Item2, Item3"

            // Add more items to trigger stack overflow handling
            listModel.AddItem("Item5");
            listModel.AddItem("Item6");
            listModel.AddItem("Item7");

            Console.WriteLine("After adding more items: " + string.Join(", ", listModel.Items));  // "Item1, Item2, Item3, Item5, Item6, Item7"

            // Undo operations within limit
            listModel.Undo();
            Console.WriteLine("After undoing Item7: " + string.Join(", ", listModel.Items));  // "Item1, Item2, Item3, Item5, Item6"
        }
    }
}
