using System;
using System.Globalization;

namespace Xal;

/// <summary>
/// Provides extensions for DateTime types.
/// </summary>
public static class DateTimeExtensions
{
    extension(DateTime d)
    {
        /// <summary>
        /// Returns the absolute difference in days between the two dates, ignoring the time component.
        /// </summary>
        /// <param name="other">The other date to compare.</param>
        /// <returns>The absolute number of days between the two dates.</returns>
        public int DifferenceInDays(DateTime other) => Math.Abs((d.Date - other.Date).Days);

        /// <summary>
        /// Returns the absolute difference in months between the two dates.
        /// </summary>
        /// <param name="other">The other date to compare.</param>
        /// <returns>The absolute number of months between the two dates.</returns>
        public int DifferenceInMonths(DateTime other) => Math.Abs(((d.Year - other.Year) * 12) + d.Month - other.Month);

        /// <summary>
        /// Returns the absolute difference in weeks between the two dates.
        /// </summary>
        /// <param name="other">The other date to compare.</param>
        /// <returns>The absolute number of weeks between the two dates.</returns>
        public int DifferenceInWeeks(DateTime other) => d.DifferenceInDays(other) / 7;

        /// <summary>
        /// Returns the last instant of the day (23:59:59.9999999).
        /// </summary>
        /// <returns>A <see cref="DateTime"/> representing the end of the day.</returns>
        public DateTime EndOfDay() => d.Date.AddDays(1).AddTicks(-1);

        /// <summary>
        /// Returns the last instant of the month.
        /// </summary>
        /// <returns>A <see cref="DateTime"/> representing the end of the month.</returns>
        public DateTime EndOfMonth() => d.StartOfMonth().AddMonths(1).AddTicks(-1);

        /// <summary>
        /// Returns the last instant of the quarter.
        /// </summary>
        /// <returns>A <see cref="DateTime"/> representing the end of the quarter.</returns>
        public DateTime EndOfQuarter() => d.StartOfQuarter().AddMonths(3).AddTicks(-1);

        /// <summary>
        /// Returns the last instant of the week, using the specified culture.
        /// </summary>
        /// <param name="culture">The culture that defines the first day of the week.</param>
        /// <returns>A <see cref="DateTime"/> representing the end of the week.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="culture"/> is <c>null</c>.</exception>
        public DateTime EndOfWeek(CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(culture);
            return d.StartOfWeek().AddDays(6).EndOfDay();
        }

        /// <summary>
        /// Returns the last instant of the week, using the current culture.
        /// </summary>
        /// <returns>A <see cref="DateTime"/> representing the end of the week.</returns>
        public DateTime EndOfWeek() => d.EndOfWeek(CultureInfo.CurrentCulture);

        /// <summary>
        /// Returns the last instant of the year.
        /// </summary>
        /// <returns>A <see cref="DateTime"/> representing the end of the year.</returns>
        public DateTime EndOfYear() => d.StartOfYear().AddMonths(12).AddTicks(-1);

        /// <summary>
        /// Determines whether the date falls within the inclusive range [<paramref name="min"/>, <paramref name="max"/>].
        /// </summary>
        /// <param name="min">The inclusive lower bound.</param>
        /// <param name="max">The inclusive upper bound.</param>
        /// <returns><c>true</c> if the date is within the range; otherwise, <c>false</c>.</returns>
        public bool IsBetween(DateTime min, DateTime max) => d >= min && d <= max;

        /// <summary>
        /// Determines whether the date falls on a weekend (Saturday or Sunday).
        /// </summary>
        /// <returns><c>true</c> if the date is a weekend; otherwise, <c>false</c>.</returns>
        public bool IsWeekend() => d.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

        /// <summary>
        /// Returns the first instant of the day (00:00:00.0000000).
        /// </summary>
        /// <returns>A <see cref="DateTime"/> representing the start of the day.</returns>
        public DateTime StartOfDay() => d.Date;

        /// <summary>
        /// Returns the first instant of the month.
        /// </summary>
        /// <returns>A <see cref="DateTime"/> representing the start of the month.</returns>
        public DateTime StartOfMonth() => d.Date.AddDays(1 - d.Day);

        /// <summary>
        /// Returns the first instant of the quarter.
        /// </summary>
        /// <returns>A <see cref="DateTime"/> representing the start of the quarter.</returns>
        public DateTime StartOfQuarter()
        {
            var quarter = (d.Month - 1) / 3 + 1;
            var month = (quarter - 1) * 3 + 1 - d.Month;
            return d.Date.AddMonths(month).StartOfMonth();
        }

        /// <summary>
        /// Returns the first instant of the week, using the specified culture.
        /// </summary>
        /// <param name="culture">The culture that defines the first day of the week.</param>
        /// <returns>A <see cref="DateTime"/> representing the start of the week.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="culture"/> is <c>null</c>.</exception>
        public DateTime StartOfWeek(CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(culture);
            var diff = (d.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek + 7) % 7;
            return d.Date.AddDays(-diff);
        }

        /// <summary>
        /// Returns the first instant of the week, using the current culture.
        /// </summary>
        /// <returns>A <see cref="DateTime"/> representing the start of the week.</returns>
        public DateTime StartOfWeek() => d.StartOfWeek(CultureInfo.CurrentCulture);

        /// <summary>
        /// Returns the first instant of the year.
        /// </summary>
        /// <returns>A <see cref="DateTime"/> representing the start of the year.</returns>
        public DateTime StartOfYear() => d.Date.AddDays(1 - d.DayOfYear);
    }
}