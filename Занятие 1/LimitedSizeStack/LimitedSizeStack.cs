using System;

namespace LimitedSizeStack
{
    public class LimitedSizeStack<T>
    {
        private T[] items;
        private int top = 0;
        private int count = 0;

        // Constructor that initializes the stack with a given capacity
        public LimitedSizeStack(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than 0.");
            items = new T[capacity];
        }

        // Pushes an item onto the stack
        public void Push(T item)
        {
            if (count == items.Length)
            {
                // If the stack is full, pop the oldest item to make room for the new one
                top = (top + 1) % items.Length;
            }
            else
            {
                // If the stack isn't full, just increment the count
                count++;
            }
            items[(top + count - 1) % items.Length] = item; // Add item in the current available slot
        }

        // Pops an item from the stack
        public T Pop()
        {
            if (count == 0)
                throw new InvalidOperationException("Stack is empty");

            int index = (top + count - 1) % items.Length;
            T item = items[index];
            count--;  // Decrement the count

            return item;
        }

        // Property to get the current count of items in the stack
        public int Count => count;
    }

    // A sample usage of the LimitedSizeStack
    public class Program
    {
        public static void Main()
        {
            var stack = new LimitedSizeStack<int>(3);

            stack.Push(1);
            stack.Push(2);
            stack.Push(3);
            Console.WriteLine($"Count after 3 pushes: {stack.Count}");  // Output: 3

            // At this point, the stack is full (3 items)
            Console.WriteLine($"Popped: {stack.Pop()}");  // Output: 3
            Console.WriteLine($"Count after popping: {stack.Count}");  // Output: 2

            stack.Push(4); // This will overwrite the oldest element, which was 1
            Console.WriteLine($"Popped: {stack.Pop()}");  // Output: 2
            Console.WriteLine($"Popped: {stack.Pop()}");  // Output: 4
            Console.WriteLine($"Count after popping all: {stack.Count}");  // Output: 0
        }
    }
}
