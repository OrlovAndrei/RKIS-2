using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class ParsingTask
{
    public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
    {
        return lines
            .Skip(1)
            .Select(line =>
            {
                var parts = line.Split(';');
                if (parts.Length != 3)
                    return null;

                if (!int.TryParse(parts[0], out int id))
                    return null;

                if (!Enum.TryParse(parts[1], true, out SlideType type))
                    return null;

                return new SlideRecord(id, type, parts[2]);
            })
            .Where(slide => slide != null)
            .ToDictionary(slide => slide.SlideId);
    }

    public static IEnumerable<VisitRecord> ParseVisitRecords(
        IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
    {
        return lines
            .Skip(1)
            .Select(line => {
                var parts = line.Split(';');
                if (parts.Length != 4)
                    throw new FormatException($"Wrong line [{line}]");

                if (!int.TryParse(parts[0], out int userId) ||
                    !int.TryParse(parts[1], out int slideId) ||
                    !slides.ContainsKey(slideId))
                    throw new FormatException($"Wrong line [{line}]");

                if (!DateTime.TryParse($"{parts[3]} {parts[2]}", out DateTime dateTime))
                    throw new FormatException($"Wrong line [{line}]");

                return new VisitRecord(
                    userId,
                    slideId,
                    dateTime,
                    slides[slideId].SlideType);
            });
    }
}