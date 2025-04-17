using System;
using System.Collections.Generic;

namespace LimitedSizeStack
{
    public class StackItem<T>
    {
        public T Value { get; set; }
        public StackItem<T> Next { get; set; }
    }

    public class LimitedSizeStack<T>
    {
        private readonly int _undoLimit;
        private StackItem<T> head;
        private StackItem<T> tail;
        private int count = 0;

        public LimitedSizeStack(int undoLimit)
        {
            _undoLimit = undoLimit;
        }

        public void Push(T item)
        {
            var newItem = new StackItem<T> { Value = item, Next = null };
            count += 1;
            if (head == null)
                tail = head = newItem;
           else if(_undoLimit > count)
            {
                tail.Next = newItem;
                tail = newItem;
            }
            else
            
                tail.Next = newItem;
                tail = newItem;
                head = head.Next;
                count -= 1;
            }
        }

        public T Pop()
        {
            if (count == 0) throw new InvalidOperationException();
            var result = _list[^1];
            _list.RemoveAt(_list.Count - 1);
            return result;
        }

        public int Count => _list.Count;
    }
}
