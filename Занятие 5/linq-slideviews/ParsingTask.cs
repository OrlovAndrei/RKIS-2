using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class ParsingTask
{
    public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
    {
        var result = new Dictionary<int, SlideRecord>();

        foreach (var line in lines.Skip(1))
        {
            var parts = line.Split(',');

            if (parts.Length < 3)
                continue;

            try
            {
                int slideId = int.Parse(parts[0].Trim());
                string slideTypeStr = parts[1].Trim();
                string unitTitle = parts[2].Trim();

                if (!Enum.TryParse(slideTypeStr, ignoreCase: true, out SlideType slideType))
                    continue;

                result[slideId] = new SlideRecord(slideId, slideType, unitTitle);
            }
            catch
            {
                continue;
            }
        }

        return result;
    }

    public static IEnumerable<VisitRecord> ParseVisitRecords(
        IEnumerable<string> lines,
        IDictionary<int, SlideRecord> slides)
    {
        var result = new List<VisitRecord>();

        foreach (var line in lines.Skip(1))
        {
            var parts = line.Split(',');

            if (parts.Length < 3)
                throw new FormatException("Некорректная строка: недостаточно данных.");

            try
            {
                int userId = int.Parse(parts[0].Trim());
                int slideId = int.Parse(parts[1].Trim());
                DateTime dateTime = DateTime.Parse(parts[2].Trim());


                if (!slides.TryGetValue(slideId, out var slideRecord))
                    continue;

                result.Add(new VisitRecord(userId, slideId, dateTime, slideRecord.SlideType));
            }
            catch
            {

                continue;
            }
        }

        return result;
    }
}
