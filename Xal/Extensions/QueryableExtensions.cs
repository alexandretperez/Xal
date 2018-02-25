using System;
using System.Linq;
using System.Linq.Expressions;
using Xal.Data;

namespace Xal.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="IQueryable{T}"/> objects.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Distincts the reference <paramref name="source"/> by the specified <paramref name="selector"/>.
        /// </summary>
        /// <typeparam name="T">The type of the list items.</typeparam>
        /// <typeparam name="TKey">The type of the selector key.</typeparam>
        /// <param name="source">The list of items.</param>
        /// <param name="selector">The selector key.</param>
        /// <returns>A list of distinct items.</returns>
        public static IQueryable<T> Distinct<T, TKey>(this IQueryable<T> source, Expression<Func<T, TKey>> selector)
        {
            return source.GroupBy(selector).Select(p => p.FirstOrDefault());
        }

        /// <summary>
        /// Sorts the elements of a sequence according to an element member.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="member">The member name.</param>
        /// <param name="ascending"><c>true</c> to sort the elements by ascending and <c>false</c> to order by descending.</param>
        /// <returns>A ordered <see cref="IQueryable{T}"/>.</returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string member, bool ascending = true)
        {
            var p = Expression.Parameter(typeof(T), "p");
            var m = Expression.PropertyOrField(p, member);
            var e = Expression.Lambda(m, p);
            var method = ascending ? nameof(OrderBy) : "OrderByDescending";
            var types = new[] { source.ElementType, e.Body.Type };
            var call = Expression.Call(typeof(Queryable), method, types, source.Expression, e);
            return source.Provider.CreateQuery<T>(call);
        }

        /// <summary>
        /// Creates a new <see cref="PagedResult{T}"/> based on the reference <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements source.</typeparam>
        /// <param name="source">The source elements.</param>
        /// <param name="pageIndex">The current page index.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>A <see cref="PagedResult{T}"/>.</returns>
        public static PagedResult<T> ToPagedResult<T>(this IQueryable<T> source, int pageIndex, int pageSize) where T : class
        {
            return new PagedResult<T>(source, pageIndex, pageSize);
        }
    }
}