using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace linq_slideviews;

public class ParsingTask
{
        /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
        /// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
        /// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
        public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
        {
            return lines.Skip(1)
                .Select(line =>
                {
                    var parts = line.Split(';');
                    if (parts.Length != 3) return null;
                    if (!int.TryParse(parts[0], out var slideId)) return null;
                    // Check for null or empty strings
                    if (string.IsNullOrEmpty(parts[1]) || string.IsNullOrEmpty(parts[2]))
                    {
                        return null; // Skip the record if title or type is null/empty
                    }
                    return new SlideRecord(slideId, parts[1], parts[2]);
                })
                .Where(record => record != null)
                .ToDictionary(record => record.SlideId, record => record);
        }

        /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
        /// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
        /// Такой словарь можно получить методом ParseSlideRecords</param>
        /// <returns>Список информации о посещениях</returns>
        /// <exception cref="FormatException">Если среди строк есть некорректные</exception>
        public static IEnumerable<VisitRecord> ParseVisitRecords(
            IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
        {
            return lines.Skip(1)
                .Select(line =>
                {
                    var parts = line.Split(';');
                    if (parts.Length != 4) throw new FormatException($"Invalid line format: {line}");
                    if (!int.TryParse(parts[0], out var userId)) throw new FormatException($"Invalid UserId: {line}");
                    if (!int.TryParse(parts[1], out var slideId)) throw new FormatException($"Invalid SlideId: {line}");
                    if (!slides.ContainsKey(slideId)) throw new FormatException($"SlideId {slideId} not found in slides dictionary: {line}");

                    if (!TimeSpan.TryParseExact(parts[2], "hh\\:mm\\:ss", CultureInfo.InvariantCulture, out var time))
                        throw new FormatException($"Invalid Time: {line}");

                    if (!DateTime.TryParseExact(parts[3], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                        throw new FormatException($"Invalid Date: {line}");

                    return new VisitRecord(userId, slideId, time, date);
                })

                .ToList();
        }
}
