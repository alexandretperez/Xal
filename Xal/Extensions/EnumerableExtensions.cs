using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using Xal.Data;

namespace Xal.Extensions
{
    /// <summary>
    /// Provides extension methods to <see cref="Enumerable"/> objects.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Converts the reference <paramref name="items" /> to a list of <see cref="Hierarchy{T, TKey, TRelatedKey}" /> object based on the relation between specified <paramref name="key" /> and <paramref name="relatedKey" />.
        /// </summary>
        /// <typeparam name="T">The type of the list items.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TRelatedKey">The type of the related key.</typeparam>
        /// <param name="items">The list of items.</param>
        /// <param name="key">The reference key.</param>
        /// <param name="relatedKey">The reference foreign key.</param>
        /// <returns>
        /// Returns a <see cref="List{T}" /> of <see cref="Hierarchy" /> object.
        /// </returns>
        public static List<Hierarchy<T, TKey, TRelatedKey>> AsHierarchy<T, TKey, TRelatedKey>(this IEnumerable<T> items, Func<T, TKey> key, Func<T, TRelatedKey> relatedKey)
        {
            return Hierarchy.FromData(items, key, relatedKey);
        }

        /// <summary>
        /// Distincts the reference <paramref name="items"/> by the specified <paramref name="selector"/>.
        /// </summary>
        /// <typeparam name="T">The type of the list items.</typeparam>
        /// <typeparam name="TKey">The type of the selector key.</typeparam>
        /// <param name="items">The list of items.</param>
        /// <param name="selector">The selector key.</param>
        /// <returns>A list of distinct items.</returns>
        public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> items, Func<T, TKey> selector)
        {
            return items.GroupBy(selector).Select(p => p.First());
        }

        /// <summary>
        /// Determines whether a list of items has cross-reference by checking if the left selector value exists on the right selector or vice-versa.
        /// </summary>
        /// <typeparam name="T">The type of the list items.</typeparam>
        /// <typeparam name="TProperty">The type of the properties.</typeparam>
        /// <param name="items">The list of items.</param>
        /// <param name="leftSelector">The left selector.</param>
        /// <param name="rightSelector">The right selector.</param>
        /// <returns><c>true</c> if has cross-reference; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The left selector cannot be null
        /// or
        /// The right selector cannot be null
        /// </exception>
        public static bool IsCrossReferenced<T, TProperty>(this IEnumerable<T> items, Func<T, TProperty> leftSelector, Func<T, TProperty> rightSelector)
        {
            if (leftSelector == null)
                throw new ArgumentNullException(nameof(leftSelector), "The left selector cannot be null");

            if (rightSelector == null)
                throw new ArgumentNullException(nameof(rightSelector), "The right selector cannot be null");

            var collection = items.Select(p => new { Left = leftSelector(p), Right = rightSelector(p) }).ToHashSet();
            return (from a in collection
                    join b in collection on a.Left equals b.Right
                    where Equals(a.Right, b.Left)
                    select 1).Any();
        }

        /// <summary>
        /// Shuffles the list of items.
        /// </summary>
        /// <typeparam name="T">The type of the list items.</typeparam>
        /// <param name="items">The list of items.</param>
        /// <returns>A shuffled list of items.</returns>
        public static IOrderedEnumerable<T> Shuffle<T>(this IEnumerable<T> items)
        {
            return items.OrderBy(_ => Guid.NewGuid());
        }

        /// <summary>
        /// Joins the items of the list into a concatenated string.
        /// </summary>
        /// <typeparam name="T">The type of the list items.</typeparam>
        /// <param name="items">The list of items.</param>
        /// <param name="separator">The items separator.</param>
        /// <param name="format">The string format to be applied on each item.</param>
        /// <returns>A <see cref="string"/>.</returns>
        public static string ToConcatString<T>(this IEnumerable<T> items, string separator = ",", string format = "{0}")
        {
            var sb = new StringBuilder();
            foreach (var item in items)
                sb.AppendFormat(format, item).Append(separator);

            return sb.Length == 0
                ? ""
                : sb.ToString(0, sb.Length - separator.Length);
        }

        /// <summary>
        /// Joins the items of the list into a concatenated string applying the specified template for each item of the list.
        /// </summary>
        /// <typeparam name="T">The type of the list items.</typeparam>
        /// <param name="items">The list of items.</param>
        /// <param name="template">The string template. See <see cref="StringExtensions.FormatTemplate(string, object[])"/></param>
        /// <param name="separator">The items separator.</param>
        /// <returns>A <see cref="string"/>.</returns>
        /// <seealso cref="StringExtensions.FormatTemplate(string, object[])"/>
        [Obsolete("This method will be removed in future versions")]
        public static string ToConcatTemplate<T>(this IEnumerable<T> items, string template, string separator = ",")
        {
            var sb = new StringBuilder();
            foreach (var item in items)
                sb.Append(template.FormatTemplate(item)).Append(separator);

            return sb.Length == 0
                 ? ""
                 : sb.ToString(0, sb.Length - separator.Length);
        }

        /// <summary>
        /// Converts a list of items into a <see cref="DataTable" />.
        /// </summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="items">The list of items.</param>
        /// <param name="tableName">The table name.</param>
        /// <returns>
        /// A <see cref="DataTable" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">table name is null or empty.</exception>
        public static DataTable ToDataTable<T>(this IEnumerable<T> items, string tableName = "Table1")
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName), "table name is null or empty");

            var table = new DataTable(tableName);

            if (items.FirstOrDefault() is ExpandoObject sample)
                return ToTableFromExpandoObject(items, table);

            var properties = typeof(T).GetProperties().Where(p => p.CanRead);

            foreach (var prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (var item in items)
            {
                var dr = table.NewRow();
                foreach (var prop in properties)
                    dr[prop.Name] = prop.GetValue(item, null) ?? DBNull.Value;

                table.Rows.Add(dr);
            }

            return table;
        }

        private static DataTable ToTableFromExpandoObject<T>(IEnumerable<T> items, DataTable table)
        {
            if (!(items.FirstOrDefault() is IDictionary<string, object> sample))
                return table;

            foreach (var prop in sample)
            {
                Type type = prop.Value?.GetType() ?? typeof(object);
                table.Columns.Add(prop.Key, Nullable.GetUnderlyingType(type) ?? type);
            }

            foreach (var item in items)
            {
                var dr = table.NewRow();
                foreach (var prop in (IDictionary<string, object>)item)
                    dr[prop.Key] = prop.Value ?? DBNull.Value;

                table.Rows.Add(dr);
            }

            return table;
        }

