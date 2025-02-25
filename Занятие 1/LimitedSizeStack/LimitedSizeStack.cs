public class LimitedSizeStack<T>
{
    private readonly T[] _dataBuffer; // Массив для хранения элементов стека
    private int _positionIndex = 0; // Индекс текущей позиции
    private int _elementQuantity = 0; // Количество элементов в стеке

    public LimitedSizeStack(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentException("Capacity must be a positive number.", nameof(capacity));

        _dataBuffer = new T[capacity];
    }

    public void InsertItem(T item)
    {
        if (_dataBuffer.Length == 0)
            return; // Если размер массива равен нулю, операция не выполняется

        _dataBuffer[_positionIndex] = item;
        _positionIndex = (_positionIndex + 1) % _dataBuffer.Length; // Циклический переход к следующей позиции
        if (_elementQuantity < _dataBuffer.Length)
            _elementQuantity++; // Увеличиваем счетчик только если еще есть место
    }

    public T RetrieveItem()
    {
        if (_elementQuantity == 0)
            throw new InvalidOperationException("The stack is empty.");

        _positionIndex = (_dataBuffer.Length + _positionIndex - 1) % _dataBuffer.Length; // Переход к предыдущему элементу
        _elementQuantity--; // Уменьшаем количество элементов
        return _dataBuffer[_positionIndex]; // Возвращаем элемент
    }

    public int GetElementCount => _elementQuantity; // Свойство для получения текущего количества элементов
}
