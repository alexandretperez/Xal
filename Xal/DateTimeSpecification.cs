using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xal.Extensions;

namespace Xal
{
    /// <summary>
    /// Represents a class that allows create a set of rules to be applied into a specified <see cref="DateTime"/>.
    /// </summary>
    public class DateTimeSpecification
    {
        private readonly HashSet<DayOfWeek> _dows = new HashSet<DayOfWeek>();
        private readonly HashSet<DateTime> _invalids = new HashSet<DateTime>();
        private readonly DayOfWeek _lastDayOfWeek = DayOfWeek.Saturday;
        private DateTimeRestriction _restriction = DateTimeRestriction.None;

        /// <summary>
        /// Creates a new <see cref="DateTimeSpecification"/> instance.
        /// </summary>
        public DateTimeSpecification()
        {
        }

        /// <summary>
        /// Creates a new <see cref="DateTimeSpecification"/> instance with an specified <paramref name="culture"/>.
        /// </summary>
        /// <param name="culture">The <seealso cref="CultureInfo"/> to be used.</param>
        public DateTimeSpecification(CultureInfo culture)
        {
            if (culture.DateTimeFormat.FirstDayOfWeek > DayOfWeek.Sunday)
                _lastDayOfWeek = culture.DateTimeFormat.FirstDayOfWeek - 1;
        }

        /// <summary>
        /// Gets the easter sunday based on Gregorian Calendar.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>An easter sunday date</returns>
        public static DateTime GetEasterSunday(int year)
        {
            var g = year % 19;
            var c = year / 100;
            var h = (c - c / 4 - (8 * c + 13) / 25 + 19 * g + 15) % 30;
            var i = h - h / 28 * (1 - h / 28 * (29 / (h + 1)) * ((21 - g) / 11));
            var day = i - ((year + year / 4 + i + 2 - c + c / 4) % 7) + 28;
            var month = 3;

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            return new DateTime(year, month, day);
        }

        /// <summary>
        /// Represents the method that will handle the events.
        /// </summary>
        /// <param name="date">The date.</param>
        public delegate void ActionHandler(object sender, ref DateTime e);

        /// <summary>
        /// Occurs before the rules being applied.
        /// </summary>
        public event ActionHandler BeforeApply;

        /// <summary>
        /// Occurs when the rules was applied.
        /// </summary>
        public event ActionHandler AfterApply;

        /// <summary>
        /// Applies the rules to the specified <paramref name="date"/>.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>A DateTime resulting from the applied rules.</returns>
        public DateTime ApplyTo(DateTime date)
        {
            var d = date.Date;

            BeforeApply?.Invoke(this, ref d);

            if (_restriction == DateTimeRestriction.None)
            {
                while (_dows.Contains(d.DayOfWeek) || _invalids.Contains(d))
                    d = d.AddDays(1);
            }
            else
            {
                DateTime? minValidDate = null;
                DateTime? maxValidDate = null;

                switch (_restriction)
                {
                    case DateTimeRestriction.Week:
                        minValidDate = d.BeginningOfWeek().Date;
                        maxValidDate = d.EndOfWeek().Date;
                        break;

                    case DateTimeRestriction.Month:
                        minValidDate = d.BeginningOfMonth().Date;
                        maxValidDate = d.EndOfMonth().Date;
                        break;

                    case DateTimeRestriction.Year:
                        minValidDate = d.BeginningOfYear().Date;
                        maxValidDate = d.EndOfYear().Date;
                        break;
                }

                var offset = 1;
                while (_dows.Contains(d.DayOfWeek) || _invalids.Contains(d))
                {
                    d = d.AddDays(offset);
                    if (d > maxValidDate)
                    {
                        offset = -1;
                        d = date.Date;
                        continue;
                    }

                    if (d < minValidDate)
                        throw new Exception("Impossible apply the DateTimeHandler's constraints");
                }
            }

            AfterApply?.Invoke(this, ref d);
            return d;
        }

        /// <summary>
        /// Adds a rule to avoid the specified day of weeks.
        /// </summary>
        /// <param name="dows">The day of weeks to be avoided.</param>
        /// <returns>Returns the current <see cref="DateTimeSpecification"/> instance.</returns>
        public DateTimeSpecification Avoid(params DayOfWeek[] dows)
        {
            _dows.AddRange(dows);
            if (_dows.Count == 7)
                throw new ArgumentException("Is not allowed specify all days of week to avoiding.", nameof(dows));

            return this;
        }

        /// <summary>
        /// Adds a rule to avoid the specified <paramref name="dates"/>.
        /// </summary>
        /// <param name="dates">The dates to be avoided.</param>
        /// <returns>Returns the current <see cref="DateTimeSpecification"/> instance.</returns>
        public DateTimeSpecification Avoid(IEnumerable<DateTime> dates)
        {
            _invalids.AddRange(dates.Select(p => p.Date));
            return this;
        }

        /// <summary>
        /// Determines a range restriction to the date.
        /// </summary>
        /// <param name="restriction"></param>
        /// <returns>Returns the current <see cref="DateTimeSpecification"/> instance.</returns>
        public DateTimeSpecification RestrictTo(DateTimeRestriction restriction)
        {
            _restriction = restriction;
            return this;
        }
    }
}