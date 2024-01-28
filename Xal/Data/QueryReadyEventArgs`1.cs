using System;
using System.Linq;

namespace Xal.Data
{
    /// <summary>
    /// Provides data for the <see cref="Query{T}.Ready"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Obsolete("Prefer to use PagedDataSet class of the Xal.EntityFrameworkCore package instead.")]
    public class QueryReadyEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of <see cref="QueryReadyEventArgs{T}"/>.
        /// </summary>
        /// <param name="rawData">Represents the non paged data.</param>
        /// <param name="result">Represents the final data.</param>
        public QueryReadyEventArgs(IQueryable<T> rawData, QueryResult<T> result)
        {
            RawData = rawData;
            Result = result;
        }

        /// <summary>
        /// Gets the data with filter and order applied with no pagination.
        /// </summary>
        public IQueryable<T> RawData { get; }

        /// <summary>
        /// Gets the result data.
        /// </summary>
        public QueryResult<T> Result { get; }
    }
}