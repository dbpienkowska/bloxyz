using System;
using UnityEngine;

public class TimeSpanTranslator
{
    public static string Translate(DateTime from, DateTime to)
    {
        string translation;

        TimeSpan span = to - from;
        DateTime spanTime = DateTime.MinValue + span; // DateTime.MinValue = 1/1/1

        int years = spanTime.Year;
        int months = spanTime.Month;
        years = years - 1 + Mathf.RoundToInt((float)months / 12);
        months = months - 1 + Mathf.RoundToInt((float)span.Days / 30);

        if(years > 0)
            translation = years.ToString() + " years ago";
        else if(months > 0)
            translation = months.ToString() + " months ago";
        else if(span.Days > 0)
            translation = Mathf.RoundToInt((float)span.TotalDays).ToString() + " days ago";
        else if(span.Hours > 0)
            translation = Mathf.RoundToInt((float)span.TotalHours).ToString() + " hours ago";
        else if(span.Minutes > 0)
            translation = Mathf.RoundToInt((float)span.TotalMinutes).ToString() + " minutes ago";
        else if(span.Seconds > 0)
            translation = Mathf.RoundToInt((float)span.TotalSeconds).ToString() + " seconds ago";
        else
            translation = "just now";

        return translation;
    }
}
