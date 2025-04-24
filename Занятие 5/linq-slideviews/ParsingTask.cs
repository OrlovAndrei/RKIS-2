﻿﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace linq_slideviews
{
    public class ParsingTask
    {
        public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
        {
            return lines
                .Skip(1)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line =>
                {
                    var parts = line.Split(';');
                    if (parts.Length != 3) return null;

                    int slideId;
                    if (!int.TryParse(parts[0], out slideId)) return null;

                    SlideType slideType;
                    if (!Enum.TryParse(parts[2], true, out slideType)) return null;

                    return new SlideRecord(slideId, slideType, parts[1]);
                })
                .Where(record => record != null)
                .ToDictionary(record => record.SlideId);
        }

        public static IEnumerable<VisitRecord> ParseVisitRecords(
            IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
        {
            return lines
                .Skip(1)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line =>
                {
                    var parts = line.Split(';');
                    if (parts.Length != 4) throw new FormatException($"Некорректная строка: {line}"); 

                    int userId;
                    if (!int.TryParse(parts[0], out userId)) throw new FormatException($"Некорректная строка: {line}");

                    int slideId;
                    if (!int.TryParse(parts[1], out slideId)) throw new FormatException($"Некорректная строка: {line}");

                    DateTime time;
                    if (!DateTime.TryParse(parts[2] + " " + parts[3], out time)) throw new FormatException($"Некорректная строка: {line}");

                    if (!slides.ContainsKey(slideId)) throw new FormatException($"Слайд с ID {slideId} не найден.");

                    var slideType = slides[slideId].SlideType;

                    return new VisitRecord(userId, slideId, time, slideType);
                })
                .ToList();
        }
    }
}