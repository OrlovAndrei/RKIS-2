﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
    public class StatisticsTask
    {
        public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
        {
            var visitDifferences = visits
               .Where(v => v.SlideType == slideType)
               .GroupBy(v => v.UserId)
               .SelectMany(g =>
               {
                   var visitList = g.OrderBy(v => v.DateTime).ToList();
                   return visitList
                       .Zip(visitList.Skip(1), (first, second) => (second.DateTime - first.DateTime).TotalMinutes);
               })
               .OrderBy(v => v)
               .ToList();

            if (!visitDifferences.Any()) return 0;

            int count = visitDifferences.Count;
            if (count % 2 == 1)
            {
                return visitDifferences[count / 2];
            }
            else
            {
                return (visitDifferences[count / 2 - 1] + visitDifferences[count / 2]) / 2.0;
            }
        }
    }
}