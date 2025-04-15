using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
    public class StatisticsTask
    {
        public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
        {
            return visits
                .GroupBy(visit => visit.UserId)
                .SelectMany(group => group
                    .OrderBy(x => x.DateTime)
                    .Select((visit, index) => new { visit, index }) // Добавляем индекс для получения пар
                    .Where(x => x.index < group.Count() - 1) // Убедимся, что индекс не выходит за границы
                    .Select(x => (x.visit, group.ElementAt(x.index + 1))) // Создаем кортеж
                )
                .Select(bigram => GetTime(bigram, slideType))
                .Where(time => time >= 1 && time <= 120)
                .DefaultIfEmpty(0)
                .GetMedian(); // Используем метод GetMedian для получения медианы
        }

        private static double GetTime((VisitRecord First, VisitRecord Second) visits, SlideType slideType)
        {
            // Проверяем, что оба визита принадлежат одному пользователю и соответствуют типу слайда
            if (visits.First.UserId.Equals(visits.Second.UserId) && visits.First.SlideType == slideType)
            {
                return visits.Second.DateTime.Subtract(visits.First.DateTime).TotalMinutes;
            }
            return 0; // Если условия не выполнены, возвращаем 0
        }
    }
}