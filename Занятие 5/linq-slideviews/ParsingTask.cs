using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace linq_slideviews
{
    public class SlideRecord
    {
        public int SlideId { get; set; }
        public string UnitTitle { get; set; }
        public string SlideType { get; set; }
    }

    public class VisitRecord
    {
        public int UserId { get; set; }
        public int SlideId { get; set; }
        public DateTime Time { get; set; }
    }

    public class ParsingTask
    {
        /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
        /// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
        /// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
        public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
        {
            return lines
                .Skip(1) // Пропускаем заголовочную строку
                .Select(line => line.Split(';'))
                .Where(parts => parts.Length == 3 && // Проверяем корректность строки
                                int.TryParse(parts[0], out _) &&
                                !string.IsNullOrWhiteSpace(parts[1]) &&
                                !string.IsNullOrWhiteSpace(parts[2]))
                .Select(parts => new SlideRecord
                {
                    SlideId = int.Parse(parts[0]),
                    UnitTitle = parts[1],
                    SlideType = parts[2]
                })
                .ToDictionary(record => record.SlideId);
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
                .Skip(1) // Пропускаем заголовочную строку
                .Select(line => line.Split(';'))
                .Select(parts =>
                {
                    if (parts.Length != 4 ||
                        !int.TryParse(parts[0], out int userId) ||
                        !int.TryParse(parts[1], out int slideId) ||
                        !slides.ContainsKey(slideId) ||
                        !DateTime.TryParseExact(parts[2] + " " + parts[3], "HH:mm:ss dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime time))
                    {
                        throw new FormatException("Некорректная строка данных.");
                    }

                    return new VisitRecord
                    {
                        UserId = userId,
                        SlideId = slideId,
                        Time = time
                    };
                });
        }
    }
}
