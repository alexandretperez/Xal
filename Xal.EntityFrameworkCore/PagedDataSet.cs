using Microsoft.EntityFrameworkCore;

namespace Xal
{
    /// <summary>
    /// Represents a paged dataset.
    /// </summary>
    /// <typeparam name="TSource">The source type</typeparam>
    public sealed class PagedDataSet<TSource>
    {
        public PagedDataSet(List<TSource> items, int pageIndex, int pageSize, int totalCount)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(TotalCount / (float)pageSize);
            Count = items.Count;
            Items = items;
            Meta = new Dictionary<string, object>();
        }

        /// <summary>
        /// The list of records present on the dataset.
        /// </summary>
        public IReadOnlyList<TSource> Items { get; }

        /// <summary>
        /// The page index (zero-based) of the dataset
        /// </summary>
        public int PageIndex { get; }

        /// <summary>
        /// The page size of the dataset
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// The total number of pages on the dataset
        /// </summary>
        public int TotalPages { get; }

        /// <summary>
        /// The total number of records available on the dataset
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        /// The number of records of the current page of the dataset
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// A dictionary that can be used to store custom data related or not to the dataset
        /// </summary>
        public Dictionary<string, object> Meta { get; set; }

        /// <summary>
        /// Creates a new instance of the PagedDataSet object
        /// </summary>
        /// <param name="source">The async queryable source</param>
        /// <param name="pageIndex">The page index (zero-based)</param>
        /// <param name="pageSize">The page size</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>A new instance of <see cref="PagedDataSet{TSource}"/></returns>
        public static async Task<PagedDataSet<TSource>> CreateAsync(
            IQueryable<TSource> source,
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            ValidatePagination(pageIndex, pageSize);
            var count = await source.CountAsync(cancellationToken);
            var items = await source.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return new PagedDataSet<TSource>(items, pageIndex, pageSize, count);
        }

        private static void ValidatePagination(int pageIndex, int pageSize)
        {
            if (pageIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(pageIndex), "Value must be greater than or equal to 0.");

            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Value must be greater than 0.");
        }
    }
}