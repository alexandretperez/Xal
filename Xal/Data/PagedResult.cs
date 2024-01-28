using System;
using System.Collections.Generic;
using System.Linq;

namespace Xal.Data
{
    /// <summary>
    /// Represents a paged query result.
    /// </summary>
    /// <typeparam name="T">The type of each element in query.</typeparam>
    /// <seealso cref="Query{T}.PagedRun(IQueryable{T})"/>
    [Obsolete("Prefer to use PagedDataSet class of the Xal.EntityFrameworkCore package instead.")]
    public class PagedResult<T> : QueryResult<T>
    {
        private PagedResult(int pageIndex, int pageSize)
        {
            if (pageIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(pageIndex), "Value must be equal or greater than 0.");

            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Value must be greater than 0.");

            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PagedResult{T}"/>.
        /// </summary>
        /// <param name="data">The query.</param>
        /// <param name="pageIndex">The zero-based page index.</param>
        /// <param name="pageSize">The page size.</param>
        public PagedResult(IQueryable<T> data, int pageIndex, int pageSize) : this(pageIndex, pageSize)
        {
            RowsCount = data.Count();
            PagesCount = (int)Math.Ceiling(RowsCount / (double)pageSize);
            Data = (pageIndex > 0
                ? data.Skip(pageIndex * pageSize).Take(pageSize)
                : data.Take(pageSize)).ToList();
        }

        /// <summary>
        /// The page index of this instance.
        /// </summary>
        public int PageIndex { get; }

        /// <summary>
        /// The page size of this instance.
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// The total rows count of the query data.
        /// </summary>
        public int RowsCount { get; set; }

        /// <summary>
        /// The total pages count of the query data.
        /// </summary>
        public int PagesCount { get; set; }

        /// <summary>
        /// Converts each element of the query <see cref="QueryResult{T}.Data"/> to the specified <typeparamref name="TResult"/> type.
        /// </summary>
        /// <typeparam name="TResult">The destination type.</typeparam>
        /// <param name="converter">The converter.</param>
        /// <returns>A new instance of <see cref="PagedResult{TResult}"/></returns>
        public PagedResult<TResult> Convert<TResult>(Converter<IEnumerable<T>, IEnumerable<TResult>> converter) where TResult : class
        {
            return new PagedResult<TResult>(PageIndex, PageSize)
            {
                RowsCount = RowsCount,
                PagesCount = PagesCount,
                Data = converter(Data),
                CustomData = CustomData
            };
        }
    }
}