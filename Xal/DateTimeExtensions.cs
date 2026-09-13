using System;
using System.Globalization;

namespace Xal;

public static class DateTimeExtensions
{
    extension(DateTime d)
    {
        public int DifferenceInDays(DateTime other) => Math.Abs((d.Date - other.Date).Days);

        public int DifferenceInMonths(DateTime other) => Math.Abs(((d.Year - other.Year) * 12) + d.Month - other.Month);

        public int DifferenceInWeeks(DateTime other) => d.DifferenceInDays(other) / 7;

        public DateTime EndOfDay() => d.Date.AddDays(1).AddTicks(-1);

        public DateTime EndOfMonth() => d.StartOfMonth().AddMonths(1).AddTicks(-1);

        public DateTime EndOfQuarter() => d.StartOfQuarter().AddMonths(3).AddTicks(-1);

        public DateTime EndOfWeek(CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(culture);
            var diff = (culture.DateTimeFormat.FirstDayOfWeek - d.DayOfWeek + 6 + 7) % 7;
            return d.EndOfDay().AddDays(diff);
        }

        public DateTime EndOfWeek() => d.EndOfWeek(CultureInfo.CurrentCulture);

        public DateTime EndOfYear() => d.StartOfYear().AddMonths(12).AddTicks(-1);

        public bool IsBetween(DateTime min, DateTime max) => d >= min && d <= max;

        public bool IsWeekend() => d.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

        public DateTime StartOfDay() => d.Date;

        public DateTime StartOfMonth() => d.Date.AddDays(1 - d.Day);

        public DateTime StartOfQuarter()
        {
            var quarter = (d.Month - 1) / 3 + 1;
            var month = (quarter - 1) * 3 + 1 - d.Month;
            return d.Date.AddMonths(month).StartOfMonth();
        }

        public DateTime StartOfWeek(CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(culture);
            var diff = (d.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek + 7) % 7;
            return d.Date.AddDays(-diff);
        }

        public DateTime StartOfWeek() => d.StartOfWeek(CultureInfo.CurrentCulture);

        public DateTime StartOfYear() => d.Date.AddDays(1 - d.DayOfYear);
    }
}