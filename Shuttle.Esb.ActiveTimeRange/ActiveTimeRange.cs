using System;
using System.Globalization;

namespace Shuttle.Esb.ActiveTimeRange;

public class ActiveTimeRange
{
    private readonly int _activeFromHour;
    private readonly int _activeFromMinute;

    private readonly int _activeToHour;
    private readonly int _activeToMinute;

    public ActiveTimeRange(string from, string to)
    {
        var fromTime = string.IsNullOrEmpty(from) ? "*" : from;
        var toTime = string.IsNullOrEmpty(to) ? "*" : to;

        DateTimeOffset dt;

        if (!fromTime.Equals("*"))
        {
            if (!DateTimeOffset.TryParseExact(fromTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                throw new ArgumentException(string.Format(Resources.InvalidActiveFromTime, fromTime));
            }

            _activeFromHour = dt.Hour;
            _activeFromMinute = dt.Minute;
        }
        else
        {
            _activeFromHour = 0;
            _activeFromMinute = 0;
        }

        if (!toTime.Equals("*"))
        {
            if (!DateTimeOffset.TryParseExact(toTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                throw new ArgumentException(string.Format(Resources.InvalidActiveToTime, toTime));
            }

            _activeToHour = dt.Hour;
            _activeToMinute = dt.Minute;
        }
        else
        {
            _activeToHour = 23;
            _activeToMinute = 59;
        }
    }

    public bool Active()
    {
        return Active(DateTimeOffset.Now);
    }

    public bool Active(DateTimeOffset date)
    {
        return
            date >= new DateTimeOffset(date.Date, TimeSpan.Zero).AddHours(_activeFromHour).AddMinutes(_activeFromMinute)
            &&
            date <= new DateTimeOffset(date.Date, TimeSpan.Zero).AddHours(_activeToHour).AddMinutes(_activeToMinute);
    }
}