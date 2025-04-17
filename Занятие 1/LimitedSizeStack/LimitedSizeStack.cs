public class LimitedSizeStack<T>
{
    private T[] items; // массив для хранения элементов стека
    private int top = 0; // индекс, указывающий на место, куда будет записан следующий элемент
    private int count = 0; // текущее количество элементов в стеке
    public LimitedSizeStack(int capacity)
    {
        items = new T[capacity];
    }

    public void Push(T item)
    {
        if (items.Length == 0)
            return; // Если размер 0, стек не работает

        items[top] = item; // Записывается элемент в top
        top = (top + 1) % items.Length; // Сдвигается top для цикличности

        if (count < items.Length)
            count++; // Увеличивается количество элементов, но не превышаем размер массива
    }

    public T Pop()
    {
        if (count == 0)
            throw new System.InvalidOperationException("Stack is empty");

        top = (items.Length + top - 1) % items.Length; // Смещается top назад

        count--; // Уменьшение количества элементов

        return items[top]; // Возвращение верхнего элемента
    }

    public int Count => count; // Возвращает текущее количество элементов в стеке.
}