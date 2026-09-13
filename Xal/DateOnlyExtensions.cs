using System;
using System.Globalization;

namespace Xal;

public static class DateOnlyExtensions
{
    extension(DateOnly d)
    {
        public int DifferenceInDays(DateOnly other) => Math.Abs(d.DayNumber - other.DayNumber);

        public int DifferenceInMonths(DateOnly other) => Math.Abs(((d.Year - other.Year) * 12) + d.Month - other.Month);

        public int DifferenceInWeeks(DateOnly other) => d.DifferenceInDays(other) / 7;

        public DateOnly EndOfMonth() => d.StartOfMonth().AddMonths(1).AddDays(-1);

        public DateOnly EndOfQuarter() => d.StartOfQuarter().AddMonths(3).AddDays(-1);

        public DateOnly EndOfWeek(CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(culture);
            var diff = (culture.DateTimeFormat.FirstDayOfWeek - d.DayOfWeek + 6 + 7) % 7;
            return d.StartOfWeek(culture).AddDays(6 + diff); // atenção: revisar, ver nota abaixo
        }

        public DateOnly EndOfWeek() => d.EndOfWeek(CultureInfo.CurrentCulture);

        public DateOnly EndOfYear() => d.StartOfYear().AddMonths(12).AddDays(-1);

        public bool IsBetween(DateOnly min, DateOnly max) => d >= min && d <= max;

        public bool IsWeekend() => d.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

        public DateOnly StartOfMonth() => d.AddDays(1 - d.Day);

        public DateOnly StartOfQuarter()
        {
            var quarter = (d.Month - 1) / 3 + 1;
            var month = (quarter - 1) * 3 + 1 - d.Month;
            return d.AddMonths(month).StartOfMonth();
        }

        public DateOnly StartOfWeek(CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(culture);
            var diff = (d.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek + 7) % 7;
            return d.AddDays(-diff);
        }

        public DateOnly StartOfWeek() => d.StartOfWeek(CultureInfo.CurrentCulture);

        public DateOnly StartOfYear() => d.AddDays(1 - d.DayOfYear);
    }
}