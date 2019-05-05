using System;
using System.Collections.Generic;
using System.Linq;
using Xal.Extensions;

namespace Xal
{
    /// <summary>
    /// Represents a class that allows create a set of rules to be applied into a specified <see cref="DateTime"/>.
    /// </summary>
    public class DateSpecification
    {
        private readonly HashSet<DayOfWeek> _dows = new HashSet<DayOfWeek>();
        private readonly HashSet<DateTime> _invalids = new HashSet<DateTime>();
        private DateTime? _minDate;
        private DateTime? _maxDate;

        /// <summary>
        /// Gets the easter sunday based on Gregorian Calendar.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>An easter sunday date</returns>
        public static DateTime GetEasterSunday(int year)
        {
            var g = year % 19;
            var c = year / 100;
            var h = (c - (c / 4) - (((8 * c) + 13) / 25) + (19 * g) + 15) % 30;
            var i = h - (h / 28 * (1 - (h / 28 * (29 / (h + 1)) * ((21 - g) / 11))));
            var day = i - ((year + (year / 4) + i + 2 - c + (c / 4)) % 7) + 28;
            var month = 3;

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            return new DateTime(year, month, day);
        }

        /// <summary>
        /// Occurs before the rules being applied.
        /// </summary>
        public event EventHandler<DateTimeEventArgs> BeforeApply;

        /// <summary>
        /// Occurs after the rules was applied.
        /// </summary>
        public event EventHandler<DateTimeEventArgs> AfterApply;

        /// <summary>
        /// Applies the rules to the specified <paramref name="date"/>.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>A DateTime resulting from the applied rules.</returns>
        public DateTime ApplyTo(DateTime date)
        {
            var d = date.Date;
            var args = new DateTimeEventArgs(d);

            BeforeApply?.Invoke(this, args);
            d = args.Result ?? d;

            if (_minDate is null && _maxDate is null)
            {
                while (_dows.Contains(d.DayOfWeek) || _invalids.Contains(d))
                    d = d.AddDays(1);
            }
            else
            {
                int offset = 1, minCalc = 0, maxCalc = 0;
                while (_dows.Contains(d.DayOfWeek) || _invalids.Contains(d) || IsLessThenMinimum() || IsGreaterThanMaximum())
                {
                    d = d.AddDays(offset);
                    if (IsGreaterThanMaximum())
                    {
                        offset = -1;
                        d = date.Date;
                        maxCalc++;
                        continue;
                    }

                    if (IsLessThenMinimum())
                    {
                        offset = 1;
                        d = _minDate.Value;
                        minCalc++;
                    }

                    if (minCalc > 1 && maxCalc > 1)
                        throw new Exception("Impossible apply the DateTimeSpecification's constraints.");
                }

                bool IsLessThenMinimum() => _minDate.HasValue && d < _minDate;
                bool IsGreaterThanMaximum() => _maxDate.HasValue && d > _maxDate;
            }

            args = new DateTimeEventArgs(d);
            AfterApply?.Invoke(this, args);

            return (args.Result ?? d).Date + date.TimeOfDay;
        }

        /// <summary>
        /// Adds a rule to avoid the specified day of weeks.
        /// </summary>
        /// <param name="dows">The day of weeks to be avoided.</param>
        /// <returns>Returns the current <see cref="DateSpecification"/> instance.</returns>
        public DateSpecification Avoid(params DayOfWeek[] dows)
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
        /// <returns>Returns the current <see cref="DateSpecification"/> instance.</returns>
        public DateSpecification Avoid(IEnumerable<DateTime> dates)
        {
            _invalids.AddRange(dates.Select(p => p.Date));
            return this;
        }

        /// <summary>
        /// Sets the maximum date allowed.
        /// </summary>
        /// <param name="date">The maximum date, when <c>null</c> there's no limit.</param>
        /// <returns>Returns the current <see cref="DateSpecification"/> instance.</returns>
        public DateSpecification MaxDate(DateTime? date)
        {
            _maxDate = date;
            return this;
        }

        /// <summary>
        /// Sets the minimum date allowed.
        /// </summary>
        /// <param name="date">The minimum date, when <c>null</c> there's no limit.</param>
        /// <returns>Returns the current <see cref="DateSpecification"/> instance.</returns>
        public DateSpecification MinDate(DateTime? date)
        {
            _minDate = date;
            return this;
        }
    }
}