using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class ParsingTask
{
    /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
    /// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
    /// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
    public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
    {
        return lines
            .Skip(1)
            .Select(line => line.Split(';'))
            .Where(parts => parts.Length == 3 
                           && int.TryParse(parts[0], out _)
                           && Enum.TryParse(parts[2], true, out SlideType _))
            .Select(parts => new
            {
                Id = int.Parse(parts[0]),
                Title = parts[1],
                Type = (SlideType)Enum.Parse(typeof(SlideType), parts[2], true)
            })
            .ToDictionary(x => x.Id, x => new SlideRecord(x.Id, x.Type, x.Title));
    }

    /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
    /// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
    /// Такой словарь можно получить методом ParseSlideRecords</param>
    /// <returns>Список информации о посещениях</returns>
    /// <exception cref="FormatException">Если среди строк есть некорректные</exception>
    public static IEnumerable<VisitRecord> ParseVisitRecords(
        IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
    {
        return lines
            .Skip(1)
            .Select((line, index) =>
            {
                var parts = line.Split(';');
                if (parts.Length != 4)
                    throw new FormatException($"Wrong line [{line}]");

                if (!int.TryParse(parts[0], out int userId) ||
                    !int.TryParse(parts[1], out int slideId) ||
                    !slides.ContainsKey(slideId) ||
                    !DateTime.TryParse(parts[3], out DateTime date) ||
                    !TimeSpan.TryParse(parts[2], out TimeSpan time))
                {
                    throw new FormatException($"Wrong line [{line}]");
                }

                return new VisitRecord(
                    userId,
                    slideId,
                    date.Date.Add(time),
                    slides[slideId].SlideType);
            });
    }
}