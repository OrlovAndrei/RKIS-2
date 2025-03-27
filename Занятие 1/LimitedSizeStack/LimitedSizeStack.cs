using System;
using System.Collections.Generic;

namespace LimitedSizeStack
{
    public class LimitedSizeStack<T>
    {
        private readonly int _undoLimit;
        private readonly LinkedList<T> _stack;

        public LimitedSizeStack(int undoLimit)
        {
            if (undoLimit <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(undoLimit), "Undo limit must be greater than zero.");
            }

            _undoLimit = undoLimit;
            _stack = new LinkedList<T>();
        }

        public void Push(T item)
        {
            if (_stack.Count == _undoLimit)
            {
                // Remove the oldest item (the last one in the LinkedList)
                _stack.RemoveLast();
            }
            _stack.AddFirst(item); // Add the new item to the top of the stack
        }

        public T Pop()
        {
            if (_stack.Count == 0)
            {
                throw new InvalidOperationException("The stack is empty.");
            }

            T value = _stack.First.Value; // Get the top item
            _stack.RemoveFirst(); // Remove the top item
            return value; // Return the popped item
        }

        public int Count => _stack.Count; // Return the current count of items in the stack
    }
}
