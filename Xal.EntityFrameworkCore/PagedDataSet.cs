using Microsoft.EntityFrameworkCore;

namespace Xal
{
    /// <summary>
    /// Represents a paged dataset.
    /// </summary>
    /// <typeparam name="TSource">The source type.</typeparam>
    public sealed class PagedDataSet<TSource>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="PagedDataSet{TSource}"/> assuming the paging logic was already performed before hand.
        /// </summary>
        /// <param name="items">The source data.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="totalCount">The total records of the source data.</param>
        public PagedDataSet(IEnumerable<TSource> items, int pageIndex, int pageSize, int totalCount)
        {
            Items = items.ToList().AsReadOnly();
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(TotalCount / (float)PageSize);
            Meta = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PagedDataSet{TSource}"/>.
        /// </summary>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="pageSize">The page size.</param>
        private PagedDataSet(int pageIndex, int pageSize)
        {
            ValidatePagination(pageIndex, pageSize);

            PageIndex = pageIndex;
            PageSize = pageSize;
            Meta = new Dictionary<string, object>();
            Items = new List<TSource>();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PagedDataSet{TSource}"/> applying the pagination logic.
        /// </summary>
        /// <param name="items">The source data.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="pageSize">The page size.</param>
        public PagedDataSet(IEnumerable<TSource> items, int pageIndex, int pageSize) : this(pageIndex, pageSize)
        {
            TotalCount = items.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (float)PageSize);
            Items = (PageSize > 0 ? items.Skip(PageIndex * PageSize).Take(PageSize) : items.Take(PageSize)).ToList();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PagedDataSet{TSource}"/> applying the pagination logic.
        /// </summary>
        /// <param name="items">The source data.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="pageSize">The page size.</param>
        public PagedDataSet(IQueryable<TSource> items, int pageIndex, int pageSize) : this(pageIndex, pageSize)
        {
            _ = InitializeQueryable(items, false);
        }

        private async Task InitializeQueryable(IQueryable<TSource> items, bool asyncronously, CancellationToken cancellationToken = default)
        {
            TotalCount = asyncronously ? await items.CountAsync(cancellationToken) : items.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (float)PageSize);

            var paging = PageSize > 0 ? items.Skip(PageIndex * PageSize).Take(PageSize) : items.Take(PageSize);
            Items = asyncronously ? await paging.ToListAsync(cancellationToken) : paging.ToList();
        }

        /// <summary>
        /// The list of records present on the dataset.
        /// </summary>
        public IReadOnlyList<TSource> Items { get; private set; }

        /// <summary>
        /// The page index (zero-based) of the dataset.
        /// </summary>
        public int PageIndex { get; }

        /// <summary>
        /// The page size of the dataset.
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// The total number of pages on the dataset.
        /// </summary>
        public int TotalPages { get; private set; }

        /// <summary>
        /// The total number of records available on the dataset.
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        /// The number of records of the current page of the dataset.
        /// </summary>
        public int Count => Items.Count;

        /// <summary>
        /// Determines if there is a previous page.
        /// </summary>
        public bool HasPreviousPage => PageIndex > 0;

        /// <summary>
        /// Determines if there is a next page.
        /// </summary>
        public bool HasNextPage => PageIndex + 1 < TotalPages;

        /// <summary>
        /// A dictionary that can be used to store custom data related or not to the dataset.
        /// </summary>
        public Dictionary<string, object> Meta { get; set; }

        /// <summary>
        /// Creates a new instance of the PagedDataSet object.
        /// </summary>
        /// <param name="source">The async queryable source.</param>
        /// <param name="pageIndex">The page index (zero-based).</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A new instance of <see cref="PagedDataSet{TSource}"/>.</returns>
        public static async Task<PagedDataSet<TSource>> CreateAsync(
            IQueryable<TSource> source,
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var result = new PagedDataSet<TSource>(pageIndex, pageSize);
            await result.InitializeQueryable(source, true, cancellationToken);
            return result;
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