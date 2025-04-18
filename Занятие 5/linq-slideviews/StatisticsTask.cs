using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
    public static class StatisticsTask
    {
        public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
        {
            if (visits == null || visits.Count == 0)
            {
                Console.WriteLine("Warning: Нет данных о посещениях.");
                return 0;
            }

            var filteredVisits = visits.Where(v => v.SlideType == slideType).ToList();

            if (filteredVisits.Count == 0)
            {
                Console.WriteLine($"Warning: Нет посещений для типа слайда: {slideType}.");
                return 0;
            }

            var timeIntervals = new List<TimeSpan>();

            foreach (var userGroup in filteredVisits.GroupBy(v => v.UserId))
            {
                var userVisits = userGroup.OrderBy(v => v.DateTime).ToList();

                for (int i = 1; i < userVisits.Count; i++)
                {
                    var timeSpan = userVisits[i].DateTime - userVisits[i - 1].DateTime;
                    timeIntervals.Add(timeSpan);
                }
            }

            if (timeIntervals.Count == 0)
            {
                Console.WriteLine("Warning: Не удалось вычислить интервалы времени.");
                return 0;
            }

            timeIntervals.Sort();
            int middleIndex = timeIntervals.Count / 2;

            if (timeIntervals.Count % 2 == 0)
            {
                return (timeIntervals[middleIndex - 1] + timeIntervals[middleIndex]).TotalMinutes / 2;
            }
            else
            {
                return timeIntervals[middleIndex].TotalMinutes;
            }
        }

        public static List<VisitRecord> ParseVisitRecords(List<string> inputLines)
        {
            var visitRecords = new List<VisitRecord>();

            foreach (var line in inputLines)
            {
                var parts = line.Split(';');
                if (parts.Length != 4)
                {
                    Console.WriteLine("Warning: Некорректная строка данных: " + line);
                    continue;
                }

                int userId = int.Parse(parts[0]);
                int slideId = int.Parse(parts[1]);
                TimeSpan time = TimeSpan.Parse(parts[2]);
                DateTime date = DateTime.Parse(parts[3]);

                var visitRecord = new VisitRecord(userId, slideId, date + time, SlideType.Exercise); // Пример типа слайда
                visitRecords.Add(visitRecord);
            }

            return visitRecords;
        }
    }
}

public enum SlideType
{
    Theory,
    Exercise,
    Quiz
}

public record VisitRecord(int UserId, int SlideId, DateTime DateTime, SlideType SlideType);
