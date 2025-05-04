using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
    public static class StatisticsTask
    {
        public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
        {
            if (visits == null || visits.Count == 0)
                return 0;

            var filteredVisits = visits
                .OrderBy(v => v.UserId)
                .ThenBy(v => v.DateTime)
                .ToList();

            var timeDiffs = new List<double>();
            int i = 0;

            while (i < filteredVisits.Count - 1)
            {
                if (filteredVisits[i].UserId == filteredVisits[i + 1].UserId && 
                    filteredVisits[i].SlideType == slideType)
                {
                    timeDiffs.Add((filteredVisits[i + 1].DateTime - filteredVisits[i].DateTime).TotalMinutes);
                }
                i++;
            }

            if (timeDiffs.Count == 0)
                return 0;

            timeDiffs.Sort();
            int middle = timeDiffs.Count / 2;

            if (timeDiffs.Count % 2 == 1)
                return timeDiffs[middle];
            else
                return (timeDiffs[middle - 1] + timeDiffs[middle]) / 2;
        }
    }
}
