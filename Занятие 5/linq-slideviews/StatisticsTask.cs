using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class StatisticsTask
{
    public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
    {
        // Фильтруем посещения по типу слайда и группируем по ID пользователя
        var userVisitGroups = visits
            .Where(v => v.SlideType == slideType)
            .GroupBy(v => v.UserId)
            .ToList();

        // Если нет посещений данного типа, возвращаем 0 
        if (!userVisitGroups.Any())
            return 0.0;

        // Вычисляем временные промежутки между последовательными посещениями
        var timeSpans = userVisitGroups
            .SelectMany(group => 
            {
                // Сортируем посещения пользователя по времени
                var orderedVisits = group.OrderBy(v => v.DateTime).ToList();
                // Сопоставляем каждое посещение со следующим и вычисляем разницу во времени
                return orderedVisits.Zip(orderedVisits.Skip(1), (first, second) => 
                    (second.DateTime - first.DateTime).TotalMinutes);
            })
            // Отфильтровываем нереалистичные промежутки
            .Where(time => time >= 1 && time <= 120)
            .ToList();

        // Если нет валидных временных промежутков, возвращаем 0
        if (!timeSpans.Any())
            return 0.0;

        // Вычисляем и возвращаем медиану
        return timeSpans.GetMedian();
    }
}