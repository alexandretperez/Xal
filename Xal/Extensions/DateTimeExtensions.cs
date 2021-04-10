using System;
using System.Globalization;

namespace Xal.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="DateTime"/> objects.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Applies the specified <see cref="DateSpecification"/> to the reference date.
        /// </summary>
        /// <param name="date">The reference date.</param>
        /// <param name="handler">The <see cref="DateSpecification"/>.</param>
        /// <seealso cref="DateSpecification.ApplyTo(DateTime)"/>
        /// <returns>A date.</returns>
        [Obsolete("Use the DateTimeExtensions.Apply method instead.")]
        public static DateTime ComplyWith(this DateTime date, DateSpecification handler)
        {
            return handler.ApplyTo(date);
        }

        /// <summary>
        /// Applies the specified <see cref="DateSpecification"/> to the reference date.
        /// </summary>
        /// <param name="date">The reference date.</param>
        /// <param name="handler">The <see cref="DateSpecification"/>.</param>
        /// <seealso cref="DateSpecification.ApplyTo(DateTime)"/>
        /// <returns>The resulting DateTime after applying the specification.</returns>
        public static DateTime Apply(this DateTime date, DateSpecification handler)
        {
            return handler.ApplyTo(date);
        }

        /// <summary>
        /// Applies the specified expression of <see cref="DateSpecification"/> to the reference date.
        /// </summary>
        /// <param name="date">The reference date.</param>
        /// <param name="handler">The <see cref="DateSpecification"/>.</param>
        /// <seealso cref="DateSpecification.ApplyTo(DateTime)"/>
        /// <returns>A date.</returns>
        [Obsolete("Use the DateTimeExtensions.Apply method instead.")]
        public static DateTime ComplyWith(this DateTime date, Action<DateSpecification> handler)
        {
            return Apply(date, handler);
        }

        /// <summary>
        /// Applies the specified <see cref="DateSpecification"/> to the reference date.
        /// </summary>
        /// <param name="date">The reference date.</param>
        /// <param name="handler">The <see cref="DateSpecification"/>.</param>
        /// <seealso cref="DateSpecification.ApplyTo(DateTime)"/>
        /// <returns>The resulting DateTime after applying the specification.</returns>
        public static DateTime Apply(this DateTime date, Action<DateSpecification> handler)
        {
            var instance = new DateSpecification();
            handler.Invoke(instance);
            return Apply(date, instance);
        }

        /// <summary>
        /// Returns the first DateTime of the month based on the reference date.
        /// </summary>
        /// <param name="date">The reference date.</param>
        /// <returns>A <see cref="DateTime"/> whose value represents the beginning of the month.</returns>
        public static DateTime BeginningOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        }

        /// <summary>
        /// Returns the first DateTime of the week based on the reference date.
        /// </summary>
        /// <param name="date">The reference date.</param>
        /// <returns>A <see cref="DateTime"/> whose value represents the beginning of the week.</returns>
        public static DateTime BeginningOfWeek(this DateTime date)
        {
            var firstDow = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            return (date.Date + new TimeSpan(firstDow - date.DayOfWeek, 0, 0, 0)).Date;
        }

        /// <summary>
        /// Returns the first DateTime of the year based on the reference date.
        /// </summary>
        /// <param name="date">The reference date.</param>
        /// <returns>A <see cref="DateTime"/> whose value represents the beginning of the year.</returns>
        public static DateTime BeginningOfYear(this DateTime date)
        {
            return new DateTime(date.Year, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        }

        /// <summary>
        /// Returns the last DateTime of the day based on the reference date.
        /// </summary>
        /// <param name="date">The reference date.</param>
        /// <returns>A <see cref="DateTime"/> whose value represents the end of the day.</returns>
        public static DateTime EndOfDay(this DateTime date)
        {
            return date.Date + new TimeSpan(0, 23, 59, 59, 999);
        }

        /// <summary>
        /// Returns the last DateTime of the month based on the reference date.
        /// </summary>
        /// <param name="date">The reference date.</param>
        /// <returns>A <see cref="DateTime"/> whose value represents the end of the month.</returns>
        public static DateTime EndOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 23, 59, 59, 999, DateTimeKind.Utc);
        }

        /// <summary>
        /// Returns the last DateTime of the week based on the reference date.
        /// </summary>
        /// <param name="date">The reference date.</param>
        /// <returns>A <see cref="DateTime"/> whose value represents the end of the week.</returns>
        public static DateTime EndOfWeek(this DateTime date)
        {
            var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            var diff = 6 - (date.DayOfWeek - firstDayOfWeek);
            return date.Date + new TimeSpan(diff, 23, 59, 59, 999);
        }

        /// <summary>
        /// Returns the last DateTime of the year based on the reference date.
        /// </summary>
        /// <param name="date">The reference date.</param>
        /// <returns>A <see cref="DateTime"/> whose value represents the end of the year.</returns>
        public static DateTime EndOfYear(this DateTime date)
        {
            return new DateTime(date.Year, 12, 31, 23, 59, 59, 999, DateTimeKind.Utc);
        }

        /// <summary>
        /// Gets the difference in months between the specified dates.
        /// </summary>
        /// <param name="date">The reference date.</param>
        /// <param name="value">The object to compare to.</param>
        /// <returns>A <see cref="int"/> that represents the difference in months between the dates.</returns>
        public static int MonthsDiff(this DateTime date, DateTime value)
        {
            return ((date.Year - value.Year) * 12) + date.Month - value.Month;
        }

        /// <summary>
        /// Sets the specified <paramref name="day"/> to <paramref name="date"/>.
        /// </summary>
        /// <param name="date">The reference date.</param>
        /// <param name="day">The desired day.</param>
        /// <param name="restrictToMonth">When <c>true</c> ensures the parameter <paramref name="day"/> stay between the first and last day of the month.</param>
        /// <returns>The <see cref="DateTime"/> with the new day.</returns>
        public static DateTime SetDay(this DateTime date, int day, bool restrictToMonth = false)
        {
            var lastDay = DateTime.DaysInMonth(date.Year, date.Month);
            if (restrictToMonth)
            {
                if (day > lastDay)
                    day = lastDay;

                if (day < 1)
                    day = 1;
            }

            return date + new TimeSpan(day - date.Day, 0, 0, 0, 0);
        }

        /// <summary>
        /// Sets the specified time to the reference <paramref name="date"/>.
        /// </summary>
        /// <param name="date">The reference date.</param>
        /// <param name="hours">The hours.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <param name="milliseconds">The milliseconds.</param>
        /// <returns>A <see cref="DateTime"/> with the new time values.</returns>
        public static DateTime SetTime(this DateTime date, int hours, int minutes, int seconds = 0, int milliseconds = 0)
        {
            return SetTime(date, new TimeSpan(0, hours, minutes, seconds, milliseconds));
        }

        /// <summary>
        /// Sets the specified time to the reference <paramref name="date"/>.
        /// </summary>
        /// <param name="date">The reference date.</param>
        /// <param name="timeOfDay">The time of day.</param>
        /// <returns>A <see cref="DateTime"/> with the new time values.</returns>
        public static DateTime SetTime(this DateTime date, TimeSpan timeOfDay)
        {
            return date.Date + timeOfDay;
        }
    }
}