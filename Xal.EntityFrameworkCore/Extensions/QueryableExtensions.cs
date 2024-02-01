namespace Xal.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="IQueryable{T}"/> objects.
    /// </summary>
    public static class QueryableExtensionsEFCore
    {
        /// <summary>
        /// Creates a new instance of the PagedDataSet object.
        /// </summary>
        /// <param name="source">The async queryable source.</param>
        /// <param name="pageIndex">The page index (zero-based).</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A new instance of <see cref="PagedDataSet{TSource}"/>.</returns>
        public static Task<PagedDataSet<TSource>> ToPagedDataSetAsync<TSource>(
            this IQueryable<TSource> source,
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            return PagedDataSet<TSource>.CreateAsync(source, pageIndex, pageSize, cancellationToken);
        }

        /// <summary>
        /// Creates a new instance of the PagedDataSet object.
        /// </summary>
        /// <param name="source">The queryable source.</param>
        /// <param name="pageIndex">The page index (zero-based).</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>A new instance of <see cref="PagedDataSet{TSource}"/>.</returns>
        public static PagedDataSet<TSource> ToPagedDataSet<TSource>(
            this IQueryable<TSource> source,
            int pageIndex,
            int pageSize)
        {
            return new PagedDataSet<TSource>(source, pageIndex, pageSize);
        }
    }
}