using System.Collections.Generic;
using System.Linq;

namespace Xal.Data
{
    /// <summary>
    /// Represents a query result.
    /// </summary>
    /// <typeparam name="T">The type of each element in query.</typeparam>
    /// <seealso cref="Query{T}.Run(IQueryable{T})"/>
    public class QueryResult<T>
    {
        /// <summary>
        /// Empty constructor
        /// </summary>
        protected QueryResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="QueryResult{T}"/>.
        /// </summary>
        /// <param name="data">The query data.</param>
        public QueryResult(IQueryable<T> data)
        {
            Data = data.ToList();
        }

        /// <summary>
        /// The query data.
        /// </summary>
        public IEnumerable<T> Data { get; protected set; }

        /// <summary>
        /// A custom data dictionary.
        /// </summary>
        public Dictionary<string, object> CustomData { get; set; } = new Dictionary<string, object>();
    }
}