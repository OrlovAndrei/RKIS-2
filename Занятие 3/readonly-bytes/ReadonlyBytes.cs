using System;
using System.Collections;
using System.Collections.Generic;

namespace hashes
{
    // Класс, представляющий неизменяемый список байтов
    public class ReadonlyBytes : IReadOnlyList<byte>
    {
        // Поле для хранения массива байтов
        readonly byte[] array;
        // Кешированное значение хэш-кода
        int hash;

        // Конструктор принимает массив байтов и создает его копию
        public ReadonlyBytes(params byte[] array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array)); // Проверка на null
            
            this.array = new byte[array.Length];
            for (int i = 0; i < array.Length; i++)
                this.array[i] = array[i]; // Копирование массива
            
            hash = CalculateHashCode(); // Вычисление хэш-кода при создании объекта
        }

        // Индексатор для доступа к элементам массива с обработкой исключений
        public byte this[int index]
        {
            get
            {
                try
                {
                    return ((IReadOnlyList<byte>)array)[index]; // Безопасный доступ к элементу
                }
                catch (Exception e)
                {
                    throw new IndexOutOfRangeException($"Ошибка доступа к индексу: {index}", e);
                }
            }
        }

        // Количество элементов в массиве (реализация IReadOnlyCollection<byte>)
        int IReadOnlyCollection<byte>.Count => ((IReadOnlyList<byte>)array).Count;
        // Дополнительное свойство для получения длины массива
        public int Length => ((IReadOnlyList<byte>)array).Count;

        // Метод сравнения двух объектов ReadonlyBytes
        bool Equals(ReadonlyBytes other)
        {
            if (other == null || Length != other.Length)
                return false;
            
            for (int i = 0; i < Length; i++)
                if (this[i] != other[i])
                    return false;
            
            return true;
        }

        // Переопределенный метод Equals для сравнения объектов
        public override bool Equals(object other)
        {
            if (other == null || GetType() != other.GetType())
                return false;
            return this.Equals((ReadonlyBytes)other);
        }

        // Метод вычисления хэш-кода (используется при создании объекта)
        int CalculateHashCode()
        {
            int hashCode = -985847861;
            if (array != null)
                foreach (byte number in array)
                    hashCode = unchecked(hashCode * -1521134295 + number.GetHashCode()); // Хэширование каждого элемента
            return hashCode;
        }

        // Переопределенный метод GetHashCode, возвращающий кешированное значение
        public override int GetHashCode() => hash;

        // Переопределенный метод ToString для удобного вывода массива в виде строки
        public override string ToString()
        {
            string output = "[";
            if (array.Length > 0)
            {
                foreach (byte number in array)
                    output += number + ", ";
                output = output.Remove(output.Length - 2); // Убираем последнюю запятую
            }
            return output += "]";
        }

        // Реализация интерфейса IEnumerable<byte>, позволяющая перебирать элементы массива
        public IEnumerator<byte> GetEnumerator()
        {
            return ((IReadOnlyList<byte>)array).GetEnumerator();
        }

        // Реализация интерфейса IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
