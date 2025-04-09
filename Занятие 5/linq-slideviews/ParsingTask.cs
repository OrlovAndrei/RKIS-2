using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace linq_slideviews
{
    public enum SlideType
    {
        Exercise,
        Quiz,
        Theory
    }

    public class SlideRecord
    {
        public int SlideId { get; }
        public SlideType SlideType { get; }
        public string Title { get; }

        public SlideRecord(int slideId, SlideType slideType, string title)
        {
            SlideId = slideId;
            SlideType = slideType;
            Title = title;
        }
    }

    public class VisitRecord
    {
        public int UserId { get; }
        public int SlideId { get; }
        public DateTime DateTime { get; }
        public SlideType SlideType { get; }

        public VisitRecord(int userId, int slideId, DateTime dateTime, SlideType slideType)
        {
            UserId = userId;
            SlideId = slideId;
            DateTime = dateTime;
            SlideType = slideType;
        }
    }

    public class ParsingTask
    {
        /// <summary>
        /// Парсит информацию о слайдах из строк файла.
        /// </summary>
        public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
        {
            if (lines == null)
                throw new ArgumentNullException(nameof(lines));

            return lines
                .Skip(1)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(ParseSlideLine)
                .Where(record => record != null)
                .ToDictionary(record => record.SlideId);
        }

        private static SlideRecord ParseSlideLine(string line)
        {
            var parts = line.Split(';');
            if (parts.Length != 3) 
                return null;

            if (!int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int slideId))
                return null;

            if (!Enum.TryParse(parts[2], true, out SlideType slideType))
                return null;

            return new SlideRecord(slideId, slideType, parts[1]);
        }

        /// <summary>
        /// Парсит информацию о посещениях из строк файла.
        /// </summary>
        public static IEnumerable<VisitRecord> ParseVisitRecords(
            IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
        {
            if (lines == null)
                throw new ArgumentNullException(nameof(lines));
            if (slides == null)
                throw new ArgumentNullException(nameof(slides));

            return lines
                .Skip(1)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => ParseVisitLine(line, slides))
                .ToList();
        }

        private static VisitRecord ParseVisitLine(string line, IDictionary<int, SlideRecord> slides)
        {
            var parts = line.Split(';');
            if (parts.Length != 4)
                throw new FormatException($"Некорректная строка: {line}");

            if (!int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int userId))
                throw new FormatException($"Некорректный ID пользователя в строке: {line}");

            if (!int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int slideId))
                throw new FormatException($"Некорректный ID слайда в строке: {line}");

            if (!DateTime.TryParseExact(
                $"{parts[2]} {parts[3]}", 
                "yyyy-MM-dd HH:mm:ss", 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None, 
                out DateTime time))
            {
                throw new FormatException($"Некорректная дата/время в строке: {line}");
            }

            if (!slides.TryGetValue(slideId, out SlideRecord slide))
                throw new FormatException($"Слайд с ID {slideId} не найден.");

            return new VisitRecord(userId, slideId, time, slide.SlideType);
        }
    }
}
