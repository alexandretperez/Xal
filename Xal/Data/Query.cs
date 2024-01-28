using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Xal.Data
{
    /// <summary>
    /// Provides methods to create a new <see cref="Query"/> based on the argument type from the specified <seealso cref="IQueryable{T}"/> source.
    /// </summary>
    [Obsolete("Prefer to use PagedDataSet class of the Xal.EntityFrameworkCore package instead.")]
    public class Query
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Query"/> class without a paging behavior.
        /// </summary>
        protected Query()
        {
            IsPaging = false;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Query"/> class determining the paging range.
        /// </summary>
        /// <param name="pageIndex">The zero-based page index.</param>
        /// <param name="pageSize">The page size.</param>
        protected Query(int pageIndex, int pageSize)
        {
            if (pageIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(pageIndex), "The value must be equal or greater than 0.");

            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "The value must be greater than 0.");

            PageIndex = pageIndex;
            PageSize = pageSize;
            IsPaging = true;
        }

        /// <summary>
        /// Returns the page index.
        /// </summary>
        public int PageIndex { get; }

        /// <summary>
        /// Returns the page size.
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// Determines whether the paging is defined.
        /// </summary>
        public bool IsPaging { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="Query{T}"/> class.
        /// </summary>
        /// <typeparam name="T">The argument type</typeparam>
        /// <param name="_">The IQueryable source</param>
        /// <returns>A new <see cref="Query{T}"/> object</returns>
        public static Query<T> FromSource<T>(IQueryable<T> _)
        {
            return new Query<T>();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Query{T}"/> class determining the paging range.
        /// </summary>
        /// <typeparam name="T">The argument type</typeparam>
        /// <param name="_">The IQueryable source</param>
        /// <param name="pageIndex">The zero-based page index.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>A new <see cref="Query{T}"/> object</returns>
        public static Query<T> FromSource<T>(IQueryable<T> _, int pageIndex, int pageSize)
        {
            return new Query<T>(pageIndex, pageSize);
        }
    }

    /// <summary>
    /// Provides a class that keeps a group of filters, sorting and paging range to be later executed against a query.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Query<T> : Query
    {
        private readonly List<Expression<Func<T, bool>>> _filtering = new List<Expression<Func<T, bool>>>();

        private readonly List<IOrderBy<T>> _ordering = new List<IOrderBy<T>>();

        private readonly Dictionary<string, string> _parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Initializes a new instance of <see cref="Query{T}" /> class without a paging behavior.
        /// </summary>
        public Query()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Query{T}" /> class determining the paging range.
        /// </summary>
        /// <param name="pageIndex">The zero-based page index.</param>
        /// <param name="pageSize">The page size.</param>
        public Query(int pageIndex, int pageSize) : base(pageIndex, pageSize)
        {
        }

        /// <summary>
        /// Determines whether there are filters defined.
        /// </summary>
        public bool IsFiltered => _filtering.Count > 0;

        /// <summary>
        /// Determines whether there are sortings defined.
        /// </summary>
        public bool IsOrdered => _ordering.Count > 0;

        /// <summary>
        /// Adds a new query filter.
        /// </summary>
        /// <param name="predicate">A function to test each source element for a condition.</param>
        public void AddFilter(Expression<Func<T, bool>> predicate)
        {
            _filtering.Insert(0, predicate);
        }

        /// <summary>
        /// Adds a new parameter to an insensitive case dictionary that can be used later for different query strategies.
        /// </summary>
        /// <param name="key">Key name.</param>
        /// <param name="value">Value.</param>
        public void AddParameter(string key, string value)
        {
            if (!_parameters.ContainsKey(key))
                _parameters.Add(key, value);

            _parameters[key] = value;
        }

        /// <summary>
        /// Gets the stored parameter by its key.
        /// </summary>
        /// <param name="key">Key name.</param>
        /// <returns>The value associated with the specified key.</returns>
        public string GetParameter(string key)
        {
            if (_parameters.ContainsKey(key))
                return _parameters[key];

            return null;
        }

        /// <summary>
        /// Adds a new sorting operation.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="descending">When <c>true</c> determines to sorting in descending order; othewise in asceding order.</param>
        public void AddOrder<TProperty>(Expression<Func<T, TProperty>> keySelector, bool descending = false)
        {
            _ordering.Insert(0, new OrderBy<T, TProperty>(keySelector, descending));
        }

        /// <summary>
        /// Runs the filters, sorting and paging against the specified <paramref name="query"/>.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A <see cref="QueryResult{T}"/>.</returns>
        public QueryResult<T> Run(IQueryable<T> query)
        {
            IQueryable<T> rawQuery = query = BuildQuery(query);

            if (PageSize > 0)
            {
                query = PageIndex > 0
                   ? query.Skip(PageIndex).Take(PageSize)
                   : query.Take(PageSize);
            }

            var result = new QueryResult<T>(query);
            Ready?.Invoke(this, new QueryReadyEventArgs<T>(rawQuery, result));
            return result;
        }

        /// <summary>
        /// Runs the filters, sorting and paging against the specified <paramref name="query"/>.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>A <see cref="PagedResult{T}"/>.</returns>
        public PagedResult<T> PagedRun(IQueryable<T> query)
        {
            query = BuildQuery(query);

            var pi = PageIndex;
            var ps = PageSize;
            if (!IsPaging)
            {
                pi = 0;
                ps = 1;
            }

            var result = new PagedResult<T>(query, pi, ps);
            Ready?.Invoke(this, new QueryReadyEventArgs<T>(query, result));
            return result;
        }

        private IQueryable<T> BuildQuery(IQueryable<T> query)
        {
            foreach (var filter in _filtering)
                query = query.Where(filter);

            foreach (var order in _ordering)
                query = order.Run(query);

            return query;
        }

        /// <summary>
        /// An event that occurs after the <see cref="Run(IQueryable{T})"/>/<see cref="PagedRun(IQueryable{T})"/> method gets the query result.
        /// </summary>
        public event EventHandler<QueryReadyEventArgs<T>> Ready;
    }
}