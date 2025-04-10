using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace linq_slideviews
{
    public class ParsingTask
    {
        public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
        {
            return lines.Skip(1).Select(line => // Пропускаем заголовочную строку
            {
                var lineParams = line.Split(new[] { ';' }, 3, StringSplitOptions.RemoveEmptyEntries);
                if (lineParams.Length < 3 || !int.TryParse(lineParams[0], out int slideID))
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
            .Where(slideRecord => slideRecord != null)
            .ToDictionary(slideRecord => slideRecord.SlideId);
        }

        public static IEnumerable<VisitRecord> ParseVisitRecords(
            IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
        {
            string format = "yyyy-MM-dd;HH:mm:ss";
            foreach (var line in lines.Skip(1)) // Пропускаем заголовок
            {
                var lineParams = line.Split(new[] { ';' }, 3, StringSplitOptions.RemoveEmptyEntries);
                if (lineParams.Length < 3 ||
                    !int.TryParse(lineParams[0], out int userID) ||
                    !int.TryParse(lineParams[1], out int slideID) ||
                    !slides.TryGetValue(slideID, out SlideRecord slideRecord) ||
                    !DateTime.TryParseExact(lineParams[2], format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
                {
                    throw new FormatException($"Wrong line [{line}]");
                }

                yield return new VisitRecord(userID, slideID, dateTime, slideRecord.SlideType);
            }
        }
    }
}
