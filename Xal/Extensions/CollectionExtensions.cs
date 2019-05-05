using System;
using System.Collections.Generic;
using System.Linq;

namespace Xal.Extensions
{
    /// <summary>
    /// Provides extensions for collections and lists.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Gets the average of the number of occurrences matching to the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of the list items.</typeparam>
        /// <param name="items">The list of items.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static double AverageCount<T>(this ICollection<T> items, Func<T, bool> predicate)
        {
            return (double)items.Count(predicate) * 1 / items.Count.Min(1);
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of <see cref="ICollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the list items.</typeparam>
        /// <param name="self">The current collection.</param>
        /// <param name="collection">
        /// The collection whose elements should be added to the end of the <see cref="ICollection{T}"/>.
        /// The collection itself cannot be null, but it can contain elements that are null, if type T is a reference type.
        /// </param>
        /// <exception cref="ArgumentNullException">collection is null.</exception>
        public static void AddRange<T>(this ICollection<T> self, IEnumerable<T> collection)
        {
            foreach (var item in collection)
                self.Add(item);
        }

        /// <summary>
        /// Gets the subsequent value to the reference <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="T">The type of the list items.</typeparam>
        /// <param name="list">The reference list.</param>
        /// <param name="value">The reference value.</param>
        /// <param name="defaultValue">The default value to return when obtaining the subsequent value is not possible.</param>
        /// <returns>Returns the immediately subsequent value to the reference <paramref name="value"/> when possible, otherwise, returns the <paramref name="defaultValue"/>.</returns>
        public static T After<T>(this IList<T> list, T value, T defaultValue = default(T))
        {
            var index = list.IndexOf(value) + 1;
            return index >= list.Count || index == 0
                       ? defaultValue
                       : list[index];
        }

        /// <summary>
        /// Gets the preceding value to the reference <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="T">The type of the list items.</typeparam>
        /// <param name="list">The reference list.</param>
        /// <param name="value">The reference value.</param>
        /// <param name="defaultValue">The default value to return when obtaining the preceding value is not possible.</param>
        /// <returns>Returns the immediately preceding value to the reference <paramref name="value"/> when possible, otherwise, returns the <paramref name="defaultValue"/>.</returns>
        public static T Before<T>(this IList<T> list, T value, T defaultValue = default(T))
        {
            var index = list.IndexOf(value) - 1;
            return index < 0
                       ? defaultValue
                       : list[index];
        }

        /// <summary>
        /// Splits the list into chunks by the specified <paramref name="size"/>.
        /// </summary>
        /// <typeparam name="T">The type of the list items.</typeparam>
        /// <param name="items">The list of items.</param>
        /// <param name="size">The maximum size of each chunk.</param>
        /// <returns>An array of <see cref="List{T}"/>.</returns>
        public static List<T>[] Chunks<T>(this IList<T> items, int size)
        {
            var count = (int)Math.Ceiling(items.Count / (double)size);
            var chunks = new List<T>[count];
            var max = items.Count;

            for (int i = 0, j = 0, s = size; i < count; i++)
            {
                chunks[i] = new List<T>(size);
                while (j < s && j < max)
                    chunks[i].Add(items[j++]);

                s += size;
            }

            return chunks;
        }

        /// <summary>
        /// Finds all indexes of a specified <paramref name="match" />.
        /// </summary>
        /// <typeparam name="T">The type of the list items.</typeparam>
        /// <param name="items">The list of items.</param>
        /// <param name="match">The match to test.</param>
        /// <returns>A list of the indexes found.</returns>
        /// <exception cref="ArgumentNullException">match is null.</exception>
        public static List<int> FindAllIndex<T>(this IList<T> items, Predicate<T> match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            var result = new List<int>();
            for (int i = 0, j = items.Count; i < j; i++)
            {
                if (match(items[i]))
                    result.Add(i);
            }

            return result;
        }

        /// <summary>
        /// Performs the specified <paramref name="action"/> on each element of the <paramref name="items"/>.
        /// </summary>
        /// <typeparam name="T">The type of the list items.</typeparam>
        /// <param name="items">The list of items.</param>
        /// <param name="action">The <see cref="Action{T}"/> delegate to perform on each element of the <paramref name="items"/>.</param>
        /// <exception cref="ArgumentNullException">action is null.</exception>
        public static void ForEach<T>(this IList<T> items, Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var list = new List<int>();
            for (int i = 0, j = items.Count; i < j; i++)
                action(items[i]);
        }

        /// <summary>
        /// Performs the specified <paramref name="action"/> on each element of the <paramref name="items"/>.
        /// </summary>
        /// <typeparam name="T">The type of the list items.</typeparam>
        /// <param name="items">The list of items.</param>
        /// <param name="action">The <see cref="Action{T1, T2}"/> delegate to perform on each element of the <paramref name="items"/>.</param>
        /// <exception cref="ArgumentNullException">action is null.</exception>
        public static void ForEach<T>(this IList<T> items, Action<T, int> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            for (int i = 0, j = items.Count; i < j; i++)
                action(items[i], i);
        }

        /// <summary>
        /// Splits the list by the number of the specified <paramref name="fragments"/>.
        /// </summary>
        /// <typeparam name="T">The type of the list items.</typeparam>
        /// <param name="items">The list of items.</param>
        /// <param name="fragments">The amount of fragments.</param>
        /// <returns>An array of <see cref="List{T}"/>.</returns>
        public static List<T>[] Split<T>(this IList<T> items, int fragments)
        {
            var size = (int)Math.Ceiling(items.Count / (double)fragments);
            return items.Chunks(size);
        }
    }
}