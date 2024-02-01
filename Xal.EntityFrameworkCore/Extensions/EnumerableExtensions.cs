namespace Xal.Extensions
{
    /// <summary>
    /// Provides extension methods to <see cref="Enumerable"/> objects.
    /// </summary>
    public static class EnumerableExtensionsEFCore
    {
        /// <summary>
        /// Creates a new instance of the PagedDataSet object.
        /// </summary>
        /// <param name="source">The enumerable source.</param>
        /// <param name="pageIndex">The page index (zero-based).</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>A new instance of <see cref="PagedDataSet{TSource}"/>.</returns>
        public static PagedDataSet<TSource> ToPagedDataSet<TSource>(
            this IEnumerable<TSource> source,
            int pageIndex,
            int pageSize)
        {
            return new PagedDataSet<TSource>(source, pageIndex, pageSize);
        }
    }
}