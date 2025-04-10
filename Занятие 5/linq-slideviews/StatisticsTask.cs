﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class StatisticsTask
{
    public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
    {
        List<double> minutes = new List<double>();
        var visitsByUsers = visits.GroupBy(visit => visit.UserId);
        foreach (var visitsByUser in visitsByUsers)
            CalculateMinutesForAllSlidesForOneUser(visitsByUser.OrderBy(visit => visit.DateTime).GetBigrams(), minutes, slideType);
        if (minutes.Count == 0)
            return 0;
        return ExtensionsTask.GetMedian(minutes);
    }

    static void CalculateMinutesForAllSlidesForOneUser
        (IEnumerable<Tuple<VisitRecord, VisitRecord>> bigramsOfVisitsByOneUser, List<double> minutes, SlideType slideType)
    {
        double minutesToAdd = 0;
        foreach (var bigramOfVisits in bigramsOfVisitsByOneUser)
        {
            if (bigramOfVisits.Item1.SlideType == slideType)
            {
                minutesToAdd += (bigramOfVisits.Item2.DateTime - bigramOfVisits.Item1.DateTime).TotalMinutes;
                if (bigramOfVisits.Item1.SlideId != bigramOfVisits.Item2.SlideId)
                {
                    if (minutesToAdd >= 1 && minutesToAdd <= 120)
                        minutes.Add(minutesToAdd);
                    minutesToAdd = 0;
                }
            }
        }
        if (minutesToAdd >= 1 && minutesToAdd <= 120)
            minutes.Add(minutesToAdd);
    }
}