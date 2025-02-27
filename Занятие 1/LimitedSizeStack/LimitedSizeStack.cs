using System;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
	public LimitedSizeStack(int undoLimit)
	{
        private readonly LinkedList<T> _list = new LinkedList<T>(); // Двусвязный список для хранения элементов
        private readonly Dictionary<T, LinkedListNode<T>> _nodeMap = new Dictionary<T, LinkedListNode<T>>(); // Словарь для быстрого доступа к узлам
        private readonly int _maxSize; // Максимальный размер стека

        public LimitedSizeStack(int maxSize)
        {
            if (maxSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxSize), "Максимальный размер должен быть больше 0");
            _maxSize = maxSize;
        }

	public void Push(T item)
	{
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Элемент не может быть нулевым");

            // Если элемент уже есть в стеке, удаляем его
            if (_nodeMap.ContainsKey(item))
            {
                var node = _nodeMap[item];
                _list.Remove(node);
                _nodeMap.Remove(item);
            }

            // Добавляем элемент в начало списка
            var newNode = _list.AddFirst(item);
            _nodeMap[item] = newNode;

            // Если размер стека превышен, удаляем последний элемент
            if (_list.Count > _maxSize)
            {
                var lastNode = _list.Last;
                _list.RemoveLast();
                _nodeMap.Remove(lastNode.Value);
            }
        }

	public T Pop()
	{
            if (_list.Count == 0)
                throw new InvalidOperationException("Стек пуст");

            // Удаляем элемент из начала списка
            var firstNode = _list.First;
            _list.RemoveFirst();
            _nodeMap.Remove(firstNode.Value);

            return firstNode.Value;
        }

	public int Count => _list.Count;
	}
}
