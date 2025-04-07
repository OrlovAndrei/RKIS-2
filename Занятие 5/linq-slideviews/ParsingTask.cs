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
        return lines.Select(line =>
        {
            var lineParams = line.Split(new[] { ';' }, 3, StringSplitOptions.RemoveEmptyEntries);
            if (lineParams.Length < 3 ||
                !int.TryParse(lineParams[0], out int slideID))
                return null;
            switch (lineParams[1])
            {
                case "theory":
                    return new SlideRecord(slideID, SlideType.Theory, lineParams[2]);
                case "quiz":
                    return new SlideRecord(slideID, SlideType.Quiz, lineParams[2]);
                case "exercise":
                    return new SlideRecord(slideID, SlideType.Exercise, lineParams[2]);
                default:
                    return null;
            }
        })
            .Where(slideRecord => slideRecord != null).ToDictionary(slideRecord => slideRecord.SlideId);
    }

    /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
    /// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
    /// Такой словарь можно получить методом ParseSlideRecords</param>
    /// <returns>Список информации о посещениях</returns>
    /// <exception cref="FormatException">Если среди строк есть некорректные</exception>
    public static IEnumerable<VisitRecord> ParseVisitRecords(
        IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
    {
        string format = "yyyy-MM-dd;HH:mm:ss";
        return lines.Skip(1).Select(line =>
        {
            var lineParams = line.Split(new[] { ';' }, 3, StringSplitOptions.RemoveEmptyEntries);
            if (lineParams.Length < 3 ||
                !int.TryParse(lineParams[0], out int userID) ||
                !int.TryParse(lineParams[1], out int slideID) ||
                !slides.TryGetValue(slideID, out SlideRecord slideRecord) ||
                !DateTime.TryParseExact(lineParams[2], format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))

                throw new FormatException($"Wrong line [{line}]");

            return new VisitRecord(userID, slideID, dateTime, slideRecord.SlideType);
        });
    }
}