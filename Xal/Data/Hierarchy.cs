using System;
using System.Collections.Generic;
using System.Linq;

namespace Xal.Data
{
    /// <summary>
    /// Provides a class which has the method to create a new hierarchical list.
    /// </summary>
    public static class Hierarchy
    {
        /// <summary>
        /// Transform the reference <paramref name="source"/> into a list of <see cref="Hierarchy{T, TKey, TRelatedKey}"/> objects.
        /// </summary>
        /// <typeparam name="T">Type of the source items.</typeparam>
        /// <typeparam name="TKey">The type of the key of the source item.</typeparam>
        /// <typeparam name="TRelatedKey">The type of the related key of the source item.</typeparam>
        /// <param name="source">The reference source.</param>
        /// <param name="key">The reference key.</param>
        /// <param name="relatedKey">The related key.</param>
        /// <returns>A list of <see cref="Hierarchy{T, TKey, TRelatedKey}"/> objects.</returns>
        public static List<Hierarchy<T, TKey, TRelatedKey>> FromData<T, TKey, TRelatedKey>(IEnumerable<T> source, Func<T, TKey> key, Func<T, TRelatedKey> relatedKey)
        {
            var items = source.Select(item => new Hierarchy<T, TKey, TRelatedKey>
            {
                Key = key.Invoke(item),
                RelatedKey = relatedKey.Invoke(item),
                Item = item
            }).ToList();

            var dataToRemove = new List<Hierarchy<T, TKey, TRelatedKey>>();
            foreach (var data in items)
            {
                var current = data;
                data.Children = items.Where(p => object.Equals(current.Key, p.RelatedKey)).ToList();
                dataToRemove.AddRange(data.Children);
            }

            return items.Except(dataToRemove).ToList();
        }
    }
}