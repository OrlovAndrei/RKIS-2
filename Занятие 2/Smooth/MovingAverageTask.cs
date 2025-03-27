using System;
using System.Collections.Generic;
using System.Linq; // Добавляем это пространство имён для использования метода ElementAt

namespace yield
{
    public static class MovingMaxTask
    {
        public static IEnumerable<int> MovingMax(this IEnumerable<int> data, int windowWidth)
        {
            LinkedList<int> deque = new LinkedList<int>();  // Дек для хранения индексов кандидатов на максимум
            int index = 0;

            foreach (var value in data)
            {
                // Удаляем элементы, которые выходят за пределы окна
                if (deque.Count > 0 && deque.First.Value <= index - windowWidth)
                {
                    deque.RemoveFirst();
                }

                // Удаляем все элементы из конца, которые меньше текущего элемента
                while (deque.Count > 0 && data.ElementAt(deque.Last.Value) <= value)
                {
                    deque.RemoveLast();
                }

                // Добавляем индекс текущего элемента в дек
                deque.AddLast(index);

                // Если окно полностью заполнено, возвращаем максимум
                if (index >= windowWidth - 1)
                {
                    yield return data.ElementAt(deque.First.Value);
                }

                index++;
            }
        }
    }

    // Пример использования метода
    class Program
    {
        static void Main(string[] args)
        {
            var data = new List<int> { 2, 6, 2, 1, 3, 2, 5, 8, 1 };
            var windowWidth = 5;

            var movingMax = data.MovingMax(windowWidth);

            foreach (var max in movingMax)
            {
                Console.WriteLine(max);
            }
        }
    }
}
