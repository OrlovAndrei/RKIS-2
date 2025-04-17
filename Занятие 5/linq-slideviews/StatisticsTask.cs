using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
    public class StatisticsTask
    {
        /// <summary>
        /// Вычисляет медианное время просмотра слайдов определённого типа.
        /// </summary>
        /// <param name="visits">Список записей о посещениях.</param>
        /// <param name="slideType">Тип слайдов для вычисления медианного времени просмотра.</param>
        /// <returns>Медианное время просмотра слайдов заданного типа.</returns>
        public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
        {
            var visitDifferences = visits
               .Where(v => v.SlideType == slideType) // Фильтрует записи только для заданного типа слайдов
               .GroupBy(v => v.UserId) // Группирует по UserId
               .SelectMany(g =>
               {
                   var visitList = g.OrderBy(v => v.DateTime).ToList(); // Сортирует записи по времени для каждого пользователя
                   return visitList
                       .Zip(visitList.Skip(1), (first, second) => (second.DateTime - first.DateTime).TotalMinutes); // Вычисляет разницу во времени между соседними по времени записями
               })
               .OrderBy(v => v) // Сортирует разницы во времени
               .ToList();

            if (!visitDifferences.Any()) return 0; // Если список разниц пуст, возвращает 0

            int count = visitDifferences.Count;
            if (count % 2 == 1)
            {
                return visitDifferences[count / 2]; // Если количество элементов нечётное, возвращает средний элемент
            }
            else
            {
                return (visitDifferences[count / 2 - 1] + visitDifferences[count / 2]) / 2.0; // Если количество элементов чётное, возвращает среднее из двух средних элементов
            }
        }
    }
}
