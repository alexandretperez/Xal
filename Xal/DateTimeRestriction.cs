namespace Xal
{
    /// <summary>
    /// Specifies the restriction mode.
    /// </summary>
    /// <see cref="DateTimeSpecification.RestrictTo(DateTimeRestriction)"/>
    public enum DateTimeRestriction : byte
    {
        /// <summary>
        /// Indicates no restriction
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates restriction by week
        /// </summary>
        Week = 1,

        /// <summary>
        /// Indicates restriction by month
        /// </summary>
        Month = 2,

        /// <summary>
        /// Indicates restriction by year
        /// </summary>
        Year = 3
    }
}