using System;
using System.Collections;
using System.Collections.Generic;

namespace hashes
{
	// TODO: Создайте класс ReadonlyBytes
    // класс ReadonlyBytes это обёртка над массивом байтов, к-я позволяет сравнивать массивы по содержимому, а не по ссылкам
    public class ReadonlyBytes : IEnumerable<byte>
    {
        // хранение массива байтов
        private readonly byte[] _bytes;

        // Поле для хранения предвычисленного хэш-кода
        private readonly int _hashCode;

        // Если массив равен null, выбрасывается исключение ArgumentNullException
        public ReadonlyBytes(params byte[] bytes)
        {
            _bytes = bytes ?? throw new ArgumentNullException(nameof(bytes));
            // Вычисляется хэш-код один раз при создании объекта для повышения производительности
            _hashCode = CalculateHashCode(_bytes);
        }

        // возвращение длинф массива байтов
        public int Length => _bytes.Length;

        // Индексатор для доступа к элементам массива по индексу
        public byte this[int index] => _bytes[index];

        // Переопределение метода Equals для сравнения объектов по содержимому массива байтов
        public override bool Equals(object obj)
        {
            // Если объект равен null или имеет др тип, возвращаем false
            if (obj is null || GetType() != obj.GetType())
                return false;

            // Приводим объект к типу ReadonlyBytes.
            var other = (ReadonlyBytes)obj;

            // Сравниваем массивы байтов с помощью AsSpan().SequenceEqual
            return _bytes.AsSpan().SequenceEqual(other._bytes);
        }

        // Переопределение метода GetHashCode для возврата предвычисленного хэш-кода
        public override int GetHashCode() => _hashCode;

        // Метод для вычисления хэш-кода массива байтов с использованием FNV
        private static int CalculateHashCode(byte[] bytes)
        {
            // Используем unchecked для подавления проверки переполнения
            unchecked
            {
                // Начальное значение хэш-кода
                int hash = (int)2166136261;

                // Проходим по каждому байту в массиве и обновляем хэш-код.
                foreach (var b in bytes)
                    hash = (hash ^ b) * 16777619; 

                // Возвращаем вычисленный хэш-код
                return hash;
            }
        }

        // Переопределение метода ToString для красивого вывода массива байтов
        public override string ToString() => $"[{string.Join(", ", _bytes)}]";

        // Реализация интерфейса IEnumerable<byte> и IEnumerable для поддержки foreach
        public IEnumerator<byte> GetEnumerator() => ((IEnumerable<byte>)_bytes).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _bytes.GetEnumerator();
    }
}