#if !NETCOREAPP2_0 && !NET472

        /// <summary>
        /// Converts a <see cref="IEnumerable{T}"/> to a <see cref="HashSet{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the list items.</typeparam>
        /// <param name="items">The list of items.</param>
        /// <returns>A <see cref="HashSet{T}"/>.</returns>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> items)
        {
            return new HashSet<T>(items);
        }

#endif

        /// <summary>
        /// Returns a collection with the values present in the <see cref="IGrouping{TKey, TElement}"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the key of the <see cref="IGrouping{TKey, TElement}"/>.</typeparam>
        /// <typeparam name="TElement">The type of the values in the <see cref="IGrouping{TKey, TElement}"/>.</typeparam>
        /// <param name="group">The <see cref="IGrouping{TKey, TElement}"/></param>
        /// <returns>A <see cref="IEnumerable{TElement}"/>.</returns>
        public static IEnumerable<TElement> Values<TKey, TElement>(this IGrouping<TKey, TElement> group) => group.Select(p => p);

        /// <summary>
        /// Returns a collection with the keys present in the <see cref="IGrouping{TKey, TElement}"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the key of the <see cref="IGrouping{TKey, TElement}"/>.</typeparam>
        /// <typeparam name="TElement">The type of the values in the <see cref="IGrouping{TKey, TElement}"/>.</typeparam>
        /// <param name="enumerable">The collection of <see cref="IGrouping{TKey, TElement}"/>.</param>
        /// <returns>A <see cref="IEnumerable{TElement}"/>.</returns>
        public static IEnumerable<TKey> Keys<TKey, TElement>(this IEnumerable<IGrouping<TKey, TElement>> enumerable) => enumerable.Select(p => p.Key);

        /// <summary>
        /// Returns a collection with the values present in the current collection of <see cref="IGrouping{TKey, TElement}"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the key of the <see cref="IGrouping{TKey, TElement}"/>.</typeparam>
        /// <typeparam name="TElement">The type of the values in the <see cref="IGrouping{TKey, TElement}"/>.</typeparam>
        /// <param name="enumerable">The collection of <see cref="IGrouping{TKey, TElement}"/>.</param>
        /// <returns>A <see cref="IEnumerable{TElement}"/>.</returns>
        public static IEnumerable<IEnumerable<TElement>> Values<TKey, TElement>(this IEnumerable<IGrouping<TKey, TElement>> enumerable) => enumerable.Select(Values);
    }
}