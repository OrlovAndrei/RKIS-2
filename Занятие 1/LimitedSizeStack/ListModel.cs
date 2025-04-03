using System; 
using System.Collections.Generic;

namespace LimitedSizeStack
{
    public class ListModel<TItem>
    {
        // Перечисление возможных операций
        public enum ActionEnum { AddItem, RemoveItem }

        // Коллекция элементов
        private readonly List<TItem> _itemCollection;

        // Лимит на количество отменяемых операций
        private readonly int _undoCapacity;

        // Стек для хранения истории операций
        private readonly LimitedSizeStack<(ActionEnum ActionType, TItem ItemValue, int Position)> _actionHistory;

        // Основной конструктор
        public ListModel(List<TItem>? initialItems, int undoLimit)
        {
            if (undoLimit <= 0)
                throw new ArgumentException("Undo limit must be greater than zero.", nameof(undoLimit));

            _itemCollection = initialItems ?? new List<TItem>();
            _undoCapacity = undoLimit;
            _actionHistory = new LimitedSizeStack<(ActionEnum, TItem, int)>(undoLimit);
        }

        // Конструктор для пустого списка
        public ListModel(int undoLimit) : this(null, undoLimit) { }

        // Добавление элемента с записью в историю
        public void AddItem(TItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _actionHistory.Push((ActionEnum.AddItem, item, _itemCollection.Count));
            _itemCollection.Add(item);
        }

        // Удаление элемента с записью в историю
        public void RemoveItem(int index)
        {
            if (index < 0 || index >= _itemCollection.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            var removedItem = _itemCollection[index];
            _actionHistory.Push((ActionEnum.RemoveItem, removedItem, index));
            _itemCollection.RemoveAt(index);
        }

        // Проверка возможности отмены последней операции
        public bool CanUndo() => _actionHistory.Count > 0;

        // Отмена последней операции
        public void Undo()
        {
            if (!CanUndo())
                throw new InvalidOperationException("No actions to undo.");

            var (actionType, item, position) = _actionHistory.Pop();

            switch (actionType)
            {
                case ActionEnum.AddItem:
                    _itemCollection.RemoveAt(position);
                    break;
                case ActionEnum.RemoveItem:
                    _itemCollection.Insert(position, item);
                    break;
            }
        }

        // Свойство для доступа к коллекции элементов
        public IReadOnlyList<TItem> Items => _itemCollection.AsReadOnly();
    }
}
