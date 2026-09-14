using System;
using System.Globalization;

namespace Xal;

/// <summary>
/// Provides extensions for DateOnly types.
/// </summary>
public static class DateOnlyExtensions
{
    extension(DateOnly d)
    {
        /// <summary>
        /// Returns the absolute difference in days between the two dates.
        /// </summary>
        /// <param name="other">The other date to compare.</param>
        /// <returns>The absolute number of days between the two dates.</returns>
        public int DifferenceInDays(DateOnly other) => Math.Abs(d.DayNumber - other.DayNumber);

        /// <summary>
        /// Returns the absolute difference in months between the two dates.
        /// </summary>
        /// <param name="other">The other date to compare.</param>
        /// <returns>The absolute number of months between the two dates.</returns>
        public int DifferenceInMonths(DateOnly other) => Math.Abs(((d.Year - other.Year) * 12) + d.Month - other.Month);

        /// <summary>
        /// Returns the absolute difference in weeks between the two dates.
        /// </summary>
        /// <param name="other">The other date to compare.</param>
        /// <returns>The absolute number of weeks between the two dates.</returns>
        public int DifferenceInWeeks(DateOnly other) => d.DifferenceInDays(other) / 7;

        /// <summary>
        /// Returns the last day of the month for the current date.
        /// </summary>
        /// <returns>A <see cref="DateOnly"/> representing the last day of the month.</returns>
        public DateOnly EndOfMonth() => d.StartOfMonth().AddMonths(1).AddDays(-1);

        /// <summary>
        /// Returns the last day of the quarter for the current date.
        /// </summary>
        /// <returns>A <see cref="DateOnly"/> representing the last day of the quarter.</returns>
        public DateOnly EndOfQuarter() => d.StartOfQuarter().AddMonths(3).AddDays(-1);

        /// <summary>
        /// Returns the last day of the week for the current date, using the specified culture.
        /// </summary>
        /// <param name="culture">The culture that defines the first day of the week.</param>
        /// <returns>A <see cref="DateOnly"/> representing the last day of the week.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="culture"/> is <c>null</c>.</exception>
        public DateOnly EndOfWeek(CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(culture);
            return d.StartOfWeek().AddDays(6);
        }

        /// <summary>
        /// Returns the last day of the week for the current date, using the current culture.
        /// </summary>
        /// <returns>A <see cref="DateOnly"/> representing the last day of the week.</returns>
        public DateOnly EndOfWeek() => d.EndOfWeek(CultureInfo.CurrentCulture);

        /// <summary>
        /// Returns the last day of the year for the current date.
        /// </summary>
        /// <returns>A <see cref="DateOnly"/> representing the last day of the year.</returns>
        public DateOnly EndOfYear() => d.StartOfYear().AddMonths(12).AddDays(-1);

        /// <summary>
        /// Determines whether the date falls within the inclusive range [<paramref name="min"/>, <paramref name="max"/>].
        /// </summary>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The inclusive upper bound.</param>
        /// <returns><c>true</c> if the date is within the range; otherwise, <c>false</c>.</returns>
        public bool IsBetween(DateOnly min, DateOnly max) => d >= min && d <= max;

        /// <summary>
        /// Determines whether the date falls on a weekend (Saturday or Sunday).
        /// </summary>
        /// <returns><c>true</c> if the date is a weekend; otherwise, <c>false</c>.</returns>
        public bool IsWeekend() => d.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

        /// <summary>
        /// Returns the first day of the month for the current date.
        /// </summary>
        /// <returns>A <see cref="DateOnly"/> representing the first day of the month.</returns>
        public DateOnly StartOfMonth() => d.AddDays(1 - d.Day);

        /// <summary>
        /// Returns the first day of the quarter for the current date.
        /// </summary>
        /// <returns>A <see cref="DateOnly"/> representing the first day of the quarter.</returns>
        public DateOnly StartOfQuarter()
        {
            var quarter = (d.Month - 1) / 3 + 1;
            var month = (quarter - 1) * 3 + 1 - d.Month;
            return d.AddMonths(month).StartOfMonth();
        }

        /// <summary>
        /// Returns the first day of the week for the current date, using the specified culture.
        /// </summary>
        /// <param name="culture">The culture that defines the first day of the week.</param>
        /// <returns>A <see cref="DateOnly"/> representing the first day of the week.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="culture"/> is <c>null</c>.</exception>
        public DateOnly StartOfWeek(CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(culture);
            var diff = (d.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek + 7) % 7;
            return d.AddDays(-diff);
        }

        /// <summary>
        /// Returns the first day of the week for the current date, using the current culture.
        /// </summary>
        /// <returns>A <see cref="DateOnly"/> representing the first day of the week.</returns>
        public DateOnly StartOfWeek() => d.StartOfWeek(CultureInfo.CurrentCulture);

        /// <summary>
        /// Returns the first day of the year for the current date.
        /// </summary>
        /// <returns>A <see cref="DateOnly"/> representing the first day of the year.</returns>
        public DateOnly StartOfYear() => d.AddDays(1 - d.DayOfYear);
    }
}