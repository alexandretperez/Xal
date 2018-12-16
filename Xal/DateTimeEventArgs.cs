using System;

namespace Xal
{
    /// <summary>
    /// Provides data for the <see cref="DateSpecification.BeforeApply"/> and <see cref="DateSpecification.AfterApply"/> events.
    /// </summary>
    public class DateTimeEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DateTimeEventArgs"/>.
        /// </summary>
        /// <param name="date">The date object.</param>
        public DateTimeEventArgs(DateTime date)
        {
            Date = date;
        }

        /// <summary>
        /// Gets the date that is being handled by <see cref="DateSpecification"/>.
        /// </summary>
        public DateTime Date { get; }

        /// <summary>
        /// Gets or sets a new result for the date that is being handled. If <c>null</c>, no changes is made.
        /// </summary>
        public DateTime? Result { get; set; }
    }
}